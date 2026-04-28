Imports MySql.Data.MySqlClient
Imports SalesManViewer.helpers
Imports SalesManViewer.models

Namespace repositories
    Public Class ProductRepository
        Public Function GetLocalProducts() As DataTable
            Dim dt As New DataTable()
            Using conn = DatabaseHelper.GetConnection()
                Dim sql As String = "SELECT ProductCode, ProductName, DepartmentCode, SupplierPackingDetails," &
                    "ProductUnit, TagPrice, Product_VAT_Code, Product_Cost_Price, Product_Last_Cost_Price, " &
                    "Product_Margin, Product_Selling_Price, DistanceStepKM, SalesmanExtra, MaxDeliveryCharge, " &
                    "Min_Qty, ReOrd_Level, Qty_to_Order, SupplierCode, Weight, Product_Qty, isAlternetUnit, " &
                    "AlternetUnit, UnitValue, AlternetUnitValue, HSCode, HSDesc, isActive, isStockItem, Remark, " &
                    "Created, CreatedBy, Modified, ModifiedBy, SrNo FROM productmaster ORDER BY ProductName"
                Using cmd As New MySqlCommand(sql, conn)
                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
            Return dt
        End Function

        ' =========================
        ' LOAD PRODUCTS
        ' =========================
        Public Function LoadProducts() As List(Of Product)
            Dim list As New List(Of Product)
            Using conn = DatabaseHelper.GetConnection()
                Dim sql As String = "SELECT * FROM productmaster"
                Using cmd As New MySqlCommand(sql, conn)
                    Using r = cmd.ExecuteReader()
                        While r.Read()
                            Dim p As New Product With {
                                .SrNo = Convert.ToInt32(r("SrNo")),
                                .ProductCode = r("ProductCode").ToString(),
                                .ProductName = r("ProductName").ToString(),
                                .DepartmentCode = r("DepartmentCode").ToString(),
                                .SupplierPacking = Convert.ToDecimal(r("SupplierPacking")),
                                .SupplierPackingDetails = If(IsDBNull(r("SupplierPackingDetails")), Nothing, r("SupplierPackingDetails").ToString()),
                                .ProductUnit = r("ProductUnit").ToString(),
                                .TagPrice = Convert.ToDecimal(r("TagPrice")),
                                .Product_VAT_Code = r("Product_VAT_Code").ToString(),
                                .Product_Cost_Price = Convert.ToDecimal(r("Product_Cost_Price")),
                                .Product_Last_Cost_Price = Convert.ToDecimal(r("Product_Last_Cost_Price")),
                                .Product_Margin = Convert.ToDecimal(r("Product_Margin")),
                                .Product_Selling_Price = Convert.ToDecimal(r("Product_Selling_Price")),
                                .DistanceStepKM = If(IsDBNull(r("DistanceStepKM")), Nothing, Convert.ToDecimal(r("DistanceStepKM"))),
                                .SalesmanExtra = If(IsDBNull(r("SalesmanExtra")), Nothing, Convert.ToDecimal(r("SalesmanExtra"))),
                                .MaxDeliveryCharge = If(IsDBNull(r("MaxDeliveryCharge")), Nothing, Convert.ToDecimal(r("MaxDeliveryCharge"))),
                                .Min_Qty = Convert.ToDecimal(r("Min_Qty")),
                                .ReOrd_Level = Convert.ToDecimal(r("ReOrd_Level")),
                                .Qty_to_Order = Convert.ToDecimal(r("Qty_to_Order")),
                                .SupplierCode = If(IsDBNull(r("SupplierCode")), Nothing, r("SupplierCode").ToString()),
                                .Weight = Convert.ToDecimal(r("Weight")),
                                .Product_Qty = Convert.ToDecimal(r("Product_Qty")),
                                .isAlternetUnit = Convert.ToBoolean(r("isAlternetUnit")),
                                .AlternetUnit = If(IsDBNull(r("AlternetUnit")), Nothing, r("AlternetUnit").ToString()),
                                .UnitValue = Convert.ToDecimal(r("UnitValue")),
                                .AlternetUnitValue = Convert.ToDecimal(r("AlternetUnitValue")),
                                .HSCode = If(IsDBNull(r("HSCode")), Nothing, r("HSCode").ToString()),
                                .HSDesc = If(IsDBNull(r("HSDesc")), Nothing, r("HSDesc").ToString()),
                                .isActive = Convert.ToBoolean(r("isActive")),
                                .isStockItem = Convert.ToBoolean(r("isStockItem")),
                                .Remark = If(IsDBNull(r("Remark")), Nothing, r("Remark").ToString()),
                                .img_src = If(IsDBNull(r("img_src")), Nothing, r("img_src").ToString())
                            }
                            list.Add(p)
                        End While
                    End Using
                End Using
            End Using
            Return list
        End Function

        ' =========================
        ' LOAD ALTERNATE UNITS
        ' =========================
        Public Function LoadAlternates() As List(Of AlternateUnit)
            Dim list As New List(Of AlternateUnit)
            Using conn = DatabaseHelper.GetConnection()
                Dim sql As String = "SELECT * FROM alternetunitmaster"
                Using cmd As New MySqlCommand(sql, conn)
                    Using r = cmd.ExecuteReader()
                        While r.Read()
                            Dim a As New AlternateUnit With {
                                .SrNo = Convert.ToInt32(r("SrNo")),
                                .ProductCode = r("ProductCode").ToString(),
                                .AlternetUnit = r("AlternetUnit").ToString(),
                                .AlternetQty = Convert.ToDecimal(r("AlternetQty")),
                                .PrimaryQty = Convert.ToDecimal(r("PrimaryQty")),
                                .AlternetCostPrice = Convert.ToDecimal(r("AlternetCostPrice")),
                                .AlternetMrgn = Convert.ToDecimal(r("AlternetMrgn")),
                                .AlternetTradePrice = Convert.ToDecimal(r("AlternetTradePrice")),
                                .AlternetPrice = Convert.ToDecimal(r("AlternetPrice")),
                                .DistanceStepKM = If(IsDBNull(r("DistanceStepKM")), Nothing, Convert.ToDecimal(r("DistanceStepKM"))),
                                .SalesmanExtra = If(IsDBNull(r("SalesmanExtra")), Nothing, Convert.ToDecimal(r("SalesmanExtra"))),
                                .MaxDeliveryCharge = If(IsDBNull(r("MaxDeliveryCharge")), Nothing, Convert.ToDecimal(r("MaxDeliveryCharge"))),
                                .AlternetWeight = Convert.ToDecimal(r("AlternetWeight"))
                            }
                            list.Add(a)
                        End While
                    End Using
                End Using
            End Using
            Return list
        End Function
    End Class
End Namespace
