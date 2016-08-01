
Partial Class HR_MyInfo
    Inherits BasePage

    Protected Sub HR_MyInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim userdetails As DataLibrary.Users_GetDetailsByID_Result = _
                                UserManager.GetDetailsByID(UserAuthentication.User.UserID)
                SetEntityFooter(userdetails.CreatedBy, userdetails.DateCreated, userdetails.ModifiedBy, userdetails.DateModified)
                PopulateBasicInfo(userdetails)
                PopulateContactInfo(userdetails)
            End If
    End Sub

    Private Sub PopulateBasicInfo(obj As DataLibrary.Users_GetDetailsByID_Result)
        ucPicture.ObjectID = MilesMetaObjects.Users
        ucPicture.KeyID = obj.UserID

        lblFullName.Text = obj.Fullname
        lblUserName.Text = obj.Username
        lblStatus.Text = obj.Status
        lblJobTitle.Text = If(String.IsNullOrEmpty(obj.JobTitle), "", obj.JobTitle)
        lblUserRoles.Text = If(String.IsNullOrEmpty(obj.UserRoles), "", obj.UserRoles)


        Dim btnchangePassword As RadToolBarButton = rdActions.FindButtonByCommandName("UpdatePassword")
        btnchangePassword.NavigateUrl = "javascript: changePasswordDialog(" & UserAuthentication.User.UserID & ");"
        rdActions.FindButtonByCommandName("UpdatePicture").NavigateUrl = "javascript: changePictureDialog();"

        If UserAuthentication.User.UserID = 1 Then
            btnchangePassword.Visible = False
        End If
        
    End Sub

    Private Sub PopulateContactInfo(obj As DataLibrary.Users_GetDetailsByID_Result)

        lnkContactInfoHeader.NavigateUrl = "/HR/EditMyInfo.aspx"

        lnkContactEmail.Text = obj.Email
        lnkContactEmail.NavigateUrl = "mailto:" & obj.Email
        lblPhone.Text = (<div><a href=<%= "tel:" & obj.WorkPhone %>><%= FormatPhone(PhoneNumberType.Work, obj.WorkPhone, obj.WorkPhoneExt) %></a></div>).ToString() & _
                        (<div><a href=<%= "tel:" & obj.HomePhone %>><%= FormatPhone(PhoneNumberType.Home, obj.HomePhone) %></a></div>).ToString() &
                        (<div><a href=<%= "tel:" & obj.MobilePhone %>><%= FormatPhone(PhoneNumberType.Mobile, obj.MobilePhone) %></a></div>).ToString()

        lblAddress.Text = obj.FullAddress
    End Sub

 
End Class
