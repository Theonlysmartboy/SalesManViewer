Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports SalesManViewer.models
Imports SalesManViewer.repositories

Public Class Form1
    Private repo As New ProductRepository()
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadLocalProducts()
        AddCheckboxColumntoLocalDataGrid()
        Dim chk1 As New DataGridViewCheckBoxColumn()
        chk1.HeaderText = "Select"
        chk1.Width = 50
        DgvOnlineProducts.Columns.Insert(0, chk1)
        DgvOnlineProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DgvOnlineProducts.MultiSelect = False
        DgvOnlineProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    'Local tab events
    Private Async Sub BtnUploadSelected_Click(sender As Object, e As EventArgs) Handles BtnUploadSelected.Click
        Try
            toggleControls(False, BtnUploadSelected, "Uploading...")
            Dim products As New List(Of Object)
            For Each row As DataGridViewRow In DgvLocalProducts.Rows
                If Convert.ToBoolean(row.Cells("Select").Value) = True Then
                    Dim product As New Dictionary(Of String, Object)
                    For Each col As DataGridViewColumn In DgvLocalProducts.Columns
                        If col.Name <> "Select" Then
                            product(col.Name) = row.Cells(col.Name).Value
                        End If
                    Next
                    products.Add(product)
                End If
            Next
            If products.Count = 0 Then
                MessageBox.Show("No products selected")
                Return
            End If
            Await SendProductsToServer(products)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            toggleControls(True, BtnUploadSelected, "Upload Selected")
        End Try

    End Sub

    Private Sub BtnSelectAll_Click(sender As Object, e As EventArgs) Handles BtnSelectAll.Click
        Dim selectAll As Boolean = BtnSelectAll.Text = "Select All"
        For Each row As DataGridViewRow In DgvLocalProducts.Rows
            row.Cells("Select").Value = selectAll
        Next
        If selectAll Then
            BtnSelectAll.Text = "Deselect All"
        Else
            BtnSelectAll.Text = "Select All"
        End If
    End Sub

    Private Async Sub BtnUploadAll_Click(sender As Object, e As EventArgs) Handles BtnUploadAll.Click
        Try
            toggleControls(False, BtnUploadAll, "Uploading...")
            Dim products As New List(Of Object)
            For Each row As DataGridViewRow In DgvLocalProducts.Rows
                Dim product As New Dictionary(Of String, Object)
                For Each col As DataGridViewColumn In DgvLocalProducts.Columns
                    If col.Name <> "Select" Then
                        product(col.Name) = row.Cells(col.Name).Value
                    End If
                Next
                products.Add(product)
            Next
            Await SendProductsToServer(products)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            toggleControls(True, BtnUploadAll, "Upload All")
        End Try

    End Sub
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            toggleControls(False, btnRefresh, "Loading")
            LoadLocalProducts()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            toggleControls(True, btnRefresh, "Refresh")
        End Try
    End Sub

    'helpers
    'local tab helpers
    Private Sub LoadLocalProducts()
        Dim dt = repo.GetLocalProducts()
        DgvLocalProducts.DataSource = dt
    End Sub

    Private Sub AddCheckboxColumntoLocalDataGrid()
        If Not DgvLocalProducts.Columns.Contains("Select") Then
            Dim chk As New DataGridViewCheckBoxColumn()
            chk.HeaderText = "Select"
            chk.Name = "Select"
            chk.Width = 60
            chk.ReadOnly = False
            chk.TrueValue = True
            chk.FalseValue = False
            chk.ThreeState = False
            DgvLocalProducts.Columns.Insert(0, chk)
        End If
        ' Allow editing
        DgvLocalProducts.SelectionMode = DataGridViewSelectionMode.CellSelect
        DgvLocalProducts.MultiSelect = False
        DgvLocalProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub

    Private Async Function SendProductsToServer(products As List(Of Object)) As Task
        Dim url As String = "http://197.248.220.180/salesman-backend/api/products.php?action=bulk-create"
        Dim payload = New Dictionary(Of String, Object)
        payload("products") = products
        Dim json As String = JsonConvert.SerializeObject(payload)
        Using client As New HttpClient()
            Dim content As New StringContent(json, Encoding.UTF8, "application/json")
            Dim response = Await client.PostAsync(url, content)
            Dim result = Await response.Content.ReadAsStringAsync()
            MessageBox.Show(result)
        End Using
    End Function

    Private Sub DgvLocalProducts_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles DgvLocalProducts.CurrentCellDirtyStateChanged
        If TypeOf DgvLocalProducts.CurrentCell Is DataGridViewCheckBoxCell Then
            DgvLocalProducts.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    'online tab 

    Private Async Function FetchAllRemoteProducts(lastSync As String) As Task(Of List(Of RemoteProduct))
        Dim allProducts As New List(Of RemoteProduct)
        Dim offset As Integer = 0
        Dim limit As Integer = 20
        Dim fetched As Integer

        Do
            Dim chunk As List(Of RemoteProduct) = Await GetRemoteProductsAsync(lastSync, limit, offset)
            fetched = chunk.Count
            allProducts.AddRange(chunk)
            offset += fetched
        Loop While fetched = limit ' continue if full page retrieved

        Return allProducts
    End Function
    Private Async Function GetRemoteProductsAsync(lastSync As String, limit As Integer, offset As Integer) As Task(Of List(Of RemoteProduct))
        Dim products As New List(Of RemoteProduct)
        Using client As New HttpClient()
            Dim url As String = $"http://197.248.220.180:80/salesman-backend/api/products.php?action=sync&lastSync={lastSync}&limit={limit}&offset={offset}"
            Dim response As HttpResponseMessage = Await client.GetAsync(url)
            If response.IsSuccessStatusCode Then
                Dim jsonString As String = Await response.Content.ReadAsStringAsync()
                Dim apiResponse As ProductApiResponse = JsonConvert.DeserializeObject(Of ProductApiResponse)(jsonString)
                If apiResponse.success Then
                    products.AddRange(apiResponse.data)
                End If
            End If
        End Using
        Return products
    End Function

    'global helpers
    Private Sub toggleControls(status As Boolean, button As Button, text As String)
        btnRefresh.Enabled = status
        BtnUploadSelected.Enabled = status
        BtnUploadAll.Enabled = status
        BtnSelectAll.Enabled = status
        button.Text = text
    End Sub

End Class
