Public Class ResetPasswordForm
    Private userId As Integer
    Public Sub New(userId As Integer)
        InitializeComponent()
        Me.userId = userId
        Me.KeyPreview = True
    End Sub

    Private Sub ResetPasswordForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxtOldPassword.UseSystemPasswordChar = True
        TxtNewPassword.UseSystemPasswordChar = True
        TxtConfirmPassword.UseSystemPasswordChar = True
        ' Set tooltips
        Dim toolTip As New ToolTip()
        toolTip.SetToolTip(PicToggleOldPassword, "Show/Hide Old Password (Ctrl+H)")
        toolTip.SetToolTip(PicToggleNewPassword, "Show/Hide New Password (Ctrl+H)")
        toolTip.SetToolTip(PicToggleConfirmPassword, "Show/Hide Confirm Password (Ctrl+H)")
        toolTip.SetToolTip(BtnReset, "Click to reset your password (Enter)")
        toolTip.SetToolTip(BtnLoginInstead, "Back to Login (Ctrl+L)")
        toolTip.SetToolTip(Me, "Press Esc to return to Login, Ctrl+H to toggle password visibility")
        toolTip.AutoPopDelay = 5000
        toolTip.InitialDelay = 500
        toolTip.ReshowDelay = 500
        BtnLoginInstead.Visible = True
    End Sub

    Private Sub TxtOldPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtOldPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            TxtNewPassword.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub TxtNewPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtNewPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            TxtConfirmPassword.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub TxtConfirmPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtConfirmPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            BtnReset.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub BtnLoginInstead_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles BtnLoginInstead.LinkClicked
        HandleLoginNav()
    End Sub

    Private Sub PicToggleOldPassword_Click(sender As Object, e As EventArgs) Handles PicToggleOldPassword.Click
        TogglePasswordVisibility(TxtOldPassword, PicToggleOldPassword)
    End Sub

    Private Sub PicToggleNewPassword_Click(sender As Object, e As EventArgs) Handles PicToggleNewPassword.Click
        TogglePasswordVisibility(TxtNewPassword, PicToggleNewPassword)
    End Sub

    Private Sub PicToggleConfirmPassword_Click(sender As Object, e As EventArgs) Handles PicToggleConfirmPassword.Click
        TogglePasswordVisibility(TxtConfirmPassword, PicToggleConfirmPassword)
    End Sub

    ' PASSWORD/ANSWER TOGGLE helper - toggles the UseSystemPasswordChar property and updates the icon accordingly
    Private Sub TogglePasswordVisibility(txt As TextBox, pic As PictureBox)
        txt.UseSystemPasswordChar = Not txt.UseSystemPasswordChar
        If txt.UseSystemPasswordChar Then
            pic.Image = My.Resources.Resources.eye_closed_outline
        Else
            pic.Image = My.Resources.Resources.eye_open_outline
        End If
    End Sub

    ' Login navigation helper - opens the login form and closes the current form
    Private Sub HandleLoginNav()
        Dim loginForm As New LoginForm()
        loginForm.Show()
        Me.Close()
    End Sub
End Class