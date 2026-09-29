<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SalesmanTrackerForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.tblMain = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel8 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel9 = New System.Windows.Forms.TableLayoutPanel()
        Me.BtnRefreshSm = New System.Windows.Forms.Button()
        Me.TxtSearchSalesMen = New System.Windows.Forms.TextBox()
        Me.flpSalesmen = New System.Windows.Forms.FlowLayoutPanel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.WbMap = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.tblMain.SuspendLayout()
        Me.TableLayoutPanel8.SuspendLayout()
        Me.TableLayoutPanel9.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.WbMap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tblMain
        '
        Me.tblMain.ColumnCount = 2
        Me.tblMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 450.0!))
        Me.tblMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.tblMain.Controls.Add(Me.TableLayoutPanel8, 0, 0)
        Me.tblMain.Controls.Add(Me.WbMap, 1, 0)
        Me.tblMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblMain.Location = New System.Drawing.Point(0, 0)
        Me.tblMain.Name = "tblMain"
        Me.tblMain.RowCount = 1
        Me.tblMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblMain.Size = New System.Drawing.Size(1254, 511)
        Me.tblMain.TabIndex = 0
        '
        'TableLayoutPanel8
        '
        Me.TableLayoutPanel8.ColumnCount = 1
        Me.TableLayoutPanel8.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel8.Controls.Add(Me.TableLayoutPanel9, 0, 0)
        Me.TableLayoutPanel8.Controls.Add(Me.flpSalesmen, 0, 2)
        Me.TableLayoutPanel8.Controls.Add(Me.TableLayoutPanel1, 0, 1)
        Me.TableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel8.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel8.Name = "TableLayoutPanel8"
        Me.TableLayoutPanel8.RowCount = 3
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35.0!))
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel8.Size = New System.Drawing.Size(444, 505)
        Me.TableLayoutPanel8.TabIndex = 2
        '
        'TableLayoutPanel9
        '
        Me.TableLayoutPanel9.ColumnCount = 2
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.0!))
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40.0!))
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel9.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel9.Controls.Add(Me.BtnRefreshSm, 1, 0)
        Me.TableLayoutPanel9.Controls.Add(Me.TxtSearchSalesMen, 0, 0)
        Me.TableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel9.Location = New System.Drawing.Point(3, 0)
        Me.TableLayoutPanel9.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.TableLayoutPanel9.Name = "TableLayoutPanel9"
        Me.TableLayoutPanel9.RowCount = 1
        Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel9.Size = New System.Drawing.Size(438, 40)
        Me.TableLayoutPanel9.TabIndex = 0
        '
        'BtnRefreshSm
        '
        Me.BtnRefreshSm.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnRefreshSm.Location = New System.Drawing.Point(265, 3)
        Me.BtnRefreshSm.Name = "BtnRefreshSm"
        Me.BtnRefreshSm.Size = New System.Drawing.Size(170, 23)
        Me.BtnRefreshSm.TabIndex = 1
        Me.BtnRefreshSm.Text = "Refresh"
        Me.BtnRefreshSm.UseVisualStyleBackColor = True
        '
        'TxtSearchSalesMen
        '
        Me.TxtSearchSalesMen.Dock = System.Windows.Forms.DockStyle.Top
        Me.TxtSearchSalesMen.Location = New System.Drawing.Point(3, 3)
        Me.TxtSearchSalesMen.Name = "TxtSearchSalesMen"
        Me.TxtSearchSalesMen.Size = New System.Drawing.Size(256, 20)
        Me.TxtSearchSalesMen.TabIndex = 2
        '
        'flpSalesmen
        '
        Me.flpSalesmen.BackColor = System.Drawing.Color.Honeydew
        Me.flpSalesmen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpSalesmen.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpSalesmen.Location = New System.Drawing.Point(3, 75)
        Me.flpSalesmen.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.flpSalesmen.Name = "flpSalesmen"
        Me.flpSalesmen.Size = New System.Drawing.Size(438, 430)
        Me.flpSalesmen.TabIndex = 1
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Label2, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 40)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(438, 35)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(222, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(119, 35)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tracking Options"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 35)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Sales Men"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'WbMap
        '
        Me.WbMap.AllowExternalDrop = True
        Me.WbMap.CreationProperties = Nothing
        Me.WbMap.DefaultBackgroundColor = System.Drawing.Color.White
        Me.WbMap.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WbMap.Location = New System.Drawing.Point(453, 3)
        Me.WbMap.Name = "WbMap"
        Me.WbMap.Size = New System.Drawing.Size(798, 505)
        Me.WbMap.TabIndex = 1
        Me.WbMap.ZoomFactor = 1.0R
        '
        'SalesmanTrackerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1254, 511)
        Me.Controls.Add(Me.tblMain)
        Me.Name = "SalesmanTrackerForm"
        Me.Text = "Salesman Tracker"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tblMain.ResumeLayout(False)
        Me.TableLayoutPanel8.ResumeLayout(False)
        Me.TableLayoutPanel9.ResumeLayout(False)
        Me.TableLayoutPanel9.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.WbMap, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tblMain As TableLayoutPanel
    Friend WithEvents WbMap As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents TableLayoutPanel8 As TableLayoutPanel
    Friend WithEvents TableLayoutPanel9 As TableLayoutPanel
    Friend WithEvents BtnRefreshSm As Button
    Friend WithEvents TxtSearchSalesMen As TextBox
    Friend WithEvents flpSalesmen As FlowLayoutPanel
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
End Class
