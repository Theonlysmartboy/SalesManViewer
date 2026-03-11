Namespace models
    Public Class RemoteProduct
        Public Property ProductCode As String
        Public Property ProductName As String
        Public Property ProductUnit As String
        Public Property Product_Selling_Price As String
        Public Property Product_VAT_Code As String
        Public Property isStockItem As Integer
        Public Property isActive As Integer
        Public Property stockQty As String
        Public Property Modified As String
        Public Property img_src As String
    End Class

    Public Class ProductApiResponse
        Public Property success As Boolean
        Public Property count As Integer
        Public Property data As List(Of RemoteProduct)
    End Class
End Namespace
