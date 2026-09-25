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
        Me.WbMap = New Microsoft.Web.WebView2.WinForms.WebView2()
        Me.flpSalesmen = New System.Windows.Forms.FlowLayoutPanel()
        Me.tblMain.SuspendLayout()
        Me.TableLayoutPanel8.SuspendLayout()
        Me.TableLayoutPanel9.SuspendLayout()
        CType(Me.WbMap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tblMain
        '
        Me.tblMain.ColumnCount = 3
        Me.tblMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 319.0!))
        Me.tblMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 138.0!))
        Me.tblMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblMain.Controls.Add(Me.TableLayoutPanel8, 0, 0)
        Me.tblMain.Controls.Add(Me.WbMap, 2, 0)
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
        Me.TableLayoutPanel8.Controls.Add(Me.flpSalesmen, 0, 1)
        Me.TableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel8.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel8.Name = "TableLayoutPanel8"
        Me.TableLayoutPanel8.RowCount = 2
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.524753!))
        Me.TableLayoutPanel8.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.47525!))
        Me.TableLayoutPanel8.Size = New System.Drawing.Size(313, 505)
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
        Me.TableLayoutPanel9.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel9.Name = "TableLayoutPanel9"
        Me.TableLayoutPanel9.RowCount = 1
        Me.TableLayoutPanel9.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel9.Size = New System.Drawing.Size(307, 32)
        Me.TableLayoutPanel9.TabIndex = 0
        '
        'BtnRefreshSm
        '
        Me.BtnRefreshSm.Dock = System.Windows.Forms.DockStyle.Top
        Me.BtnRefreshSm.Location = New System.Drawing.Point(187, 3)
        Me.BtnRefreshSm.Name = "BtnRefreshSm"
        Me.BtnRefreshSm.Size = New System.Drawing.Size(117, 23)
        Me.BtnRefreshSm.TabIndex = 1
        Me.BtnRefreshSm.Text = "Refresh"
        Me.BtnRefreshSm.UseVisualStyleBackColor = True
        '
        'TxtSearchSalesMen
        '
        Me.TxtSearchSalesMen.Dock = System.Windows.Forms.DockStyle.Top
        Me.TxtSearchSalesMen.Location = New System.Drawing.Point(3, 3)
        Me.TxtSearchSalesMen.Name = "TxtSearchSalesMen"
        Me.TxtSearchSalesMen.Size = New System.Drawing.Size(178, 20)
        Me.TxtSearchSalesMen.TabIndex = 2
        '
        'WbMap
        '
        Me.WbMap.AllowExternalDrop = True
        Me.WbMap.CreationProperties = Nothing
        Me.WbMap.DefaultBackgroundColor = System.Drawing.Color.White
        Me.WbMap.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WbMap.Location = New System.Drawing.Point(460, 3)
        Me.WbMap.Name = "WbMap"
        Me.WbMap.Size = New System.Drawing.Size(791, 505)
        Me.WbMap.TabIndex = 1
        Me.WbMap.ZoomFactor = 1.0R
        '
        'flpSalesmen
        '
        Me.flpSalesmen.Dock = System.Windows.Forms.DockStyle.Fill
        Me.flpSalesmen.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpSalesmen.Location = New System.Drawing.Point(3, 41)
        Me.flpSalesmen.Name = "flpSalesmen"
        Me.flpSalesmen.Size = New System.Drawing.Size(307, 461)
        Me.flpSalesmen.TabIndex = 1
        '
        'SalesmanTrackerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1254, 511)
        Me.Controls.Add(Me.tblMain)
        Me.Name = "SalesmanTrackerForm"
        Me.Text = "Salesman Tracker Form"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.tblMain.ResumeLayout(False)
        Me.TableLayoutPanel8.ResumeLayout(False)
        Me.TableLayoutPanel9.ResumeLayout(False)
        Me.TableLayoutPanel9.PerformLayout()
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
End Class
