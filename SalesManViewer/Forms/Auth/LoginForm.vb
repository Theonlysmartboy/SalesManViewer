Imports System.Net.Http
Imports SalesManViewer.Config
Imports SalesManViewer.CustomControls.Alert
Imports SalesManViewer.Helpers.Db
Imports SalesManViewer.Models.Auth
Imports SalesManViewer.Services.Auth

Public Class LoginForm

    Private Const DEFAULT_BASE_URL As String = "https://197.248.109.130/salesman-backend"
    Private Const SETTING_KEY_API_BASE_URL As String = "api_base_url"
    Private _baseUrl As String

    ' LIFECYCLE
    Private Async Sub LoginForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _baseUrl = Await ResolveBaseUrlAsync()
        TxtUserName.Focus()
    End Sub

    Private Sub BtnForgotPassword_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles BtnForgotPassword.LinkClicked
        HandleForgotPassword()
    End Sub

    Private Async Sub BtnLogin_Click(sender As Object, e As EventArgs) Handles BtnLogin.Click
        Await PerformLoginAsync()
    End Sub

    Private Sub PicTogglePassword_Click(sender As Object, e As EventArgs) Handles PicTogglePassword.Click
        TxtPassword.UseSystemPasswordChar = Not TxtPassword.UseSystemPasswordChar
        Try
            PicTogglePassword.Image = If(TxtPassword.UseSystemPasswordChar, My.Resources.eye_closed_outline, My.Resources.eye_open_outline)
        Catch
            ' Icon resource not present — leave as-is.
        End Try
    End Sub

    Private Sub TxtUserName_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtUserName.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            e.Handled = True
            TxtPassword.Focus()
        End If
    End Sub

    Private Sub TxtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            e.Handled = True
            If BtnLogin.Enabled Then BtnLogin.PerformClick()
        End If
    End Sub

    ' BASE URL RESOLUTION
    Private Async Function ResolveBaseUrlAsync() As Task(Of String)
        ' --- Ensure the local DB is configured before reading settings ---
        Dim connString As String
        Try
            connString = GlobalConnectionString.GetConnectionString()
        Catch ex As Exception
            connString = Nothing
        End Try
        If String.IsNullOrWhiteSpace(connString) Then
            ' The DB isn't configured — the app can't function. Bounce to Settings.
            Dim answer = JsAlertDialog.ShowAlert(Me, "The application is not configured to reach its database." & vbCrLf & vbCrLf &
                "Would you like to open Settings now?", "Missing Configuration", JsAlertDialog.AlertType.Confirm, JsAlertDialog.ButtonType.YesNo)
            If answer = DialogResult.Yes Then
                Using frm As New SettingsForm()
                    frm.ShowDialog(Me)
                End Using
                Try
                    connString = GlobalConnectionString.GetConnectionString()
                Catch
                    connString = Nothing
                End Try
            End If
            If String.IsNullOrWhiteSpace(connString) Then
                ' Still nothing — bail out to the fallback and let login fail with a clear error.
                Return DEFAULT_BASE_URL
            End If
        End If
        ' --- Read the setting from the DB ---
        Dim mgr As New SettingsManager(connString)
        Dim stored As String = Nothing
        Try
            stored = Await mgr.GetSettingAsync(SETTING_KEY_API_BASE_URL)
        Catch ex As Exception
            ' Table doesn't exist yet, permissions, etc. — treat as unset.
            stored = Nothing
        End Try
        If Not String.IsNullOrWhiteSpace(stored) Then
            Return stored.TrimEnd("/"c)
        End If
        ' --- Not configured — prompt the user ---
        Dim answer2 = JsAlertDialog.ShowAlert(Me, "The API base URL is not configured." & vbCrLf & vbCrLf &
            "Would you like to open Settings and configure it now?" & vbCrLf & vbCrLf &
            "If you choose No, the app will use the default:" & vbCrLf & DEFAULT_BASE_URL,
            "Missing Configuration", JsAlertDialog.AlertType.Confirm, JsAlertDialog.ButtonType.YesNo)
        If answer2 = DialogResult.Yes Then
            Using frm As New SettingsForm()
                frm.ShowDialog(Me)
            End Using
            Try
                stored = Await mgr.GetSettingAsync(SETTING_KEY_API_BASE_URL)
            Catch
                stored = Nothing
            End Try
            If Not String.IsNullOrWhiteSpace(stored) Then
                Return stored.TrimEnd("/"c)
            End If
        End If
        Return DEFAULT_BASE_URL
    End Function

    ' LOGIN
    Private Async Function PerformLoginAsync() As Task
        Dim username = TxtUserName.Text.Trim()
        Dim password = TxtPassword.Text
        If String.IsNullOrWhiteSpace(username) Then
            ShowError("Please enter your username.")
            TxtUserName.Focus()
            Return
        End If
        If String.IsNullOrWhiteSpace(password) Then
            ShowError("Please enter your password.")
            TxtPassword.Focus()
            Return
        End If
        SetBusy(True)
        Try
            Dim svc As New AuthService(_baseUrl)
            Dim resp = Await svc.LoginAsync(username, password)
            If resp Is Nothing Then
                ShowError("Empty response from server.")
                TxtPassword.Clear()
                TxtPassword.Focus()
                Return
            End If
            If Not resp.Success OrElse resp.Data Is Nothing OrElse resp.Data.User Is Nothing Then
                Dim msg = If(String.IsNullOrWhiteSpace(resp.Message), "Invalid username or password.", resp.Message)
                ShowError(msg)
                TxtPassword.Clear()
                TxtPassword.Focus()
                Return
            End If
            ' Populate UserContext
            Dim u = UserContext.Instance
            u.UserId = resp.Data.User.Id
            u.UserName = resp.Data.User.UserName
            u.FullName = resp.Data.User.FullName
            u.Token = resp.Data.Token
            u.HasPin = resp.Data.User.HasPin
            u.LoginTime = DateTime.Now
            Dim singleRole = If(resp.Data.User.Role, "").Trim().ToLowerInvariant()
            u.Role = singleRole
            u.Roles = If(String.IsNullOrEmpty(singleRole), New List(Of String)(), New List(Of String) From {singleRole})
            ' Permissions aren't returned by /login — leave empty.
            ' If your API has a /permissions endpoint, fetch it here and populate.
            u.Permissions = New List(Of String)()
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch tex As TaskCanceledException
            ShowError("The request timed out. Please check your connection and try again.")
        Catch hex As HttpRequestException
            ShowError("Could not reach the server:" & vbCrLf & hex.Message)
        Catch ex As Exception
            ShowError("Login failed:" & vbCrLf & ex.Message)
        Finally
            SetBusy(False)
        End Try
    End Function

    ' UI HELPERS
    Private Sub SetBusy(busy As Boolean)
        Spinner.Visible = busy
        BtnLogin.Enabled = Not busy
        TxtUserName.Enabled = Not busy
        TxtPassword.Enabled = Not busy
        PicTogglePassword.Enabled = Not busy
        BtnForgotPassword.Enabled = Not busy
        BtnLogin.Text = If(busy, "Logging in…", "Login")
        Cursor = If(busy, Cursors.WaitCursor, Cursors.Default)
    End Sub

    Private Sub ShowError(msg As String)
        JsAlertDialog.ShowAlert(Me, msg, "Login", JsAlertDialog.AlertType.Error,
                                JsAlertDialog.ButtonType.OK, True, 20)
    End Sub

    Private Sub HandleForgotPassword()
        Me.Hide()
        Using forgotForm As New ForgotPasswordForm()
            forgotForm.ShowDialog(Me)
        End Using
        If Not Me.IsDisposed Then
            Me.Show()
            TxtUserName.Focus()
            TxtUserName.SelectAll()
        End If
    End Sub
End Class