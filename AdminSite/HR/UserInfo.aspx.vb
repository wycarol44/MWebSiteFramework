Imports System.IO

Partial Class HR_UserInfo
    Inherits BasePage



    Protected Sub HR_UserInfo_Load(sender As Object, e As System.EventArgs) Handles Me.Load
      
        If Not IsPostBack Then
            'determine header visibility
            SetHeader()

            'test
            PopulateUserRoles()

            If UserId > 0 Then
                'load record if id > 0
                Populate()

            Else
                ShowStatusDropdown()
                Page.SetFocus(txtFirstName)
                PopulateJobTitles(0)

            End If
        End If
    End Sub


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


#Region "Methods"

    ''' <summary>
    ''' Sets the header visibility based on key ids
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHeader()

        pnlHeader.Visible = (UserId > 0)

    End Sub


    Private Sub ShowStatusDropdown()
        ddlStatus.Visible = True
        ddlStatus.SelectedValue = MilesMetaTypeItem.UserStatusActive
        lblStatusRequired.Visible = True
        lblStatus.Visible = False
    End Sub

#Region "Populate Choices"

    Private Sub PopulateUserRoles()
        Dim obj = UserRoleManager.GetList()


        milesUserRole.UserID = UserId
        milesUserRole.Populate()
    End Sub


    Private Sub PopulateJobTitles(jobTitleId As Integer?)

        'gets a list of records, filtered by id and archived status. Will return non-archived OR the specific id
        Dim jobTitlesList = UserJobTitleManager.GetIdFilteredList(jobTitleId)

        'bind controls
        ControlBinding.BindListControl(ddlJobTitle, jobTitlesList, "JobTitle", "JobTitleID", True)

    End Sub

    Private Sub PopulateAuditLog()
        Dim alCount = AuditLogManager.GetCount(MilesMetaObjects.Users, UserId)
        Dim btnViewAuditLog As RadToolBarButton = rdTools.FindButtonByCommandName("ViewAuditLog")
        Dim alCounter = <span class="badge"><%= alCount %></span>

        btnViewAuditLog.Text = "Audit Log " & alCounter.ToString()
    End Sub

