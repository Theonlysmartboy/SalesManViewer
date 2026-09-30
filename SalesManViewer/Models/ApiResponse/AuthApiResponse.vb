Imports Newtonsoft.Json

Namespace Models.ApiResponse

    Public Class AuthApiResponse
        <JsonProperty("code")>
        Public Property Code As Integer
        <JsonProperty("success")>
        Public Property Success As Boolean
        <JsonProperty("message")>
        Public Property Message As String
        <JsonProperty("data")>
        Public Property Data As Object
    End Class
End Namespace