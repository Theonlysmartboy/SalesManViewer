Imports System.Runtime.InteropServices
Namespace CustomControls.Alert
    Public Class JsAlertDialog

        Public Enum AlertType
            Success
            [Error]
            Warning
            Info
            Confirm
        End Enum
        Public Enum ButtonType
            OK
            OKCancel
            YesNo
        End Enum

        Private AlertResult As DialogResult = DialogResult.None
        Private IconRotation As Single = -90 ' start rotated off-screen
        Private IconRotationStep As Single = 5 ' rotation per tick
        Private IconAnimationDone As Boolean = False
        Private IconImage As Image
        Private AutoCloseEnabled As Boolean = False
        Private AutoCloseSeconds As Integer = 0
        Private RemainingMilliseconds As Integer = 0
        Private InitialMilliseconds As Integer = 0

        ' Rounded corners
        <DllImport("Gdi32.dll", EntryPoint:="CreateRoundRectRgn")>
        Private Shared Function CreateRoundRectRgn(left As Integer, top As Integer, right As Integer, bottom As Integer,
                                                width As Integer, height As Integer) As IntPtr
        End Function

        Public Shared Function ShowAlert(owner As Form, msg As String, title As String, type As AlertType, button As ButtonType,
                                         Optional autoClose As Boolean = False, Optional displayDurationSeconds As Integer = 20) As DialogResult
            Dim frm As New JsAlertDialog()
            frm.LabelTitle.Text = title
            frm.LabelMessage.Text = msg
            frm.IconRotation = -90
            frm.IconEntryTimer.Start()
            frm.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, frm.Width, frm.Height, 20, 20))
            frm.TopMost = True
            Select Case type
                Case AlertType.Success
                    frm.LabelTitle.ForeColor = Color.White
                    frm.PanelHeader.BackColor = Color.SeaGreen
                    frm.IconImage = My.Resources.success
                Case AlertType.Error
                    frm.LabelTitle.ForeColor = Color.White
                    frm.PanelHeader.BackColor = Color.Firebrick
                    frm.IconImage = My.Resources._error
                Case AlertType.Warning
                    frm.LabelTitle.ForeColor = Color.White
                    frm.PanelHeader.BackColor = Color.Goldenrod
                    frm.IconImage = My.Resources.warning
                Case AlertType.Info
                    frm.LabelTitle.ForeColor = Color.White
                    frm.PanelHeader.BackColor = Color.SteelBlue
                    frm.IconImage = My.Resources.information
                Case AlertType.Confirm
                    frm.LabelTitle.ForeColor = Color.White
                    frm.PanelHeader.BackColor = Color.DarkSlateGray
                    frm.IconImage = My.Resources.question
            End Select
            frm.PictureIcon.Image = frm.IconImage
            Select Case button
                Case ButtonType.OK
                    frm.BtnCancel.Visible = False
                    frm.BtnOk.Text = "OK"
                    frm.BtnOk.DialogResult = DialogResult.OK
                Case ButtonType.OKCancel
                    frm.BtnCancel.Visible = True
                    frm.BtnOk.Text = "OK"
                    frm.BtnOk.DialogResult = DialogResult.OK
                    frm.BtnCancel.DialogResult = DialogResult.Cancel
                Case ButtonType.YesNo
                    frm.BtnOk.Text = "Yes"
                    frm.BtnOk.BackColor = Color.SeaGreen
                    frm.BtnOk.DialogResult = DialogResult.Yes
                    frm.BtnCancel.Visible = True
                    frm.BtnCancel.Text = "No"
                    frm.BtnCancel.BackColor = Color.Firebrick
                    frm.BtnCancel.DialogResult = DialogResult.No
            End Select
            frm.Opacity = 0

            ' Show dialog first, then start fade in
            If owner IsNot Nothing AndAlso Not owner.IsDisposed Then
                frm.Show(owner)
            Else
                frm.Show()
            End If
            frm.AutoCloseEnabled = autoClose
            frm.AutoCloseSeconds = displayDurationSeconds
            frm.FadeIn.Start()
            If autoClose Then
                frm.InitialMilliseconds = displayDurationSeconds * 1000
                frm.RemainingMilliseconds = frm.InitialMilliseconds
                frm.ProgressAutoClose.Visible = True
                frm.LabelCountdown.Visible = True
                frm.ProgressAutoClose.Minimum = 0
                frm.ProgressAutoClose.Maximum = frm.InitialMilliseconds
                frm.ProgressAutoClose.Value = frm.InitialMilliseconds
                frm.LabelCountdown.Text = $"Closing in {displayDurationSeconds}s"
                frm.AutoCloseTimer.Start()
            End If
            ' Wait until form is closed
            Do While frm.Visible
                Application.DoEvents()
            Loop

            Return frm.AlertResult
        End Function

        Private Sub BtnOK_Click(sender As Object, e As EventArgs) Handles BtnOk.Click
            AutoCloseTimer.Stop()
            AlertResult = BtnOk.DialogResult
            FadeOut.Start()
        End Sub

        Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
            AutoCloseTimer.Stop()
            AlertResult = BtnCancel.DialogResult
            FadeOut.Start()
        End Sub

        ' Fade Animation
        Private Sub FadeIn_Tick(sender As Object, e As EventArgs) Handles FadeIn.Tick
            If Me.Opacity < 1 Then
                Me.Opacity += 0.07
            Else
                FadeIn.Stop()
            End If
        End Sub

        Private Sub FadeOut_Tick(sender As Object, e As EventArgs) Handles FadeOut.Tick
            If Me.Opacity > 0 Then
                Me.Opacity -= 0.07
            Else
                FadeOut.Stop()
                Me.Close()
            End If
        End Sub

        Private Function RotateImage(img As Image, angle As Single) As Image
            Dim bmp As New Bitmap(img.Width, img.Height)
            Using g As Graphics = Graphics.FromImage(bmp)
                g.TranslateTransform(img.Width / 2, img.Height / 2)
                g.RotateTransform(angle)
                g.TranslateTransform(-img.Width / 2, -img.Height / 2)
                g.DrawImage(img, 0, 0)
            End Using
            Return bmp
        End Function
        Private Sub IconEntryTimer_Tick(sender As Object, e As EventArgs) Handles IconEntryTimer.Tick
            If IconRotation < 0 Then
                IconRotation += IconRotationStep
                PictureIcon.Image = RotateImage(IconImage, IconRotation) ' pick correct icon per alert type
            Else
                ' Stop animation
                IconEntryTimer.Stop()
                PictureIcon.Image = IconImage ' final image without rotation
                IconAnimationDone = True
            End If
        End Sub

        Private Sub AutoCloseTimer_Tick(sender As Object, e As EventArgs) Handles AutoCloseTimer.Tick
            RemainingMilliseconds -= AutoCloseTimer.Interval
            If RemainingMilliseconds < 0 Then
                RemainingMilliseconds = 0
            End If
            ProgressAutoClose.Value = RemainingMilliseconds
            Dim secsLeft As Double =
                Math.Ceiling(RemainingMilliseconds / 1000.0)
            LabelCountdown.Text = $"Closing in {secsLeft} second{If(secsLeft = 1, "", "s")}"
            If RemainingMilliseconds <= 0 Then
                AutoCloseTimer.Stop()
                AlertResult = DialogResult.OK
                FadeOut.Start()
            End If
        End Sub

        Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
            Select Case keyData
                Case Keys.Enter
                    If BtnOk.Visible Then
                        BtnOk.PerformClick()
                    Else
                        BtnCancel.PerformClick()
                    End If
                    Return True
                Case Keys.Escape
                    If BtnCancel.Visible Then
                        BtnCancel.PerformClick()
                    Else
                        BtnOk.PerformClick()
                    End If
                    Return True
            End Select
            Return MyBase.ProcessCmdKey(msg, keyData)
        End Function
    End Class
End Namespace