Imports System.Net.Http
Imports SalesManViewer.CustomControls.Alert
Imports SalesManViewer.Helpers.Config
Imports SalesManViewer.Helpers.Database
Imports SalesManViewer.Helpers.Security
Imports SalesManViewer.Services.Auth

Public Class ResetPasswordForm

    Private ReadOnly _userName As String
    Private _auth As AuthService
    Private _busy As Boolean = False
    Private ReadOnly _toolTip As New ToolTip()

    ' CONSTRUCTION
    Public Sub New(userName As String)
        InitializeComponent()
        _userName = If(userName, "").Trim()
        Me.KeyPreview = True
    End Sub

    ' LIFECYCLE
    Private Async Sub ResetPasswordForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetupTooltips()
        ' Fields start masked
        TxtNewPassword.UseSystemPasswordChar = True
        TxtConfirmPassword.UseSystemPasswordChar = True
        PicToggleOldPassword.Image = My.Resources.eye_closed_outline
        PicToggleNewPassword.Image = My.Resources.eye_closed_outline
        PicToggleConfirmPassword.Image = My.Resources.eye_closed_outline
        ' Show the account being reset
        If Not String.IsNullOrWhiteSpace(_userName) Then
            LblUserName.Text = "Resetting password for: " & _userName
            LblUserName.Visible = True
        Else
            LblUserName.Visible = False
        End If
        Await ResolveAuthServiceAsync()
        TxtOldPassword.Focus()
    End Sub

    Private Async Function ResolveAuthServiceAsync() As Task
        Dim fallback = "https://197.248.109.130/salesman-backend"
        Try
            Dim connString = GlobalConnectionString.GetConnectionString()
            If String.IsNullOrWhiteSpace(connString) Then
                _auth = New AuthService(fallback)
                Return
            End If
            Dim mgr As New SettingsManager(connString)
            Dim url = Await mgr.GetSettingAsync("api_base_url")
            _auth = New AuthService(If(String.IsNullOrWhiteSpace(url), fallback, url.TrimEnd("/"c)))
        Catch
            _auth = New AuthService(fallback)
        End Try
    End Function

    ' SUBMIT
    Private Async Sub BtnReset_Click(sender As Object, e As EventArgs) Handles BtnReset.Click
        If _busy Then Return
        ' --- Validate ---
        Dim otp = TxtOldPassword.Text.Trim()
        Dim newPwd = TxtNewPassword.Text
        Dim confirmPwd = TxtConfirmPassword.Text
        If String.IsNullOrWhiteSpace(otp) Then
            ShowError("Please enter the one-time code sent to your email or your old password", TxtOldPassword)
            Return
        End If
        If otp.Length < 4 Then
            ShowError("The code or old password you entered is too short.", TxtOldPassword)
            Return
        End If
        If String.IsNullOrWhiteSpace(newPwd) Then
            ShowError("Please enter a new password.", TxtNewPassword)
            Return
        End If
        If newPwd <> confirmPwd Then
            ShowError("The new and confirmed passwords do not match. Please re-enter them.", TxtConfirmPassword)
            Return
        End If
        Dim strengthError = ValidatePassword(newPwd)
        If strengthError IsNot Nothing Then
            ShowError(strengthError, TxtNewPassword)
            Return
        End If
        If _auth Is Nothing Then
            ShowError("The API is not configured. Please contact your administrator.", Nothing)
            Return
        End If
        ' --- Send ---
        SetBusy(True)
        Try
            Dim resp = Await _auth.ResetPasswordWithOtpAsync(_userName, otp, newPwd)
            If resp Is Nothing Then
                ShowError("No response from the server. Please try again.", Nothing)
                Return
            End If
            If Not resp.Success Then
                Dim msg = If(String.IsNullOrWhiteSpace(resp.Message),
                             "Password reset failed. Please check the code and try again.",
                             resp.Message)
                ShowError(msg, TxtOldPassword)
                Return
            End If
            JsAlertDialog.ShowAlert(Me, "Your password has been updated successfully." & vbCrLf & vbCrLf &
                "You can now log in with your new password.", "Password Updated",
                JsAlertDialog.AlertType.Success, JsAlertDialog.ButtonType.OK, True, 20)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch tex As TaskCanceledException
            ShowError("The request timed out. Please check your connection and try again.", Nothing)
        Catch hex As HttpRequestException
            ShowError("Could not reach the server." & vbCrLf & hex.Message, Nothing)
        Catch ex As Exception
            ShowError("Something went wrong:" & vbCrLf & ex.Message, Nothing)
        Finally
            SetBusy(False)
        End Try
    End Sub

    ' NAVIGATION & KEYBOARD
    Private Sub BtnLoginInstead_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles BtnLoginInstead.LinkClicked
        HandleCancel()
    End Sub

    Private Sub ResetPasswordForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            e.SuppressKeyPress = True
            e.Handled = True
            HandleCancel()
            Return
        End If
        If e.Control AndAlso e.KeyCode = Keys.L Then
            e.SuppressKeyPress = True
            e.Handled = True
            HandleCancel()
            Return
        End If
        If e.Control AndAlso e.KeyCode = Keys.H Then
            e.SuppressKeyPress = True
            e.Handled = True
            If TxtOldPassword.Focused Then
                TogglePasswordVisibility(TxtOldPassword, PicToggleOldPassword)
            ElseIf TxtNewPassword.Focused Then
                TogglePasswordVisibility(TxtNewPassword, PicToggleNewPassword)
            ElseIf TxtConfirmPassword.Focused Then
                TogglePasswordVisibility(TxtConfirmPassword, PicToggleConfirmPassword)
            End If
        End If
    End Sub

    Private Sub TxtOldPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtOldPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            e.Handled = True
            TxtNewPassword.Focus()
        End If
    End Sub

    Private Sub TxtNewPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtNewPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            e.Handled = True
            TxtConfirmPassword.Focus()
        End If
    End Sub

    Private Sub TxtConfirmPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtConfirmPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            e.Handled = True
            If Not _busy Then BtnReset.PerformClick()
        End If
    End Sub

    Private Sub HandleCancel()
        If _busy Then Return
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ' PASSWORD VISIBILITY TOGGLES
    Private Sub PicToggleOldPassword_Click(sender As Object, e As EventArgs) Handles PicToggleOldPassword.Click
        TogglePasswordVisibility(TxtOldPassword, PicToggleOldPassword)
    End Sub

    Private Sub PicToggleNewPassword_Click(sender As Object, e As EventArgs) Handles PicToggleNewPassword.Click
        TogglePasswordVisibility(TxtNewPassword, PicToggleNewPassword)
    End Sub

    Private Sub PicToggleConfirmPassword_Click(sender As Object, e As EventArgs) Handles PicToggleConfirmPassword.Click
        TogglePasswordVisibility(TxtConfirmPassword, PicToggleConfirmPassword)
    End Sub

    Private Sub TogglePasswordVisibility(txt As TextBox, pic As PictureBox)
        Dim selStart = txt.SelectionStart
        txt.UseSystemPasswordChar = Not txt.UseSystemPasswordChar
        pic.Image = If(txt.UseSystemPasswordChar, My.Resources.eye_closed_outline, My.Resources.eye_open_outline)
        txt.SelectionStart = selStart
        txt.SelectionLength = 0
    End Sub

    ' UI HELPERS
    Private Sub SetBusy(busy As Boolean)
        _busy = busy
        Loader.Visible = busy
        TxtOldPassword.Enabled = Not busy
        TxtNewPassword.Enabled = Not busy
        TxtConfirmPassword.Enabled = Not busy
        BtnReset.Enabled = Not busy
        BtnLoginInstead.Enabled = Not busy
        PicToggleNewPassword.Enabled = Not busy
        PicToggleConfirmPassword.Enabled = Not busy
        BtnReset.Text = If(busy, "Resetting…", "Reset Password")
        Cursor = If(busy, Cursors.WaitCursor, Cursors.Default)
    End Sub

    Private Sub ShowError(msg As String, Optional focusControl As Control = Nothing)
        JsAlertDialog.ShowAlert(Me, msg, "Reset Password",
            JsAlertDialog.AlertType.Error,
            JsAlertDialog.ButtonType.OK, True, 20)
        If focusControl IsNot Nothing Then
            focusControl.Focus()
            If TypeOf focusControl Is TextBox Then
                DirectCast(focusControl, TextBox).SelectAll()
            End If
        End If
    End Sub

    Private Sub SetupTooltips()
        _toolTip.AutoPopDelay = 8000
        _toolTip.InitialDelay = 500
        _toolTip.ReshowDelay = 200
        _toolTip.ShowAlways = True
        _toolTip.SetToolTip(TxtOldPassword, "Your Old Password or the one-time code sent to your email. Case sensitive.")
        _toolTip.SetToolTip(TxtNewPassword, "New password. Min 6 chars, 1 upper, 1 lower, 1 digit, 1 symbol.")
        _toolTip.SetToolTip(TxtConfirmPassword, "Re-enter the new password to confirm.")
        _toolTip.SetToolTip(PicToggleNewPassword, "Show / hide new password (Ctrl+H)")
        _toolTip.SetToolTip(PicToggleConfirmPassword, "Show / hide confirm password (Ctrl+H)")
        _toolTip.SetToolTip(BtnReset, "Submit the new password. (Enter)")
        _toolTip.SetToolTip(BtnLoginInstead, "Return to login. (Esc, Ctrl+L)")
    End Sub
End Class