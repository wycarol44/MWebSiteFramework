
Partial Class HR_ManageUsers
    Inherits BasePage

    Protected Sub HR_ManageUsers_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.SetFocus(txtFullName)
            PopulateChoices()
        End If
    End Sub


#Region "Methods"
    Private Sub BindGrid()
        clUsers.DataSource = Nothing
        clUsers.AutoBind = True
        clUsers.Rebind()

    End Sub

    Private Sub PopulateChoices()
        Dim jobTitlesList = UserJobTitleManager.GetList()

        ControlBinding.BindListControl(ddlJobTitles, jobTitlesList, "JobTitle", "JobTitleID", False)
    End Sub

#End Region

#Region "Grid"
    Protected Sub clUsers_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clUsers.ItemCommand
        If e.CommandName = "InitInsert" Then
            Navigate("~/HR/UserInfo.aspx?UserID=0")
        End If
    End Sub

    Protected Sub clUsers_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clUsers.NeedDataSource
        Dim userList = UserManager.GetList(
            fullname:=txtFullName.Text,
            phone:=txtPhone.Text,
            email:=txtEmail.Text,
            jobTitleIds:=ddlJobTitles.ToXMLIdentifiers(),
            statusIds:=ddlStatusTypes.ToXMLIdentifiers(),
            archived:=ToNegBool(chkArchived.Checked)
        )

        clUsers.DataSource = userList

    End Sub

  

    Protected Sub clUsers_ToggleArchived(sender As Object, e As RadListViewCommandEventArgs) Handles clUsers.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem

        Dim keyId As Integer = item.GetDataKeyValue("UserID")

        'archive the user
        UserManager.ToggleArchived(keyId)

        clUsers.Rebind()

    End Sub
#End Region

#Region "Events"
    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindGrid()
    End Sub
#End Region
    
    
End Class
