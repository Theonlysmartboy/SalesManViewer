Imports MySql.Data.MySqlClient

Namespace helpers
    Public Class DatabaseHelper
        Public Shared Function GetConnection() As MySqlConnection

            Dim server As String = "localhost"
            Dim database As String = "sales_man"
            Dim user As String = "root"
            Dim password As String = ""

            Dim connString As String =
            $"server={server};user id={user};password={password};database={database};SslMode=none;"

            Dim conn As New MySqlConnection(connString)
            conn.Open()

            Return conn

        End Function

    End Class
End Namespace