Imports Newtonsoft.Json

Namespace Models.Auth

    Public Class LoginData
        <JsonProperty("token")> Public Property Token As String
        <JsonProperty("user")> Public Property User As LoginUser
    End Class
End Namespace