Imports Newtonsoft.Json

Namespace Models.Auth

    Public Class LoginUser
        <JsonProperty("id")>
        Public Property Id As Integer
        <JsonProperty("username")>
        Public Property UserName As String
        <JsonProperty("has_pin")>
        Public Property HasPin As Boolean
        <JsonProperty("role")>
        Public Property Role As String
        <JsonProperty("full_name")>
        Public Property FullName As String
    End Class
End Namespace