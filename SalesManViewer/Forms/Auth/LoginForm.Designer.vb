<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LoginForm
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
        Me.LblPassword = New System.Windows.Forms.Label()
        Me.BtnForgotPassword = New System.Windows.Forms.LinkLabel()
        Me.PanelPassword = New System.Windows.Forms.Panel()
        Me.TxtPassword = New System.Windows.Forms.TextBox()
        Me.PicTogglePassword = New System.Windows.Forms.PictureBox()
        Me.TxtUserName = New System.Windows.Forms.TextBox()
        Me.Spinner = New JsToolBox.Loaders.TrailingDotsLoader()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.BtnLogin = New SalesManViewer.CustomControls.JsButton()
        Me.PanelPassword.SuspendLayout()
        CType(Me.PicTogglePassword, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'LblUserName
        '
        Me.LblUserName.AutoSize = True
        Me.LblUserName.Location = New System.Drawing.Point(24, 22)
        Me.LblUserName.Name = "LblUserName"
        Me.LblUserName.Size = New System.Drawing.Size(63, 13)
        Me.LblUserName.TabIndex = 5
        Me.LblUserName.Text = "User Name:"
        '
        'LblPassword
        '
        Me.LblPassword.AutoSize = True
        Me.LblPassword.Location = New System.Drawing.Point(27, 55)
        Me.LblPassword.Name = "LblPassword"
        Me.LblPassword.Size = New System.Drawing.Size(56, 13)
        Me.LblPassword.TabIndex = 6
        Me.LblPassword.Text = "Password:"
        '
        'BtnForgotPassword
        '
        Me.BtnForgotPassword.AutoSize = True
        Me.BtnForgotPassword.Location = New System.Drawing.Point(93, 89)
        Me.BtnForgotPassword.Name = "BtnForgotPassword"
        Me.BtnForgotPassword.Size = New System.Drawing.Size(86, 13)
        Me.BtnForgotPassword.TabIndex = 3
        Me.BtnForgotPassword.TabStop = True
        Me.BtnForgotPassword.Text = "Forgot Password"
        '
        'PanelPassword
        '
        Me.PanelPassword.BackColor = System.Drawing.Color.White
        Me.PanelPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelPassword.Controls.Add(Me.TxtPassword)
        Me.PanelPassword.Controls.Add(Me.PicTogglePassword)
        Me.PanelPassword.Location = New System.Drawing.Point(96, 51)
        Me.PanelPassword.Name = "PanelPassword"
        Me.PanelPassword.Size = New System.Drawing.Size(226, 24)
        Me.PanelPassword.TabIndex = 1
        '
        'TxtPassword
        '
        Me.TxtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtPassword.Location = New System.Drawing.Point(5, 4)
        Me.TxtPassword.Name = "TxtPassword"
        Me.TxtPassword.Size = New System.Drawing.Size(180, 13)
        Me.TxtPassword.TabIndex = 0
        Me.TxtPassword.UseSystemPasswordChar = True
        '
        'PicTogglePassword
        '
        Me.PicTogglePassword.Cursor = System.Windows.Forms.Cursors.Hand
        Me.PicTogglePassword.Image = Global.SalesManViewer.My.Resources.Resources.eye_closed_outline
        Me.PicTogglePassword.Location = New System.Drawing.Point(200, 3)
        Me.PicTogglePassword.Name = "PicTogglePassword"
        Me.PicTogglePassword.Size = New System.Drawing.Size(18, 18)
        Me.PicTogglePassword.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.PicTogglePassword.TabIndex = 1
        Me.PicTogglePassword.TabStop = False
        '
        'TxtUserName
        '
        Me.TxtUserName.Location = New System.Drawing.Point(96, 17)
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
        Me.Spinner.Location = New System.Drawing.Point(216, 67)
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
        Me.Panel1.Controls.Add(Me.Spinner)
        Me.Panel1.Controls.Add(Me.BtnLogin)
        Me.Panel1.Location = New System.Drawing.Point(10, 10)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(319, 149)
        Me.Panel1.TabIndex = 8
        '
        'BtnLogin
        '
        Me.BtnLogin.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
        Me.BtnLogin.BorderRadius = 14
        Me.BtnLogin.ButtonSize = SalesManViewer.CustomControls.JsButton.BootstrapSize.Small
        Me.BtnLogin.ButtonStyle = SalesManViewer.CustomControls.JsButton.BootstrapStyle.Success
        Me.BtnLogin.FlatAppearance.BorderSize = 0
        Me.BtnLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.BtnLogin.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.BtnLogin.ForeColor = System.Drawing.Color.White
        Me.BtnLogin.HoverColor = System.Drawing.Color.Empty
        Me.BtnLogin.Location = New System.Drawing.Point(47, 111)
        Me.BtnLogin.Name = "BtnLogin"
        Me.BtnLogin.Outline = False
        Me.BtnLogin.PressedColor = System.Drawing.Color.Empty
        Me.BtnLogin.Size = New System.Drawing.Size(120, 31)
        Me.BtnLogin.TabIndex = 0
        Me.BtnLogin.Text = "Login"
        Me.BtnLogin.UseVisualStyleBackColor = False
        '
        'LoginForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Honeydew
        Me.ClientSize = New System.Drawing.Size(343, 173)
        Me.ControlBox = False
        Me.Controls.Add(Me.TxtUserName)
        Me.Controls.Add(Me.PanelPassword)
        Me.Controls.Add(Me.BtnForgotPassword)
        Me.Controls.Add(Me.LblPassword)
        Me.Controls.Add(Me.LblUserName)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "LoginForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Loading"
        Me.PanelPassword.ResumeLayout(False)
        Me.PanelPassword.PerformLayout()
        CType(Me.PicTogglePassword, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LblUserName As Label
    Friend WithEvents LblPassword As Label
    Friend WithEvents BtnForgotPassword As LinkLabel
    Friend WithEvents TxtUserName As TextBox
    Friend WithEvents PanelPassword As Panel
    Friend WithEvents TxtPassword As TextBox
    Friend WithEvents PicTogglePassword As PictureBox
    Friend WithEvents Spinner As JsToolBox.Loaders.TrailingDotsLoader
    Friend WithEvents Panel1 As Panel
    Friend WithEvents BtnLogin As CustomControls.JsButton
End Class