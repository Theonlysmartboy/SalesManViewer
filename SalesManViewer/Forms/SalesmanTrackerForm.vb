Imports System.Globalization
Imports System.IO
Imports System.Net.Http
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports SalesManViewer.Config
Imports SalesManViewer.CustomControls.Alert
Imports SalesManViewer.Helpers
Imports SalesManViewer.Helpers.Db
Imports SalesManViewer.Models
Imports SalesManViewer.Models.tracking

''' <summary>
''' Salesman tracking dashboard: salesman list on the left, live map on the right.
''' Requires an authenticated session (UserContext.Instance must be populated).
''' </summary>
<System.Runtime.InteropServices.ComVisible(True)>
Public Class SalesmanTrackerForm
    ' CONSTANTS
    Private Const DEFAULT_BASE_URL As String = "https://197.248.109.130/salesman-backend"
    Private Const SETTING_KEY_API_BASE_URL As String = "api_base_url"
    ' Endpoint action names for /api/tracking.php
    Private Const ACTION_ALL_LAST As String = "all-last"
    Private Const ACTION_USER_LAST As String = "last"
    Private Const ACTION_USER_ON_DATE As String = "date"
    Private Const ACTION_USER_AT_DATE_TIME As String = "datetime"
    ' Endpoint action for /api/auth.php
    Private Const ACTION_GET_ALL_USERS As String = "get-all-users"
    'Shared across all instances — one HttpClient per process
    Private Shared ReadOnly _http As HttpClient = CreateHttpClient()
    Private _baseUrl As String
    Private _markerBase64 As String

    Private Shared Function CreateHttpClient() As HttpClient
        Dim c As New HttpClient()
        c.Timeout = TimeSpan.FromSeconds(30)
        c.DefaultRequestHeaders.Accept.ParseAdd("application/json")
        Return c
    End Function

    ' LIFECYCLE
    Private Async Sub SalesmanTrackerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        _baseUrl = Await ResolveBaseUrlAsync()
        SetPlaceholder(TxtSearchSalesMen, "Start typing to search...")
        Await WbMap.EnsureCoreWebView2Async()
        WbMap.CoreWebView2.AddHostObjectToScript("bridge", Me)
        Dim gmh As New GoogleMapsHelper(WbMap, New String(,) {})
        Await gmh.LoadMapAsync()
        Await WaitForMapReadyAsync()
        Await RefreshSalesmenAsync()
        Await LoadAllLastAsync()
    End Sub

    Private Sub FlpSalesmen_Resize(sender As Object, e As EventArgs) Handles flpSalesmen.Resize
        ResizeCards()
    End Sub

    Private Sub ResizeCards()
        Dim w = flpSalesmen.ClientSize.Width - flpSalesmen.Padding.Horizontal - 20
        If w < 200 Then w = 200
        For Each c In flpSalesmen.Controls.OfType(Of SalesManCard)()
            c.Width = w
        Next
    End Sub

    ' SALESMEN LIST
    Private Async Function RefreshSalesmenAsync() As Task
        Try
            BtnRefreshSm.Enabled = False
            BtnRefreshSm.Text = "Loading..."
            Dim json = Await _http.GetStringAsync(
                $"{_baseUrl}/api/auth.php?action={ACTION_GET_ALL_USERS}")
            Dim apiResponse = JsonConvert.DeserializeObject(Of SalesmanApiResponse)(json)
            If apiResponse Is Nothing OrElse Not apiResponse.success Then
                JsAlertDialog.ShowAlert(Me, "Failed to load salesmen. The server returned an error.",
                    "Salesmen", JsAlertDialog.AlertType.Warning, JsAlertDialog.ButtonType.OK, True, 10)
                Return
            End If
            BuildSalesmenCards(apiResponse.data)
        Catch ex As Exception
            JsAlertDialog.ShowAlert(Me, "Load salesmen failed: " & ex.Message, "Salesmen",
                JsAlertDialog.AlertType.Error, JsAlertDialog.ButtonType.OK, True, 10)
        Finally
            BtnRefreshSm.Enabled = True
            BtnRefreshSm.Text = "Refresh"
        End Try
    End Function

    Private Sub BuildSalesmenCards(users As IEnumerable)
        flpSalesmen.SuspendLayout()
        ' Dispose previous cards
        For i = flpSalesmen.Controls.Count - 1 To 0 Step -1
            flpSalesmen.Controls(i).Dispose()
        Next
        If users IsNot Nothing Then
            For Each u In users
                Dim id As Integer = CInt(u.[GetType]().GetProperty("id").GetValue(u))
                Dim name As String = CStr(u.[GetType]().GetProperty("full_name").GetValue(u))
                Dim card As New SalesManCard(id, name)
                AddHandler card.CurrentLocationClicked, AddressOf OnCurrentLocation
                AddHandler card.TodayMovementClicked, AddressOf OnTodayMovement
                AddHandler card.LocationByDateClicked, AddressOf OnLocationByDate
                AddHandler card.MovementHistoryClicked, AddressOf OnMovementHistory
                flpSalesmen.Controls.Add(card)
            Next
        End If
        flpSalesmen.ResumeLayout()
        ResizeCards()
        ApplySearchFilter()
    End Sub

    Private Sub TxtSearchSalesMen_TextChanged(sender As Object, e As EventArgs) Handles TxtSearchSalesMen.TextChanged
        If TxtSearchSalesMen.ForeColor = Color.Gray Then Return
        ApplySearchFilter()
    End Sub

    Private Sub ApplySearchFilter()
        Dim s = GetSearchText().ToLowerInvariant()
        For Each c In flpSalesmen.Controls.OfType(Of SalesManCard)()
            c.Visible = String.IsNullOrEmpty(s) OrElse c.SalesmanName.ToLowerInvariant().Contains(s)
        Next
    End Sub

    Private Function GetSearchText() As String
        If TxtSearchSalesMen Is Nothing Then Return ""
        If TxtSearchSalesMen.ForeColor = Color.Gray Then Return ""
        Return TxtSearchSalesMen.Text.Trim()
    End Function

    ' CARD BUTTON HANDLERS
    Private Async Sub OnCurrentLocation(sender As Object, e As EventArgs)
        Dim card = TryCast(sender, SalesManCard)
        If card Is Nothing Then Return
        Await LoadUserLastAsync(card.SalesmanId)
    End Sub

    Private Async Sub OnTodayMovement(sender As Object, e As EventArgs)
        Dim card = TryCast(sender, SalesManCard)
        If card Is Nothing Then Return
        Await LoadUserByDateAsync(card.SalesmanId, card.SalesmanName, DateTime.Today)
    End Sub

    Private Async Sub OnLocationByDate(sender As Object, e As EventArgs)
        Dim card = TryCast(sender, SalesManCard)
        If card Is Nothing Then Return
        Using dlg As New DateTimePickerDialog($"Location for {card.SalesmanName}",
                includeTime:=True, initial:=DateTime.Now)
            If dlg.ShowDialog(Me) <> DialogResult.OK Then Return
            Await LoadUserAtDateTimeAsync(card.SalesmanId, card.SalesmanName, dlg.SelectedDateTime)
        End Using
    End Sub

    Private Async Sub OnMovementHistory(sender As Object, e As EventArgs)
        Dim card = TryCast(sender, SalesManCard)
        If card Is Nothing Then Return
        Using dlg As New DateTimePickerDialog(
                $"Movement history for {card.SalesmanName}",
                includeTime:=False,
                initial:=DateTime.Today)
            If dlg.ShowDialog(Me) <> DialogResult.OK Then Return
            Await LoadUserByDateAsync(card.SalesmanId, card.SalesmanName, dlg.SelectedDateTime.Date)
        End Using
    End Sub

    ' TRACKING LOADERS
    Private Async Function LoadAllLastAsync() As Task
        Dim url = $"{_baseUrl}/api/tracking.php?action={ACTION_ALL_LAST}"
        Dim points = Await FetchTrackingAsync(url)
        PushMarkers(points)
    End Function

    Private Async Function LoadUserLastAsync(userId As Integer) As Task
        Try
            Dim url = $"{_baseUrl}/api/tracking.php?action={ACTION_USER_LAST}&user_id={userId}"
            Dim points = Await FetchTrackingAsync(url)
            PushMarkers(points)
        Catch ex As Exception
            MessageBox.Show("Failed to load the salesman's latest location: " & ex.Message,
                            "Tracking", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Async Function LoadUserByDateAsync(userId As Integer, name As String, d As DateTime) As Task
        Try
            Dim dateString = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            Dim url = $"{_baseUrl}/api/tracking.php?action={ACTION_USER_ON_DATE}" &
                      $"&user_id={userId}&date={Uri.EscapeDataString(dateString)}"
            Dim points = Await FetchTrackingAsync(url)
            Await PushRouteAsync(points, name)
        Catch ex As Exception
            MessageBox.Show("Failed to load route history: " & ex.Message,
                            "Route History", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Async Function LoadUserAtDateTimeAsync(userId As Integer, name As String, dt As DateTime) As Task
        Try
            Dim dateTimeString = dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Dim url = $"{_baseUrl}/api/tracking.php?action={ACTION_USER_AT_DATE_TIME}" &
                      $"&user_id={userId}&datetime={Uri.EscapeDataString(dateTimeString)}"
            Dim points = Await FetchTrackingAsync(url)
            If points Is Nothing OrElse points.Count = 0 Then
                Await WbMap.CoreWebView2.ExecuteScriptAsync("clearRoute();")
                MessageBox.Show("No location was recorded for the selected date and time.",
                                "Location History", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            PushMarkers(points)
        Catch ex As Exception
            MessageBox.Show("Failed to load location: " & ex.Message, "Location History", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    ' TRACKING HELPERS
    Private Async Function FetchTrackingAsync(url As String) As Task(Of List(Of TrackingPoint))
        Dim result As New List(Of TrackingPoint)()
        Try
            Dim json = Await _http.GetStringAsync(url)
            ' Guard: HTML error page
            If json.TrimStart().StartsWith("<") Then
                Debug.WriteLine("Non-JSON response from: " & url)
                Return result
            End If
            Dim root = JObject.Parse(json)
            If root.Value(Of Boolean?)("success") <> True Then Return result
            Dim dataToken = root("data")
            If dataToken Is Nothing OrElse dataToken.Type = JTokenType.Null Then Return result
            If dataToken.Type = JTokenType.Array Then
                Dim list = dataToken.ToObject(Of List(Of TrackingPoint))()
                If list IsNot Nothing Then result.AddRange(list)
            ElseIf dataToken.Type = JTokenType.Object Then
                Dim singlePoint = dataToken.ToObject(Of TrackingPoint)()
                If singlePoint IsNot Nothing Then result.Add(singlePoint)
            End If
        Catch ex As Exception
            Debug.WriteLine("Tracking error on " & url & ": " & ex.Message)
            MessageBox.Show("Tracking error: " & ex.Message,
                            "Tracking", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Return result
    End Function

    Private Sub PushMarkers(points As List(Of TrackingPoint))
        If points Is Nothing Then points = New List(Of TrackingPoint)()
        Dim icon = GetMarkerBase64()
        Dim payload As New List(Of Object())
        For Each p In points
            Dim lat As Double, lng As Double
            If Not Double.TryParse(p.latitude, NumberStyles.Float, CultureInfo.InvariantCulture, lat) Then Continue For
            If Not Double.TryParse(p.longitude, NumberStyles.Float, CultureInfo.InvariantCulture, lng) Then Continue For
            If Not IsValidCoordinate(lat, lng) Then Continue For
            payload.Add(New Object() {lat, lng, p.username, icon, If(p.tracked_at, "").ToString()})
        Next
        UpdateMap(payload)
    End Sub

    Private Async Function PushRouteAsync(points As List(Of TrackingPoint), salesmanName As String) As Task
        If WbMap.CoreWebView2 Is Nothing Then Return
        If points Is Nothing Then points = New List(Of TrackingPoint)()
        Dim icon = GetMarkerBase64()
        Dim routePoints As New List(Of Object())
        Dim ordered = points.
            Where(Function(p) p IsNot Nothing).
            OrderBy(Function(p) ParseTrackingDate(p.tracked_at))
        For Each p In ordered
            Dim lat As Double, lng As Double
            If Not Double.TryParse(p.latitude, NumberStyles.Float, CultureInfo.InvariantCulture, lat) Then Continue For
            If Not Double.TryParse(p.longitude, NumberStyles.Float, CultureInfo.InvariantCulture, lng) Then Continue For
            If Not IsValidCoordinate(lat, lng) Then Continue For
            routePoints.Add(New Object() {lat, lng, salesmanName, icon, If(p.tracked_at, "").ToString()})
        Next
        If routePoints.Count = 0 Then
            Await WbMap.CoreWebView2.ExecuteScriptAsync("clearRoute(); clearMarkers(); hideRouteSummary();")
            MessageBox.Show("No tracking locations were found for the selected date.",
                            "Route History", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim jsArray = JsonConvert.SerializeObject(routePoints)
        Dim jsName = JsonConvert.SerializeObject(salesmanName)
        Await WbMap.CoreWebView2.ExecuteScriptAsync($"drawRoute({jsArray}, {jsName});")
    End Function

    Private Function ParseTrackingDate(value As String) As DateTime
        If String.IsNullOrWhiteSpace(value) Then Return DateTime.MinValue
        Dim parsed As DateTime
        Dim formats As String() = {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss.fff",
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyy-MM-ddTHH:mm:ss.fffZ"
        }
        If DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture,
                                  DateTimeStyles.AllowWhiteSpaces, parsed) Then
            Return parsed
        End If
        If DateTime.TryParse(value, CultureInfo.InvariantCulture,
                             DateTimeStyles.AllowWhiteSpaces, parsed) Then
            Return parsed
        End If
        Return DateTime.MinValue
    End Function

    ''' <summary>
    ''' Rejects 0,0 and any out-of-range coordinate before pushing to the map.
    ''' </summary>
    Private Shared Function IsValidCoordinate(lat As Double, lng As Double) As Boolean
        If lat < -90.0 OrElse lat > 90.0 Then Return False
        If lng < -180.0 OrElse lng > 180.0 Then Return False
        If lat = 0.0 AndAlso lng = 0.0 Then Return False
        Return True
    End Function

    Private Sub UpdateMap(payload As List(Of Object()))
        Dim jsArray = JsonConvert.SerializeObject(payload)
        WbMap.CoreWebView2.ExecuteScriptAsync($"updateMarkers({jsArray});")
    End Sub

    Private Async Function WaitForMapReadyAsync(Optional timeoutMs As Integer = 5000) As Task
        Dim sw = Diagnostics.Stopwatch.StartNew()
        While sw.ElapsedMilliseconds < timeoutMs
            Try
                Dim result = Await WbMap.CoreWebView2.ExecuteScriptAsync("typeof updateMarkers === 'function'")
                If result IsNot Nothing AndAlso result.Trim().ToLower() = "true" Then Return
            Catch
            End Try
            Await Task.Delay(100)
        End While
    End Function

    Private Function GetMarkerBase64() As String
        If _markerBase64 Is Nothing Then
            _markerBase64 = RenderIconBase64(32, 32)
        End If
        Return _markerBase64
    End Function

    Private Function RenderIconBase64(width As Integer, height As Integer) As String
        Using original As Image = My.Resources.marker
            Using resized As New Bitmap(width, height)
                Using g As Graphics = Graphics.FromImage(resized)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    g.SmoothingMode = Drawing2D.SmoothingMode.HighQuality
                    g.PixelOffsetMode = Drawing2D.PixelOffsetMode.HighQuality
                    g.DrawImage(original, 0, 0, width, height)
                End Using
                Using ms As New MemoryStream()
                    resized.Save(ms, Imaging.ImageFormat.Png)
                    Return "data:image/png;base64," & Convert.ToBase64String(ms.ToArray())
                End Using
            End Using
        End Using
    End Function

    ' REFRESH + PLACEHOLDER
    Private Async Sub BtnRefreshSm_Click(sender As Object, e As EventArgs) Handles BtnRefreshSm.Click
        Await RefreshSalesmenAsync()
        Await LoadAllLastAsync()
    End Sub

    Private Sub SetPlaceholder(txt As TextBox, placeholder As String)
        If txt Is Nothing Then Return
        If String.IsNullOrEmpty(txt.Text) Then
            txt.ForeColor = Color.Gray
            txt.Text = placeholder
            txt.Tag = placeholder
        End If
    End Sub

    Private Sub TxtSearchSalesMen_Enter(sender As Object, e As EventArgs) Handles TxtSearchSalesMen.Enter
        Dim txt = DirectCast(sender, TextBox)
        If txt.Text = CStr(txt.Tag) Then
            txt.Text = ""
            txt.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TxtSearchSalesMen_Leave(sender As Object, e As EventArgs) Handles TxtSearchSalesMen.Leave
        Dim txt = DirectCast(sender, TextBox)
        If txt.Text = "" Then
            txt.ForeColor = Color.Gray
            txt.Text = CStr(txt.Tag)
        End If
    End Sub

    ' BASE URL RESOLUTION
    Private Async Function ResolveBaseUrlAsync() As Task(Of String)
        Try
            Dim connString = GlobalConnectionString.GetConnectionString()
            If String.IsNullOrWhiteSpace(connString) Then Return DEFAULT_BASE_URL

            Dim mgr As New SettingsManager(connString)
            Dim url = Await mgr.GetSettingAsync(SETTING_KEY_API_BASE_URL)

            If Not String.IsNullOrWhiteSpace(url) Then
                Return url.TrimEnd("/"c)
            End If
        Catch ex As Exception
            Debug.WriteLine("Failed to resolve base URL: " & ex.Message)
        End Try
        Return DEFAULT_BASE_URL
    End Function
End Class