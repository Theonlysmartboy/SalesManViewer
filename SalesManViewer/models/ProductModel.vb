Namespace models
    Public Class RemoteProduct
        Public Property ProductCode As String
        Public Property ProductName As String
        Public Property DepartmentCode As String
        Public Property SupplierPacking As String
        Public Property SupplierPackingDetails As String
        Public Property ProductUnit As String
        Public Property TagPrice As String
        Public Property Product_VAT_Code As String
        Public Property Product_Cost_Price As String
        Public Property Product_Last_Cost_Price As String
        Public Property Product_Margin As String
        Public Property Product_Selling_Price As String
        Public Property Min_Qty As String
        Public Property ReOrd_Level As String
        Public Property Qty_to_Order As String
        Public Property SupplierCode As String
        Public Property Weight As String
        Public Property Product_Qty As String
        Public Property isAlternetUnit As Integer
        Public Property AlternetUnit As String
        Public Property UnitValue As String
        Public Property AlternetUnitValue As String
        Public Property HSCode As String
        Public Property HSDesc As String
        Public Property isActive As Integer
        Public Property isStockItem As Integer
        Public Property Remark As String
        Public Property SrNo As String
        Public Property img_src As String
    End Class

    Public Class ProductApiResponse
        Public Property success As Boolean
        Public Property count As Integer
        Public Property data As List(Of RemoteProduct)
    End Class
End Namespace
