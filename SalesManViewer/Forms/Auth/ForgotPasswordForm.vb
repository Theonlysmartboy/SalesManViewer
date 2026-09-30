Public Class ForgotPasswordForm
    Private Sub ForgotPasswordForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim toolTip As New ToolTip()
        toolTip.SetToolTip(TxtUserName, "Enter your username")
        toolTip.SetToolTip(BtnCheck, "Click to submit username(Enter")
        toolTip.SetToolTip(BtnLoginInstead, "Go back to login form (Ctrl+L)")
        toolTip.AutoPopDelay = 5000
        toolTip.InitialDelay = 500
        toolTip.ReshowDelay = 500
    End Sub

    Private Sub BtnCheck_Click(sender As Object, e As EventArgs) Handles BtnCheck.Click

    End Sub

    Private Sub BtnLoginInstead_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles BtnLoginInstead.LinkClicked
        HandleLoginNav()
    End Sub

    Private Sub ForgotPasswordForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            HandleLoginNav()
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.Enter Then
            BtnCheck.PerformClick()
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.L AndAlso e.Modifiers = Keys.Control Then
            HandleLoginNav()
        End If
    End Sub

    Private Sub TxtUserName_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtUserName.KeyDown
        If e.KeyCode = Keys.Enter Then
            BtnCheck.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub HandleLoginNav()
        Me.Close()
    End Sub
End Class