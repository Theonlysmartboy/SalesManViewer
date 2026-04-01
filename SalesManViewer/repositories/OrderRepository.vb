Imports System.Net.Http
Imports Newtonsoft.Json
Imports System.Text

Namespace repositories
    Public Class OrderRepository
        Private serverUrl As String

        Public Sub New(serverUrl As String)
            Me.serverUrl = serverUrl
        End Sub

        Public Async Function GetOrders(filters As Dictionary(Of String, String)) As Task(Of DataTable)
            Using client As New HttpClient()
                Dim query As String = String.Join("&", filters.Select(Function(k) $"{k.Key}={k.Value}"))
                Dim url = $"{serverUrl}/api/orders.php?action=fetch&{query}"
                Dim response = Await client.GetStringAsync(url)
                Dim json = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(response)
                Dim dt As New DataTable()
                If json.ContainsKey("data") Then
                    Dim list = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, Object)))(json("data").ToString())
                    If list.Count > 0 Then
                        For Each key In list(0).Keys
                            dt.Columns.Add(key)
                        Next
                        For Each item In list
                            Dim row = dt.NewRow()
                            For Each key In item.Keys
                                row(key) = item(key)
                            Next
                            dt.Rows.Add(row)
                        Next
                    End If
                End If
                Return dt
            End Using
        End Function

        Public Async Function DeleteOrders(orderIds As List(Of String)) As Task
            Using client As New HttpClient()
                Dim url = $"{serverUrl}/api/orders.php?action=delete"
                Dim payload = JsonConvert.SerializeObject(New With {.ids = orderIds})
                Dim content = New StringContent(payload, Encoding.UTF8, "application/json")
                Await client.PostAsync(url, content)
            End Using
        End Function
    End Class
End Namespace