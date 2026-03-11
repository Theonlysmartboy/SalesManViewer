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

    'online tab events
    Private Async Sub BtnRefreshOnline_Click(sender As Object, e As EventArgs) Handles BtnRefreshOnline.Click
        BtnRefreshOnline.Enabled = False
        Try
            ' Always sync from 1 year ago
            Dim lastSync As String = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd HH:mm:ss")
            Dim products As List(Of RemoteProduct) = Await FetchAllRemoteProducts(lastSync)
            ' Populate DataGridView
            Dim dt As New DataTable()
            dt.Columns.Add("ProductCd")
            dt.Columns.Add("ProductName")
            dt.Columns.Add("DepartmentCd")
            dt.Columns.Add("SupplierPacking")
            dt.Columns.Add("SupplierPackingDetails")
            dt.Columns.Add("ProductUnit")
            dt.Columns.Add("TagPrice")
            dt.Columns.Add("VATCode")
            dt.Columns.Add("CostPrice")
            dt.Columns.Add("LastPrice")
            dt.Columns.Add("ProductMargin")
            dt.Columns.Add("SellingPrice")
            dt.Columns.Add("MinQty")
            dt.Columns.Add("ReorderLevel")
            dt.Columns.Add("QuantityToOrder")
            dt.Columns.Add("SupplierCd")
            dt.Columns.Add("Weight")
            dt.Columns.Add("ProductQty")
            dt.Columns.Add("isAlternetUnit")
            dt.Columns.Add("AlternetUnit")
            dt.Columns.Add("UnitValue")
            dt.Columns.Add("AlternetUnitValue")
            dt.Columns.Add("HSCode")
            dt.Columns.Add("HSDesc")
            dt.Columns.Add("IsActive")
            dt.Columns.Add("IsStockItem")
            dt.Columns.Add("Remark")
            dt.Columns.Add("SrNo")
            dt.Columns.Add("Image")
            For Each p In products
                dt.Rows.Add(p.ProductCode, p.ProductName, p.DepartmentCode, p.SupplierPacking, p.SupplierPackingDetails,
                            p.ProductUnit, p.TagPrice, p.Product_VAT_Code, p.Product_Cost_Price, p.Product_Last_Cost_Price,
                            p.Product_Margin, p.Product_Selling_Price, p.Min_Qty, p.ReOrd_Level, p.Qty_to_Order, p.SupplierCode,
                            p.Weight, p.Product_Qty, p.isAlternetUnit, p.AlternetUnit, p.UnitValue, p.AlternetUnitValue, p.HSCode,
                            p.HSDesc, p.isActive, p.isStockItem, p.Remark, p.SrNo, p.img_src)
            Next
            DgvOnlineProducts.DataSource = dt
        Catch ex As Exception
            MessageBox.Show("Error fetching remote products: " & ex.Message)
        Finally
            BtnRefreshOnline.Enabled = True
        End Try
    End Sub

    Private Sub DgvOnlineProducts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvOnlineProducts.CellClick
        ' Exit if header row clicked
        If e.RowIndex < 0 Then Return
        Dim row As DataGridViewRow = DgvOnlineProducts.Rows(e.RowIndex)
        ' Populate fields
        TxtProductCode.Text = row.Cells("ProductCd").Value?.ToString()
        TxtProductName.Text = row.Cells("ProductName").Value?.ToString()
        TxtDepartmentCode.Text = row.Cells("DepartmentCd").Value?.ToString()
        TxtSupplierPacking.Text = row.Cells("SupplierPacking").Value?.ToString()
        TxtSupplierPackingDetails.Text = row.Cells("SupplierPackingDetails").Value?.ToString()
        TxtProductUnit.Text = row.Cells("ProductUnit").Value?.ToString()
        TxtTagPrice.Text = row.Cells("TagPrice").Value?.ToString()
        TxtVatCode.Text = row.Cells("VATCode").Value?.ToString()
        TxtCostPrice.Text = row.Cells("CostPrice").Value?.ToString()
        TxtProductLastCost.Text = row.Cells("LastPrice").Value?.ToString()
        TxtProductMargin.Text = row.Cells("ProductMargin").Value?.ToString()
        TxtSellingPrice.Text = row.Cells("SellingPrice").Value?.ToString()
        TxtMinQuantity.Text = row.Cells("MinQty").Value?.ToString()
        TxtReorderLevel.Text = row.Cells("ReorderLevel").Value?.ToString()
        TxtQuantityToReorder.Text = row.Cells("QuantityToOrder").Value?.ToString()
        TxtSupplierCode.Text = row.Cells("SupplierCd").Value?.ToString()
        TxtWeight.Text = row.Cells("Weight").Value?.ToString()
        TxtProductQuantity.Text = row.Cells("ProductQty").Value?.ToString()
        ChkIsAlternateUnit.Checked = row.Cells("isAlternetUnit").Value IsNot Nothing AndAlso
                            (row.Cells("isAlternetUnit").Value.ToString() = "1" OrElse
                            row.Cells("isAlternetUnit").Value.ToString().ToLower() = "true")
        TxtAlternateUnit.Text = row.Cells("AlternetUnit").Value?.ToString()
        TxtUnitValue.Text = row.Cells("UnitValue").Value?.ToString()
        TxtAlternateUnitValue.Text = row.Cells("AlternetUnitValue").Value?.ToString()
        TxtHsnCode.Text = row.Cells("HSCode").Value?.ToString()
        TxtHsnDesc.Text = row.Cells("HSDesc").Value?.ToString()
        ChkIsStockItem.Checked = row.Cells("isStockItem").Value IsNot Nothing AndAlso
                            (row.Cells("isStockItem").Value.ToString() = "1" OrElse
                            row.Cells("isStockItem").Value.ToString().ToLower() = "true")
        ChkIsActive.Checked = row.Cells("isActive").Value IsNot Nothing AndAlso
                        (row.Cells("isActive").Value.ToString() = "1" OrElse
                        row.Cells("isActive").Value.ToString().ToLower() = "true")
        TxtRemark.Text = row.Cells("Remark").Value?.ToString()
        TxtSrNo.Text = row.Cells("SrNo").Value?.ToString()
        ' Load image if exists
        Dim serverUrl As String = "http://197.248.220.180:80"
        ' Get the image file name from the row
        Dim imgFile As String = If(row.Cells("Image").Value IsNot Nothing, row.Cells("Image").Value.ToString(), "")
        ' Load the image if available
        If Not String.IsNullOrEmpty(imgFile) Then
            Try
                PcbPreview.Load($"{serverUrl}/salesman-backend/assets/uploads/images/{imgFile}")
            Catch ex As Exception
                PcbPreview.Image = Nothing
            End Try
        Else
            PcbPreview.Image = Nothing
        End If
    End Sub

    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click

    End Sub

    Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles BtnBrowse.Click

    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click

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

    'online tab helpers
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
            Dim url As String = $"http://197.248.220.180/salesman-backend/api/products.php?action=sync&lastSync={lastSync}&limit={limit}&offset={offset}"
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
