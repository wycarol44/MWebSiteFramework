
Partial Class Login
    Inherits BasePage

    Private Property UserID As Integer
        Get
            Return ToInteger(ViewState("UserID"))
        End Get
        Set(value As Integer)
            ViewState("UserID") = value
        End Set
    End Property

    Protected Sub Login_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            chkRememberMe.Visible = AppSettings.RememberMeEnabled
            ResetPassword()
        End If

    End Sub

#Region "Events"

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click

        Try
            'log the user into the system
            Dim success = UserAuthentication.Login(txtUsername.Text, txtPassword.Text)

            'check value
            If (success) Then

                'clear the menu cache
                MetaMenuManager.ClearMenuCache(UserAuthentication.User.UserID)


                'save the login log
                Dim loginlog As New UserLoginLog
                loginlog.UserID = UserAuthentication.User.UserID
                UserLoginLogManager.Save(loginlog)

                'redirect to the default page
                UserAuthentication.RedirectFromLogin(chkRememberMe.Checked)
            Else
                'show message that user was not logged in
                JGrowl.ShowMessage(JGrowlMessageType.Error, "Your login information is incorrect.")
            End If
        Catch ex As Exception
            JGrowl.ShowMessage(JGrowlMessageType.Error, "There was a problem authenticating you. Please try again at a later time.")
        End Try

        'show the login panel
        ShowPanel(loginform, 0)
    End Sub

    Protected Sub btnSendResetEmail_Click(sender As Object, e As EventArgs) Handles btnSendResetEmail.Click

        SendResetPasswordEmail()



        'show the login panel
        ShowPanel(loginform)
    End Sub


    Protected Sub btnResetPassword_Click(sender As Object, e As EventArgs) Handles btnResetPassword.Click
        UserManager.UpdatePassword(UserID, txtResetPassword.Text, UserID)

        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, "Password updated successfully")

        'show the login panel
        ShowPanel(loginform)
    End Sub

#End Region

#Region "Methods"


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SendResetPasswordEmail()
        Dim cmsContent = CMSCategoryManager.GetByID(CMSCategories.ResetPassword)

        Dim u = UserManager.UpdateResetKey(txtForgotPass.Text)

        If u IsNot Nothing Then
            Dim Subject, Body As String
            Subject = cmsContent.ContentSubject
            Body = cmsContent.ContentBody

            Dim cu As User = UserManager.GetById(u.UserID)
            'Merge User Fields
            Subject = CMSMergeFieldManager.MergeUserFields(Subject, cu)
            Body = CMSMergeFieldManager.MergeUserFields(Body, cu)

            'Reset Password Content 
            Dim rc = CMSCategoryManager.GetByID(CMSCategories.ResetPasswordLink)
            Dim resetKey As String = ByteArrayToHexString(u.ResetKey)

            Dim resetPasswordLink As String = <a href=<%= AppSettings.AdminSiteURL & "/Security/Login.aspx?ResetKey=" & resetKey %>><%= rc.ContentBody %></a>.ToString()

            Body = Body.Replace("[[ResetPasswordLink]]", resetPasswordLink)


            'Get company info
            Dim c = CompanyInfoManager.GetInfo()

            Subject = CMSMergeFieldManager.MergeCompanyFields(Subject, c)
            Body = CMSMergeFieldManager.MergeCompanyFields(Body, c)


            'Send Email
            EmailLogManager.SendEmail(c.Email, u.Email, Subject, Body)

            'show message
            JGrowl.ShowMessage(JGrowlMessageType.Success, "Instructions on how to reset your password have been sent to the email address associated to your account.")
        Else
            'show message
            JGrowl.ShowMessage(JGrowlMessageType.Error, "Sorry, we could not find an active user by that name.")

        End If

        

    End Sub

    Private Sub ResetPassword()

        'if queryString Contains ResetKey... check if it is not expired : take user to reset password panel else take them to  forgot password  panel
        Dim hexResetKey = Request.QueryString("ResetKey")

        If Not String.IsNullOrWhiteSpace(hexResetKey) Then
            Dim u = UserManager.ValidateResetKey(hexResetKey)

            If IsNothing(u) Then
                lblInfo.Text = "Your Reset Password Link has been expired. Provide your username and click Submit. An email will be sent to the email address associated to your account."
                ShowPanel(forgotpass)
            Else
                UserID = u.UserID

                ShowPanel(resetpass)
            End If
        Else
            ShowPanel(loginform)
        End If

    End Sub

    Private Sub ShowPanel(pnl As Panel, Optional duration As Integer = 600)
        'add a script to execute and show the desired form
        rdAjaxPanel.ResponseScripts.Add(String.Format("login.showForm('#{0}',{1});", pnl.ClientID, duration))
    End Sub



#End Region



End Class
