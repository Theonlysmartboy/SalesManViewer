Public Class SalesmanCard
    Inherits UserControl

    Public ReadOnly Property SalesmanId As Integer
    Public ReadOnly Property SalesmanName As String

    Public Event CurrentLocationClicked As EventHandler
    Public Event TodayMovementClicked As EventHandler
    Public Event LocationByDateClicked As EventHandler
    Public Event MovementHistoryClicked As EventHandler

    Private ReadOnly _lblName As Label
    Private ReadOnly _btnCurrent As Button
    Private ReadOnly _btnToday As Button
    Private ReadOnly _btnByDate As Button
    Private ReadOnly _btnHistory As Button
    Private ReadOnly _toolTip As New ToolTip()

    Private Shared ReadOnly HoverBack As Color = Color.FromArgb(248, 249, 250)
    Private Shared ReadOnly NormalBack As Color = Color.White

    Public Sub New(id As Integer, name As String)
        _SalesmanId = id
        _SalesmanName = If(name, "").Trim()
        Me.Height = 40
        Me.Margin = New Padding(0, 0, 0, 6)
        Me.Padding = New Padding(10, 4, 6, 4)
        Me.BackColor = NormalBack
        Me.BorderStyle = BorderStyle.FixedSingle
        Me.Cursor = Cursors.Default
        ' TOOLTIP CONFIGURATION
        _toolTip.AutoPopDelay = 5000
        _toolTip.InitialDelay = 400
        _toolTip.ReshowDelay = 100
        _toolTip.ShowAlways = True
        ' MAIN LAYOUT
        Dim layout As New TableLayoutPanel With {
            .Dock = DockStyle.Fill,
            .ColumnCount = 2,
            .RowCount = 1,
            .BackColor = Color.Transparent,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0F))
        layout.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
        layout.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0F))
        ' SALESMAN NAME
        _lblName = New Label With {
            .Text = _SalesmanName,
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Font = New Font("Segoe UI", 10.5F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(33, 37, 41),
            .AutoEllipsis = True,
            .Margin = New Padding(0)
        }
        ' ACTION BUTTONS CONTAINER
        Dim flow As New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .WrapContents = False,
            .FlowDirection = FlowDirection.LeftToRight,
            .BackColor = Color.Transparent,
            .Margin = New Padding(0),
            .Padding = New Padding(0)
        }
        ' COMPACT ACTION BUTTONS
        _btnCurrent = MakeButton("Now", Color.FromArgb(25, 135, 84), "Find Current Location")
        _btnToday = MakeButton("Today", Color.FromArgb(13, 110, 253), "View Today's Route")
        _btnByDate = MakeButton("Date", Color.FromArgb(255, 160, 0), "Find Location by Date")
        _btnHistory = MakeButton("History", Color.FromArgb(108, 117, 125), "View Movement History")
        ' BUTTON EVENTS
        AddHandler _btnCurrent.Click, Sub(s, e)
                                          RaiseEvent CurrentLocationClicked(Me, EventArgs.Empty)
                                      End Sub
        AddHandler _btnToday.Click, Sub(s, e)
                                        RaiseEvent TodayMovementClicked(Me, EventArgs.Empty)
                                    End Sub
        AddHandler _btnByDate.Click, Sub(s, e)
                                         RaiseEvent LocationByDateClicked(Me, EventArgs.Empty)
                                     End Sub
        AddHandler _btnHistory.Click, Sub(s, e)
                                          RaiseEvent MovementHistoryClicked(Me, EventArgs.Empty)
                                      End Sub
        ' ADD CONTROLS
        flow.Controls.Add(_btnCurrent)
        flow.Controls.Add(_btnToday)
        flow.Controls.Add(_btnByDate)
        flow.Controls.Add(_btnHistory)
        layout.Controls.Add(_lblName, 0, 0)
        layout.Controls.Add(flow, 1, 0)
        Me.Controls.Add(layout)
    End Sub

    ' BUTTON FACTORY
    Private Function MakeButton(text As String, backColor As Color, tooltipText As String) As Button
        Dim b As New Button With {
            .Text = text,
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .MinimumSize = New Size(44, 26),
            .Padding = New Padding(5, 0, 5, 0),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = backColor,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 8.0F, FontStyle.Regular),
            .Cursor = Cursors.Hand,
            .Margin = New Padding(3, 0, 0, 0),
            .TabStop = False
        }
        b.FlatAppearance.BorderSize = 0
        b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.15F)
        b.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.08F)
        _toolTip.SetToolTip(b, tooltipText)
        Return b
    End Function

    ' HOVER EFFECT
    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        Me.BackColor = HoverBack
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        If Not Me.ClientRectangle.Contains(Me.PointToClient(Cursor.Position)) Then
            Me.BackColor = NormalBack
        End If
    End Sub
End Class
