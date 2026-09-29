Imports System.Text
Imports MySql.Data.MySqlClient
Imports SalesManViewer.CustomControls.Alert
Imports SalesManViewer.Helpers.Db

Public Class SplashScreen
    Private loadingDots As Integer = 0
    Private WithEvents FadeInTimer As New Timer With {.Interval = 30}
    Private WithEvents StayTimer As New Timer With {.Interval = 5000}
    Private WithEvents FadeOutTimer As New Timer With {.Interval = 30}
    Private WithEvents LoadingTimer As New Timer With {.Interval = 500}
    Private _nextForm As Form = Nothing
    Private _fatalError As Boolean = False
    Private _shutdownInitiated As Boolean = False

    Public Sub New()
        InitializeComponent()
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance)
    End Sub

    ' STARTUP
    Private Async Sub SplashScreen_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Me.Opacity = 0
        TrailingDotsLoader.Left = (Me.ClientSize.Width - TrailingDotsLoader.Width) \ 2
        TrailingDotsLoader.Top = 30
        lblLoading.Left = (Me.ClientSize.Width - lblLoading.Width) \ 2
        lblLoading.Top = TrailingDotsLoader.Bottom + 10
        TrailingDotsLoader.Start()
        SetupFooterLabels()
        FadeInTimer.Start()
        LoadingTimer.Start()
        Try
            Dim startupSucceeded As Boolean = Await RunStartupTasksAsync()
            If Not startupSucceeded Then
                InitiateShutdown()
                Return
            End If
            ' STARTUP SUCCESSFUL
            StayTimer.Start()
        Catch ex As Exception
            _fatalError = True
            StopAllTimers()
            JsAlertDialog.ShowAlert(Me, "Startup failed: " & ex.Message, "Fatal Error",
            JsAlertDialog.AlertType.Error, JsAlertDialog.ButtonType.OK)
            InitiateShutdown()
        End Try
    End Sub

    ' STARTUP TASKS
    Private Async Function RunStartupTasksAsync() As Task(Of Boolean)
        Dim cs = GlobalConnectionString.GetConnectionString()
        Try
            ' Test database connection first
            Using conn As New MySqlConnection(cs)
                Await conn.OpenAsync()
            End Using
        Catch ex As Exception

            ' Stop splash activity while the user is configuring the DB.
            FadeInTimer.Stop()
            StayTimer.Stop()
            FadeOutTimer.Stop()
            LoadingTimer.Stop()

            Dim result = JsAlertDialog.ShowAlert(Me, "Unable to connect to the database." &
        Environment.NewLine & Environment.NewLine &
        "Would you like to update your database connection settings?",
        "Database Connection Failed", JsAlertDialog.AlertType.Confirm, JsAlertDialog.ButtonType.YesNo)
            If result <> DialogResult.Yes Then
                Return False
            End If
            ' OPEN SETTINGS
            Using dbSettings As New SettingsForm()
                dbSettings.OpenSystemTab()
                dbSettings.ShowDialog(Me)
            End Using
        End Try
        ' Database connection is confirmed.
        ' Now it's safe to use DBHelper and seeders.
        Dim db As New DbHelper(cs)
        Await Task.CompletedTask
        Return True
    End Function

    ' CENTRAL SHUTDOWN
    Private Sub InitiateShutdown()
        If _shutdownInitiated Then Return
        _shutdownInitiated = True
        _fatalError = True
        StopAllTimers()
        Me.Close()
    End Sub

    Private Sub StopAllTimers()
        FadeInTimer.Stop()
        FadeOutTimer.Stop()
        StayTimer.Stop()
        LoadingTimer.Stop()
    End Sub

    ' FADE IN
    Private Sub fadeInTimer_Tick(sender As Object, e As EventArgs) Handles FadeInTimer.Tick
        If _shutdownInitiated OrElse _fatalError Then Return
        If Me.Opacity < 1 Then
            Me.Opacity += 0.05
        Else
            FadeInTimer.Stop()
        End If
    End Sub

    ' STAY TIMER
    Private Sub stayTimer_Tick(sender As Object, e As EventArgs) Handles StayTimer.Tick
        If _shutdownInitiated Then Return
        StayTimer.Stop()
        FadeOutTimer.Start()
    End Sub

    ' FADE OUT + NAVIGATION
    Private Sub fadeOutTimer_Tick(sender As Object, e As EventArgs) Handles FadeOutTimer.Tick
        If _shutdownInitiated OrElse _fatalError Then Return
        If Me.Opacity > 0 Then
            Me.Opacity -= 0.05
        Else
            FadeOutTimer.Stop()
            LoadingTimer.Stop()
            Me.Hide()
            Using login As New LoginForm()
                Dim result = login.ShowDialog()
                If result = DialogResult.OK Then
                    Dim home As New HomeForm()
                    home.Show()
                Else
                    InitiateShutdown()
                End If
            End Using
            Me.Close()
        End If
    End Sub

    ' LOADING TEXT
    Private Sub LoadingTimer_Tick(sender As Object, e As EventArgs) Handles LoadingTimer.Tick
        If _shutdownInitiated Then Return
        loadingDots = (loadingDots + 1) Mod 4
        lblLoading.Text = "Loading" & New String("."c, loadingDots)
    End Sub

    ' FOOTER UI
    Private Sub SetupFooterLabels()
        Dim footerPanel As New Panel() With {
            .Dock = DockStyle.Bottom,
            .Height = 30,
            .BackColor = Color.Transparent
        }
        Dim lblLeft As New Label() With {
            .AutoSize = True,
            .ForeColor = Color.Gray,
            .Font = New Font("Segoe UI", 9, FontStyle.Regular),
            .Location = New Point(10, 7)
        }
        Dim version = My.Application.Info.Version.ToString()
        Dim model = My.Application.Info.AssemblyName
        LblTitle.Text = model
        lblLeft.Text = $"V{version} "
        Dim lblRight As New Label() With {
            .AutoSize = True,
            .ForeColor = Color.Gray,
            .Font = New Font("Segoe UI", 9, FontStyle.Regular)
        }
        Dim companyName = My.Application.Info.CompanyName
        lblRight.Text = "By: " & If(String.IsNullOrWhiteSpace(companyName),
                                    "Unknown Developer",
                                    companyName)
        AddHandler footerPanel.Resize,
            Sub()
                lblRight.Left = footerPanel.Width - lblRight.Width - 10
                lblRight.Top = 7
            End Sub
        footerPanel.Controls.Add(lblLeft)
        footerPanel.Controls.Add(lblRight)
        Me.Controls.Add(footerPanel)
    End Sub
End Class