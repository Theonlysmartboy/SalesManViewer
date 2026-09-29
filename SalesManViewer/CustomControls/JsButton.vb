Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Namespace CustomControls

    <ToolboxItem(True)>
    <DefaultEvent("Click")>
    <DefaultProperty("Text")>
    Public Class JsButton
        Inherits Button

#Region "Enums"
        Public Enum BootstrapStyle
            Primary
            Success
            Danger
            Warning
            Secondary
        End Enum

        Public Enum BootstrapSize
            Small
            Normal
            Large
        End Enum
#End Region

#Region "Fields"
        Private _style As BootstrapStyle = BootstrapStyle.Primary
        Private _size As BootstrapSize = BootstrapSize.Normal
        Private _outline As Boolean = False
        Private _borderRadius As Integer = 14
        Private _hoverColor As Color = Color.Empty
        Private _pressedColor As Color = Color.Empty
        Private _originalColor As Color
#End Region

#Region "Properties"
        <Category("Bootstrap")>
        Public Property ButtonStyle As BootstrapStyle
            Get
                Return _style
            End Get
            Set(value As BootstrapStyle)
                _style = value
                ApplyStyle()
                Invalidate()
            End Set
        End Property

        <Category("Bootstrap")>
        Public Property ButtonSize As BootstrapSize
            Get
                Return _size
            End Get
            Set(value As BootstrapSize)
                _size = value
                ApplySize()
                Invalidate()
            End Set
        End Property

        <Category("Bootstrap")>
        Public Property Outline As Boolean
            Get
                Return _outline
            End Get
            Set(value As Boolean)
                _outline = value
                ApplyStyle()
                Invalidate()
            End Set
        End Property

        <Category("Bootstrap")>
        Public Property BorderRadius As Integer
            Get
                Return _borderRadius
            End Get
            Set(value As Integer)
                _borderRadius = Math.Max(1, value)
                Invalidate()
            End Set
        End Property

        <Category("Bootstrap")>
        Public Property HoverColor As Color
            Get
                Return _hoverColor
            End Get
            Set(value As Color)
                _hoverColor = value
            End Set
        End Property

        <Category("Bootstrap")>
        Public Property PressedColor As Color
            Get
                Return _pressedColor
            End Get
            Set(value As Color)
                _pressedColor = value
            End Set
        End Property
#End Region

#Region "Constructor"
        Public Sub New()
            'FlatStyle = FlatStyle.Flat
            FlatAppearance.BorderSize = 0
            FlatAppearance.MouseDownBackColor = Color.Transparent
            FlatAppearance.MouseOverBackColor = Color.Transparent
            Font = New Font("Segoe UI", 9.0F, FontStyle.Regular)
            Size = New Size(120, 38)
            SetStyle(ControlStyles.UserPaint Or
                     ControlStyles.AllPaintingInWmPaint Or
                     ControlStyles.OptimizedDoubleBuffer Or
                     ControlStyles.ResizeRedraw,
                     True)
            ApplySize()
            ApplyStyle()
        End Sub
#End Region

#Region "Theme"
        Private ReadOnly Property ThemeColor As Color
            Get
                Select Case _style
                    Case BootstrapStyle.Primary
                        Return Color.FromArgb(64, 224, 208)
                    Case BootstrapStyle.Success
                        Return Color.FromArgb(25, 135, 84)
                    Case BootstrapStyle.Danger
                        Return Color.FromArgb(220, 53, 69)
                    Case BootstrapStyle.Warning
                        Return Color.FromArgb(255, 193, 7)
                    Case BootstrapStyle.Secondary
                        Return Color.FromArgb(108, 117, 125)
                    Case Else
                        Return Color.FromArgb(64, 224, 208)
                End Select
            End Get
        End Property

        Private Sub ApplyStyle()
            If _outline Then
                BackColor = Color.White
                ForeColor = ThemeColor
                FlatAppearance.BorderSize = 1
                FlatAppearance.BorderColor = ThemeColor
            Else
                BackColor = ThemeColor
                If _style = BootstrapStyle.Warning Then
                    ForeColor = Color.Black
                Else
                    ForeColor = Color.White
                End If
                FlatAppearance.BorderSize = 0
            End If
        End Sub

        Private Sub ApplySize()
            Select Case _size
                Case BootstrapSize.Small
                    Height = 31
                Case BootstrapSize.Normal
                    Height = 38
                Case BootstrapSize.Large
                    Height = 48
            End Select
        End Sub
#End Region

#Region "Painting"
        Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
            Dim g = pevent.Graphics
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.PixelOffsetMode = PixelOffsetMode.HighQuality
            Dim rect As New Rectangle(0, 0, Width - 1, Height - 1)
            Using path As GraphicsPath = GetRoundedPath(rect, BorderRadius)
                ' clear background
                g.Clear(Parent.BackColor)
                ' fill
                Using brush As New SolidBrush(BackColor)
                    g.FillPath(brush, path)
                End Using
                ' border
                If Outline Then
                    Using pen As New Pen(ForeColor, 1.2F)
                        pen.LineJoin = LineJoin.Round
                        g.DrawPath(pen, path)
                    End Using
                End If
                ' text
                TextRenderer.DrawText(g, Text, Font, rect, ForeColor,
                                      TextFormatFlags.HorizontalCenter Or TextFormatFlags.VerticalCenter)
            End Using
        End Sub

        Protected Overrides Sub OnResize(e As EventArgs)
            MyBase.OnResize(e)
            Invalidate()
        End Sub
#End Region

#Region "Mouse Effects"
        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            _originalColor = BackColor
            If HoverColor <> Color.Empty Then
                BackColor = HoverColor
            Else
                BackColor = ControlPaint.Dark(BackColor)
            End If
            MyBase.OnMouseEnter(e)
        End Sub

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            BackColor = _originalColor
            MyBase.OnMouseLeave(e)
        End Sub

        Protected Overrides Sub OnMouseDown(mevent As MouseEventArgs)
            If PressedColor <> Color.Empty Then
                BackColor = PressedColor
            Else
                BackColor = ControlPaint.DarkDark(BackColor)
            End If
            MyBase.OnMouseDown(mevent)
        End Sub

        Protected Overrides Sub OnMouseUp(mevent As MouseEventArgs)
            If HoverColor <> Color.Empty Then
                BackColor = HoverColor
            Else
                BackColor = ControlPaint.Dark(_originalColor)
            End If
            MyBase.OnMouseUp(mevent)
        End Sub
#End Region

        Protected Overrides Sub OnPaintBackground(pevent As PaintEventArgs)
            ' DO NOTHING
        End Sub

        'Helpers
        Private Function GetRoundedPath(rect As Rectangle, radius As Integer) As GraphicsPath
            Dim path As New GraphicsPath()
            Dim d As Integer = radius * 2
            path.StartFigure()
            path.AddArc(rect.X, rect.Y, d, d, 180, 90)
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90)
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90)
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90)
            path.CloseFigure()
            Return path
        End Function
    End Class
End Namespace