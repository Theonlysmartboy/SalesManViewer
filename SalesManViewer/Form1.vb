Imports System.IO
Imports System.Net.Http
Imports System.Security.Permissions
Imports System.Text
Imports Newtonsoft.Json
Imports SalesManViewer.helpers
Imports SalesManViewer.models
Imports SalesManViewer.repositories

<PermissionSet(SecurityAction.Demand, Name:="FullTrust")>
<System.Runtime.InteropServices.ComVisible(True)>
Public Class Form1
    Private repo As New ProductRepository()
    Private orderRepo As New OrderRepository()
    Private selectedImagePath As String = ""
    Private OriginalTables As New Dictionary(Of DataGridView, DataTable)
    Dim serverUrl As String = "http://197.248.220.180/salesman-backend"

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Local
        SetPlaceholder(TxtSearchLocalProducts, "Start typing to search...")
        AddHandler TxtSearchLocalProducts.Enter, AddressOf TextBox_Enter
        AddHandler TxtSearchLocalProducts.Leave, AddressOf TextBox_Leave
        LoadLocalProducts()
        AddCheckboxColumntoLocalDataGrid()
        'Online
        SetPlaceholder(TxtSearchOnlineProducts, "Start typing to search...")
        AddHandler TxtSearchOnlineProducts.Enter, AddressOf TextBox_Enter
        AddHandler TxtSearchOnlineProducts.Leave, AddressOf TextBox_Leave
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
        'Orders
        SetPlaceholder(TxtSearchOrders, "Start typing to search...")
        AddHandler TxtSearchOrders.Enter, AddressOf TextBox_Enter
        AddHandler TxtSearchOrders.Leave, AddressOf TextBox_Leave
        LoadOrders()
    End Sub

    'Local tab events
    Private Sub TxtSearchLocalProducts_TextChanged(sender As Object, e As EventArgs) Handles TxtSearchLocalProducts.TextChanged
        If TxtSearchLocalProducts.ForeColor = Color.Gray Then Exit Sub
        FilterGrid(DgvLocalProducts, TxtSearchLocalProducts.Text)
    End Sub

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

    Private Sub DgvLocalProducts_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles DgvLocalProducts.CurrentCellDirtyStateChanged
        If TypeOf DgvLocalProducts.CurrentCell Is DataGridViewCheckBoxCell Then
            DgvLocalProducts.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    'local tab helpers
    Private Sub LoadLocalProducts()
        Dim dt = repo.GetLocalProducts()
        OriginalTables(DgvLocalProducts) = dt.Copy()
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
        Dim url As String = $"{serverUrl}/products.php?action=bulk-create"
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

    'online tab events
    Private Sub TxtSearchOnlineProducts_TextChanged(sender As Object, e As EventArgs) Handles TxtSearchOnlineProducts.TextChanged
        If TxtSearchOnlineProducts.ForeColor = Color.Gray Then Exit Sub
        FilterGrid(DgvOnlineProducts, TxtSearchOnlineProducts.Text)
    End Sub

    Private Async Sub BtnRefreshOnline_Click(sender As Object, e As EventArgs) Handles BtnRefreshOnline.Click
        BtnRefreshOnline.Enabled = False
        Try
            ' Always sync from 1 year ago
            Dim lastSync As String = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd HH:mm:ss")
            Dim products As List(Of RemoteProduct) = Await FetchAllRemoteProducts(lastSync)
            ' Populate DataGridView
            Dim dt As New DataTable()
            dt.Columns.Add("Select", GetType(Boolean)) 'checkbox column
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
                dt.Rows.Add(False, p.ProductCode, p.ProductName, p.DepartmentCode, p.SupplierPacking, p.SupplierPackingDetails,
                            p.ProductUnit, p.TagPrice, p.Product_VAT_Code, p.Product_Cost_Price, p.Product_Last_Cost_Price,
                            p.Product_Margin, p.Product_Selling_Price, p.Min_Qty, p.ReOrd_Level, p.Qty_to_Order, p.SupplierCode,
                            p.Weight, p.Product_Qty, p.isAlternetUnit, p.AlternetUnit, p.UnitValue, p.AlternetUnitValue, p.HSCode,
                            p.HSDesc, p.isActive, p.isStockItem, p.Remark, p.SrNo, p.img_src)
            Next
            OriginalTables(DgvOnlineProducts) = dt.Copy()
            DgvOnlineProducts.DataSource = dt
            DgvOnlineProducts.EditMode = DataGridViewEditMode.EditOnEnter
            DgvOnlineProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
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
        ' Get the image file name from the row
        Dim imgFile As String = If(row.Cells("Image").Value IsNot Nothing, row.Cells("Image").Value.ToString(), "")
        ' Load the image if available
        If Not String.IsNullOrEmpty(imgFile) Then
            Try
                PcbPreview.Load($"{serverUrl}/assets/uploads/images/{imgFile}")
            Catch ex As Exception
                PcbPreview.Image = Nothing
            End Try
        Else
            PcbPreview.Image = Nothing
        End If
    End Sub

    Private Sub BtnNew_Click(sender As Object, e As EventArgs) Handles BtnNew.Click
        For Each ctrl As Control In Me.Controls
            If TypeOf ctrl Is TextBox Then
                ctrl.Text = ""
            End If
        Next
        ChkIsActive.Checked = False
        ChkIsStockItem.Checked = False
        ChkIsAlternateUnit.Checked = False
        PcbPreview.Image = Nothing
    End Sub

    Private Sub BtnBrowse_Click(sender As Object, e As EventArgs) Handles BtnBrowse.Click
        Dim ofd As New OpenFileDialog()
        ofd.Title = "Select Product Image"
        ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
        If ofd.ShowDialog() = DialogResult.OK Then
            selectedImagePath = ofd.FileName
            PcbPreview.Image = Image.FromFile(selectedImagePath)
            PcbPreview.SizeMode = PictureBoxSizeMode.Zoom
        End If
    End Sub

    Private Async Function BtnSave_Click(sender As Object, e As EventArgs) As Task Handles BtnSave.Click
        Try
            Dim url As String = $"{serverUrl}/api/products.php?action=create"
            Using client As New HttpClient()
                Dim content As New MultipartFormDataContent()
                'Product fields
                content.Add(New StringContent(TxtProductCode.Text), "ProductCode")
                content.Add(New StringContent(TxtProductName.Text), "ProductName")
                content.Add(New StringContent(TxtDepartmentCode.Text), "DepartmentCode")
                content.Add(New StringContent(TxtSupplierPacking.Text), "SupplierPacking")
                content.Add(New StringContent(TxtSupplierPackingDetails.Text), "SupplierPackingDetails")
                content.Add(New StringContent(TxtProductUnit.Text), "ProductUnit")
                content.Add(New StringContent(TxtTagPrice.Text), "TagPrice")
                content.Add(New StringContent(TxtVatCode.Text), "Product_VAT_Code")
                content.Add(New StringContent(TxtCostPrice.Text), "Product_Cost_Price")
                content.Add(New StringContent(TxtProductLastCost.Text), "Product_Last_Cost_Price")
                content.Add(New StringContent(TxtProductMargin.Text), "Product_Margin")
                content.Add(New StringContent(TxtSellingPrice.Text), "Product_Selling_Price")
                content.Add(New StringContent(TxtMinQuantity.Text), "Min_Qty")
                content.Add(New StringContent(TxtReorderLevel.Text), "ReOrd_Level")
                content.Add(New StringContent(TxtQuantityToReorder.Text), "Qty_to_Order")
                content.Add(New StringContent(TxtSupplierCode.Text), "SupplierCode")
                content.Add(New StringContent(TxtWeight.Text), "Weight")
                content.Add(New StringContent(TxtProductQuantity.Text), "Product_Qty")
                content.Add(New StringContent(ChkIsAlternateUnit.Checked.ToString()), "isAlternetUnit")
                content.Add(New StringContent(TxtAlternateUnit.Text), "AlternetUnit")
                content.Add(New StringContent(TxtUnitValue.Text), "UnitValue")
                content.Add(New StringContent(TxtAlternateUnitValue.Text), "AlternetUnitValue")
                content.Add(New StringContent(TxtHsnCode.Text), "HSCode")
                content.Add(New StringContent(TxtHsnDesc.Text), "HSDesc")
                content.Add(New StringContent(ChkIsActive.Checked.ToString()), "isActive")
                content.Add(New StringContent(ChkIsStockItem.Checked.ToString()), "isStockItem")
                content.Add(New StringContent(TxtRemark.Text), "Remark")
                content.Add(New StringContent(TxtSrNo.Text), "SrNo")
                'Image upload
                If Not String.IsNullOrEmpty(selectedImagePath) Then
                    Dim fileBytes = File.ReadAllBytes(selectedImagePath)
                    Dim fileContent As New ByteArrayContent(fileBytes)
                    fileContent.Headers.ContentType =
                    New Headers.MediaTypeHeaderValue("image/jpeg")
                    content.Add(fileContent, "img_src", Path.GetFileName(selectedImagePath))
                End If
                Dim response = Await client.PostAsync(url, content)
                Dim result = Await response.Content.ReadAsStringAsync()
                MessageBox.Show(result)
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function

    Private Sub DgvOnlineProducts_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles DgvOnlineProducts.CurrentCellDirtyStateChanged
        If TypeOf DgvOnlineProducts.CurrentCell Is DataGridViewCheckBoxCell Then
            DgvOnlineProducts.CommitEdit(DataGridViewDataErrorContexts.Commit)
        End If
    End Sub

    Private Async Sub BtnDeleteSelected_Click(sender As Object, e As EventArgs) Handles BtnDeleteSelected.Click
        Try
            Dim codes As New List(Of String)
            For Each row As DataGridViewRow In DgvOnlineProducts.Rows
                If Convert.ToBoolean(row.Cells("Select").Value) = True Then
                    codes.Add(row.Cells("ProductCd").Value.ToString())
                End If
            Next
            If codes.Count = 0 Then
                MessageBox.Show("No products selected.")
                Return
            End If
            Dim confirm = MessageBox.Show($"Delete {codes.Count} products?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If confirm <> DialogResult.Yes Then Return
            Dim payload = New Dictionary(Of String, Object)
            payload("codes") = codes
            Dim json As String = JsonConvert.SerializeObject(payload)
            Dim url As String = $"{serverUrl}/api/products.php?action=delete-multiple"
            Using client As New HttpClient()
                Dim content As New StringContent(json, Encoding.UTF8, "application/json")
                Dim response = Await client.PostAsync(url, content)
                Dim result = Await response.Content.ReadAsStringAsync()
                MessageBox.Show(result)
            End Using
            BtnRefreshOnline.PerformClick()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Sub BtnDeleteSingle_Click(sender As Object, e As EventArgs) Handles BtnDeleteSingle.Click
        Try
            If TxtProductCode.Text = "" Then
                MessageBox.Show("Please select a product to delete.")
                Return
            End If
            Dim code As String = TxtProductCode.Text.Trim()
            Dim confirm = MessageBox.Show($"Delete product {code} ?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If confirm <> DialogResult.Yes Then Return
            Dim url As String = $"{serverUrl}/api/products.php?action=delete&code={code}"
            Using client As New HttpClient()
                Dim response = Await client.DeleteAsync(url)
                Dim result = Await response.Content.ReadAsStringAsync()
                MessageBox.Show(result)
            End Using
            BtnNew.PerformClick()
            BtnRefreshOnline.PerformClick()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    'online tab helpers
    Private Async Function FetchAllRemoteProducts(lastSync As String) As Task(Of List(Of RemoteProduct))
        Dim allProducts As New List(Of RemoteProduct)
        Dim offset As Integer = 0
        Dim limit As Integer = 20
        Dim fetched As Integer
        Do
            Dim chunk As List(Of RemoteProduct) =
            Await GetRemoteProductsAsync(lastSync, limit, offset)
            fetched = chunk.Count
            If fetched > 0 Then
                allProducts.AddRange(chunk)
                offset += fetched
            End If
        Loop While fetched > 0
        Return allProducts
    End Function

    Private Async Function GetRemoteProductsAsync(lastSync As String, limit As Integer, offset As Integer) As Task(Of List(Of RemoteProduct))
        Dim products As New List(Of RemoteProduct)
        Using client As New HttpClient()
            Dim url As String = $"{serverUrl}/api/products.php?action=sync&lastSync={lastSync}&limit={limit}&offset={offset}"
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

    'Salesmen events
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

    Private Async Sub BtnDeleteSelectedSm_Click(sender As Object, e As EventArgs) Handles BtnDeleteSelectedSm.Click
        Try
            Dim ids As New List(Of Integer)
            For Each row As DataGridViewRow In DgvSalesMen.Rows
                If Convert.ToBoolean(row.Cells("Select").Value) = True Then
                    ids.Add(Convert.ToInt32(row.Cells("id").Value))
                End If
            Next
            If ids.Count = 0 Then
                MessageBox.Show("No salesmen selected.")
                Return
            End If
            Dim confirm = MessageBox.Show($"Delete {ids.Count} salesman(s)?", "Confirm", MessageBoxButtons.YesNo)
            If confirm <> DialogResult.Yes Then Return
            Using client As New HttpClient()
                Dim url As String = ""
                Dim payload As String = ""
                If ids.Count = 1 Then
                    url = $"{serverUrl}/api/auth.php?action=delete-user"
                    payload = JsonConvert.SerializeObject(New With {.userId = ids(0)})
                Else
                    url = $"{serverUrl}/api/auth.php?action=bulk-delete"
                    payload = JsonConvert.SerializeObject(New With {.userIds = ids})
                End If
                Dim content As New StringContent(payload, Encoding.UTF8, "application/json")
                Dim response = Await client.PostAsync(url, content)
                Dim result = Await response.Content.ReadAsStringAsync()
                MessageBox.Show(result)
            End Using
            BtnRefreshSm.PerformClick()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Sub DgvSalesMen_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvSalesMen.CellClick
        If e.RowIndex < 0 Then Return
        Dim row = DgvSalesMen.Rows(e.RowIndex)
        Dim userId = row.Cells("id").Value.ToString()
        Await LoadTracking(userId)
    End Sub

    Private Sub BtnNewUpdateSm_Click(sender As Object, e As EventArgs) Handles BtnNewUpdateSm.Click
        Try
            Dim selectedRows As New List(Of DataGridViewRow)
            ' Get checked rows
            For Each row As DataGridViewRow In DgvSalesMen.Rows
                If Convert.ToBoolean(row.Cells("Select").Value) = True Then
                    selectedRows.Add(row)
                End If
            Next
            ' More than one selected → ERROR
            If selectedRows.Count > 1 Then
                MessageBox.Show("Please select only one salesman to edit.", "Warning")
                Return
            End If
            ' Open form
            Dim frm As New SalesMan()
            ' ONE selected → EDIT MODE
            If selectedRows.Count = 1 Then
                Dim row = selectedRows(0)
                frm.IsEditMode = True
                frm.UserId = Convert.ToInt32(row.Cells("id").Value)
                ' Prefill fields
                frm.TxtUserName.Text = row.Cells("User Name").Value?.ToString()
                frm.TxtFullName.Text = row.Cells("Full Name").Value?.ToString()
                frm.TxtEmail.Text = row.Cells("Email").Value?.ToString()
                frm.TxtPhoneNo.Text = row.Cells("Phone").Value?.ToString()
                frm.ChkIsActive.Checked = row.Cells("Is Active").Value?.ToString()
                frm.BtnSubmit.Text = "Update"
            Else
                ' NONE selected → CREATE MODE
                frm.IsEditMode = False
                frm.UserId = 0
                frm.TxtUserName.Text = ""
                frm.TxtFullName.Text = ""
                frm.TxtEmail.Text = ""
                frm.TxtPhoneNo.Text = ""
                frm.BtnSubmit.Text = "Save"
            End If
            If frm.ShowDialog() = DialogResult.OK Then
                BtnRefreshSm.PerformClick()
            End If
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    'salesman helpers
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

    'Order events
    Private Sub BtnFilter_Click(sender As Object, e As EventArgs) Handles BtnFilter.Click
        Dim filters As New Dictionary(Of String, String)
        If CmbCustomer.SelectedItem IsNot Nothing Then
            filters("CustomerCode") = CmbCustomer.SelectedValue.ToString()
        End If
        If CmbSalesman.SelectedItem IsNot Nothing Then
            filters("SalesmanCode") = CmbSalesman.SelectedValue.ToString()
        End If
        filters("DateFrom") = DtpFrom.Value.ToString("yyyy-MM-dd")
        filters("DateTo") = DtpTo.Value.ToString("yyyy-MM-dd")
        LoadOrders(filters)
    End Sub

    Private Sub TxtSearchOrders_TextChanged(sender As Object, e As EventArgs) Handles TxtSearchOrders.TextChanged
        If TxtSearchOrders.ForeColor = Color.Gray Then Exit Sub
        FilterGrid(DgvOrders, TxtSearchOrders.Text)
    End Sub

    Private Async Sub BtnDeleteSelectedOrders_Click(sender As Object, e As EventArgs) Handles BtnDeleteSelectedOrders.Click
        Try
            Dim ids As New List(Of String)
            For Each row As DataGridViewRow In DgvOrders.Rows
                If Convert.ToBoolean(row.Cells("Select").Value) = True Then
                    ids.Add(row.Cells("OrderID").Value.ToString())
                End If
            Next
            If ids.Count = 0 Then
                MessageBox.Show("No orders selected")
                Return
            End If
            Await orderRepo.DeleteOrders(ids)
            MessageBox.Show("Deleted successfully")
            LoadOrders()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Sub DgvOrders_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DgvOrders.CellClick
        If e.RowIndex < 0 Then Return
        Dim orderId = DgvOrders.Rows(e.RowIndex).Cells("OrderID").Value.ToString()
        Await LoadOrderLines(orderId)
    End Sub

    'Order helpers
    Private Async Sub LoadOrders(Optional filters As Dictionary(Of String, String) = Nothing)
        Try
            Dim dt = Await orderRepo.GetOrders(If(filters, New Dictionary(Of String, String)))
            DgvOrders.DataSource = dt
            OriginalTables(DgvOrders) = dt
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Async Function LoadOrderLines(orderId As String) As Task
        Try
            Using client As New HttpClient()
                Dim url = $"{serverUrl}/api/orders.php?action=lines&orderId={orderId}"
                Dim response = Await client.GetStringAsync(url)
                Dim json = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(response)
                Dim dt As New DataTable()
                If json.ContainsKey("data") Then
                    Dim list = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, Object)))(json("data").ToString())
                    If list.Count > 0 Then
                        For Each key In list(0).Keys
                            dt.Columns.Add(key)
                        Next
                        For Each item In list
                            Dim row = dt.NewRow()
                            For Each key In item.Keys
                                row(key) = item(key)
                            Next
                            dt.Rows.Add(row)
                        Next
                    End If
                End If
                DgvOrderLines.DataSource = dt
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Function

    'Global helpers
    Private Sub toggleControls(status As Boolean, button As Button, text As String)
        btnRefresh.Enabled = status
        BtnUploadSelected.Enabled = status
        BtnUploadAll.Enabled = status
        BtnSelectAll.Enabled = status
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
