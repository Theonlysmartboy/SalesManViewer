
Imports SalesManViewer.Models.Auth

Namespace Helpers.Security

    Public Module SecurityHelper

        Public Function HasRole(ParamArray requiredRoles() As String) As Boolean
            If UserContext.Instance.Roles Is Nothing Then Return False
            For Each r In requiredRoles
                If UserContext.Instance.Roles.Contains(r.ToLower()) Then Return True
            Next
            Return False
        End Function

        Public Function Can(permission As String) As Boolean
            If UserContext.Instance.Permissions Is Nothing Then Return False
            Return UserContext.Instance.Permissions.Contains(permission.ToLower())
        End Function

        Public Function ValidatePassword(pwd As String) As String
            If pwd.Length < 6 Then
                Return "Password must be at least 6 characters long."
            End If
            If Not pwd.Any(AddressOf Char.IsUpper) Then
                Return "Password must contain at least one uppercase letter."
            End If
            If Not pwd.Any(AddressOf Char.IsLower) Then
                Return "Password must contain at least one lowercase letter."
            End If
            If Not pwd.Any(AddressOf Char.IsDigit) Then
                Return "Password must contain at least one number."
            End If
            Dim specialChars As String = "!@#$%^&*()_+-={}[]|:;'<>,.?/~`"
            If Not pwd.Any(Function(c) specialChars.Contains(c)) Then
                Return "Password must contain at least one special character (e.g., !, @, #, $, %)."
            End If
            Dim weakList As String() = {"password", "12345678", "qwerty", "letmein", "admin"}
            If weakList.Any(Function(w) pwd.ToLower().Contains(w)) Then
                Return "The password you entered is too weak. Please choose a stronger password."
            End If
            Return Nothing  ' valid password
        End Function
    End Module
End Namespace