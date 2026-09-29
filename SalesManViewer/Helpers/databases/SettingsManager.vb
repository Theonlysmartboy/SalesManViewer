Imports SalesManViewer.Helpers.Db

Namespace Config
    Public Class SettingsManager

        Private ReadOnly _connectionString As String

        Public Sub New(connectionString As String)
            _connectionString = connectionString
        End Sub

        Public Function SetSettingAsync(key As String, value As String) As Task
            Dim sql = "INSERT INTO settings (setting_key, setting_value) VALUES (@key, @value) ON DUPLICATE KEY UPDATE setting_value = @value"
            Dim db = New DbHelper(_connectionString)
            ' Capture old value for audit
            Dim oldObj = db.ExecuteScalar("SELECT setting_value FROM settings WHERE setting_key=@key LIMIT 1", New Dictionary(Of String, Object) From {{"@key", key}})
            Dim oldVal = If(oldObj Is Nothing OrElse IsDBNull(oldObj), Nothing, oldObj.ToString())
            db.ExecuteNonQuery(sql, New Dictionary(Of String, Object) From {{"@key", key}, {"@value", value}})
            Return Task.CompletedTask
        End Function

        Public Function GetSettingAsync(key As String) As Task(Of String)
            Dim sql = "SELECT setting_value FROM settings WHERE setting_key = @key LIMIT 1"
            Dim db = New DbHelper(_connectionString)
            Dim result = db.ExecuteScalar(sql, New Dictionary(Of String, Object) From {{"@key", key}})
            Return Task.FromResult(If(result Is Nothing, Nothing, result.ToString()))
        End Function

        Public Function GetAllSettings() As Task(Of Dictionary(Of String, String))
            Dim settings As New Dictionary(Of String, String)()
            Dim sql = "SELECT setting_key, setting_value FROM settings"
            Dim db = New DbHelper(_connectionString)
            Dim dt = db.ExecuteSelect(sql)
            For Each row As DataRow In dt.Rows
                Dim key = row("setting_key").ToString()
                Dim value = If(row("setting_value") IsNot DBNull.Value, row("setting_value").ToString(), "")
                settings(key) = value
            Next
            Return Task.FromResult(settings)
        End Function

        Public Function DeleteSettingAsync(key As String) As Task
            Dim sql = "DELETE FROM settings WHERE setting_key = @key LIMIT 1"
            Dim db = New DbHelper(_connectionString)
            ' Capture old value
            Dim oldObj = db.ExecuteScalar("SELECT setting_value FROM settings WHERE setting_key=@key LIMIT 1", New Dictionary(Of String, Object) From {{"@key", key}})
            Dim oldVal = If(oldObj Is Nothing OrElse IsDBNull(oldObj), Nothing, oldObj.ToString())
            db.ExecuteNonQuery(sql, New Dictionary(Of String, Object) From {{"@key", key}})
            Return Task.CompletedTask
        End Function

        Private Function IsSensitiveKey(key As String) As Boolean
            If String.IsNullOrWhiteSpace(key) Then Return False
            Dim sensitive = New String() {"password", "secret", "token", "api_key", "apikey", "access_key", "private_key", "connection_string", "credential"}
            Dim lower = key.ToLower()
            For Each s In sensitive
                If lower.Contains(s) Then Return True
            Next
            Return False
        End Function
    End Class
End Namespace