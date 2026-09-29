<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HomeForm
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
        Me.TableLayoutPanelMain = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelContent = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanelHeader = New System.Windows.Forms.TableLayoutPanel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripTimeLabel = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripDateLabel = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ToolStrip2 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripStatusLabelBreadcrumb = New System.Windows.Forms.ToolStripLabel()
        Me.ToolStripUserLabel = New System.Windows.Forms.ToolStripLabel()
        Me.DashboardPanel = New System.Windows.Forms.Panel()
        Me.MasterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SalesmanTrackerToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TableLayoutPanelMain.SuspendLayout()
        Me.TableLayoutPanelContent.SuspendLayout()
        Me.TableLayoutPanelHeader.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.ToolStrip2.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanelMain
        '
        Me.TableLayoutPanelMain.ColumnCount = 1
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanelMain.Controls.Add(Me.TableLayoutPanelContent, 0, 0)
        Me.TableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelMain.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanelMain.Name = "TableLayoutPanelMain"
        Me.TableLayoutPanelMain.RowCount = 1
        Me.TableLayoutPanelMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelMain.Size = New System.Drawing.Size(1085, 475)
        Me.TableLayoutPanelMain.TabIndex = 0
        '
        'TableLayoutPanelContent
        '
        Me.TableLayoutPanelContent.ColumnCount = 1
        Me.TableLayoutPanelContent.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelContent.Controls.Add(Me.TableLayoutPanelHeader, 0, 0)
        Me.TableLayoutPanelContent.Controls.Add(Me.TableLayoutPanel1, 0, 2)
        Me.TableLayoutPanelContent.Controls.Add(Me.DashboardPanel, 0, 1)
        Me.TableLayoutPanelContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelContent.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelContent.Name = "TableLayoutPanelContent"
        Me.TableLayoutPanelContent.RowCount = 3
        Me.TableLayoutPanelContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.TableLayoutPanelContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.0!))
        Me.TableLayoutPanelContent.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.0!))
        Me.TableLayoutPanelContent.Size = New System.Drawing.Size(1079, 469)
        Me.TableLayoutPanelContent.TabIndex = 0
        '
        'TableLayoutPanelHeader
        '
        Me.TableLayoutPanelHeader.ColumnCount = 1
        Me.TableLayoutPanelHeader.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanelHeader.Controls.Add(Me.ToolStrip1, 0, 1)
        Me.TableLayoutPanelHeader.Controls.Add(Me.MenuStrip1, 0, 0)
        Me.TableLayoutPanelHeader.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanelHeader.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanelHeader.Name = "TableLayoutPanelHeader"
        Me.TableLayoutPanelHeader.RowCount = 2
        Me.TableLayoutPanelHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelHeader.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanelHeader.Size = New System.Drawing.Size(1073, 40)
        Me.TableLayoutPanelHeader.TabIndex = 0
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripTimeLabel, Me.ToolStripSeparator7, Me.ToolStripDateLabel, Me.ToolStripSeparator8})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 20)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1073, 20)
        Me.ToolStrip1.TabIndex = 1
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripTimeLabel
        '
        Me.ToolStripTimeLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripTimeLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.ToolStripTimeLabel.Name = "ToolStripTimeLabel"
        Me.ToolStripTimeLabel.Size = New System.Drawing.Size(93, 17)
        Me.ToolStripTimeLabel.Text = "ToolStripLabel1"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 20)
        '
        'ToolStripDateLabel
        '
        Me.ToolStripDateLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripDateLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.ToolStripDateLabel.Name = "ToolStripDateLabel"
        Me.ToolStripDateLabel.Size = New System.Drawing.Size(93, 17)
        Me.ToolStripDateLabel.Text = "ToolStripLabel1"
        '
        'ToolStripSeparator8
        '
        Me.ToolStripSeparator8.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripSeparator8.Name = "ToolStripSeparator8"
        Me.ToolStripSeparator8.Size = New System.Drawing.Size(6, 20)
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.Color.Turquoise
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MasterToolStripMenuItem, Me.SalesmanTrackerToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Padding = New System.Windows.Forms.Padding(5, 2, 0, 2)
        Me.MenuStrip1.Size = New System.Drawing.Size(1073, 20)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.ToolStrip2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 442)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1073, 24)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'ToolStrip2
        '
        Me.ToolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ToolStrip2.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelBreadcrumb, Me.ToolStripUserLabel})
        Me.ToolStrip2.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip2.Name = "ToolStrip2"
        Me.ToolStrip2.Size = New System.Drawing.Size(1073, 24)
        Me.ToolStrip2.TabIndex = 1
        Me.ToolStrip2.Text = "ToolStrip2"
        '
        'ToolStripStatusLabelBreadcrumb
        '
        Me.ToolStripStatusLabelBreadcrumb.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold)
        Me.ToolStripStatusLabelBreadcrumb.Name = "ToolStripStatusLabelBreadcrumb"
        Me.ToolStripStatusLabelBreadcrumb.Size = New System.Drawing.Size(93, 21)
        Me.ToolStripStatusLabelBreadcrumb.Text = "ToolStripLabel1"
        '
        'ToolStripUserLabel
        '
        Me.ToolStripUserLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.ToolStripUserLabel.Name = "ToolStripUserLabel"
        Me.ToolStripUserLabel.Size = New System.Drawing.Size(87, 21)
        Me.ToolStripUserLabel.Text = "ToolStripLabel1"
        '
        'DashboardPanel
        '
        Me.DashboardPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DashboardPanel.Location = New System.Drawing.Point(3, 49)
        Me.DashboardPanel.Name = "DashboardPanel"
        Me.DashboardPanel.Size = New System.Drawing.Size(1073, 387)
        Me.DashboardPanel.TabIndex = 2
        '
        'MasterToolStripMenuItem
        '
        Me.MasterToolStripMenuItem.Name = "MasterToolStripMenuItem"
        Me.MasterToolStripMenuItem.Size = New System.Drawing.Size(55, 16)
        Me.MasterToolStripMenuItem.Text = "Master"
        '
        'SalesmanTrackerToolStripMenuItem
        '
        Me.SalesmanTrackerToolStripMenuItem.Name = "SalesmanTrackerToolStripMenuItem"
        Me.SalesmanTrackerToolStripMenuItem.Size = New System.Drawing.Size(109, 16)
        Me.SalesmanTrackerToolStripMenuItem.Text = "Salesman Tracker"
        '
        'HomeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1085, 475)
        Me.Controls.Add(Me.TableLayoutPanelMain)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "HomeForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cypos Accounts"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.TableLayoutPanelMain.ResumeLayout(False)
        Me.TableLayoutPanelContent.ResumeLayout(False)
        Me.TableLayoutPanelHeader.ResumeLayout(False)
        Me.TableLayoutPanelHeader.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ToolStrip2.ResumeLayout(False)
        Me.ToolStrip2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanelMain As TableLayoutPanel
    Friend WithEvents TableLayoutPanelContent As TableLayoutPanel
    Friend WithEvents TableLayoutPanelHeader As TableLayoutPanel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents DashboardPanel As Panel
    Friend WithEvents ToolStrip2 As ToolStrip
    Friend WithEvents ToolStripStatusLabelBreadcrumb As ToolStripLabel
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripTimeLabel As ToolStripLabel
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents ToolStripDateLabel As ToolStripLabel
    Friend WithEvents ToolStripSeparator8 As ToolStripSeparator
    Friend WithEvents ToolStripUserLabel As ToolStripLabel
    Friend WithEvents MasterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SalesmanTrackerToolStripMenuItem As ToolStripMenuItem
End Class