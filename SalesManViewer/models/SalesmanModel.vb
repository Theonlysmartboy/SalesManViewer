Namespace models
    Public Class Salesman
        Public Property id As Integer
        Public Property username As String
        Public Property full_name As String
        Public Property email As String
        Public Property phone As String
        Public Property is_Active As Boolean
    End Class

    Public Class SalesmanApiResponse
        Public Property success As Boolean
        Public Property data As List(Of Salesman)
    End Class
End Namespace
