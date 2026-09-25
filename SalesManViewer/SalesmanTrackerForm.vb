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
    Private Const ACTION_USER_BY_DATE As String = "date"
    Private Const ACTION_USER_BY_DATE_TIME As String = "datetime"
    Private ReadOnly _http As New HttpClient()
    Private _markerBase64 As String
    Private _cardsById As New Dictionary(Of Integer, SalesmanCard)

    ' FORM LIFECYCLE
    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetPlaceholder(TxtSearchSalesMen, "Start typing to search...")
        AddHandler TxtSearchSalesMen.Enter, AddressOf TextBox_Enter
        AddHandler TxtSearchSalesMen.Leave, AddressOf TextBox_Leave
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
        Dim points = Await FetchTrackingAsync(
            $"{SERVER_URL}/api/tracking.php?action={ACTION_USER_LAST}&user_id={userId}")
        PushMarkers(points)
    End Function

    Private Async Function LoadUserByDateAsync(userId As Integer, name As String, d As DateTime) As Task
        Dim q = $"{SERVER_URL}/api/tracking.php?action={ACTION_USER_BY_DATE}&user_id={userId}&date={d:yyyy-MM-dd}"
        Dim points = Await FetchTrackingAsync(q)
        PushMarkers(points)
    End Function

    Private Async Function LoadUserAtDateTimeAsync(userId As Integer, name As String, dt As DateTime) As Task
        Dim q = $"{SERVER_URL}/api/tracking.php?action={ACTION_USER_BY_DATE_TIME}&user_id={userId}&datetime={Uri.EscapeDataString(dt.ToString("yyyy-MM-dd HH:mm:ss",
                                                                                                                CultureInfo.InvariantCulture))}"
        Dim points = Await FetchTrackingAsync(q)
        If points Is Nothing OrElse points.Count = 0 Then
            points = Await FetchTrackingAsync(
                $"{SERVER_URL}/api/tracking.php?action={ACTION_USER_LAST}&user_id={userId}")
        End If
        PushMarkers(points)
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

    Private Sub TextBox_Enter(sender As Object, e As EventArgs)
        Dim txt = DirectCast(sender, TextBox)
        If txt.Text = CStr(txt.Tag) Then
            txt.Text = ""
            txt.ForeColor = Color.Black
        End If
    End Sub

    Private Sub TextBox_Leave(sender As Object, e As EventArgs)
        Dim txt = DirectCast(sender, TextBox)
        If txt.Text = "" Then
            txt.ForeColor = Color.Gray
            txt.Text = CStr(txt.Tag)
        End If
    End Sub
End Class