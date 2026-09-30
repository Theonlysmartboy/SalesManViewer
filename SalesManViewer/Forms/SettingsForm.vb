Imports System.Runtime.InteropServices
Imports MySql.Data.MySqlClient
Imports SalesManViewer.Config
Imports SalesManViewer.CustomControls.Alert
Imports SalesManViewer.Helpers.Cryptography

Public Class SettingsForm
    Private _settingsManager As SettingsManager
    Private _isPasswordVisible As Boolean = False
    Private ReadOnly _toolTip As New ToolTip()

    Public Sub New(Optional connectionString As String = "")
        InitializeComponent()
        If Not String.IsNullOrWhiteSpace(connectionString) Then
            _settingsManager = New SettingsManager(connectionString)
        Else
            Dim built = BuildConnectionStringFromMySettings()
            _settingsManager = If(String.IsNullOrWhiteSpace(built), Nothing, New SettingsManager(built))
        End If
    End Sub

    ' --- Form Load ---
    Private Async Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _isPasswordVisible = False
        txtDbPassword.UseSystemPasswordChar = True
        PicTogglePassword.Image = My.Resources.eye_closed_outline
        InitializeToolTips()
        Await LoadDBSettings()
        TxtDbServer.Text = If(My.Settings("db_server") IsNot Nothing, My.Settings("db_server"), "")
        TxtDbPort.Text = If(My.Settings("db_port") IsNot Nothing, My.Settings("db_port").ToString(),
                    "3306")
        TxtDbName.Text = If(My.Settings("db_name") IsNot Nothing, My.Settings("db_name"), "")
        TxtDbPrefix.Text = If(My.Settings("db_prefix") IsNot Nothing, My.Settings("db_prefix"), "")
        TxtDbUser.Text = If(My.Settings("db_user") IsNot Nothing, My.Settings("db_user"), "")
        Dim encryptedPassword = TryCast(My.Settings("db_password"), String)
        If Not String.IsNullOrWhiteSpace(encryptedPassword) Then
            Try
                txtDbPassword.Text = Encorder.Decrypt(encryptedPassword)
            Catch
                txtDbPassword.Clear()
            End Try
        Else
            txtDbPassword.Clear()
        End If
        PanelPassword.TabStop = False
        PicTogglePassword.TabStop = False
    End Sub

    Private Sub PicTogglePassword_Click(sender As Object, e As EventArgs) Handles PicTogglePassword.Click
        TogglePasswordVisibility()
    End Sub

    ' --- Save DB Settings from DataGridView ---
    Private Async Sub btnSaveDB_Click(sender As Object, e As EventArgs) Handles btnSaveDB.Click
        toggleGridView(False)
        Try
            For Each row As DataGridViewRow In dgvSettings.Rows
                If Not row.IsNewRow Then
                    Dim key = row.Cells("Key").Value?.ToString().Trim()
                    Dim value = row.Cells("Value").Value?.ToString().Trim()
                    If Not String.IsNullOrEmpty(key) Then
                        Await _settingsManager.SetSettingAsync(key, value)
                    End If
                End If
            Next
            ' Ask user if they want to restart
            Dim result = JsAlertDialog.ShowAlert(Me, "Settings saved successfully." & vbCrLf &
        "Some changes require restarting the application." & vbCrLf &
        "Do you want to restart now?", "Restart Required",
        JsAlertDialog.AlertType.Confirm, JsAlertDialog.ButtonType.YesNo)
            If result = DialogResult.Yes Then
                RestartApplication()
            End If
        Catch ex As Exception
            JsAlertDialog.ShowAlert(Me, "Error saving database settings: " & ex.Message, "Error", JsAlertDialog.AlertType.Error, JsAlertDialog.ButtonType.OK)
        End Try
        toggleGridView(True)
    End Sub

    '--- Handle Delete Button Click in DataGridView ---
    Private Async Sub dgvSettings_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSettings.CellContentClick
        If e.RowIndex < 0 Then Exit Sub
        If dgvSettings.Columns(e.ColumnIndex).Name = "Delete" Then
            Dim key As String = dgvSettings.Rows(e.RowIndex).Cells("Key").Value?.ToString()
            If String.IsNullOrEmpty(key) Then Exit Sub
            toggleGridView(False)
            Dim confirm = JsAlertDialog.ShowAlert(Me, $"Are you sure you want to delete '{key}'?" & vbCrLf & "This action cannot be undone.",
                "Confirm Delete", JsAlertDialog.AlertType.Confirm, JsAlertDialog.ButtonType.OK)
            If confirm = DialogResult.OK Then
                Try
                    Await _settingsManager.DeleteSettingAsync(key)
                    JsAlertDialog.ShowAlert(Me, $"'{key}' deleted successfully.", "Success", JsAlertDialog.AlertType.Success, JsAlertDialog.ButtonType.OK)
                    ' Reload grid
                    Await LoadDBSettings()
                Catch ex As Exception
                    JsAlertDialog.ShowAlert(Me, "Error deleting setting: " & ex.Message, "Error", JsAlertDialog.AlertType.Error, JsAlertDialog.ButtonType.OK)
                End Try
            End If
        End If
        toggleGridView(True)
    End Sub

    Private Sub dgvSettings_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvSettings.CellDoubleClick
        If e.RowIndex < 0 Then Exit Sub ' Ignore headers
        Dim row = dgvSettings.Rows(e.RowIndex)
        Dim copiedText As New Text.StringBuilder()
        For Each cell As DataGridViewCell In row.Cells
            copiedText.AppendLine($"{dgvSettings.Columns(cell.ColumnIndex).HeaderText}: {cell.Value}")
        Next
        Clipboard.SetText(copiedText.ToString())
        toggleGridView(False)
        JsAlertDialog.ShowAlert(Me, "Row copied to clipboard!", "Copied", JsAlertDialog.AlertType.Info, JsAlertDialog.ButtonType.OK)
        toggleGridView(True)
    End Sub

    '--- Test db connection before saving ---
    Private Async Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        Dim server As String = TxtDbServer.Text.Trim()
        Dim portText As String = TxtDbPort.Text.Trim()
        Dim database As String = TxtDbName.Text.Trim()
        Dim user As String = TxtDbUser.Text.Trim()
        Dim password As String = txtDbPassword.Text
        If String.IsNullOrWhiteSpace(server) Then
            JsAlertDialog.ShowAlert(Me, "Please enter the database server.", "Missing Server",
            JsAlertDialog.AlertType.Warning, JsAlertDialog.ButtonType.OK, True, 20)
            TxtDbServer.Focus()
            Return
        End If
        If String.IsNullOrWhiteSpace(user) Then
            JsAlertDialog.ShowAlert(Me, "Please enter the database username.", "Missing Username",
            JsAlertDialog.AlertType.Warning, JsAlertDialog.ButtonType.OK, True, 20)
            TxtDbUser.Focus()
            Return
        End If
        If String.IsNullOrWhiteSpace(database) Then
            JsAlertDialog.ShowAlert(Me, "Please enter the database name.", "Missing Database",
            JsAlertDialog.AlertType.Warning, JsAlertDialog.ButtonType.OK, True, 20)
            TxtDbName.Focus()
            Return
        End If
        Dim port As UInteger = 3306
        If Not String.IsNullOrWhiteSpace(portText) Then
            If Not UInteger.TryParse(portText, port) Then
                JsAlertDialog.ShowAlert(Me, "Please enter a valid MySQL port number.", "Invalid Port",
                JsAlertDialog.AlertType.Warning, JsAlertDialog.ButtonType.OK, True, 20)
                TxtDbPort.Focus()
                Return
            End If
        End If
        btnTest.Enabled = False
        Try
            Dim builder As New MySqlConnectionStringBuilder With {
                .Server = server,
                .Port = port,
                .UserID = user,
                .Password = password,
                .Database = database
            }
            Using connection As New MySqlConnection(builder.ConnectionString)
                Await connection.OpenAsync()
            End Using
            JsAlertDialog.ShowAlert(Me, "Database connection successful!", "Success",
            JsAlertDialog.AlertType.Success, JsAlertDialog.ButtonType.OK, True, 20)
        Catch ex As MySqlException
            JsAlertDialog.ShowAlert(Me, "Unable to connect to the database." & vbCrLf & vbCrLf &
            "Error: " & ex.Message & vbCrLf & vbCrLf & "Server: " & server & vbCrLf & "Port: " & port &
            vbCrLf & "Database: [" & database & "]", "Connection Failed", JsAlertDialog.AlertType.Error,
                                                JsAlertDialog.ButtonType.OK, True, 20)
        Catch ex As Exception
            JsAlertDialog.ShowAlert(Me, "An unexpected error occurred while testing the connection." &
            vbCrLf & ex.Message, "Connection Test Failed", JsAlertDialog.AlertType.Error, JsAlertDialog.ButtonType.OK, True, 20)
        Finally
            btnTest.Enabled = True
        End Try
    End Sub

    ' --- Save System Settings ---
    Private Sub btnSaveSystem_Click(sender As Object, e As EventArgs) Handles btnSaveSystem.Click
        toggleGridView(False)
        Try
            My.Settings("db_server") = TxtDbServer.Text.Trim()
            My.Settings("db_port") = TxtDbPort.Text.Trim()
            My.Settings("db_name") = TxtDbName.Text.Trim()
            My.Settings("db_prefix") = TxtDbPrefix.Text.Trim()
            My.Settings("db_user") = TxtDbUser.Text.Trim()
            ' Encrypt the MySQL password before storing it.
            My.Settings("db_password") = Encorder.Encrypt(txtDbPassword.Text)
            My.Settings.Save()
            Dim result = JsAlertDialog.ShowAlert(Me, "Database Config saved successfully." &
            vbCrLf & vbCrLf & "Do you want to restart now?", "Restart Required",
            JsAlertDialog.AlertType.Confirm, JsAlertDialog.ButtonType.YesNo)
            If result = DialogResult.Yes Then
                RestartApplication()
            End If
        Catch ex As Exception
            JsAlertDialog.ShowAlert(Me, "Unable to save database configuration." &
            vbCrLf & vbCrLf & ex.Message, "Save Failed", JsAlertDialog.AlertType.Error,
            JsAlertDialog.ButtonType.OK, True, 20)
        Finally
            toggleGridView(True)
        End Try
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean
        ' CTRL + T test database connection
        If keyData = (Keys.Control Or Keys.T) Then
            btnTest.PerformClick()
            Return True
        End If
        ' CTRL + H Toggle password visibility
        If keyData = (Keys.Control Or Keys.H) Then
            TogglePasswordVisibility()
            Return True
        End If
        ' CTRL + D + S Database Server
        If IsKeyDown(Keys.ControlKey) AndAlso IsKeyDown(Keys.D) AndAlso IsKeyDown(Keys.S) Then
            FocusAndSelectAll(TxtDbServer)
            Return True
        End If
        ' CTRL + P + N Database Port
        If IsKeyDown(Keys.ControlKey) AndAlso IsKeyDown(Keys.P) AndAlso IsKeyDown(Keys.N) Then
            FocusAndSelectAll(TxtDbPort)
            Return True
        End If
        ' CTRL + D + N Database Name
        If IsKeyDown(Keys.ControlKey) AndAlso IsKeyDown(Keys.D) AndAlso IsKeyDown(Keys.N) Then
            FocusAndSelectAll(TxtDbName)
            Return True
        End If
        ' CTRL + D + P Database Prefix
        If IsKeyDown(Keys.ControlKey) AndAlso IsKeyDown(Keys.D) AndAlso IsKeyDown(Keys.P) Then
            FocusAndSelectAll(TxtDbPrefix)
            Return True
        End If
        ' CTRL + U + N Database Username
        If IsKeyDown(Keys.ControlKey) AndAlso IsKeyDown(Keys.U) AndAlso IsKeyDown(Keys.N) Then
            FocusAndSelectAll(TxtDbUser)
            Return True
        End If
        ' CTRL + U + P Database Password
        If IsKeyDown(Keys.ControlKey) AndAlso IsKeyDown(Keys.U) AndAlso IsKeyDown(Keys.P) Then
            FocusAndSelectAll(txtDbPassword)
            Return True
        End If
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Private Sub TxtDbServer_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtDbServer.KeyDown
        If e.KeyCode = Keys.Enter Then
            TxtDbPort.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub TxtDbPort_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtDbPort.KeyDown
        If e.KeyCode = Keys.Enter Then
            TxtDbName.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub TxtDbName_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtDbName.KeyDown
        If e.KeyCode = Keys.Enter Then
            TxtDbPrefix.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub TxtDbPrefix_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtDbPrefix.KeyDown
        If e.KeyCode = Keys.Enter Then
            TxtDbUser.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub TxtDbUser_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtDbUser.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtDbPassword.Focus()
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub TxtDbPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDbPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnSaveSystem.PerformClick()
            e.SuppressKeyPress = True
        End If
    End Sub

    'Helpers
    ' --- Load DB Settings into DataGridView ---
    Private Async Function LoadDBSettings() As Task
        dgvSettings.Rows.Clear()
        ' --- No manager → ask the user to configure the DB first ---
        If _settingsManager Is Nothing Then
            dgvSettings.Enabled = False
            Return
        End If
        dgvSettings.Enabled = True
        Try
            Dim allSettings = Await _settingsManager.GetAllSettings()
            ' Predefined default keys
            Dim defaultKeys = {"api_base_url", "shop_latitude", "shop_longitude", "timeout"}
            For Each keyName In defaultKeys
                Dim value As String = If(allSettings.ContainsKey(keyName), allSettings(keyName), "")
                dgvSettings.Rows.Add(keyName, value)
            Next
            ' Add other settings from DB
            For Each kvp In allSettings
                If Not defaultKeys.Contains(kvp.Key) Then
                    dgvSettings.Rows.Add(kvp.Key, kvp.Value)
                End If
            Next
        Catch ex As Exception
            ' Show error if DB cannot be reached
            JsAlertDialog.ShowAlert(SettingsForm.ActiveForm, "Cannot load DB settings: " & ex.Message,
                                "Error", JsAlertDialog.AlertType.Error, JsAlertDialog.ButtonType.OK)
            TabPageDB.Enabled = False
        End Try
        If dgvSettings.Columns("Delete") Is Nothing Then
            Dim btnCol As New DataGridViewButtonColumn()
            btnCol.Name = "Delete"
            btnCol.HeaderText = "Action"
            btnCol.Text = "Delete"
            btnCol.UseColumnTextForButtonValue = True
            btnCol.Width = 20
            btnCol.ToolTipText = "Delete this database setting"
            dgvSettings.Columns.Add(btnCol)
        End If
        SetupGridColumnWidths()
        dgvSettings.DefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvSettings.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvSettings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        dgvSettings.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
        dgvSettings.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        dgvSettings.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        dgvSettings.ColumnHeadersHeight = 30
        dgvSettings.EnableHeadersVisualStyles = False
    End Function

    ' --- For exposing system settings to core application ---
    Public Function GetSystemSetting(key As String) As String
        Return CStr(My.Settings(key))
    End Function

    Public Sub OpenSystemTab()
        Me.TabControlSettings.SelectedTab = Me.TabPageSystem
    End Sub

    Private Sub SetupGridColumnWidths()
        If dgvSettings.Columns.Count = 0 Then Exit Sub
        ' Adjust based on your column names
        With dgvSettings.Columns
            .Item("Key").FillWeight = 70     ' medium
            .Item("Value").FillWeight = 300 ' Big
            .Item("Delete").FillWeight = 50  ' Small
        End With
    End Sub

    Private Sub RestartApplication()
        Try
            ' Optional: clean up resources
            Application.ExitThread() ' closes all forms cleanly
            ' Restart application
            Application.Restart()
        Catch ex As Exception
            MessageBox.Show("Failed to restart application: " & ex.Message)
        End Try
    End Sub

    Private Sub toggleGridView(status As Boolean)
        dgvSettings.Enabled = status
    End Sub

    Private Sub TogglePasswordVisibility()
        Dim cursorPos As Integer = txtDbPassword.SelectionStart
        _isPasswordVisible = Not _isPasswordVisible
        txtDbPassword.UseSystemPasswordChar = Not _isPasswordVisible
        If _isPasswordVisible Then
            PicTogglePassword.Image = My.Resources.eye_open_outline
            _toolTip.SetToolTip(PicTogglePassword, "Hide password")
        Else
            PicTogglePassword.Image = My.Resources.eye_closed_outline
            _toolTip.SetToolTip(PicTogglePassword, "Show password")
        End If
        txtDbPassword.SelectionStart = cursorPos
        txtDbPassword.SelectionLength = 0
        txtDbPassword.Focus()
    End Sub

    Private Sub InitializeToolTips()
        ' General ToolTip behaviour
        _toolTip.AutoPopDelay = 8000
        _toolTip.InitialDelay = 500
        _toolTip.ReshowDelay = 200
        _toolTip.ShowAlways = True
        ' SYSTEM / DATABASE SETTINGS
        _toolTip.SetToolTip(TxtDbServer, "MySQL server hostname or IP address." & vbCrLf &
            "Example: localhost or 192.168.1.100" & vbCrLf & "Shortcut: Ctrl+D+S")
        _toolTip.SetToolTip(TxtDbPort, "Optional MySQL server port." & vbCrLf &
            "Leave blank if default MySQL port(3306)" & vbCrLf & "Shortcut: Ctrl+P+N")
        _toolTip.SetToolTip(TxtDbName, "Name of the MySQL database used by the application." & vbCrLf &
            "Shortcut: Ctrl+D+N")
        _toolTip.SetToolTip(TxtDbPrefix, "Optional database table prefix." & vbCrLf &
            "Leave blank if your tables do not use a prefix." & vbCrLf &
            "Shortcut: Ctrl+D+P")
        _toolTip.SetToolTip(TxtDbUser, "MySQL username used to connect to the database." & vbCrLf &
            "Shortcut: Ctrl+U+N")
        _toolTip.SetToolTip(txtDbPassword, "MySQL password used to connect to the database." & vbCrLf &
            "Shortcut: Ctrl+U+P")
        _toolTip.SetToolTip(PicTogglePassword, "Show password")
        ' BUTTONS
        _toolTip.SetToolTip(btnTest,
            "Test the database connection using the values currently entered." & vbCrLf &
            "This does not save the settings." & vbCrLf & "Shortcut: Ctrl+T")
        _toolTip.SetToolTip(btnSaveSystem, "Save the database configuration to the application's local settings." & vbCrLf &
            "The application may need to restart after saving." & vbCrLf & "Shortcut: Enter")
        _toolTip.SetToolTip(btnSaveDB,
            "Save the settings displayed in the database settings grid." & vbCrLf &
            "The application may need to restart after saving.")
        ' DATABASE SETTINGS GRID
        _toolTip.SetToolTip(dgvSettings, "Application settings stored in the database." & vbCrLf &
            "Double-click a row to copy its contents to the clipboard.")
        ' TABS
        _toolTip.SetToolTip(TabPageSystem, "Configure the database connection Settings used by the application.")
        _toolTip.SetToolTip(TabPageDB, "View and edit application settings stored in the database.")
    End Sub

    Private Sub FocusAndSelectAll(control As Control)
        control.Focus()
        If TypeOf control Is TextBox Then
            DirectCast(control, TextBox).SelectAll()
        ElseIf TypeOf control Is MaskedTextBox Then
            DirectCast(control, MaskedTextBox).SelectAll()
        ElseIf TypeOf control Is RichTextBox Then
            DirectCast(control, RichTextBox).SelectAll()
        End If
    End Sub

    <DllImport("user32.dll")>
    Private Shared Function GetAsyncKeyState(vKey As Keys) As Short
    End Function

    Private Shared Function IsKeyDown(key As Keys) As Boolean
        Return (GetAsyncKeyState(key) And &H8000) <> 0
    End Function

    Private Function BuildConnectionStringFromMySettings() As String
        Dim server = TryCast(My.Settings("db_server"), String)
        Dim user = TryCast(My.Settings("db_user"), String)
        Dim db = TryCast(My.Settings("db_name"), String)
        Dim encPwd = TryCast(My.Settings("db_password"), String)
        Dim portStr = TryCast(My.Settings("db_port"), String)
        ' --- Required fields ---
        If String.IsNullOrWhiteSpace(server) Then
            Return Nothing
        End If
        If String.IsNullOrWhiteSpace(user) Then
            Return Nothing
        End If
        If String.IsNullOrWhiteSpace(db) Then
            Return Nothing
        End If
        ' --- Password is OPTIONAL (root with no password is legal) ---
        Dim pwd As String = ""
        If Not String.IsNullOrWhiteSpace(encPwd) Then
            Try
                pwd = Encorder.Decrypt(encPwd)
                If pwd Is Nothing Then pwd = ""
            Catch ex As Exception
                Return Nothing
            End Try
        End If
        Dim builder As New MySqlConnectionStringBuilder With {
            .Server = server,
            .UserID = user,
            .Password = pwd,
            .Database = db
        }
        Dim port As UInteger
        If UInteger.TryParse(portStr, port) AndAlso port > 0 Then
            builder.Port = port
        End If
        Return builder.ConnectionString
    End Function
End Class