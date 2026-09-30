<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ForgotPasswordForm
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
        Me.TxtUserName = New System.Windows.Forms.TextBox()
        Me.LblUserName = New System.Windows.Forms.Label()
        Me.BtnLoginInstead = New System.Windows.Forms.LinkLabel()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.BtnCheck = New SalesManViewer.CustomControls.JsButton()
        Me.Loader = New JsToolBox.Loaders.TrailingDotsLoader()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TxtUserName
        '
        Me.TxtUserName.Location = New System.Drawing.Point(81, 21)
        Me.TxtUserName.Name = "TxtUserName"
        Me.TxtUserName.Size = New System.Drawing.Size(226, 20)
        Me.TxtUserName.TabIndex = 6
        '
        'LblUserName
        '
        Me.LblUserName.AutoSize = True
        Me.LblUserName.Location = New System.Drawing.Point(9, 21)
        Me.LblUserName.Name = "LblUserName"
        Me.LblUserName.Size = New System.Drawing.Size(63, 13)
        Me.LblUserName.TabIndex = 5
        Me.LblUserName.Text = "User Name:"
        '
        'BtnLoginInstead
        '
        Me.BtnLoginInstead.AutoSize = True
        Me.BtnLoginInstead.Location = New System.Drawing.Point(33, 103)
        Me.BtnLoginInstead.Name = "BtnLoginInstead"
        Me.BtnLoginInstead.Size = New System.Drawing.Size(73, 13)
        Me.BtnLoginInstead.TabIndex = 7
        Me.BtnLoginInstead.TabStop = True
        Me.BtnLoginInstead.Text = "Back to Login"
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.BtnLoginInstead)
        Me.Panel1.Controls.Add(Me.BtnCheck)
        Me.Panel1.Controls.Add(Me.Loader)
        Me.Panel1.Location = New System.Drawing.Point(7, 6)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(312, 155)
        Me.Panel1.TabIndex = 10
        '
        'BtnCheck
        '
        Me.BtnCheck.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
        Me.BtnCheck.BorderRadius = 14
        Me.BtnCheck.ButtonSize = SalesManViewer.CustomControls.JsButton.BootstrapSize.Small
        Me.BtnCheck.ButtonStyle = SalesManViewer.CustomControls.JsButton.BootstrapStyle.Success
        Me.BtnCheck.FlatAppearance.BorderSize = 0
        Me.BtnCheck.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent
        Me.BtnCheck.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent
        Me.BtnCheck.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.BtnCheck.ForeColor = System.Drawing.Color.White
        Me.BtnCheck.HoverColor = System.Drawing.Color.Empty
        Me.BtnCheck.Location = New System.Drawing.Point(13, 69)
        Me.BtnCheck.Name = "BtnCheck"
        Me.BtnCheck.Outline = False
        Me.BtnCheck.PressedColor = System.Drawing.Color.Empty
        Me.BtnCheck.Size = New System.Drawing.Size(120, 31)
        Me.BtnCheck.TabIndex = 9
        Me.BtnCheck.Text = "Check"
        Me.BtnCheck.UseVisualStyleBackColor = False
        '
        'Loader
        '
        Me.Loader.DotCount = 12
        Me.Loader.DotSize = 6
        Me.Loader.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Loader.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.Loader.LoaderColor = System.Drawing.Color.Green
        Me.Loader.Location = New System.Drawing.Point(208, 41)
        Me.Loader.Name = "Loader"
        Me.Loader.Radius = 32
        Me.Loader.Size = New System.Drawing.Size(75, 75)
        Me.Loader.Speed = 100
        Me.Loader.TabIndex = 8
        Me.Loader.Text = "Processing"
        Me.Loader.Visible = False
        '
        'ForgotPasswordForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Honeydew
        Me.ClientSize = New System.Drawing.Size(326, 173)
        Me.Controls.Add(Me.TxtUserName)
        Me.Controls.Add(Me.LblUserName)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ForgotPasswordForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ForgotPasswordForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents TxtUserName As TextBox
    Friend WithEvents LblUserName As Label
    Friend WithEvents BtnLoginInstead As LinkLabel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents Loader As JsToolBox.Loaders.TrailingDotsLoader
    Friend WithEvents BtnCheck As CustomControls.JsButton
End Class