Imports MySql.Data.MySqlClient
Namespace Helpers.Database

    Public Class DbTransactionContext
        Public Property Connection As MySqlConnection
        Public Property Transaction As MySqlTransaction
    End Class

    Public Class DbHelper
        Private ReadOnly _connectionString As String

        Public Sub New(connString As String)
            _connectionString = connString
        End Sub

        'Begin transaction
        Public Function BeginTransaction() As DbTransactionContext
            Dim conn As New MySqlConnection(_connectionString)
            conn.Open()
            Dim tx = conn.BeginTransaction()
            Return New DbTransactionContext With {
                .Connection = conn,
                .Transaction = tx
            }
        End Function

        'Commit transaction
        Public Sub Commit(dbContext As DbTransactionContext)
            If dbContext Is Nothing Then Return
            dbContext.Transaction?.Commit()
            dbContext.Connection?.Close()
            dbContext.Transaction = Nothing
            dbContext.Connection = Nothing
        End Sub

        'Rollback
        Public Sub Rollback(dbContext As DbTransactionContext)
            If dbContext Is Nothing Then Return
            Try
                dbContext.Transaction?.Rollback()
            Catch
            End Try
            dbContext.Connection?.Close()
            dbContext.Transaction = Nothing
            dbContext.Connection = Nothing
        End Sub

        'INSERT
        Public Function ExecuteInsert(query As String, dbContext As DbTransactionContext, Optional params As Dictionary(Of String, Object) = Nothing) As Long
            Using cmd As New MySqlCommand(query, dbContext.Connection, dbContext.Transaction)
                If params IsNot Nothing Then
                    For Each kvp In params
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value)
                    Next
                End If
                cmd.ExecuteNonQuery()
                Return cmd.LastInsertedId
            End Using
        End Function

        ' SELECT
        Public Function ExecuteSelect(query As String, dbContext As DbTransactionContext, Optional params As Dictionary(Of String, Object) = Nothing) As DataTable
            Dim dt As New DataTable()
            Using cmd As New MySqlCommand(query, dbContext.Connection, dbContext.Transaction)
                If params IsNot Nothing Then
                    For Each kvp In params
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value)
                    Next
                End If
                Using adapter As New MySqlDataAdapter(cmd)
                    adapter.Fill(dt)
                End Using
            End Using
            Return dt
        End Function

        ' Non-transaction overloads
        Public Function ExecuteInsert(query As String, Optional params As Dictionary(Of String, Object) = Nothing) As Long
            Using conn As New MySqlConnection(_connectionString)
                conn.Open()
                Using cmd As New MySqlCommand(query, conn)
                    If params IsNot Nothing Then
                        For Each kvp In params
                            cmd.Parameters.AddWithValue(kvp.Key, kvp.Value)
                        Next
                    End If
                    cmd.ExecuteNonQuery()
                    Return cmd.LastInsertedId
                End Using
            End Using
        End Function

        Public Function ExecuteSelect(query As String, Optional params As Dictionary(Of String, Object) = Nothing) As DataTable
            Dim dt As New DataTable()
            Using conn As New MySqlConnection(_connectionString)
                Using cmd As New MySqlCommand(query, conn)
                    If params IsNot Nothing Then
                        For Each kvp In params
                            cmd.Parameters.AddWithValue(kvp.Key, kvp.Value)
                        Next
                    End If
                    Using adapter As New MySqlDataAdapter(cmd)
                        conn.Open()
                        adapter.Fill(dt)
                    End Using
                End Using
            End Using
            Return dt
        End Function

        ' UPDATE / DELETE
        Public Function ExecuteNonQuery(query As String, dbContext As DbTransactionContext, Optional params As Dictionary(Of String, Object) = Nothing) As Integer
            Using cmd As New MySqlCommand(query, dbContext.Connection, dbContext.Transaction)
                If params IsNot Nothing Then
                    For Each kvp In params
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value)
                    Next
                End If
                Return cmd.ExecuteNonQuery()
            End Using
        End Function

        Public Function ExecuteNonQuery(query As String, Optional params As Dictionary(Of String, Object) = Nothing) As Integer
            Using conn As New MySqlConnection(_connectionString)
                Using cmd As New MySqlCommand(query, conn)
                    If params IsNot Nothing Then
                        For Each kvp In params
                            cmd.Parameters.AddWithValue(kvp.Key, kvp.Value)
                        Next
                    End If
                    conn.Open()
                    Return cmd.ExecuteNonQuery()
                End Using
            End Using
        End Function

        ' SCALAR
        Public Function ExecuteScalar(query As String, dbContext As DbTransactionContext, Optional params As Dictionary(Of String, Object) = Nothing) As Object
            Using cmd As New MySqlCommand(query, dbContext.Connection, dbContext.Transaction)
                If params IsNot Nothing Then
                    For Each kvp In params
                        cmd.Parameters.AddWithValue(kvp.Key, kvp.Value)
                    Next
                End If
                Return cmd.ExecuteScalar()
            End Using
        End Function

        Public Function ExecuteScalar(query As String, Optional params As Dictionary(Of String, Object) = Nothing) As Object
            Using conn As New MySqlConnection(_connectionString)
                Using cmd As New MySqlCommand(query, conn)
                    If params IsNot Nothing Then
                        For Each kvp In params
                            cmd.Parameters.AddWithValue(kvp.Key, kvp.Value)
                        Next
                    End If
                    conn.Open()
                    Return cmd.ExecuteScalar()
                End Using
            End Using
        End Function

        Public Function DataTableToList(Of T As New)(dt As DataTable) As List(Of T)
            Dim list As New List(Of T)()
            For Each row As DataRow In dt.Rows
                Dim obj As New T()
                For Each prop In GetType(T).GetProperties()
                    Dim colAttr = prop.GetCustomAttributes(GetType(DataColumnNameAttribute), True).FirstOrDefault()
                    Dim columnName As String = If(colAttr IsNot Nothing, DirectCast(colAttr,
                            DataColumnNameAttribute).Name, prop.Name)
                    If dt.Columns.Contains(columnName) AndAlso row(columnName) IsNot DBNull.Value Then
                        Dim value = row(columnName)
                        Dim targetType As Type = If(Nullable.GetUnderlyingType(prop.PropertyType), prop.PropertyType)
                        If targetType Is GetType(Boolean) Then
                            prop.SetValue(obj, Convert.ToBoolean(value))
                        ElseIf targetType Is GetType(Byte()) Then
                            prop.SetValue(obj, DirectCast(value, Byte()))
                        Else
                            Dim convertedValue = Convert.ChangeType(value, targetType)
                            prop.SetValue(obj, convertedValue)
                        End If
                    End If
                Next
                list.Add(obj)
            Next
            Return list
        End Function

        <AttributeUsage(AttributeTargets.Property)>
        Public Class DataColumnNameAttribute
            Inherits Attribute
            Public Property Name As String
            Public Sub New(columnName As String)
                Name = columnName
            End Sub
        End Class

        Public Function IsTableEmpty(tableName As String) As Boolean
            Dim query = $"SELECT COUNT(*) FROM {tableName}"
            Using conn As New MySqlConnection(_connectionString)
                Using cmd As New MySqlCommand(query, conn)
                    conn.Open()
                    Dim count = Convert.ToInt32(cmd.ExecuteScalar())
                    Return count = 0
                End Using
            End Using
        End Function
    End Class
End Namespace