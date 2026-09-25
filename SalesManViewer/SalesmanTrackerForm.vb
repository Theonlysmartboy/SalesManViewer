Imports System.IO
Imports System.Net.Http
Imports System.Security.Permissions
Imports System.Text
Imports Newtonsoft.Json
Imports SalesManViewer.helpers
Imports SalesManViewer.models

<PermissionSet(SecurityAction.Demand, Name:="FullTrust")>
<System.Runtime.InteropServices.ComVisible(True)>
Public Class SalesmanTrackerForm
    Dim serverUrl As String = "http://197.248.109.130/salesman-backend"
    Private OriginalTables As New Dictionary(Of DataGridView, DataTable)

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetPlaceholder(TxtSearchSalesMen, "Start typing to search...")
        AddHandler TxtSearchSalesMen.Enter, AddressOf TextBox_Enter
        AddHandler TxtSearchSalesMen.Leave, AddressOf TextBox_Leave
        Await WbMap.EnsureCoreWebView2Async()
        WbMap.CoreWebView2.AddHostObjectToScript("bridge", Me)
        ' Load the empty map shell
        Dim gmh As New GoogleMapsHelper(WbMap, New String(,) {})
        Await gmh.LoadMapAsync()
        ' Give the page a moment to finish running its scripts, then push the markers.
        Await WaitForMapReadyAsync()
        BtnRefreshSm.PerformClick()
        Await LoadTracking()
    End Sub

    Private Sub TxtSearchSalesMen_TextChanged(sender As Object, e As EventArgs) Handles TxtSearchSalesMen.TextChanged
        If TxtSearchSalesMen.ForeColor = Color.Gray Then Exit Sub
        FilterGrid(DgvSalesMen, TxtSearchSalesMen.Text)
    End Sub

    Private Async Sub BtnRefreshSm_Click(sender As Object, e As EventArgs) Handles BtnRefreshSm.Click
        Try
            BtnRefreshSm.Enabled = False
            BtnRefreshSm.Text = "Loading..."
            Dim url As String = $"{serverUrl}/api/auth.php?action=get-all-users"
            Using client As New HttpClient()
                Dim response = Await client.GetAsync(url)
                Dim json = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of SalesmanApiResponse)(json)
                If apiResponse IsNot Nothing AndAlso apiResponse.success Then
                    Dim dt As New DataTable()
                    dt.Columns.Add("Id")
                    dt.Columns.Add("Full Name")
                    For Each u In apiResponse.data
                        dt.Rows.Add(u.id, u.full_name)
                    Next
                    OriginalTables(DgvSalesMen) = dt.Copy()
                    DgvSalesMen.DataSource = dt
                    DgvSalesMen.EditMode = DataGridViewEditMode.EditOnEnter
                    DgvSalesMen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                    Await LoadTracking()
                Else
                    MessageBox.Show("Failed to load salesmen.")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            BtnRefreshSm.Enabled = True
            BtnRefreshSm.Text = "Refresh"
        End Try
    End Sub

    Private Async Sub DgvSalesMen_CellClick(sender As Object, e As DataGridViewCellEventArgs)
        If e.RowIndex < 0 Then Return
        Dim row = DgvSalesMen.Rows(e.RowIndex)
        Dim userId = row.Cells("Id").Value.ToString()
        Await LoadTracking(userId)
    End Sub

    'helpers
    Private Async Function LoadTracking(Optional userId As String = "") As Task
        Try
            Using client As New HttpClient()
                Dim url As String
                If String.IsNullOrEmpty(userId) Then
                    url = $"{serverUrl}/api/tracking.php?action=all-last"
                    Dim response = Await client.GetAsync(url)
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Dim data = JsonConvert.DeserializeObject(Of TrackingResponseList)(json)
                    If data.success Then
                        Dim markers As New List(Of String())
                        Dim iconBase64 = GetImageBase64()
                        For Each t In data.data
                            markers.Add(New String() {
                            t.latitude,
                            t.longitude,
                            t.username,
                            iconBase64
                        })
                        Next
                        UpdateMap(markers)
                    End If
                Else
                    url = $"{serverUrl}/api/tracking.php?action=user&user_id={userId}"
                    Dim response = Await client.GetAsync(url)
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Dim data = JsonConvert.DeserializeObject(Of TrackingResponseSingle)(json)
                    If data.success AndAlso data.data IsNot Nothing Then
                        Dim markers As New List(Of String()) From {
                        New String() {
                            data.data.latitude,
                            data.data.longitude,
                            data.data.username,
                            GetImageBase64()
                        }
                    }
                        UpdateMap(markers)
                    End If
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Tracking error: " & ex.Message)
        End Try
    End Function

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

    Private Sub UpdateMap(markers As List(Of String()))
        ' Build a List(Of Object()) of [lat, lng, label, icon] and let Json.NET do the escaping.
        Dim payload As New List(Of Object())
        For Each m In markers
            Dim lat As Double, lng As Double
            Double.TryParse(m(0), Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, lat)
            Double.TryParse(m(1), Globalization.NumberStyles.Float, Globalization.CultureInfo.InvariantCulture, lng)
            payload.Add(New Object() {lat, lng, m(2), m(3)})
        Next
        Dim jsArray = JsonConvert.SerializeObject(payload)
        WbMap.CoreWebView2.ExecuteScriptAsync($"updateMarkers({jsArray});")
    End Sub

    Private Function GetImageBase64(Optional width As Integer = 32, Optional height As Integer = 32) As String
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

    Private Sub toggleControls(status As Boolean, button As Button, text As String)
        BtnRefreshSm.Enabled = status
        button.Text = text
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

    Private Sub FilterGrid(grid As DataGridView, search As String)
        If Not OriginalTables.ContainsKey(grid) Then Exit Sub
        Dim dt As DataTable = OriginalTables(grid)
        Dim dv As DataView = dt.DefaultView
        If String.IsNullOrWhiteSpace(search) Then
            dv.RowFilter = ""
        Else
            Dim safeSearch = search.Replace("'", "''")
            Dim filter As String = String.Join(" OR ", dt.Columns.Cast(Of DataColumn).Select(
                        Function(c) $"Convert([{c.ColumnName}], 'System.String') LIKE '%{safeSearch}%'"))
            dv.RowFilter = filter
        End If
        grid.DataSource = dv
    End Sub
End Class