Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Web.Security
Imports CommonLibrary


Public Class UserAuthentication

    ''' <summary>
    ''' Gets the currently logged in user's info
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Property User As DataLibrary.Users_ValidateLogin_Result
        Get
            Return HttpContext.Current.Session("User")
        End Get
        Set(value As DataLibrary.Users_ValidateLogin_Result)
            HttpContext.Current.Session("User") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets a string representing the logged in user
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared ReadOnly Property UserName As String
        Get
            Return String.Format("{0}|{1}", User.UserID, User.Email)
        End Get
    End Property

    ' ''' <summary>
    ' ''' Reloads the user object based on id
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Shared Sub Refresh()
    '    'refresh the user object
    '    UserAuthentication.User = UserManager.ValidateUserId(UserAuthentication.User.UserID)
    'End Sub


    ''' <summary>
    ''' check if there is a authentication ticket, if yes login, if not then signout
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsUserAuthenticated() As Boolean
        Dim userId = GetUserIdentity()
        If userId > 0 Then

            'check login by using id
            User = UserManager.ValidateUserId(userId)

            If User IsNot Nothing Then
                Return True
            Else
                SignOut()
                Return False
            End If
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Check if the user is already authenticated in forms authentication. if yes return the user id.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUserIdentity() As Integer
        Dim userId As Integer = 0

        Try
            Dim identity = System.Web.HttpContext.Current.User.Identity.Name

            If Not String.IsNullOrWhiteSpace(identity) Then
                'split the username
                Dim idParts = identity.Split("|")

                'check to see if we have an id in the identity
                If idParts.Length > 0 AndAlso ToInteger(idParts(0)) > 0 Then

                    'store the user id
                    userId = ToInteger(idParts(0))
                End If
            End If

        Catch ex As Exception
            'catch this exception, as we arent interested in errors
        End Try

        Return userId
    End Function

    ''' <summary>
    ''' Checks the user's credentials
    ''' </summary>
    ''' <param name="username"></param>
    ''' <param name="password"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Login(username As String, password As String) As Boolean
        'use forms authentication
        User = UserManager.ValidateLogin(username, password)

        If User Is Nothing AndAlso AppSettings.ADEnabled Then
            Try
                'check active directory
                User = UserManager.ValidateADLogin(username, password)
            Catch ex As Exception

            End Try
            
        End If
        'check if we have successful login
        Return User IsNot Nothing
    End Function

    ''' <summary>
    ''' Redirect the logged in user from the login page
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub RedirectFromLogin(createPersistentCookie As Boolean)

        If createPersistentCookie AndAlso User.UserID <> AppSettings.ADImpersonateUserID Then
            'create a ticket, then redirect
            CreatePersistentTicket()

            'get redirect url
            Dim url = FormsAuthentication.GetRedirectUrl(UserName, True)

            'do redirect
            HttpContext.Current.Response.Redirect(url)

        Else
            FormsAuthentication.RedirectFromLoginPage(UserName, False)
        End If


    End Sub

    ''' <summary>
    ''' Creates a persistent ticket used to determine if a user is still logged in
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub CreatePersistentTicket()

        Dim authTicket As New FormsAuthenticationTicket(UserName, True, 1439200)
        'should be same as cookie expiration
        Dim encryptedTicket As String = FormsAuthentication.Encrypt(authTicket)
        Dim authCookie As New HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
        authCookie.Expires = DateTime.Now.AddMonths(3)
        'make sure its same as the formsauthentication ticket expiry value
        HttpContext.Current.Response.Cookies.Add(authCookie)
    End Sub

    ''' <summary>
    ''' Signs a user out of the system and takes them back to the login page
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub SignOut()
        FormsAuthentication.SignOut()
        'clear session
        HttpContext.Current.Session.Clear()
        HttpContext.Current.Session.Abandon()

        'go back to login page
        FormsAuthentication.RedirectToLoginPage()
    End Sub

End Class
