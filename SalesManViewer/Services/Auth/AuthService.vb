Imports System.Net.Http
Imports System.Text
Imports Newtonsoft.Json
Imports SalesManViewer.Models.ApiResponse

Namespace Services.Auth

    Public Class AuthService
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
        ''' POST /api/auth.php?action=login with form fields userName and password. Returns the parsed response, 
        ''' or Nothing if the body was empty.
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

        ''' <summary>
        ''' POST /api/auth.php?action=request-reset Sends an OTP to the user's registered email, 
        ''' if the account exists. Server always returns success — this prevents username enumeration.
        ''' </summary>
        Public Async Function RequestPasswordResetAsync(userName As String) As Task(Of AuthApiResponse)
            Dim url = $"{_baseUrl}/api/auth.php?action=request-reset"
            Dim payload = JsonConvert.SerializeObject(New With {.username = userName})
            Return Await PostJsonAsync(Of AuthApiResponse)(url, payload)
        End Function

        ''' <summary>
        ''' POST /api/auth.php?action=reset-password-otp Validates the OTP and sets the new password.
        ''' </summary>
        Public Async Function ResetPasswordWithOtpAsync(userName As String, otp As String, newPassword As String) As Task(Of AuthApiResponse)
            Dim url = $"{_baseUrl}/api/auth.php?action=reset-password-otp"
            Dim payload = JsonConvert.SerializeObject(New With {
                .userName = userName,
                .otp = otp,
                .newPassword = newPassword
            })
            Return Await PostJsonAsync(Of AuthApiResponse)(url, payload)
        End Function

        ''' <summary>
        ''' Shared POST-with-JSON helper. Handles HTML error pages and JSON parse failures .
        ''' </summary>
        Private Async Function PostJsonAsync(Of T)(url As String, jsonPayload As String) As Task(Of T)
            Using content As New StringContent(jsonPayload, Encoding.UTF8, "application/json")
                Dim response = Await _http.PostAsync(url, content)
                Dim body = Await response.Content.ReadAsStringAsync()
                ' Guard: HTML error page (404, 500, WAF, etc.)
                If String.IsNullOrWhiteSpace(body) OrElse body.TrimStart().StartsWith("<") Then
                    Return JsonConvert.DeserializeObject(Of T)(
                        JsonConvert.SerializeObject(New With {
                            .code = CInt(response.StatusCode),
                            .success = False,
                            .message = $"Server returned {CInt(response.StatusCode)}."
                        }))
                End If
                Try
                    Return JsonConvert.DeserializeObject(Of T)(body)
                Catch ex As JsonException
                    Return JsonConvert.DeserializeObject(Of T)(
                        JsonConvert.SerializeObject(New With {
                            .code = CInt(response.StatusCode),
                            .success = False,
                            .message = "Unexpected response from server: " & ex.Message
                        }))
                End Try
            End Using
        End Function
    End Class
End Namespace