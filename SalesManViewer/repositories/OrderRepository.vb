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
                If Not filters.ContainsKey("limit") Then filters("limit") = "20"
                If Not filters.ContainsKey("offset") Then filters("offset") = "0"
                Dim query As String = String.Join("&", filters.Select(Function(k) $"{k.Key}={Uri.EscapeDataString(k.Value)}"))
                Dim url = $"{serverUrl}/api/orders.php?action=fetch&{query}"
                Dim response = Await client.GetAsync(url)
                response.EnsureSuccessStatusCode()
                Dim jsonString = Await response.Content.ReadAsStringAsync()
                Dim json = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(jsonString)
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
                                row(key) = If(item(key), DBNull.Value)
                            Next
                            dt.Rows.Add(row)
                        Next
                    End If
                End If
                Return dt
            End Using
        End Function

        Public Async Function UpdateOrderStatus(orderNo As String, status As String) As Task(Of Boolean)
            Using client As New HttpClient()
                Dim url = $"{serverUrl}/api/orders.php?action=update-status"
                Dim cleanStatus = status.Trim()
                Dim payload = New With {
                    .OrderNo = orderNo,
                    .Status = cleanStatus
                }
                Dim jsonPayload = JsonConvert.SerializeObject(payload)
                Dim content = New StringContent(jsonPayload, Encoding.UTF8, "application/json")
                Dim response = Await client.PostAsync(url, content)
                'response.EnsureSuccessStatusCode()
                'Dim response = Await client.PostAsync(url, content)
                Dim responseBody = Await response.Content.ReadAsStringAsync()
                If Not response.IsSuccessStatusCode Then
                    Throw New Exception($"Error {response.StatusCode}: {responseBody}")
                End If
                Dim jsonString = Await response.Content.ReadAsStringAsync()
                Dim json = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(jsonString)
                Return json.ContainsKey("success") AndAlso CBool(json("success"))
            End Using
        End Function

        Public Async Function DeleteOrders(orderNos As List(Of String)) As Task(Of Boolean)
            Using client As New HttpClient()
                Dim url As String
                Dim payload As String
                If orderNos.Count = 1 Then
                    ' Single delete
                    url = $"{serverUrl}/api/orders.php?action=delete"
                    payload = JsonConvert.SerializeObject(New With {
                        .OrderNumber = orderNos(0)
                    })
                Else
                    ' Bulk delete
                    url = $"{serverUrl}/api/orders.php?action=bulk-delete"
                    payload = JsonConvert.SerializeObject(New With {
                        .OrderNumbers = orderNos
                    })
                End If
                Dim content = New StringContent(payload, Encoding.UTF8, "application/json")
                Dim response = Await client.PostAsync(url, content)
                Dim responseBody = Await response.Content.ReadAsStringAsync()
                If Not response.IsSuccessStatusCode Then
                    Throw New Exception($"Error {response.StatusCode}: {responseBody}")
                End If
                Dim json = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(responseBody)
                ' single response
                If orderNos.Count = 1 Then
                    If json.ContainsKey("success") AndAlso CBool(json("success")) Then
                        Return True
                    Else
                        Throw New Exception(json("message").ToString())
                    End If
                End If
                ' bulk response
                If json.ContainsKey("results") Then
                    Dim results = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, Object)))(json("results").ToString())
                    Dim failed = results.Where(Function(r) Not CBool(r("success"))).ToList()
                    If failed.Count > 0 Then
                        Dim failedIds = String.Join(", ", failed.Select(Function(f) f("OrderNumber").ToString()))
                        Throw New Exception($"Some deletions failed: {failedIds}")
                    End If
                    Return True
                End If
                Return False
            End Using
        End Function
    End Class
End Namespace