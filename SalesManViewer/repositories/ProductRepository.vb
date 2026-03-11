Imports System.Net.Mime.MediaTypeNames
Imports System.Reflection.Emit
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports MySql.Data.MySqlClient
Imports SalesManViewer.helpers

Namespace repositories
    Public Class ProductRepository
        Public Function GetLocalProducts() As DataTable
            Dim dt As New DataTable()
            Using conn = DatabaseHelper.GetConnection()
                Dim sql As String = "SELECT ProductCode, ProductName, DepartmentCode, SupplierPacking, SupplierPackingDetails, ProductUnit, " &
                    "TagPrice, Product_VAT_Code, Product_Cost_Price, Product_Last_Cost_Price, Product_Margin, Product_Selling_Price, Min_Qty, " &
                    "ReOrd_Level, Qty_to_Order, SupplierCode, Weight, Product_Qty, isAlternetUnit, AlternetUnit, UnitValue, AlternetUnitValue, " &
                    "HSCode, HSDesc, isActive, isStockItem, Remark, Created, CreatedBy, Modified, ModifiedBy, SrNo, img_src FROM productmaster " &
                    "ORDER BY ProductName"
                Using cmd As New MySqlCommand(sql, conn)
                    Using adapter As New MySqlDataAdapter(cmd)
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
            Return dt
        End Function
    End Class
End Namespace
