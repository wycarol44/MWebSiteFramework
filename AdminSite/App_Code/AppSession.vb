Imports Microsoft.VisualBasic

Public Class AppSession

#Region "Private"
    Private Shared ReadOnly Property Session As HttpSessionState
        Get
            Return HttpContext.Current.Session
        End Get
    End Property

#End Region


#Region "Public"

    'Public Shared Property User As DataLibrary.Users_ValidateLogin_Result
    '    Get
    '        Return Session("User")
    '    End Get
    '    Set(value As DataLibrary.Users_ValidateLogin_Result)
    '        Session("User") = value
    '    End Set
    'End Property


#End Region

End Class
