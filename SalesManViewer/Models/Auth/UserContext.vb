Namespace Models.Auth

    Public Class UserContext

        Private Shared _instance As UserContext
        Private Shared ReadOnly _lock As New Object()

        Private Sub New()
        End Sub

        Public Shared ReadOnly Property Instance As UserContext
            Get
                If _instance Is Nothing Then
                    SyncLock _lock
                        If _instance Is Nothing Then
                            _instance = New UserContext()
                        End If
                    End SyncLock
                End If
                Return _instance
            End Get
        End Property

        ' Session State
        Public Property UserId As Integer
        Public Property UserName As String
        Public Property Roles As List(Of String)
        Public Property Permissions As List(Of String)
        Public Property FullName As String
        Public Property LoginTime As DateTime
        Public Property Token As String
        Public Property Role As String
        Public Property HasPin As Boolean

        ' Lifecycle Control
        Public Shared Sub Clear()
            SyncLock _lock
                _instance = Nothing
            End SyncLock
        End Sub

        Public Shared Function IsAuthenticated() As Boolean
            Return _instance IsNot Nothing AndAlso _instance.UserId > 0
        End Function
    End Class
End Namespace