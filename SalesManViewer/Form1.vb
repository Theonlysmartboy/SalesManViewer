Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim chk As New DataGridViewCheckBoxColumn()
        chk.HeaderText = "Select"
        chk.Width = 50
        DgvLocalProducts.Columns.Insert(0, chk)
        DgvLocalProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DgvLocalProducts.MultiSelect = False
        DgvLocalProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

        Dim chk1 As New DataGridViewCheckBoxColumn()
        chk1.HeaderText = "Select"
        chk1.Width = 50
        DgvOnlineProducts.Columns.Insert(0, chk1)
        DgvOnlineProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DgvOnlineProducts.MultiSelect = False
        DgvOnlineProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub
End Class
