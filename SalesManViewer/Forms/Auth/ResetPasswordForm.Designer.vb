<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ResetPasswordForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.LblUserName = New System.Windows.Forms.Label()
        Me.LblOldPassword = New System.Windows.Forms.Label()
        Me.BtnLoginInstead = New System.Windows.Forms.LinkLabel()
        Me.PanelOldPassword = New System.Windows.Forms.Panel()
        Me.TxtOldPassword = New System.Windows.Forms.TextBox()
        Me.PicToggleOldPassword = New System.Windows.Forms.PictureBox()
        Me.TxtUserName = New System.Windows.Forms.TextBox()
        Me.Spinner = New JsToolBox.Loaders.TrailingDotsLoader()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LblConfirmPassword = New System.Windows.Forms.Label()
        Me.PanelConfirmPassword = New System.Windows.Forms.Panel()
        Me.TxtConfirmPassword = New System.Windows.Forms.TextBox()
        Me.PicToggleConfirmPassword = New System.Windows.Forms.PictureBox()
        Me.LblNewPassword = New System.Windows.Forms.Label()
        Me.PanelNewPassword = New System.Windows.Forms.Panel()
        Me.TxtNewPassword = New System.Windows.Forms.TextBox()
        Me.PicToggleNewPassword = New System.Windows.Forms.PictureBox()
        Me.BtnReset = New SalesManViewer.CustomControls.JsButton()
        Me.PanelOldPassword.SuspendLayout()
        CType(Me.PicToggleOldPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.PanelConfirmPassword.SuspendLayout()
        CType(Me.PicToggleConfirmPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelNewPassword.SuspendLayout()
        CType(Me.PicToggleNewPassword, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LblUserName
        '
        Me.LblUserName.AutoSize = True
        Me.LblUserName.Location = New System.Drawing.Point(15, 8)
        Me.LblUserName.Name = "LblUserName"
        Me.LblUserName.Size = New System.Drawing.Size(63, 13)
        Me.LblUserName.TabIndex = 5
        Me.LblUserName.Text = "User Name:"
        '
        'LblOldPassword
        '
        Me.LblOldPassword.AutoSize = True
        Me.LblOldPassword.Location = New System.Drawing.Point(15, 42)
        Me.LblOldPassword.Name = "LblOldPassword"
        Me.LblOldPassword.Size = New System.Drawing.Size(75, 13)
        Me.LblOldPassword.TabIndex = 6
        Me.LblOldPassword.Text = "Old Password:"
        '
        'BtnLoginInstead
        '
        Me.BtnLoginInstead.AutoSize = True
        Me.BtnLoginInstead.Location = New System.Drawing.Point(36, 183)
        Me.BtnLoginInstead.Name = "BtnLoginInstead"
        Me.BtnLoginInstead.Size = New System.Drawing.Size(73, 13)
        Me.BtnLoginInstead.TabIndex = 3
        Me.BtnLoginInstead.TabStop = True
        Me.BtnLoginInstead.Text = "Back to Login"
        '
        'PanelOldPassword
        '
        Me.PanelOldPassword.BackColor = System.Drawing.Color.White
        Me.PanelOldPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelOldPassword.Controls.Add(Me.TxtOldPassword)
        Me.PanelOldPassword.Controls.Add(Me.PicToggleOldPassword)
        Me.PanelOldPassword.Location = New System.Drawing.Point(115, 31)
        Me.PanelOldPassword.Name = "PanelOldPassword"
        Me.PanelOldPassword.Size = New System.Drawing.Size(226, 24)
        Me.PanelOldPassword.TabIndex = 1
        '
        'TxtOldPassword
        '
        Me.TxtOldPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtOldPassword.Location = New System.Drawing.Point(5, 4)
        Me.TxtOldPassword.Name = "TxtOldPassword"
        Me.TxtOldPassword.Size = New System.Drawing.Size(180, 13)
        Me.TxtOldPassword.TabIndex = 0
        Me.TxtOldPassword.UseSystemPasswordChar = True
        '
        'PicToggleOldPassword
        '
        Me.PicToggleOldPassword.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PicToggleOldPassword.Image = Global.SalesManViewer.My.Resources.Resources.eye_closed_outline
        Me.PicToggleOldPassword.Location = New System.Drawing.Point(200, 3)
        Me.PicToggleOldPassword.Name = "PicToggleOldPassword"
        Me.PicToggleOldPassword.Size = New System.Drawing.Size(18, 18)
        Me.PicToggleOldPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PicToggleOldPassword.TabIndex = 1
        Me.PicToggleOldPassword.TabStop = False
        '
        'TxtUserName
        '
        Me.TxtUserName.Location = New System.Drawing.Point(115, 5)
        Me.TxtUserName.Name = "TxtUserName"
        Me.TxtUserName.Size = New System.Drawing.Size(226, 20)
        Me.TxtUserName.TabIndex = 0
        '
        'Spinner
        '
        Me.Spinner.DotCount = 12
        Me.Spinner.DotSize = 6
        Me.Spinner.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Spinner.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Spinner.LoaderColor = System.Drawing.Color.Green
        Me.Spinner.Location = New System.Drawing.Point(266, 121)
        Me.Spinner.Name = "Spinner"
        Me.Spinner.Radius = 32
        Me.Spinner.Size = New System.Drawing.Size(75, 75)
        Me.Spinner.Speed = 100
        Me.Spinner.TabIndex = 7
        Me.Spinner.Text = "Processing"
        Me.Spinner.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.LblConfirmPassword)
        Me.Panel1.Controls.Add(Me.PanelConfirmPassword)
        Me.Panel1.Controls.Add(Me.LblUserName)
        Me.Panel1.Controls.Add(Me.LblOldPassword)
        Me.Panel1.Controls.Add(Me.TxtUserName)
        Me.Panel1.Controls.Add(Me.LblNewPassword)
        Me.Panel1.Controls.Add(Me.PanelOldPassword)
        Me.Panel1.Controls.Add(Me.Spinner)
        Me.Panel1.Controls.Add(Me.PanelNewPassword)
        Me.Panel1.Controls.Add(Me.BtnReset)
        Me.Panel1.Controls.Add(Me.BtnLoginInstead)
        Me.Panel1.Location = New System.Drawing.Point(10, 10)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(350, 210)
        Me.Panel1.TabIndex = 8
        '
        'LblConfirmPassword
        '
        Me.LblConfirmPassword.AutoSize = True
        Me.LblConfirmPassword.Location = New System.Drawing.Point(15, 95)
        Me.LblConfirmPassword.Name = "LblConfirmPassword"
        Me.LblConfirmPassword.Size = New System.Drawing.Size(94, 13)
        Me.LblConfirmPassword.TabIndex = 12
        Me.LblConfirmPassword.Text = "Confirm Password:"
        '
        'PanelConfirmPassword
        '
        Me.PanelConfirmPassword.BackColor = System.Drawing.Color.White
        Me.PanelConfirmPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelConfirmPassword.Controls.Add(Me.TxtConfirmPassword)
        Me.PanelConfirmPassword.Controls.Add(Me.PicToggleConfirmPassword)
        Me.PanelConfirmPassword.Location = New System.Drawing.Point(115, 91)
        Me.PanelConfirmPassword.Name = "PanelConfirmPassword"
        Me.PanelConfirmPassword.Size = New System.Drawing.Size(226, 24)
        Me.PanelConfirmPassword.TabIndex = 11
        '
        'TxtConfirmPassword
        '
        Me.TxtConfirmPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtConfirmPassword.Location = New System.Drawing.Point(5, 4)
        Me.TxtConfirmPassword.Name = "TxtConfirmPassword"
        Me.TxtConfirmPassword.Size = New System.Drawing.Size(180, 13)
        Me.TxtConfirmPassword.TabIndex = 0
        Me.TxtConfirmPassword.UseSystemPasswordChar = True
        '
        'PicToggleConfirmPassword
        '
        Me.PicToggleConfirmPassword.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PicToggleConfirmPassword.Image = Global.SalesManViewer.My.Resources.Resources.eye_closed_outline
        Me.PicToggleConfirmPassword.Location = New System.Drawing.Point(200, 3)
        Me.PicToggleConfirmPassword.Name = "PicToggleConfirmPassword"
        Me.PicToggleConfirmPassword.Size = New System.Drawing.Size(18, 18)
        Me.PicToggleConfirmPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PicToggleConfirmPassword.TabIndex = 1
        Me.PicToggleConfirmPassword.TabStop = False
        '
        'LblNewPassword
        '
        Me.LblNewPassword.AutoSize = True
        Me.LblNewPassword.Location = New System.Drawing.Point(15, 66)
        Me.LblNewPassword.Name = "LblNewPassword"
        Me.LblNewPassword.Size = New System.Drawing.Size(81, 13)
        Me.LblNewPassword.TabIndex = 10
        Me.LblNewPassword.Text = "New Password:"
        '
        'PanelNewPassword
        '
        Me.PanelNewPassword.BackColor = System.Drawing.Color.White
        Me.PanelNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelNewPassword.Controls.Add(Me.TxtNewPassword)
        Me.PanelNewPassword.Controls.Add(Me.PicToggleNewPassword)
        Me.PanelNewPassword.Location = New System.Drawing.Point(115, 61)
        Me.PanelNewPassword.Name = "PanelNewPassword"
        Me.PanelNewPassword.Size = New System.Drawing.Size(226, 24)
        Me.PanelNewPassword.TabIndex = 9
        '
        'TxtNewPassword
        '
        Me.TxtNewPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtNewPassword.Location = New System.Drawing.Point(5, 4)
        Me.TxtNewPassword.Name = "TxtNewPassword"
        Me.TxtNewPassword.Size = New System.Drawing.Size(180, 13)
        Me.TxtNewPassword.TabIndex = 0
        Me.TxtNewPassword.UseSystemPasswordChar = True
        '
        'PicToggleNewPassword
        '
        Me.PicToggleNewPassword.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PicToggleNewPassword.Image = Global.SalesManViewer.My.Resources.Resources.eye_closed_outline
        Me.PicToggleNewPassword.Location = New System.Drawing.Point(200, 3)
        Me.PicToggleNewPassword.Name = "PicToggleNewPassword"
        Me.PicToggleNewPassword.Size = New System.Drawing.Size(18, 18)
        Me.PicToggleNewPassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PicToggleNewPassword.TabIndex = 1
        Me.PicToggleNewPassword.TabStop = False
        '
        'BtnReset
        '
        Me.BtnReset.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
        Me.BtnReset.BorderRadius = 14
        Me.BtnReset.ButtonSize = SalesManViewer.CustomControls.JsButton.BootstrapSize.Small
        Me.BtnReset.ButtonStyle = SalesManViewer.CustomControls.JsButton.BootstrapStyle.Success
        Me.BtnReset.FlatAppearance.BorderSize = 0
        Me.BtnReset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.BtnReset.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.BtnReset.ForeColor = System.Drawing.Color.White
        Me.BtnReset.HoverColor = System.Drawing.Color.Empty
        Me.BtnReset.Location = New System.Drawing.Point(18, 135)
        Me.BtnReset.Name = "BtnReset"
        Me.BtnReset.Outline = False
        Me.BtnReset.PressedColor = System.Drawing.Color.Empty
        Me.BtnReset.Size = New System.Drawing.Size(120, 31)
        Me.BtnReset.TabIndex = 0
        Me.BtnReset.Text = "Reset"
        Me.BtnReset.UseVisualStyleBackColor = False
        '
        'ResetPasswordForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Honeydew
        Me.ClientSize = New System.Drawing.Size(370, 230)
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ResetPasswordForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Loading"
        Me.PanelOldPassword.ResumeLayout(False)
        Me.PanelOldPassword.PerformLayout()
        CType(Me.PicToggleOldPassword, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PanelConfirmPassword.ResumeLayout(False)
        Me.PanelConfirmPassword.PerformLayout()
        CType(Me.PicToggleConfirmPassword, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelNewPassword.ResumeLayout(False)
        Me.PanelNewPassword.PerformLayout()
        CType(Me.PicToggleNewPassword, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LblUserName As Label
    Friend WithEvents LblOldPassword As Label
    Friend WithEvents BtnLoginInstead As LinkLabel
    Friend WithEvents TxtUserName As TextBox
    Friend WithEvents PanelOldPassword As Panel
    Friend WithEvents TxtOldPassword As TextBox
    Friend WithEvents PicToggleOldPassword As PictureBox
    Friend WithEvents Spinner As JsToolBox.Loaders.TrailingDotsLoader
    Friend WithEvents Panel1 As Panel
    Friend WithEvents BtnReset As CustomControls.JsButton
    Friend WithEvents LblNewPassword As Label
    Friend WithEvents PanelNewPassword As Panel
    Friend WithEvents TxtNewPassword As TextBox
    Friend WithEvents PicToggleNewPassword As PictureBox
    Friend WithEvents LblConfirmPassword As Label
    Friend WithEvents PanelConfirmPassword As Panel
    Friend WithEvents TxtConfirmPassword As TextBox
    Friend WithEvents PicToggleConfirmPassword As PictureBox
End Class