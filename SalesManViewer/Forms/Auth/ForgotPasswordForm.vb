Imports System.Net.Http
Imports SalesManViewer.Config
Imports SalesManViewer.CustomControls.Alert
Imports SalesManViewer.Helpers.Db
Imports SalesManViewer.Services.Auth

Public Class ForgotPasswordForm

    Private ReadOnly _toolTip As New ToolTip()
    Private _auth As AuthService
    Private _busy As Boolean = False

    ' LIFECYCLE
    Private Async Sub ForgotPasswordForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetupTooltips()
        Await ResolveAuthServiceAsync()
        TxtUserName.Focus()
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
    Private Async Sub BtnCheck_Click(sender As Object, e As EventArgs) Handles BtnCheck.Click
        If _busy Then Return
        Dim userName = TxtUserName.Text.Trim()
        If String.IsNullOrWhiteSpace(userName) Then
            ShowError("Please enter your username.", TxtUserName)
            Return
        End If
        If userName.Length < 3 Then
            ShowError("Username is too short.", TxtUserName)
            Return
        End If
        If _auth Is Nothing Then
            ShowError("The API is not configured. Please contact your administrator.", Nothing)
            Return
        End If
        SetBusy(True)
        Try
            Dim resp = Await _auth.RequestPasswordResetAsync(userName)
            If resp Is Nothing Then
                ShowError("No response from the server. Please try again.", Nothing)
                Return
            End If
            If Not resp.Success Then
                ShowError(If(String.IsNullOrWhiteSpace(resp.Message),
                             "Unable to process the request. Please try again.",
                             resp.Message), Nothing)
                Return
            End If
            JsAlertDialog.ShowAlert(Me, "If an account exists for '" & userName & "', reset instructions have been sent." & vbCrLf & vbCrLf &
                "Please check your email inbox (and spam folder) for the one-time code.", "Check Your Email",
                JsAlertDialog.AlertType.Success, JsAlertDialog.ButtonType.OK, True, 20)
            Using reset As New ResetPasswordForm(userName)
                Me.Hide()
                Dim result = reset.ShowDialog(Me)
                Me.Close()
            End Using
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
        HandleLoginNav()
    End Sub

    Private Sub ForgotPasswordForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            e.SuppressKeyPress = True
            e.Handled = True
            HandleLoginNav()
            Return
        End If
        If e.Control AndAlso e.KeyCode = Keys.L Then
            e.SuppressKeyPress = True
            e.Handled = True
            HandleLoginNav()
        End If
    End Sub

    Private Sub TxtUserName_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtUserName.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            e.Handled = True
            If Not _busy Then BtnCheck.PerformClick()
        End If
    End Sub

    Private Sub HandleLoginNav()
        If _busy Then Return
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ' UI HELPERS
    Private Sub SetBusy(busy As Boolean)
        _busy = busy
        Loader.Visible = busy
        TxtUserName.Enabled = Not busy
        BtnCheck.Enabled = Not busy
        BtnLoginInstead.Enabled = Not busy
        BtnCheck.Text = If(busy, "Sending…", "Check")
        Cursor = If(busy, Cursors.WaitCursor, Cursors.Default)
    End Sub

    Private Sub ShowError(msg As String, Optional focusControl As Control = Nothing)
        JsAlertDialog.ShowAlert(Me, msg, "Reset Password", JsAlertDialog.AlertType.Error,
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
        _toolTip.SetToolTip(TxtUserName, "Enter the username associated with your account.")
        _toolTip.SetToolTip(BtnCheck, "Send the reset code to the email on file. (Enter)")
        _toolTip.SetToolTip(BtnLoginInstead, "Return to the login screen. (Esc, Ctrl+L)")
    End Sub
End Class