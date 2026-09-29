Imports System.Drawing
Imports System.Windows.Forms

Public Class DateTimePickerDialog
    Inherits Form

    Public ReadOnly Property SelectedDateTime As DateTime
        Get
            Return _dtp.Value
        End Get
    End Property

    Private ReadOnly _dtp As DateTimePicker

    ''' <param name="title">Window title.</param>
    ''' <param name="includeTime">True = pick date + time; False = pick date only.</param>
    ''' <param name="initial">Initial value to display.</param>
    Public Sub New(title As String, includeTime As Boolean, Optional initial As DateTime = Nothing)
        If initial = Nothing Then initial = DateTime.Now
        Me.Text = title
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterParent
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.ShowInTaskbar = False
        Me.Font = New Font("Segoe UI", 9.0F)
        Me.ClientSize = New Size(320, 120)
        Dim lbl As New Label With {
            .Text = If(includeTime, "Select date and time:", "Select date:"),
            .Location = New Point(18, 12),
            .AutoSize = True
        }
        _dtp = New DateTimePicker With {
            .Format = If(includeTime, DateTimePickerFormat.Custom, DateTimePickerFormat.Long),
            .CustomFormat = If(includeTime, "yyyy-MM-dd HH:mm:ss", ""),
            .ShowUpDown = includeTime,
            .Value = initial,
            .Location = New Point(18, 34),
            .Width = 280
        }
        Dim btnOk As New Button With {
            .Text = "OK",
            .DialogResult = DialogResult.OK,
            .Location = New Point(118, 76),
            .Size = New Size(80, 30)
        }
        Dim btnCancel As New Button With {
            .Text = "Cancel",
            .DialogResult = DialogResult.Cancel,
            .Location = New Point(204, 76),
            .Size = New Size(80, 30)
        }
        Me.Controls.Add(lbl)
        Me.Controls.Add(_dtp)
        Me.Controls.Add(btnOk)
        Me.Controls.Add(btnCancel)
        Me.AcceptButton = btnOk
        Me.CancelButton = btnCancel
    End Sub
End Class