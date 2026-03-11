Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
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
        For Each row As DataGridViewRow In DgvLocalProducts.Rows
            row.Cells("Select").Value = True
        Next
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
            DgvLocalProducts.Columns.Insert(0, chk)
            DgvLocalProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            DgvLocalProducts.MultiSelect = False
            DgvLocalProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        End If
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
    'global helpers
    Private Sub toggleControls(status As Boolean, button As Button, text As String)
        btnRefresh.Enabled = status
        BtnUploadSelected.Enabled = status
        BtnUploadAll.Enabled = status
        BtnSelectAll.Enabled = status
        button.Text = text
    End Sub
End Class
