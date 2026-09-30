Imports SalesManViewer.Helpers.Database
Imports SalesManViewer.Models

Namespace Repositories
    Public Class ProductRepository

        Private ReadOnly _db As DbHelper

        Public Sub New()
            _db = New DbHelper(GlobalConnectionString.GetConnectionString())
        End Sub

        ' FAST GRID LOAD — returns a DataTable ready for binding
        Public Function GetLocalProducts() As DataTable
            Const sql As String = "SELECT ProductCode, ProductName, DepartmentCode, SupplierPackingDetails," &
                                "ProductUnit, TagPrice, Product_VAT_Code, Product_Cost_Price, Product_Last_Cost_Price, " &
                                "Product_Margin, Product_Selling_Price, SalesmanPrice1, SalesmanPrice2, SalesmanPrice3, " &
                                "DistanceStepKM, SalesmanExtra, MaxDeliveryCharge, Min_Qty, ReOrd_Level, Qty_to_Order, " &
                                "SupplierCode, Weight, Product_Qty, isAlternetUnit, AlternetUnit, UnitValue, AlternetUnitValue, " &
                                "HSCode, HSDesc, isActive, isStockItem, Remark, Created, CreatedBy, Modified, ModifiedBy, " &
                                "SrNo FROM productmaster ORDER BY ProductName"
            Return _db.ExecuteSelect(sql)
        End Function

        ' TYPED LIST — full objects
        Public Function LoadProducts() As List(Of Product)
            Const sql As String = "SELECT * FROM productmaster"
            Dim dt = _db.ExecuteSelect(sql)
            Return MapProducts(dt)
        End Function

        Public Function LoadAlternates() As List(Of AlternateUnit)
            Const sql As String = "SELECT * FROM alternetunitmaster"
            Dim dt = _db.ExecuteSelect(sql)
            Return MapAlternates(dt)
        End Function

        ' MAPPERS
        Private Function MapProducts(dt As DataTable) As List(Of Product)
            Dim list As New List(Of Product)()
            For Each r As DataRow In dt.Rows
                Dim p As New Product With {
                    .SrNo = Convert.ToInt32(r("SrNo")),
                    .ProductCode = ToStr(r("ProductCode")),
                    .ProductName = ToStr(r("ProductName")),
                    .DepartmentCode = ToStr(r("DepartmentCode")),
                    .SupplierPacking = ToDecimal(r("SupplierPacking"), 0D),
                    .SupplierPackingDetails = ToNullableStr(r("SupplierPackingDetails")),
                    .ProductUnit = ToStr(r("ProductUnit")),
                    .TagPrice = ToDecimal(r("TagPrice"), 0D),
                    .Product_VAT_Code = ToStr(r("Product_VAT_Code")),
                    .Product_Cost_Price = ToDecimal(r("Product_Cost_Price"), 0D),
                    .Product_Last_Cost_Price = ToDecimal(r("Product_Last_Cost_Price"), 0D),
                    .Product_Margin = ToDecimal(r("Product_Margin"), 0D),
                    .Product_Selling_Price = ToDecimal(r("Product_Selling_Price"), 0D),
                    .SalesmanPrice1 = ToNullableDecimal(r("SalesmanPrice1")),
                    .SalesmanPrice2 = ToNullableDecimal(r("SalesmanPrice2")),
                    .SalesmanPrice3 = ToNullableDecimal(r("SalesmanPrice3")),
                    .DistanceStepKM = ToNullableDecimal(r("DistanceStepKM")),
                    .SalesmanExtra = ToNullableDecimal(r("SalesmanExtra")),
                    .MaxDeliveryCharge = ToNullableDecimal(r("MaxDeliveryCharge")),
                    .Min_Qty = ToDecimal(r("Min_Qty"), 0D),
                    .ReOrd_Level = ToDecimal(r("ReOrd_Level"), 0D),
                    .Qty_to_Order = ToDecimal(r("Qty_to_Order"), 0D),
                    .SupplierCode = ToNullableStr(r("SupplierCode")),
                    .Weight = ToDecimal(r("Weight"), 0D),
                    .Product_Qty = ToDecimal(r("Product_Qty"), 0D),
                    .isAlternetUnit = ToBool(r("isAlternetUnit")),
                    .AlternetUnit = ToNullableStr(r("AlternetUnit")),
                    .UnitValue = ToDecimal(r("UnitValue"), 0D),
                    .AlternetUnitValue = ToDecimal(r("AlternetUnitValue"), 0D),
                    .HSCode = ToNullableStr(r("HSCode")),
                    .HSDesc = ToNullableStr(r("HSDesc")),
                    .isActive = ToBool(r("isActive")),
                    .isStockItem = ToBool(r("isStockItem")),
                    .Remark = ToNullableStr(r("Remark")),
                    .img_src = If(dt.Columns.Contains("img_src"), ToNullableStr(r("img_src")), Nothing)
                }
                list.Add(p)
            Next
            Return list
        End Function

        Private Function MapAlternates(dt As DataTable) As List(Of AlternateUnit)
            Dim list As New List(Of AlternateUnit)()
            For Each r As DataRow In dt.Rows
                Dim a As New AlternateUnit With {
                    .SrNo = Convert.ToInt32(r("SrNo")),
                    .ProductCode = ToStr(r("ProductCode")),
                    .AlternetUnit = ToStr(r("AlternetUnit")),
                    .AlternetQty = ToDecimal(r("AlternetQty"), 0D),
                    .PrimaryQty = ToDecimal(r("PrimaryQty"), 0D),
                    .AlternetCostPrice = ToDecimal(r("AlternetCostPrice"), 0D),
                    .AlternetMrgn = ToDecimal(r("AlternetMrgn"), 0D),
                    .AlternetTradePrice = ToDecimal(r("AlternetTradePrice"), 0D),
                    .AlternetPrice = ToDecimal(r("AlternetPrice"), 0D),
                    .DistanceStepKM = ToNullableDecimal(r("DistanceStepKM")),
                    .SalesmanExtra = ToNullableDecimal(r("SalesmanExtra")),
                    .MaxDeliveryCharge = ToNullableDecimal(r("MaxDeliveryCharge")),
                    .AlternetWeight = ToDecimal(r("AlternetWeight"), 0D)
                }
                list.Add(a)
            Next
            Return list
        End Function

        ' SAFE CONVERSION HELPERS
        Private Shared Function ToStr(v As Object) As String
            If v Is Nothing OrElse v Is DBNull.Value Then Return ""
            Return v.ToString()
        End Function

        Private Shared Function ToNullableStr(v As Object) As String
            If v Is Nothing OrElse v Is DBNull.Value Then Return Nothing
            Return v.ToString()
        End Function

        Private Shared Function ToDecimal(v As Object, fallback As Decimal) As Decimal
            If v Is Nothing OrElse v Is DBNull.Value Then Return fallback
            Dim d As Decimal
            If Decimal.TryParse(v.ToString(), d) Then Return d
            Return fallback
        End Function

        Private Shared Function ToNullableDecimal(v As Object) As Decimal?
            If v Is Nothing OrElse v Is DBNull.Value Then Return Nothing
            Dim d As Decimal
            If Decimal.TryParse(v.ToString(), d) Then Return d
            Return Nothing
        End Function

        Private Shared Function ToBool(v As Object) As Boolean
            If v Is Nothing OrElse v Is DBNull.Value Then Return False
            If TypeOf v Is Boolean Then Return CBool(v)
            Dim i As Integer
            If Integer.TryParse(v.ToString(), i) Then Return i <> 0
            Dim b As Boolean
            If Boolean.TryParse(v.ToString(), b) Then Return b
            Return False
        End Function
    End Class
End Namespace