<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SettingsForm
    Inherits System.Windows.Forms.Form

    Private components As System.ComponentModel.IContainer
    Private TabControlSettings As TabControl
    Private TabPageDB As TabPage
    Private TabPageSystem As TabPage

    ' Database settings controls
    Private WithEvents dgvSettings As DataGridView
    Private WithEvents btnSaveDB As CustomControls.JsButton

    ' System settings controls
    Private WithEvents btnSaveSystem As CustomControls.JsButton

    Private Sub InitializeComponent()
        TabControlSettings = New TabControl()
        TabPageDB = New TabPage()
        dgvSettings = New DataGridView()
        Key = New DataGridViewTextBoxColumn()
        Value = New DataGridViewTextBoxColumn()
        btnSaveDB = New CustomControls.JsButton()
        TabPageSystem = New TabPage()
        Panel1 = New Panel()
        lblServer = New Label()
        Label1 = New Label()
        TxtDbPort = New TextBox()
        btnTest = New CustomControls.JsButton()
        lblUser = New Label()
        btnSaveSystem = New CustomControls.JsButton()
        lblName = New Label()
        TxtDbUser = New TextBox()
        lblPassword = New Label()
        TxtDbServer = New TextBox()
        lblPrefix = New Label()
        TxtDbName = New TextBox()
        TxtDbPrefix = New TextBox()
        txtDbPassword = New TextBox()
        PanelPassword = New Panel()
        PicTogglePassword = New PictureBox()
        TabControlSettings.SuspendLayout()
        TabPageDB.SuspendLayout()
        CType(dgvSettings, ComponentModel.ISupportInitialize).BeginInit()
        TabPageSystem.SuspendLayout()
        Panel1.SuspendLayout()
        PanelPassword.SuspendLayout()
        CType(PicTogglePassword, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' TabControlSettings
        ' 
        TabControlSettings.Controls.Add(TabPageDB)
        TabControlSettings.Controls.Add(TabPageSystem)
        TabControlSettings.Dock = DockStyle.Fill
        TabControlSettings.Location = New Point(0, 0)
        TabControlSettings.Name = "TabControlSettings"
        TabControlSettings.SelectedIndex = 0
        TabControlSettings.Size = New Size(469, 421)
        TabControlSettings.TabIndex = 0
        ' 
        ' TabPageDB
        ' 
        TabPageDB.Controls.Add(dgvSettings)
        TabPageDB.Controls.Add(btnSaveDB)
        TabPageDB.Location = New Point(4, 24)
        TabPageDB.Name = "TabPageDB"
        TabPageDB.Padding = New Padding(10)
        TabPageDB.Size = New Size(461, 393)
        TabPageDB.TabIndex = 0
        TabPageDB.Text = "Application Settings"
        ' 
        ' dgvSettings
        ' 
        dgvSettings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvSettings.Columns.AddRange(New DataGridViewColumn() {Key, Value})
        dgvSettings.Dock = DockStyle.Top
        dgvSettings.Location = New Point(10, 10)
        dgvSettings.Name = "dgvSettings"
        dgvSettings.Size = New Size(441, 336)
        dgvSettings.TabIndex = 0
        ' 
        ' Key
        ' 
        Key.HeaderText = "Key"
        Key.Name = "Key"
        ' 
        ' Value
        ' 
        Value.HeaderText = "Value"
        Value.Name = "Value"
        ' 
        ' btnSaveDB
        ' 
        btnSaveDB.BackColor = Color.FromArgb(CByte(25), CByte(135), CByte(84))
        btnSaveDB.BorderRadius = 14
        btnSaveDB.ButtonSize = CustomControls.JsButton.BootstrapSize.Normal
        btnSaveDB.ButtonStyle = CustomControls.JsButton.BootstrapStyle.Success
        btnSaveDB.Dock = DockStyle.Bottom
        btnSaveDB.FlatAppearance.BorderColor = Color.Navy
        btnSaveDB.FlatAppearance.BorderSize = 0
        btnSaveDB.FlatAppearance.MouseDownBackColor = Color.Navy
        btnSaveDB.FlatAppearance.MouseOverBackColor = Color.Navy
        btnSaveDB.FlatStyle = FlatStyle.Flat
        btnSaveDB.Font = New Font("Segoe UI", 9.0F)
        btnSaveDB.ForeColor = Color.White
        btnSaveDB.HoverColor = Color.Empty
        btnSaveDB.Location = New Point(10, 353)
        btnSaveDB.Name = "btnSaveDB"
        btnSaveDB.Outline = False
        btnSaveDB.PressedColor = Color.Empty
        btnSaveDB.Size = New Size(441, 30)
        btnSaveDB.TabIndex = 1
        btnSaveDB.Text = "Save"
        btnSaveDB.UseVisualStyleBackColor = False
        ' 
        ' TabPageSystem
        ' 
        TabPageSystem.Controls.Add(Panel1)
        TabPageSystem.Location = New Point(4, 24)
        TabPageSystem.Name = "TabPageSystem"
        TabPageSystem.Padding = New Padding(10)
        TabPageSystem.Size = New Size(461, 393)
        TabPageSystem.TabIndex = 1
        TabPageSystem.Text = "System Settings"
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(PanelPassword)
        Panel1.Controls.Add(lblServer)
        Panel1.Controls.Add(Label1)
        Panel1.Controls.Add(TxtDbPort)
        Panel1.Controls.Add(btnTest)
        Panel1.Controls.Add(lblUser)
        Panel1.Controls.Add(btnSaveSystem)
        Panel1.Controls.Add(lblName)
        Panel1.Controls.Add(TxtDbUser)
        Panel1.Controls.Add(lblPassword)
        Panel1.Controls.Add(TxtDbServer)
        Panel1.Controls.Add(lblPrefix)
        Panel1.Controls.Add(TxtDbName)
        Panel1.Controls.Add(TxtDbPrefix)
        Panel1.Location = New Point(3, 13)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(444, 367)
        Panel1.TabIndex = 14
        ' 
        ' lblServer
        ' 
        lblServer.AutoSize = True
        lblServer.Location = New Point(105, 12)
        lblServer.Name = "lblServer"
        lblServer.Size = New Size(45, 15)
        lblServer.TabIndex = 6
        lblServer.Text = "Server :"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(96, 41)
        Label1.Name = "Label1"
        Label1.Size = New Size(54, 15)
        Label1.TabIndex = 12
        Label1.Text = "Port No :"
        ' 
        ' TxtDbPort
        ' 
        TxtDbPort.Location = New Point(156, 38)
        TxtDbPort.Name = "TxtDbPort"
        TxtDbPort.Size = New Size(263, 23)
        TxtDbPort.TabIndex = 11
        ' 
        ' btnTest
        ' 
        btnTest.BackColor = Color.FromArgb(CByte(64), CByte(224), CByte(208))
        btnTest.BorderRadius = 14
        btnTest.ButtonSize = CustomControls.JsButton.BootstrapSize.Normal
        btnTest.ButtonStyle = CustomControls.JsButton.BootstrapStyle.Primary
        btnTest.FlatAppearance.BorderSize = 0
        btnTest.Font = New Font("Segoe UI", 9.0F)
        btnTest.ForeColor = Color.White
        btnTest.HoverColor = Color.Empty
        btnTest.Location = New Point(10, 187)
        btnTest.Name = "btnTest"
        btnTest.Outline = False
        btnTest.PressedColor = Color.Empty
        btnTest.Size = New Size(100, 30)
        btnTest.TabIndex = 13
        btnTest.TabStop = False
        btnTest.Text = "Test Connection"
        btnTest.UseVisualStyleBackColor = False
        ' 
        ' lblUser
        ' 
        lblUser.AutoSize = True
        lblUser.Location = New Point(54, 70)
        lblUser.Name = "lblUser"
        lblUser.Size = New Size(96, 15)
        lblUser.TabIndex = 7
        lblUser.Text = "Database Name :"
        ' 
        ' btnSaveSystem
        ' 
        btnSaveSystem.BackColor = Color.FromArgb(CByte(25), CByte(135), CByte(84))
        btnSaveSystem.BorderRadius = 14
        btnSaveSystem.ButtonSize = CustomControls.JsButton.BootstrapSize.Normal
        btnSaveSystem.ButtonStyle = CustomControls.JsButton.BootstrapStyle.Success
        btnSaveSystem.FlatAppearance.BorderSize = 0
        btnSaveSystem.Font = New Font("Segoe UI", 9.0F)
        btnSaveSystem.ForeColor = Color.White
        btnSaveSystem.HoverColor = Color.Empty
        btnSaveSystem.Location = New Point(319, 187)
        btnSaveSystem.Name = "btnSaveSystem"
        btnSaveSystem.Outline = False
        btnSaveSystem.PressedColor = Color.Empty
        btnSaveSystem.Size = New Size(100, 30)
        btnSaveSystem.TabIndex = 5
        btnSaveSystem.Text = "Save"
        btnSaveSystem.UseVisualStyleBackColor = False
        ' 
        ' lblName
        ' 
        lblName.AutoSize = True
        lblName.Location = New Point(28, 128)
        lblName.Name = "lblName"
        lblName.Size = New Size(122, 15)
        lblName.TabIndex = 8
        lblName.Text = "Database User Name :"
        ' 
        ' txtDbUser
        ' 
        TxtDbUser.Location = New Point(156, 125)
        TxtDbUser.Name = "txtDbUser"
        TxtDbUser.Size = New Size(263, 23)
        TxtDbUser.TabIndex = 1
        ' 
        ' lblPassword
        ' 
        lblPassword.AutoSize = True
        lblPassword.Location = New Point(10, 154)
        lblPassword.Name = "lblPassword"
        lblPassword.Size = New Size(140, 15)
        lblPassword.TabIndex = 9
        lblPassword.Text = "Database User Password :"
        ' 
        ' txtDbServer
        ' 
        TxtDbServer.Location = New Point(156, 9)
        TxtDbServer.Name = "txtDbServer"
        TxtDbServer.Size = New Size(263, 23)
        TxtDbServer.TabIndex = 0
        ' 
        ' lblPrefix
        ' 
        lblPrefix.AutoSize = True
        lblPrefix.Location = New Point(56, 99)
        lblPrefix.Name = "lblPrefix"
        lblPrefix.Size = New Size(94, 15)
        lblPrefix.TabIndex = 10
        lblPrefix.Text = "Database Prefix :"
        ' 
        ' txtDbName
        ' 
        TxtDbName.Location = New Point(156, 67)
        TxtDbName.Name = "txtDbName"
        TxtDbName.Size = New Size(263, 23)
        TxtDbName.TabIndex = 2
        ' 
        ' txtDbPrefix
        ' 
        TxtDbPrefix.Location = New Point(156, 96)
        TxtDbPrefix.Name = "txtDbPrefix"
        TxtDbPrefix.Size = New Size(263, 23)
        TxtDbPrefix.TabIndex = 4
        ' 
        ' txtDbPassword
        ' 
        txtDbPassword.BorderStyle = BorderStyle.None
        txtDbPassword.Location = New Point(6, 5)
        txtDbPassword.Margin = New Padding(4, 3, 4, 3)
        txtDbPassword.Name = "txtDbPassword"
        txtDbPassword.Size = New Size(210, 16)
        txtDbPassword.TabIndex = 0
        txtDbPassword.UseSystemPasswordChar = True
        ' 
        ' PanelPassword
        ' 
        PanelPassword.BackColor = Color.White
        PanelPassword.BorderStyle = BorderStyle.FixedSingle
        PanelPassword.Controls.Add(txtDbPassword)
        PanelPassword.Controls.Add(PicTogglePassword)
        PanelPassword.Location = New Point(156, 154)
        PanelPassword.Margin = New Padding(4, 3, 4, 3)
        PanelPassword.Name = "PanelPassword"
        PanelPassword.Size = New Size(263, 27)
        PanelPassword.TabIndex = 14
        ' 
        ' PicTogglePassword
        ' 
        PicTogglePassword.Cursor = Cursors.Hand
        PicTogglePassword.Image = My.Resources.Resources.eye_closed_outline
        PicTogglePassword.Location = New Point(233, 3)
        PicTogglePassword.Margin = New Padding(4, 3, 4, 3)
        PicTogglePassword.Name = "PicTogglePassword"
        PicTogglePassword.Size = New Size(21, 21)
        PicTogglePassword.SizeMode = PictureBoxSizeMode.Zoom
        PicTogglePassword.TabIndex = 1
        PicTogglePassword.TabStop = False
        ' 
        ' SettingsForm
        ' 
        ClientSize = New Size(469, 421)
        Controls.Add(TabControlSettings)
        MaximizeBox = False
        Name = "SettingsForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Settings"
        TabControlSettings.ResumeLayout(False)
        TabPageDB.ResumeLayout(False)
        CType(dgvSettings, ComponentModel.ISupportInitialize).EndInit()
        TabPageSystem.ResumeLayout(False)
        Panel1.ResumeLayout(False)
        Panel1.PerformLayout()
        PanelPassword.ResumeLayout(False)
        PanelPassword.PerformLayout()
        CType(PicTogglePassword, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)

    End Sub
    Friend WithEvents lblServer As Label
    Friend WithEvents lblUser As Label
    Friend WithEvents lblName As Label
    Friend WithEvents lblPassword As Label
    Friend WithEvents lblPrefix As Label
    Friend WithEvents DtgvKey As DataGridViewTextBoxColumn
    Friend WithEvents DtgvValue As DataGridViewTextBoxColumn
    Friend WithEvents Key As DataGridViewTextBoxColumn
    Friend WithEvents Value As DataGridViewTextBoxColumn
    Friend WithEvents TxtDbServer As TextBox
    Friend WithEvents TxtDbPort As TextBox
    Friend WithEvents TxtDbName As TextBox
    Friend WithEvents TxtDbPrefix As TextBox
    Friend WithEvents TxtDbUser As TextBox
    Friend WithEvents Label1 As Label
    Private WithEvents btnTest As CustomControls.JsButton
    Friend WithEvents Panel1 As Panel
    Friend WithEvents PanelPassword As Panel
    Friend WithEvents PicTogglePassword As PictureBox
    Private WithEvents txtDbPassword As TextBox
End Class