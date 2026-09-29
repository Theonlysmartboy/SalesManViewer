Imports System.Drawing.Drawing2D

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

    ' CARD APPEARANCE
    Private Shared ReadOnly CardBack As Color = Color.White
    Private Shared ReadOnly CardHoverBack As Color = Color.FromArgb(250, 248, 253)
    Private Shared ReadOnly ShadowColor As Color = Color.FromArgb(35, 0, 0, 0)

    Private Const CardRadius As Integer = 8
    Private Const ShadowSize As Integer = 5

    Public Sub New(id As Integer, name As String)
        _SalesmanId = id
        _SalesmanName = If(name, "").Trim()
        Me.Height = 48
        Me.Margin = New Padding(4, 3, 4, 8)
        Me.Padding = New Padding(ShadowSize + 10, ShadowSize + 5, ShadowSize + 10, ShadowSize + 5)
        Me.BackColor = CardBack
        Me.BorderStyle = BorderStyle.None
        Me.Cursor = Cursors.Default
        Me.DoubleBuffered = True
        Me.ResizeRedraw = True
        Me.SetStyle(ControlStyles.UserPaint Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or ControlStyles.ResizeRedraw, True)
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
        _btnCurrent = MakeButton("Last", Color.FromArgb(25, 135, 84), "Find Current/last known Location")
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
        Me.Invalidate()
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        MyBase.OnMouseLeave(e)
        Me.Invalidate()
    End Sub

    ' DRAW ROUNDED RECTANGLE
    Private Function CreateRoundedPath(rect As RectangleF, radius As Single) As GraphicsPath
        Dim path As New GraphicsPath()
        Dim diameter As Single = radius * 2
        If diameter > rect.Width Then diameter = rect.Width
        If diameter > rect.Height Then diameter = rect.Height
        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90)
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90)
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90)
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90)
        path.CloseFigure()
        Return path
    End Function

    ' PAINT THE BACKGROUND USING THE PARENT'S COLOR
    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        If Me.Parent IsNot Nothing Then
            Using brush As New SolidBrush(Me.Parent.BackColor)
                e.Graphics.FillRectangle(brush, Me.ClientRectangle)
            End Using
        Else
            e.Graphics.Clear(Color.FromArgb(245, 246, 250))
        End If
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.PixelOffsetMode = PixelOffsetMode.HighQuality
        g.CompositingQuality = CompositingQuality.HighQuality
        ' Card area, leaving room for shadow
        Dim cardRect As New RectangleF(ShadowSize, ShadowSize, Me.ClientSize.Width - ShadowSize * 2 - 1, Me.ClientSize.Height - ShadowSize * 2 - 1)
        If cardRect.Width <= 0 OrElse cardRect.Height <= 0 Then Return
        ' Draw soft shadow
        For i As Integer = ShadowSize To 1 Step -1
            Dim shadowRect As New RectangleF(cardRect.X, cardRect.Y + i * 0.4F, cardRect.Width, cardRect.Height)
            Using shadowPath As GraphicsPath = CreateRoundedPath(shadowRect, CardRadius)
                Dim alpha As Integer = Math.Max(1, 12 - i)
                Using shadowBrush As New SolidBrush(ShadowColor)
                    g.FillPath(shadowBrush, shadowPath)
                End Using
            End Using
        Next
        ' Draw rounded card background
        Using cardPath As GraphicsPath = CreateRoundedPath(cardRect, CardRadius)
            Dim backgroundColor As Color = If(Me.ClientRectangle.Contains(Me.PointToClient(Cursor.Position)), CardHoverBack, CardBack)
            Using cardBrush As New SolidBrush(backgroundColor)
                g.FillPath(cardBrush, cardPath)
            End Using
        End Using
    End Sub
End Class
