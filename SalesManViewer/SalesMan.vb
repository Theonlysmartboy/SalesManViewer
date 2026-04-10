Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json

Public Class SalesMan
    Public IsEditMode As Boolean = False
    Public UserId As Integer = 0
    Dim serverUrl As String = "http://197.248.109.130/salesman-backend"

    Private Async Sub BtnSubmit_Click(sender As Object, e As EventArgs) Handles BtnSubmit.Click
        Try
            Using client As New HttpClient()
                Dim url As String
                Dim payload As String
                Dim status As Boolean
                If ChkIsActive.Checked Then
                    status = True
                Else
                    status = False
                End If
                If IsEditMode Then
                    ' UPDATE
                    url = $"{serverUrl}/api/auth.php?action=update-user"
                    payload = JsonConvert.SerializeObject(New With {
                        .userId = UserId,
                        .username = TxtUserName.Text,
                        .password = TxtPassword.Text.Trim(),
                        .fullname = TxtFullName.Text,
                        .email = TxtEmail.Text,
                        .phone = TxtPhoneNo.Text,
                        .is_active = status
                    })
                Else
                    ' CREATE
                    url = $"{serverUrl}/api/auth.php?action=register"
                    payload = JsonConvert.SerializeObject(New With {
                        .username = TxtUserName.Text.Trim(),
                        .password = TxtPassword.Text.Trim(),
                        .fullname = TxtFullName.Text.Trim(),
                        .email = TxtEmail.Text.Trim(),
                        .phone = TxtPhoneNo.Text.Trim(),
                        .is_active = Status
                    })
                End If
                Dim content As New StringContent(payload, Encoding.UTF8, "application/json")
                Dim response = Await client.PostAsync(url, content)
                Dim result = Await response.Content.ReadAsStringAsync()
                MessageBox.Show(result)
                Me.DialogResult = DialogResult.OK
                Me.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        TxtUserName.Text = ""
        TxtPassword.Text = ""
        TxtFullName.Text = ""
        TxtEmail.Text = ""
        TxtPhoneNo.Text = ""
        ChkIsActive.Checked = False
    End Sub
End Class