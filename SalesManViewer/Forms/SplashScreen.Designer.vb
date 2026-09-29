<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SplashScreen
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer
    Private lblLoading As Label
    Private lblBuildInfo As Label
    Private lblDeveloper As Label


    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblBuildInfo = New System.Windows.Forms.Label()
        Me.lblDeveloper = New System.Windows.Forms.Label()
        Me.lblLoading = New System.Windows.Forms.Label()
        Me.LblTitle = New System.Windows.Forms.Label()
        Me.TrailingDotsLoader = New JsToolBox.Loaders.TrailingDotsLoader()
        Me.SuspendLayout()
        '
        'lblBuildInfo
        '
        Me.lblBuildInfo.Location = New System.Drawing.Point(0, 0)
        Me.lblBuildInfo.Name = "lblBuildInfo"
        Me.lblBuildInfo.Size = New System.Drawing.Size(100, 23)
        Me.lblBuildInfo.TabIndex = 0
        '
        'lblDeveloper
        '
        Me.lblDeveloper.Location = New System.Drawing.Point(0, 0)
        Me.lblDeveloper.Name = "lblDeveloper"
        Me.lblDeveloper.Size = New System.Drawing.Size(100, 23)
        Me.lblDeveloper.TabIndex = 0
        '
        'lblLoading
        '
        Me.lblLoading.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.lblLoading.ForeColor = System.Drawing.Color.Gray
        Me.lblLoading.Location = New System.Drawing.Point(0, 113)
        Me.lblLoading.Name = "lblLoading"
        Me.lblLoading.Size = New System.Drawing.Size(350, 40)
        Me.lblLoading.TabIndex = 1
        Me.lblLoading.Text = "Loading"
        Me.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblTitle
        '
        Me.LblTitle.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold)
        Me.LblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.LblTitle.Location = New System.Drawing.Point(0, 3)
        Me.LblTitle.Name = "LblTitle"
        Me.LblTitle.Size = New System.Drawing.Size(350, 40)
        Me.LblTitle.TabIndex = 2
        Me.LblTitle.Text = "Loading"
        Me.LblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TrailingDotsLoader
        '
        Me.TrailingDotsLoader.DotCount = 12
        Me.TrailingDotsLoader.DotSize = 6
        Me.TrailingDotsLoader.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrailingDotsLoader.ForeColor = System.Drawing.SystemColors.ControlDarkDark
        Me.TrailingDotsLoader.LoaderColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.TrailingDotsLoader.Location = New System.Drawing.Point(135, 46)
        Me.TrailingDotsLoader.Name = "TrailingDotsLoader"
        Me.TrailingDotsLoader.Radius = 30
        Me.TrailingDotsLoader.Size = New System.Drawing.Size(72, 72)
        Me.TrailingDotsLoader.Speed = 100
        Me.TrailingDotsLoader.TabIndex = 3
        Me.TrailingDotsLoader.Text = "Loading"
        '
        'SplashScreen
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(350, 180)
        Me.Controls.Add(Me.TrailingDotsLoader)
        Me.Controls.Add(Me.LblTitle)
        Me.Controls.Add(Me.lblLoading)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "SplashScreen"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents LblTitle As Label
    Friend WithEvents TrailingDotsLoader As JsToolBox.Loaders.TrailingDotsLoader
End Class