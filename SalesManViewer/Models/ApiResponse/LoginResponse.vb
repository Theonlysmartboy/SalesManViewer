Imports Newtonsoft.Json
Imports SalesManViewer.Models.Auth

Namespace Models.ApiResponse

    ''' <summary>
    ''' Matches the JSON returned by POST /api/auth.php?action=login
    ''' </summary>
    Public Class LoginResponse
        <JsonProperty("code")>
        Public Property Code As Integer
        <JsonProperty("success")>
        Public Property Success As Boolean
        <JsonProperty("message")>
        Public Property Message As String
        <JsonProperty("data")>
        Public Property Data As LoginData
    End Class
End Namespace