Imports System.Security.Cryptography
Imports System.Text

Namespace Helpers.Cryptography

    Public NotInheritable Class Encorder

        Private Sub New()
        End Sub

        Public Shared Function Encrypt(plainText As String) As String
            If String.IsNullOrEmpty(plainText) Then
                Return String.Empty
            End If
            Dim plainBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
            Dim protectedBytes As Byte() = ProtectedData.Protect(plainBytes, Nothing, DataProtectionScope.CurrentUser)
            Return Convert.ToBase64String(protectedBytes)
        End Function

        Public Shared Function Decrypt(cipherText As String) As String
            If String.IsNullOrEmpty(cipherText) Then
                Return String.Empty
            End If
            Dim protectedBytes As Byte() = Convert.FromBase64String(cipherText)
            Dim plainBytes As Byte() = ProtectedData.Unprotect(protectedBytes, Nothing, DataProtectionScope.CurrentUser)
            Return Encoding.UTF8.GetString(plainBytes)
        End Function
    End Class
End Namespace