
Partial Class HR_UserDashboard
    Inherits BasePage
#Region "Properties"
    Public Property UserId As Integer
        Get
            Dim v As Object = ViewState("UserId")
            If v Is Nothing Then
                v = ToInteger(Request("UserId"))
                ViewState("UserId") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("UserId") = value
        End Set
    End Property
#End Region

    Protected Sub HR_UserDashboard_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If UserId > 0 Then
                Dim userdetails As DataLibrary.Users_GetDetailsByID_Result = _
                                    UserManager.GetDetailsByID(UserId)
                SetPageHeader(userdetails)
                SetEntityFooter(userdetails.CreatedBy, userdetails.DateCreated, userdetails.ModifiedBy, userdetails.DateModified)
                PopulateBasicInfo(userdetails)
                PopulateContactInfo(userdetails)
                PopulateNotes()
                PopulateDocuments()
                PopulateAuditLog()
            Else
                Navigate("/HR/ManageUsers.aspx")
            End If
        End If
    End Sub

    Private Sub SetPageHeader(obj As DataLibrary.Users_GetDetailsByID_Result)
        PageHeader = obj.Fullname & " - Dashboard"
    End Sub

#Region "Methods"
    Private Sub PopulateBasicInfo(obj As DataLibrary.Users_GetDetailsByID_Result)
        lnkBasicInfoHeader.NavigateUrl = "/HR/UserInfo.aspx?UserID=" & UserId

        ucPicture.ObjectID = MilesMetaObjects.Users
        ucPicture.KeyID = obj.UserID

        lnkFullName.Text = obj.Fullname
        lnkFullName.NavigateUrl = "/HR/UserInfo.aspx?UserID=" & UserId
        lblUserName.Text = obj.Username
        lblStatus.Text = obj.Status
        lblJobTitle.Text = If(String.IsNullOrEmpty(obj.JobTitle), "", obj.JobTitle)
        lblUserRoles.Text = If(String.IsNullOrEmpty(obj.UserRoles), "", obj.UserRoles)

        ConfigureActionsToolBar(obj.StatusID)
    End Sub

    Private Sub ConfigureActionsToolBar(statusid As MilesMetaTypeItem)

        rdActions.FindButtonByCommandName("UpdatePassword").NavigateUrl = "javascript: changePasswordDialog(" & UserId & ");"
        rdActions.FindButtonByCommandName("UpdatePicture").NavigateUrl = "javascript: changePictureDialog(" & UserId & ");"

        Dim btnActive As RadToolBarButton = rdActions.FindButtonByCommandName("Active")
        Dim btnInActive As RadToolBarButton = rdActions.FindButtonByCommandName("InActive")

        btnActive.Visible = True
        btnInActive.Visible = True

        btnActive.CommandArgument = MilesMetaTypeItem.UserStatusActive
        btnInActive.CommandArgument = MilesMetaTypeItem.UserStatusInactive

        Select Case statusid
            Case MilesMetaTypeItem.UserStatusActive
                btnActive.Visible = False
            Case MilesMetaTypeItem.UserStatusInactive
                btnInActive.Visible = False
        End Select
    End Sub

    Private Sub PopulateStatusLabel(statusid As Integer)
        lblStatus.Text = MetaTypeItemManager.GetById(statusid).ItemName
        'reconfigure the toolbar after populating status
        ConfigureActionsToolBar(statusid)
    End Sub

    Private Sub PopulateContactInfo(obj As DataLibrary.Users_GetDetailsByID_Result)
        lnkContactEmail.Text = obj.Email
        lnkContactEmail.NavigateUrl = "mailto:" & obj.Email
        lblContactPhone.Text = (<div><a href=<%= "tel:" & obj.WorkPhone %>><%= FormatPhone(PhoneNumberType.Work, obj.WorkPhone, obj.WorkPhoneExt) %></a></div>).ToString() &
                       (<div><a href=<%= "tel:" & obj.HomePhone %>><%= FormatPhone(PhoneNumberType.Home, obj.HomePhone) %></a></div>).ToString() &
                       (<div><a href=<%= "tel:" & obj.MobilePhone %>><%= FormatPhone(PhoneNumberType.Mobile, obj.MobilePhone) %></a></div>).ToString()

        lblAddress.Text = obj.FullAddress
    End Sub

    Private Sub PopulateNotes()
        With ucNotesDashboard
            .ObjectID = MilesMetaObjects.Users
            .KeyID = UserId
            .ShowCounter = True
            .HeaderNavigationURL = "/HR/UserNotes.aspx?UserID=" & UserId
            .InfoPage = "/CommonDialogs/DialogAddEditNotes.aspx"
            .Populate()
        End With
    End Sub

    Private Sub PopulateDocuments()
        With ucDocumentsDashboard
            .ObjectID = MilesMetaObjects.Users
            .KeyID = UserId
            .ShowCounter = True
            .HeaderNavigationURL = "/HR/UserDocuments.aspx?UserID=" & UserId
            .Populate()
        End With
    End Sub

    Private Sub PopulateAuditLog()
        With ucAuditLogDashboard
            .ObjectID = MilesMetaObjects.Users
            .KeyID = UserId
            .ShowCounter = True
            .Populate()
        End With
    End Sub
#End Region

    Protected Sub rdAjaxPanel_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxPanel.AjaxRequest
        'refresh the display picture
        ucPicture.LoadPicture()
    End Sub

    Protected Sub rdActions_ButtonClick(sender As Object, e As RadToolBarEventArgs) Handles rdActions.ButtonClick
        If TypeOf e.Item Is RadToolBarButton Then
            Dim button As RadToolBarButton = e.Item
            If button.Value = "Active" Or _
                button.Value = "Inactive" Then
                Try
                    'update the status
                    UserManager.UpdateStatus(UserId, button.CommandArgument)
                    'change status shown in status label and reconfigure toolbar
                    PopulateStatusLabel(button.CommandArgument)
                    PopulateAuditLog()
                    JGrowl.ShowMessage(JGrowlMessageType.Success, "User Status Changed")
                Catch wex As WarningException
                    JGrowl.ShowMessage(JGrowlMessageType.Error, wex.Message)

                Catch ex As Exception
                    Throw ex
                End Try
            End If
        End If
    End Sub

End Class
