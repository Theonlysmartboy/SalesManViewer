Namespace models
    Public Class Salesman
        Public Property id As Integer
        Public Property username As String
        Public Property full_name As String
        Public Property email As String
        Public Property phone As String
    End Class

    Public Class SalesmanApiResponse
        Public Property success As Boolean
        Public Property data As List(Of Salesman)
    End Class

    Public Class Tracking
        Public Property id As Integer
        Public Property user_id As Integer
        Public Property username As String
        Public Property latitude As String
        Public Property longitude As String
        Public Property tracked_at As String
    End Class

    Public Class TrackingResponseSingle
        Public Property success As Boolean
        Public Property data As Tracking
    End Class

    Public Class TrackingResponseList
        Public Property success As Boolean
        Public Property data As List(Of Tracking)
    End Class
End Namespace
