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

    Public Class Product
        Public Property ProductCode As String
        Public Property ProductName As String
        Public Property DepartmentCode As String
        Public Property SupplierPacking As Decimal
        Public Property SupplierPackingDetails As String
        Public Property ProductUnit As String
        Public Property TagPrice As Decimal
        Public Property Product_VAT_Code As String
        Public Property Product_Cost_Price As Decimal
        Public Property Product_Last_Cost_Price As Decimal
        Public Property Product_Margin As Decimal
        Public Property Product_Selling_Price As Decimal

        Public Property DistanceStepKM As Decimal?
        Public Property SalesmanExtra As Decimal?
        Public Property MaxDeliveryCharge As Decimal?

        Public Property Min_Qty As Decimal
        Public Property ReOrd_Level As Decimal
        Public Property Qty_to_Order As Decimal
        Public Property SupplierCode As String
        Public Property Weight As Decimal
        Public Property Product_Qty As Decimal

        Public Property isAlternetUnit As Boolean
        Public Property AlternetUnit As String
        Public Property UnitValue As Decimal
        Public Property AlternetUnitValue As Decimal

        Public Property HSCode As String
        Public Property HSDesc As String

        Public Property isActive As Boolean
        Public Property isStockItem As Boolean
        Public Property Remark As String

        Public Property Created As DateTime?
        Public Property CreatedBy As String
        Public Property Modified As DateTime?
        Public Property ModifiedBy As String

        Public Property SrNo As Integer
        Public Property img_src As String
    End Class

    Public Class AlternateUnit
        Public Property SrNo As Integer
        Public Property ProductCode As String
        Public Property AlternetUnit As String

        Public Property AlternetQty As Decimal
        Public Property PrimaryQty As Decimal

        Public Property AlternetCostPrice As Decimal
        Public Property AlternetMrgn As Decimal
        Public Property AlternetTradePrice As Decimal
        Public Property AlternetPrice As Decimal

        Public Property DistanceStepKM As Decimal?
        Public Property SalesmanExtra As Decimal?
        Public Property MaxDeliveryCharge As Decimal?

        Public Property AlternetWeight As Decimal

        Public Property Created As DateTime?
        Public Property CreatedBy As String
        Public Property Modified As DateTime?
        Public Property ModifiedBy As String
    End Class

    Public Class SyncPayload
        Public Property products As List(Of Product)
        Public Property alternate_units As List(Of AlternateUnit)
    End Class
End Namespace
