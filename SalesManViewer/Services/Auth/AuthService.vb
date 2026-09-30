Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports SalesManViewer.Models.Auth

Namespace Services.Auth

    Public Class AuthService

        ' A single, shared HttpClient — correct practice for modern .NET.
        Private Shared ReadOnly _http As HttpClient = CreateHttpClient()
        Private ReadOnly _baseUrl As String

        Public Sub New(baseUrl As String)
            If String.IsNullOrWhiteSpace(baseUrl) Then
                Throw New ArgumentException("Base URL is required.", NameOf(baseUrl))
            End If
            _baseUrl = baseUrl.TrimEnd("/"c)
        End Sub

        Private Shared Function CreateHttpClient() As HttpClient
            Dim c As New HttpClient()
            c.Timeout = TimeSpan.FromSeconds(30)
            c.DefaultRequestHeaders.Accept.ParseAdd("application/json")
            Return c
        End Function

        ''' <summary>
        ''' POST /api/auth.php?action=login with form fields userName and password.
        ''' Returns the parsed response, or Nothing if the body was empty.
        ''' </summary>

        Public Async Function LoginAsync(userName As String, password As String) As Task(Of LoginResponse)
            Dim url = $"{_baseUrl}/api/auth.php?action=login"
            ' --- JSON payload instead of form-encoded ---
            Dim payload As String = JsonConvert.SerializeObject(New With {
                .userName = userName,
                .password = password
            })
            Using content As New StringContent(payload, Encoding.UTF8, "application/json")
                Dim response = Await _http.PostAsync(url, content)
                Dim json = Await response.Content.ReadAsStringAsync()
                Debug.WriteLine("LOGIN REQ: " & payload)
                Debug.WriteLine("LOGIN RES: " & json)
                If String.IsNullOrWhiteSpace(json) OrElse json.TrimStart().StartsWith("<") Then
                    Return New LoginResponse With {
                        .Success = False,
                        .Code = CInt(response.StatusCode),
                        .Message = $"Server returned {CInt(response.StatusCode)}. Unable to parse response."
                    }
                End If
                Try
                    Return JsonConvert.DeserializeObject(Of LoginResponse)(json)
                Catch ex As JsonException
                    Return New LoginResponse With {
                        .Success = False,
                        .Code = CInt(response.StatusCode),
                        .Message = "Unexpected response from server: " & ex.Message
                    }
                End Try
            End Using
        End Function
    End Class
End Namespace