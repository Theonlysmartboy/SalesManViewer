Imports System.Globalization
Imports System.IO
Imports System.Net.Http
Imports System.Security.Permissions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports SalesManViewer.helpers
Imports SalesManViewer.models
Imports SalesManViewer.models.tracking

<PermissionSet(SecurityAction.Demand, Name:="FullTrust")>
<System.Runtime.InteropServices.ComVisible(True)>
Public Class SalesmanTrackerForm
    Private Const SERVER_URL As String = "http://197.248.109.130/salesman-backend"
    ' --- Endpoint action names --
    Private Const ACTION_ALL_LAST As String = "all-last"
    Private Const ACTION_USER_LAST As String = "last"
    Private Const ACTION_USER As String = "user"
    Private Const ACTION_ALL As String = "all"
    Private Const ACTION_USER_ON_DATE As String = "date"
    Private Const ACTION_ALL_ON_DATE As String = "date-all"
    Private Const ACTION_USER_AT_DATE_TIME As String = "datetime"
    Private Const ACTION_ALL_AT_DATE_TIME As String = "datetime-all"
    Private ReadOnly _http As New HttpClient()
    Private _markerBase64 As String
    Private _cardsById As New Dictionary(Of Integer, SalesmanCard)

    ' FORM LIFECYCLE
    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        'flpSalesmen.BackColor = Color.FromArgb(245, 246, 250)
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
        For Each c In flpSalesmen.Controls.OfType(Of SalesmanCard)()
            c.Width = w
        Next
    End Sub

    ' SALESMEN LIST
    Private Async Function RefreshSalesmenAsync() As Task
        Try
            BtnRefreshSm.Enabled = False
            BtnRefreshSm.Text = "Loading..."
            Dim json = Await _http.GetStringAsync($"{SERVER_URL}/api/auth.php?action=get-all-users")
            Dim apiResponse = JsonConvert.DeserializeObject(Of SalesmanApiResponse)(json)
            If apiResponse Is Nothing OrElse Not apiResponse.success Then
                MessageBox.Show("Failed to load salesmen.")
                Return
            End If
            BuildSalesmenCards(apiResponse.data)
        Catch ex As Exception
            MessageBox.Show("Load salesmen failed: " & ex.Message)
        Finally
            BtnRefreshSm.Enabled = True
            BtnRefreshSm.Text = "Refresh"
        End Try
    End Function

    Private Sub BuildSalesmenCards(users As IEnumerable(Of Object))
        flpSalesmen.SuspendLayout()
        ' dispose old cards
        For i = flpSalesmen.Controls.Count - 1 To 0 Step -1
            flpSalesmen.Controls(i).Dispose()
        Next
        _cardsById.Clear()
        For Each u In users
            Dim id = CInt(u.GetType().GetProperty("id").GetValue(u))
            Dim name = CStr(u.GetType().GetProperty("full_name").GetValue(u))
            Dim card As New SalesmanCard(id, name)
            AddHandler card.CurrentLocationClicked, AddressOf OnCurrentLocation
            AddHandler card.TodayMovementClicked, AddressOf OnTodayMovement
            AddHandler card.LocationByDateClicked, AddressOf OnLocationByDate
            AddHandler card.MovementHistoryClicked, AddressOf OnMovementHistory
            flpSalesmen.Controls.Add(card)
            _cardsById(id) = card
        Next
        flpSalesmen.ResumeLayout()
        ResizeCards()
        ApplySearchFilter()
    End Sub

    Private Sub TxtSearchSalesMen_TextChanged(sender As Object, e As EventArgs) Handles TxtSearchSalesMen.TextChanged
        If TxtSearchSalesMen.ForeColor = Color.Gray Then Exit Sub
        ApplySearchFilter()
    End Sub

    Private Sub ApplySearchFilter()
        Dim s = GetSearchText().ToLowerInvariant()
        For Each c In flpSalesmen.Controls.OfType(Of SalesmanCard)()
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
        Dim card = TryCast(sender, SalesmanCard)
        If card Is Nothing Then Return
        Await LoadUserLastAsync(card.SalesmanId)
    End Sub
    Private Async Sub OnTodayMovement(sender As Object, e As EventArgs)
        Dim card = TryCast(sender, SalesmanCard)
        If card Is Nothing Then Return
        Await LoadUserByDateAsync(card.SalesmanId, card.SalesmanName, DateTime.Now)
    End Sub

    Private Async Sub OnLocationByDate(sender As Object, e As EventArgs)
        Dim card = TryCast(sender, SalesmanCard)
        If card Is Nothing Then Return
        Using dlg As New DateTimePickerDialog(
                $"Location for {card.SalesmanName}",
                includeTime:=True,
                initial:=DateTime.Now)
            If dlg.ShowDialog(Me) <> DialogResult.OK Then Return
            Await LoadUserAtDateTimeAsync(card.SalesmanId, card.SalesmanName, dlg.SelectedDateTime)
        End Using
    End Sub

    Private Async Sub OnMovementHistory(sender As Object, e As EventArgs)
        Dim card = TryCast(sender, SalesmanCard)
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
        Dim points = Await FetchTrackingAsync(
            $"{SERVER_URL}/api/tracking.php?action={ACTION_ALL_LAST}")
        PushMarkers(points)
    End Function

    Private Async Function LoadUserLastAsync(userId As Integer) As Task
        Try
            Dim url As String = $"{SERVER_URL}/api/tracking.php" &
            $"?action={ACTION_USER_LAST}&user_id={userId}"
            Dim points As List(Of TrackingPoint) = Await FetchTrackingAsync(url)
            PushMarkers(points)
        Catch ex As Exception
            MessageBox.Show("Failed to load the salesman's latest location: " &
            ex.Message, "Tracking", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Async Function LoadUserByDateAsync(userId As Integer, name As String, d As DateTime) As Task
        Try
            Dim dateString As String = d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            Dim url As String = $"{SERVER_URL}/api/tracking.php?action={ACTION_USER_ON_DATE}" &
            $"&user_id={userId}&date={Uri.EscapeDataString(dateString)}"
            Dim points As List(Of TrackingPoint) = Await FetchTrackingAsync(url)
            PushRoute(points, name)
        Catch ex As Exception
            MessageBox.Show("Failed to load route history: " & ex.Message, "Route History",
            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    Private Async Function LoadUserAtDateTimeAsync(userId As Integer, name As String, dt As DateTime) As Task
        Try
            Dim dateTimeString As String = dt.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
            Dim url As String = $"{SERVER_URL}/api/tracking.php?action={ACTION_USER_AT_DATE_TIME}" &
            $"&user_id={userId}&datetime={Uri.EscapeDataString(dateTimeString)}"
            Dim points As List(Of TrackingPoint) = Await FetchTrackingAsync(url)
            If points Is Nothing OrElse points.Count = 0 Then
                Await WbMap.CoreWebView2.ExecuteScriptAsync("clearRoute();")
                MessageBox.Show("No location was recorded for the selected date and time.",
                "Location History", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If
            PushMarkers(points)
        Catch ex As Exception
            MessageBox.Show("Failed to load location: " & ex.Message, "Location History",
            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    ' TRACKING HELPERS
    Private Async Function FetchTrackingAsync(url As String) As Task(Of List(Of TrackingPoint))
        Dim result As New List(Of TrackingPoint)()
        Try
            Dim json = Await _http.GetStringAsync(url)
            Dim trimmed = json.TrimStart()
            If trimmed.StartsWith("<") Then
                Debug.WriteLine("Non-JSON response from: " & url)
                Return result
            End If
            Dim root = JObject.Parse(json)
            Dim success = root.Value(Of Boolean?)("success")
            If success <> True Then Return result
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
            MessageBox.Show("Tracking error: " & ex.Message)
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
            If lat = 0 AndAlso lng = 0 Then Continue For
            Dim tracked_at = If(p.tracked_at, "").ToString()
            payload.Add(New Object() {lat, lng, p.username, icon, tracked_at})
        Next
        UpdateMap(payload)
    End Sub

    Private Sub PushRoute(points As List(Of TrackingPoint), salesmanName As String)
        If WbMap.CoreWebView2 Is Nothing Then
            Return
        End If
        If points Is Nothing Then
            points = New List(Of TrackingPoint)()
        End If
        Dim icon As String = GetMarkerBase64()
        Dim routePoints As New List(Of Object())
        ' Sort by actual timestamp.
        Dim orderedPoints = points.Where(Function(p) p IsNot Nothing).OrderBy(Function(p) ParseTrackingDate(p.tracked_at)).ToList()
        For Each p In orderedPoints
            Dim lat As Double
            Dim lng As Double
            If Not Double.TryParse(p.latitude, NumberStyles.Float, CultureInfo.InvariantCulture, lat) Then
                Continue For
            End If
            If Not Double.TryParse(p.longitude, NumberStyles.Float, CultureInfo.InvariantCulture, lng) Then
                Continue For
            End If
            ' Validate coordinate ranges.
            If lat < -90 OrElse lat > 90 Then Continue For
            If lng < -180 OrElse lng > 180 Then Continue For
            ' Ignore empty GPS coordinates.
            If lat = 0 AndAlso lng = 0 Then Continue For
            Dim trackedAt As String = If(p.tracked_at, "").ToString()
            routePoints.Add(New Object() {lat, lng, salesmanName, icon, trackedAt})
        Next
        If routePoints.Count = 0 Then
            WbMap.CoreWebView2.ExecuteScriptAsync("clearRoute();")
            MessageBox.Show("No tracking locations were found for the selected date.",
            "Route History", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim jsArray As String = JsonConvert.SerializeObject(routePoints)
        Dim jsName As String = JsonConvert.SerializeObject(salesmanName)
        Dim script As String = $"drawRoute({jsArray}, {jsName});"
        WbMap.CoreWebView2.ExecuteScriptAsync(script)
    End Sub

    Private Function ParseTrackingDate(value As String) As DateTime
        If String.IsNullOrWhiteSpace(value) Then
            Return DateTime.MinValue
        End If
        Dim parsed As DateTime
        Dim formats As String() = {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd HH:mm:ss.fff",
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ss.fff",
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyy-MM-ddTHH:mm:ss.fffZ"
        }
        If DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, parsed) Then
            Return parsed
        End If
        If DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, parsed) Then
            Return parsed
        End If
        Return DateTime.MinValue
    End Function

    Private Sub UpdateMap(payload As List(Of Object()))
        Dim jsArray = JsonConvert.SerializeObject(payload)
        WbMap.CoreWebView2.ExecuteScriptAsync($"updateMarkers({jsArray});")
    End Sub

    Private Async Function WaitForMapReadyAsync(Optional timeoutMs As Integer = 5000) As Task
        Dim sw = Stopwatch.StartNew()
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

    ' REFRESH + SEARCH HELPERS
    Private Async Sub BtnRefreshSm_Click(sender As Object, e As EventArgs) Handles BtnRefreshSm.Click
        Await RefreshSalesmenAsync()
        Await LoadAllLastAsync()
    End Sub

    Private Sub SetPlaceholder(txt As TextBox, placeholder As String)
        If txt Is Nothing Then Exit Sub
        If String.IsNullOrEmpty(txt.Text) Then
            txt.ForeColor = Color.Gray
            txt.Text = placeholder
            txt.Tag = placeholder
        End If
    End Sub

    Private Sub TextBox_Enter(sender As Object, e As EventArgs) Handles TxtSearchSalesMen.Enter
        Dim txt = DirectCast(sender, TextBox)
        If txt.Text = CStr(txt.Tag) Then
            txt.Text = ""
            txt.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBox_Leave(sender As Object, e As EventArgs) Handles TxtSearchSalesMen.Leave
        Dim txt = DirectCast(sender, TextBox)
        If txt.Text = "" Then
            txt.ForeColor = Color.Gray
            txt.Text = CStr(txt.Tag)
        End If
    End Sub
End Class