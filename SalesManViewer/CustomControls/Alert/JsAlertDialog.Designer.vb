Namespace CustomControls.Alert
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class JsAlertDialog
        Inherits System.Windows.Forms.Form

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

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.PanelTop = New System.Windows.Forms.TableLayoutPanel()
            Me.PanelHeader = New System.Windows.Forms.Panel()
            Me.LabelTitle = New System.Windows.Forms.Label()
            Me.PictureIcon = New System.Windows.Forms.PictureBox()
            Me.LabelMessage = New System.Windows.Forms.Label()
            Me.LabelCountdown = New System.Windows.Forms.Label()
            Me.ProgressAutoClose = New System.Windows.Forms.ProgressBar()
            Me.ButtonFlow = New System.Windows.Forms.FlowLayoutPanel()
            Me.BtnOk = New System.Windows.Forms.Button()
            Me.BtnCancel = New System.Windows.Forms.Button()
            Me.AutoCloseTimer = New System.Windows.Forms.Timer(Me.components)
            Me.FadeIn = New System.Windows.Forms.Timer(Me.components)
            Me.FadeOut = New System.Windows.Forms.Timer(Me.components)
            Me.IconEntryTimer = New System.Windows.Forms.Timer(Me.components)
            Me.PanelTop.SuspendLayout()
            Me.PanelHeader.SuspendLayout()
            CType(Me.PictureIcon, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.ButtonFlow.SuspendLayout()
            Me.SuspendLayout()
            '
            'PanelTop
            '
            Me.PanelTop.ColumnCount = 1
            Me.PanelTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.PanelTop.Controls.Add(Me.PanelHeader, 0, 0)
            Me.PanelTop.Controls.Add(Me.PictureIcon, 0, 1)
            Me.PanelTop.Controls.Add(Me.LabelMessage, 0, 2)
            Me.PanelTop.Controls.Add(Me.LabelCountdown, 0, 4)
            Me.PanelTop.Controls.Add(Me.ProgressAutoClose, 0, 5)
            Me.PanelTop.Controls.Add(Me.ButtonFlow, 0, 3)
            Me.PanelTop.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PanelTop.Location = New System.Drawing.Point(0, 0)
            Me.PanelTop.Margin = New System.Windows.Forms.Padding(0)
            Me.PanelTop.Name = "PanelTop"
            Me.PanelTop.RowCount = 6
            Me.PanelTop.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.PanelTop.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.PanelTop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.PanelTop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48.0!))
            Me.PanelTop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.PanelTop.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8.0!))
            Me.PanelTop.Size = New System.Drawing.Size(450, 225)
            Me.PanelTop.TabIndex = 7
            '
            'PanelHeader
            '
            Me.PanelHeader.Controls.Add(Me.LabelTitle)
            Me.PanelHeader.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PanelHeader.Location = New System.Drawing.Point(0, 0)
            Me.PanelHeader.Margin = New System.Windows.Forms.Padding(0)
            Me.PanelHeader.Name = "PanelHeader"
            Me.PanelHeader.Size = New System.Drawing.Size(450, 40)
            Me.PanelHeader.TabIndex = 6
            '
            'LabelTitle
            '
            Me.LabelTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LabelTitle.Font = New System.Drawing.Font("Segoe UI", 14.0!, System.Drawing.FontStyle.Bold)
            Me.LabelTitle.Location = New System.Drawing.Point(0, 0)
            Me.LabelTitle.Margin = New System.Windows.Forms.Padding(0)
            Me.LabelTitle.Name = "LabelTitle"
            Me.LabelTitle.Size = New System.Drawing.Size(450, 40)
            Me.LabelTitle.TabIndex = 5
            Me.LabelTitle.Text = "Title"
            Me.LabelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'PictureIcon
            '
            Me.PictureIcon.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.PictureIcon.Location = New System.Drawing.Point(202, 43)
            Me.PictureIcon.Name = "PictureIcon"
            Me.PictureIcon.Size = New System.Drawing.Size(45, 45)
            Me.PictureIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.PictureIcon.TabIndex = 4
            Me.PictureIcon.TabStop = False
            '
            'LabelMessage
            '
            Me.LabelMessage.AutoSize = True
            Me.LabelMessage.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LabelMessage.Font = New System.Drawing.Font("Segoe UI", 9.0!)
            Me.LabelMessage.Location = New System.Drawing.Point(3, 91)
            Me.LabelMessage.MaximumSize = New System.Drawing.Size(400, 0)
            Me.LabelMessage.Name = "LabelMessage"
            Me.LabelMessage.Size = New System.Drawing.Size(400, 58)
            Me.LabelMessage.TabIndex = 3
            Me.LabelMessage.Text = "Message goes here"
            Me.LabelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'LabelCountdown
            '
            Me.LabelCountdown.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.LabelCountdown.ForeColor = System.Drawing.SystemColors.ControlDarkDark
            Me.LabelCountdown.Location = New System.Drawing.Point(3, 197)
            Me.LabelCountdown.Name = "LabelCountdown"
            Me.LabelCountdown.Size = New System.Drawing.Size(444, 20)
            Me.LabelCountdown.TabIndex = 0
            Me.LabelCountdown.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.LabelCountdown.Visible = False
            '
            'ProgressAutoClose
            '
            Me.ProgressAutoClose.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.ProgressAutoClose.Location = New System.Drawing.Point(3, 220)
            Me.ProgressAutoClose.Name = "ProgressAutoClose"
            Me.ProgressAutoClose.Size = New System.Drawing.Size(444, 2)
            Me.ProgressAutoClose.TabIndex = 0
            Me.ProgressAutoClose.Visible = False
            '
            'ButtonFlow
            '
            Me.ButtonFlow.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.ButtonFlow.AutoSize = True
            Me.ButtonFlow.Controls.Add(Me.BtnOk)
            Me.ButtonFlow.Controls.Add(Me.BtnCancel)
            Me.ButtonFlow.Location = New System.Drawing.Point(119, 152)
            Me.ButtonFlow.Name = "ButtonFlow"
            Me.ButtonFlow.Size = New System.Drawing.Size(212, 41)
            Me.ButtonFlow.TabIndex = 2
            Me.ButtonFlow.WrapContents = False
            '
            'BtnOk
            '
            Me.BtnOk.BackColor = System.Drawing.Color.RoyalBlue
            Me.BtnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.BtnOk.ForeColor = System.Drawing.Color.White
            Me.BtnOk.Location = New System.Drawing.Point(3, 3)
            Me.BtnOk.Name = "BtnOk"
            Me.BtnOk.Size = New System.Drawing.Size(100, 35)
            Me.BtnOk.TabIndex = 0
            Me.BtnOk.Text = "OK"
            Me.BtnOk.UseVisualStyleBackColor = False
            '
            'BtnCancel
            '
            Me.BtnCancel.BackColor = System.Drawing.Color.Gray
            Me.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.BtnCancel.ForeColor = System.Drawing.Color.White
            Me.BtnCancel.Location = New System.Drawing.Point(109, 3)
            Me.BtnCancel.Name = "BtnCancel"
            Me.BtnCancel.Size = New System.Drawing.Size(100, 35)
            Me.BtnCancel.TabIndex = 1
            Me.BtnCancel.Text = "Cancel"
            Me.BtnCancel.UseVisualStyleBackColor = False
            Me.BtnCancel.Visible = False
            '
            'AutoCloseTimer
            '
            '
            'FadeIn
            '
            Me.FadeIn.Interval = 20
            '
            'FadeOut
            '
            Me.FadeOut.Interval = 20
            '
            'IconEntryTimer
            '
            Me.IconEntryTimer.Interval = 15
            '
            'JsAlertDialog
            '
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(450, 225)
            Me.Controls.Add(Me.PanelTop)
            Me.DoubleBuffered = True
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "JsAlertDialog"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.PanelTop.ResumeLayout(False)
            Me.PanelTop.PerformLayout()
            Me.PanelHeader.ResumeLayout(False)
            CType(Me.PictureIcon, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ButtonFlow.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents PanelTop As TableLayoutPanel
        Friend WithEvents PanelHeader As Panel
        Friend WithEvents LabelTitle As Label
        Friend WithEvents PictureIcon As PictureBox
        Friend WithEvents LabelMessage As Label
        Friend WithEvents ButtonFlow As FlowLayoutPanel
        Friend WithEvents BtnOk As Button
        Friend WithEvents BtnCancel As Button
        Friend WithEvents FadeIn As Timer
        Friend WithEvents FadeOut As Timer
        Friend WithEvents IconEntryTimer As Timer
        Friend WithEvents ProgressAutoClose As ProgressBar
        Friend WithEvents LabelCountdown As Label
        Friend WithEvents AutoCloseTimer As Timer
    End Class
End Namespace