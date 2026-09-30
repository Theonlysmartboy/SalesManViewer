Imports SalesManViewer.CustomControls.Alert
Imports SalesManViewer.Helpers.Cryptography

Namespace Helpers.Database

    Public Class GlobalConnectionString

        Public Shared Function GetConnectionString() As String
            Dim server = My.Settings.db_server
            Dim user = My.Settings.db_user
            Dim encryptedPassword = My.Settings.db_password
            Dim db = My.Settings.db_name
            Dim db_prefix = My.Settings.db_prefix
            Dim port = My.Settings.db_port
            ' Validate
            If String.IsNullOrWhiteSpace(server) OrElse String.IsNullOrWhiteSpace(user) OrElse String.IsNullOrWhiteSpace(db) Then
                Dim AlertResult As DialogResult = JsAlertDialog.ShowAlert(Nothing, "Your database settings are incomplete. " &
                                                "Please configure the database connection.", "Missing Configuration",
                                                JsAlertDialog.AlertType.Confirm, JsAlertDialog.ButtonType.OKCancel)
                If AlertResult = DialogResult.Cancel Then
                    Application.Exit()
                    Return Nothing
                End If
                Dim frm As New SettingsForm()
                frm.Show()
                Return Nothing
            End If
            ' Decrypt the database password.
            Dim password As String
            Try
                password = Encorder.Decrypt(encryptedPassword)
            Catch ex As Exception
                JsAlertDialog.ShowAlert(Nothing, "The stored database password could not be decrypted." &
                    vbCrLf & vbCrLf & "Please re-enter and save the database password.", "Invalid Database Password",
                    JsAlertDialog.AlertType.Error, JsAlertDialog.ButtonType.OK, True, 20)
                Return Nothing
            End Try
            ' Build connection string.
            Dim builder As New MySql.Data.MySqlClient.MySqlConnectionStringBuilder With {
                .Server = server,
                .UserID = user,
                .Password = password,
                .Database = db
            }
            If Not String.IsNullOrWhiteSpace(port) Then
                Dim parsedPort As UInteger
                If UInteger.TryParse(port, parsedPort) Then
                    builder.Port = parsedPort
                End If
            End If
            Return builder.ConnectionString
        End Function
    End Class
End Namespace