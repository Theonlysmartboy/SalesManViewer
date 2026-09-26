Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' A single row in the salesmen list: salesman name on the left,
''' three action buttons on the right.
''' </summary>
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
    Private Const HoverBack As Integer = &HF8F9FA
    Private Const NormalBack As Integer = &HFFFFFF

    Public Sub New(id As Integer, name As String)
        _SalesmanId = id
        _SalesmanName = If(name, "").Trim()
        Me.Height = 40
        Me.Margin = New Padding(0, 0, 0, 6)
        Me.Padding = New Padding(12, 6, 8, 6)
        Me.BackColor = Color.White
        Me.BorderStyle = BorderStyle.FixedSingle
        Me.Cursor = Cursors.Default
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
        _lblName = New Label With {
            .Text = _SalesmanName,
            .Dock = DockStyle.Fill,
            .TextAlign = ContentAlignment.MiddleLeft,
            .Font = New Font("Segoe UI", 10.5F, FontStyle.Bold),
            .ForeColor = Color.FromArgb(33, 37, 41),
            .AutoEllipsis = True,
            .Margin = New Padding(0)
        }
        Dim flow As New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .WrapContents = False,
            .FlowDirection = FlowDirection.LeftToRight,
            .BackColor = Color.Transparent,
            .Margin = New Padding(0)
        }
        _btnCurrent = MakeButton("Find", Color.FromArgb(25, 135, 84))
        _btnToday = MakeButton("Today's Route", Color.FromArgb(13, 110, 253))
        _btnByDate = MakeButton("Past Location", Color.FromArgb(255, 160, 0))
        _btnHistory = MakeButton("Route History", Color.FromArgb(108, 117, 125))
        AddHandler _btnCurrent.Click, Sub(s, e) RaiseEvent CurrentLocationClicked(Me, EventArgs.Empty)
        AddHandler _btnToday.Click, Sub(s, e) RaiseEvent TodayMovementClicked(Me, EventArgs.Empty)
        AddHandler _btnByDate.Click, Sub(s, e) RaiseEvent LocationByDateClicked(Me, EventArgs.Empty)
        AddHandler _btnHistory.Click, Sub(s, e) RaiseEvent MovementHistoryClicked(Me, EventArgs.Empty)
        flow.Controls.Add(_btnCurrent)
        flow.Controls.Add(_btnToday)
        flow.Controls.Add(_btnByDate)
        flow.Controls.Add(_btnHistory)
        layout.Controls.Add(_lblName, 0, 0)
        layout.Controls.Add(flow, 1, 0)
        Me.Controls.Add(layout)
    End Sub

    Private Function MakeButton(text As String, backColor As Color) As Button
        Dim b As New Button With {
            .Text = text,
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .MinimumSize = New Size(64, 28),
            .Padding = New Padding(8, 0, 8, 0),
            .FlatStyle = FlatStyle.Flat,
            .BackColor = backColor,
            .ForeColor = Color.White,
            .Font = New Font("Segoe UI", 8.5F, FontStyle.Regular),
            .Cursor = Cursors.Hand,
            .Margin = New Padding(4, 0, 0, 0),
            .TabStop = False
        }
        b.FlatAppearance.BorderSize = 0
        b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.15F)
        b.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(backColor, 0.08F)
        Return b
    End Function

    Protected Overrides Sub OnMouseEnter(e As EventArgs)
        MyBase.OnMouseEnter(e)
        Me.BackColor = Color.FromArgb(HoverBack)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        If Not Me.ClientRectangle.Contains(Me.PointToClient(Cursor.Position)) Then
            Me.BackColor = Color.FromArgb(NormalBack)
        End If
    End Sub
End Class