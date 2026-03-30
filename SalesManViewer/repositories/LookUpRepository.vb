Imports System.Net.Http
Imports Newtonsoft.Json

Namespace repositories
    Public Class LookupRepository
        Private serverUrl As String = "http://197.248.220.180/salesman-backend"

        Public Async Function GetCustomers() As Task(Of DataTable)
            Return Await FetchLookup("customers")
        End Function

        Public Async Function GetSalesmen() As Task(Of DataTable)
            Return Await FetchLookup("salesmen")
        End Function

        Private Async Function FetchLookup(type As String) As Task(Of DataTable)
            Using client As New HttpClient()
                Dim url = $"{serverUrl}/api/lookups.php?action={type}"
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
    End Class
End Namespace