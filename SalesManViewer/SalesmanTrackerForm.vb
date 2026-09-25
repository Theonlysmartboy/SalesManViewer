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
        'Salesmen
        SetPlaceholder(TxtSearchSalesMen, "Start typing to search...")
        AddHandler TxtSearchSalesMen.Enter, AddressOf TextBox_Enter
        AddHandler TxtSearchSalesMen.Leave, AddressOf TextBox_Leave
        ' --- WEBVIEW2 MAP SETUP ---
        Await WbMap.EnsureCoreWebView2Async()
        WbMap.CoreWebView2.AddHostObjectToScript("bridge", Me)
        ' Load empty map
        Dim gmh As New GoogleMapsHelper(WbMap, New String(,) {})
        Await gmh.LoadMapAsync()
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
                    dt.Columns.Add("Select", GetType(Boolean))
                    dt.Columns.Add("Id")
                    dt.Columns.Add("User Name")
                    dt.Columns.Add("Full Name")
                    dt.Columns.Add("Email")
                    dt.Columns.Add("Phone")
                    dt.Columns.Add("Is Active")
                    For Each u In apiResponse.data
                        dt.Rows.Add(False, u.id, u.username, u.full_name, u.email, u.phone, u.is_Active)
                    Next
                    OriginalTables(DgvSalesMen) = dt.Copy()
                    DgvSalesMen.DataSource = dt
                    DgvSalesMen.EditMode = DataGridViewEditMode.EditOnEnter
                    DgvSalesMen.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
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

    Private Async Sub DgvSalesMen_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvSalesMen.CellClick
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
                    url = $"{serverUrl}/api/tracking.php?action=last&user_id={userId}"
                    Dim response = Await client.GetAsync(url)
                    Dim json = Await response.Content.ReadAsStringAsync()
                    Dim data = JsonConvert.DeserializeObject(Of TrackingResponseSingle)(json)
                    If data.success AndAlso data.data IsNot Nothing Then
                        Dim markers As New List(Of String()) From {
                        New String() {
                            data.data.latitude,
                            data.data.longitude,
                            data.data.username,
                            "truck_red.png"
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

    Private Sub UpdateMap(markers As List(Of String()))
        Dim jsArray As New StringBuilder("[")
        For i = 0 To markers.Count - 1
            Dim m = markers(i)
            jsArray.Append($"['{m(0)}','{m(1)}','{m(2)}','{m(3)}']")
            If i < markers.Count - 1 Then jsArray.Append(",")
        Next
        jsArray.Append("]")
        WbMap.CoreWebView2.ExecuteScriptAsync($"updateMarkers({jsArray.ToString()});")
    End Sub

    Private Function GetImageBase64() As String
        Using ms As New MemoryStream()
            My.Resources.marker.Save(ms, Imaging.ImageFormat.Png)
            Dim bytes = ms.ToArray()
            Return "data:image/png;base64," & Convert.ToBase64String(bytes)
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