Imports SalesManViewer.CustomControls.Alert
Imports SalesManViewer.Handlers
Imports SalesManViewer.Helpers.Db
Imports SalesManViewer.Helpers.Security
Imports SalesManViewer.Models.Auth

Public Class HomeForm
    Private statusTimer As Timer
    Private activeChildForm As Form = Nothing
    Private ReadOnly _db As DbHelper
    Private ReadOnly breadcrumbStack As New Stack(Of String)
    Private ReadOnly breadcrumbPath As New List(Of String)
    Private _returnBreadcrumb As String = "Home"

    Public Sub New()
        InitializeComponent()
        Me.IsMdiContainer = True
        Dim rolesText = String.Join(", ", UserContext.Instance.Roles)
        statusTimer = New Timer() With {.Interval = 1000}
        AddHandler statusTimer.Tick, AddressOf UpdateStatus
        statusTimer.Start()
        _db = New DbHelper(GlobalConnectionString.GetConnectionString())
    End Sub

    Private Sub Dashboard_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        statusTimer.Stop()
        CloseAllChildForms()
    End Sub

    ' Helpers
    Private Sub Logout()
        Dim cancel = JsAlertDialog.ShowAlert(Me, "Are you sure you want to log out?", "Logout",
                    JsAlertDialog.AlertType.Confirm, JsAlertDialog.ButtonType.YesNo, True, 20)
        If cancel = DialogResult.No Then Return
        statusTimer.Stop()
        CloseAllChildForms()
        UserContext.Clear()
        Me.Hide()
        Using login As New LoginForm()
            Dim result As DialogResult = login.ShowDialog()
            If result = DialogResult.OK Then
                Me.Show()
                statusTimer.Start()
                UpdateStatus(Nothing, EventArgs.Empty)
            Else
                Me.Close()
            End If
        End Using
    End Sub

    Private Sub UpdateStatus(sender As Object, e As EventArgs)
        Dim rolesText = String.Join(", ", UserContext.Instance.Roles)
        ToolStripUserLabel.Text = $"User: {UserContext.Instance.FullName} | Roles: {rolesText} | "
        ToolStripDateLabel.Text = DateTime.Now.ToString("dd-MMM-yyyy")
        ToolStripTimeLabel.Text = DateTime.Now.ToString("hh:mm : ss tt")
    End Sub

    Public Sub NavigateTo(form As Form, Optional fillContainer As Boolean = True, Optional title As String = "")
        LoadChildForm(form, fillContainer, title)
    End Sub

    Public Sub SetBreadcrumb(ParamArray items() As String)
        breadcrumbPath.Clear()
        For Each item In items
            breadcrumbPath.Add(item)
        Next
        UpdateBreadcrumb()
    End Sub

    Private Sub UpdateBreadcrumb()
        If breadcrumbPath.Count = 0 Then
            ToolStripStatusLabelBreadcrumb.Text = "Home"
            Return
        End If
        ToolStripStatusLabelBreadcrumb.Text = String.Join(" > ", breadcrumbPath)
    End Sub

    Public Sub LoadChildForm(child As Form, Optional fillContainer As Boolean = True, Optional title As String = "", Optional returnBreadcrumb As String = "")
        If activeChildForm IsNot Nothing Then
            RemoveHandler activeChildForm.FormClosed, AddressOf ChildFormClosed
            activeChildForm.Close()
            activeChildForm.Dispose()
        End If
        _returnBreadcrumb = If(String.IsNullOrWhiteSpace(returnBreadcrumb), "Home", returnBreadcrumb)
        activeChildForm = child
        child.TopLevel = False
        child.FormBorderStyle = FormBorderStyle.None
        If fillContainer Then
            child.Dock = DockStyle.Fill
        Else
            child.Dock = DockStyle.None
            child.StartPosition = FormStartPosition.Manual
            child.Left = (DashboardPanel.Width - child.Width) \ 2
            child.Top = (DashboardPanel.Height - child.Height) \ 2
        End If
        DashboardPanel.Controls.Add(child)
        AddHandler child.FormClosed, AddressOf ChildFormClosed
        child.Show()
    End Sub

    Private Sub CloseAllChildForms()
        If activeChildForm IsNot Nothing Then
            RemoveHandler activeChildForm.FormClosed, AddressOf ChildFormClosed
            activeChildForm.Close()
            activeChildForm.Dispose()
            activeChildForm = Nothing
        End If
        For Each ctrl As Control In DashboardPanel.Controls
            If TypeOf ctrl Is Form Then
                Dim frm = CType(ctrl, Form)
                frm.Close()
                frm.Dispose()
            End If
        Next
        DashboardPanel.Controls.Clear()
    End Sub

    Private Sub ChildFormClosed(sender As Object, e As FormClosedEventArgs)
        DashboardPanel.Controls.Clear()
        activeChildForm = Nothing
        SetBreadcrumb(_returnBreadcrumb)
    End Sub

    Private Sub RestoreDashboard()
        DashboardPanel.Controls.Clear()
        breadcrumbStack.Clear()
        breadcrumbPath.Clear()
        SetBreadcrumb("Home")
        activeChildForm = Nothing
    End Sub

    Private Sub GoBack()
        If breadcrumbStack.Count > 0 Then
            breadcrumbStack.Pop()
            UpdateBreadcrumb()
        End If
    End Sub

    Private Sub AnimateSlideIn(target As Form, container As Panel)
        Dim startX As Integer = container.Width   ' start from right
        Dim endX As Integer = 0                   ' final position
        Dim stepPixels As Integer = 40            ' speed
        target.Left = startX
        Dim t As New Timer With {.Interval = 10}
        AddHandler t.Tick, Sub()
                               If target.Left <= endX Then
                                   target.Left = endX
                                   t.Stop()
                                   t.Dispose()
                               Else
                                   target.Left -= stepPixels
                               End If
                           End Sub
        t.Start()
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        If activeChildForm IsNot Nothing AndAlso TypeOf activeChildForm Is IShortcutHandler Then
            Dim handler = DirectCast(activeChildForm, IShortcutHandler)
            If handler.HandleShortcut(keyData) Then
                Return True
            End If
        End If
        ' ESC
        If keyData = Keys.Escape Then
            ' If a child form is currently open,
            ' Escape returns to the dashboard.
            If activeChildForm IsNot Nothing Then
                CloseAllChildForms()
                RestoreDashboard()
                Return True
            End If
            ' No child form Escape logs out.
            Logout()
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub SalesmanTrackerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalesmanTrackerToolStripMenuItem.Click
        If HasRole("admin", "superadmin") Then
            SetBreadcrumb("Home", "Salesman Tracker")
            LoadChildForm(New SalesmanTrackerForm(), True)
        Else
            JsAlertDialog.ShowAlert(Me, "Access denied.", "Authorization", JsAlertDialog.AlertType.Warning,
                                JsAlertDialog.ButtonType.OK, True, 20)
        End If
    End Sub
End Class