#End Region

    ''' <summary>
    ''' Populates fields from the record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Populate()

        Dim obj = UserManager.GetById(UserId)

        'populate controls based on object
        PopulateJobTitles(obj.JobTitleID)

        'set page title
        PageHeader = obj.Fullname + " - Basic Info"

        'populate fields
        txtFirstName.Text = obj.Firstname
        txtLastName.Text = obj.Lastname
        txtUsername.Text = obj.Username
        lblStatusRequired.Visible = False
        ddlStatus.Visible = False
        lblStatus.Visible = True
        PopulateStatusLabel(obj.StatusID)

        ddlJobTitle.SelectedValue = obj.JobTitleID.GetValueOrDefault()
        txtNotes.Content = obj.Notes
        txtEmail.Text = obj.Email
        txtHomePhone.Text = obj.HomePhone
        txtMobilePhone.Text = obj.MobilePhone
        txtWorkPhone.Text = obj.WorkPhone
        txtWorkPhone.Extension = obj.WorkPhoneExt

        milesAddr.AddressID = obj.AddressID.GetValueOrDefault()

        'set picture
        milesPicUpload.ObjectID = MilesMetaObjects.Users
        milesPicUpload.KeyID = obj.UserID

        'update toolbar with ids
        UpdateToolBar()

        'populate audit log
        PopulateAuditLog()

        'set entity footer
        SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)

        'usercombo.SelectedValue = UserId
    End Sub

    ''' <summary>
    ''' Saves or updates the record
    ''' </summary>
    ''' <param name="close"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Save(Optional close As Boolean = False) As Boolean

        Try

            Dim newUser As Boolean = (UserId = 0)

            Dim obj = UserManager.GetById(UserId)

            Dim OldJobTitleID As Integer = 0

            If Not newUser Then
                OldJobTitleID = If(obj.JobTitleID IsNot Nothing, obj.JobTitleID, 0)
            End If

            obj.Firstname = txtFirstName.Text
            obj.Lastname = txtLastName.Text
            obj.Username = txtUsername.Text
            'save statusid for new user only, for old users, status is changed through action menu
            If newUser Then obj.StatusID = ddlStatus.StatusID

            obj.JobTitleID = ToNullableInteger(ddlJobTitle.SelectedValue)
            obj.Notes = txtNotes.Content
            obj.NotesText = txtNotes.Text
            obj.Email = txtEmail.Text
            obj.HomePhone = txtHomePhone.Text
            obj.MobilePhone = txtMobilePhone.Text
            obj.WorkPhone = txtWorkPhone.Text
            obj.WorkPhoneExt = txtWorkPhone.Extension


            'do some validation

            If UserManager.IsUsernameDuplicate(obj) Then
                Throw New WarningException("Please enter a unique username")
            End If

            If UserManager.IsEmailDuplicate(obj) Then
                Throw New WarningException("Please enter a unique email address")
            End If

            'save address
            obj.AddressID = milesAddr.Save()

            'save user
            UserId = UserManager.Save(obj)

            'if saving a new user, save the userroles too
            If newUser Then
                milesUserRole.UserID = UserId
                milesUserRole.SaveRoles()

                'save the picture
                milesPicUpload.ObjectID = MilesMetaObjects.Users
                milesPicUpload.KeyID = UserId
                milesPicUpload.Save()

            End If

            'add params to tab
            rdTabs.AddQueryParameter("UserID", UserId)

            'update toolbar to set new ids
            UpdateToolBar()

            'Save Status Change 
            If newUser Then
                SaveAuditLogStatusChange(obj.StatusID, 0)
            End If

            'Save JobTitle Change 

            If OldJobTitleID <> obj.JobTitleID Then
                SaveAuditLogJobTitleChange(obj.JobTitleID, OldJobTitleID)
            End If

            'show the header now
            SetHeader()

            'set entity footer
            SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)

            'success
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="User", isDelayed:=close)

            Return True

        Catch wex As WarningException

            'show a friendly message
            JGrowl.ShowMessage(JGrowlMessageType.Error, wex.Message)

            'return false
            Return False

        Catch ex As Exception
            'fail
            Throw

        End Try
    End Function

    Private Sub PopulateStatusLabel(statusid As Integer)
        lblStatus.Text = MetaTypeItemManager.GetById(statusid).ItemName
        'reconfigure the toolbar after populating status
        ConfigureActionsToolbar(statusid)
    End Sub

    Private Sub SaveAuditLogStatusChange(NewStatusID As Integer, Optional OldStatusID As Integer = 0)
        Dim NewStatus As String = MetaTypeItemManager.GetById(NewStatusID).ItemName
        'only required when creating new user, status change for existing user is handled in UpdateStatus function in UserManager
        If OldStatusID = 0 Then
            'Save Audit Log
            AuditLogManager.SaveAuditLog(AuditLogType.Insert, MilesMetaObjects.Users, UserId, NewStatusID, MilesMetaAuditLogAttributes.Status, NewStatus)
        End If
    End Sub

    Private Sub SaveAuditLogJobTitleChange(NewJobTitleID As Integer, Optional OldJobTitleID As Integer = 0)

        Dim NewJobTitle As String = UserJobTitleManager.GetById(NewJobTitleID).JobTitle
        Dim OldJobTitle As String = ""

        If OldJobTitleID <> 0 Then

            OldJobTitle = UserJobTitleManager.GetById(OldJobTitleID).JobTitle
            'Save Audit Log
            AuditLogManager.SaveAuditLog(AuditLogType.Update, MilesMetaObjects.Users, UserId, NewJobTitleID, MilesMetaAuditLogAttributes.JobTitle, NewJobTitle, OldJobTitle)

        Else
            'Save Audit Log
            AuditLogManager.SaveAuditLog(AuditLogType.Insert, MilesMetaObjects.Users, UserId, NewJobTitleID, MilesMetaAuditLogAttributes.JobTitle, NewJobTitle)
        End If

    End Sub

    ''' <summary>
    ''' Updates toolbar items with ids
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UpdateToolBar()
        Dim rtbChangePass As RadToolBarButton = rdTools.FindButtonByCommandName("ChangePassword")
        If rtbChangePass IsNot Nothing Then
            rtbChangePass.CommandArgument = UserId
        End If
    End Sub

    Private Sub ConfigureActionsToolBar(statusid As Integer)
        Dim btnActive As RadToolBarButton = rdTools.FindButtonByCommandName("Active")
        Dim btnInActive As RadToolBarButton = rdTools.FindButtonByCommandName("InActive")

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

    Public Sub CloseForm()
        If ReferrerPageURL IsNot Nothing Then
            Navigate(ReferrerPageURL.ToString())
        Else
            Navigate("~/HR/ManageUsers.aspx")
        End If
    End Sub

#End Region

#Region "Events"
    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            CloseForm()
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Save()
        PopulateAuditLog()
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseForm()
    End Sub

    Protected Sub rdTools_ButtonClick(sender As Object, e As RadToolBarEventArgs) Handles rdTools.ButtonClick
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
#End Region


End Class
