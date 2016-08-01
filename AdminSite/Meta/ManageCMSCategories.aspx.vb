
Partial Class Meta_ManageCMSCategories
    Inherits BasePage

#Region "Page Functions"

    Protected Sub Meta_ManageCMSCategories_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.SetFocus(txtCategoryName)

            PopulateChoices()
        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub BindList()
        clCMSCategories.DataSource = Nothing
        clCMSCategories.AutoBind = True
        clCMSCategories.Rebind()
    End Sub

    Private Sub PopulateChoices()
        Dim mergeFieldsList = CMSMergeFieldManager.GetList()

        ControlBinding.BindListControl(ddlMergeFields, mergeFieldsList, "MergeField", "MergeFieldID", False)
    End Sub

#End Region

#Region "ListView"

    Protected Sub clCMSCategories_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clCMSCategories.NeedDataSource
        Dim cmsCategoryList = CMSCategoryManager.GetListByMergeFields(
                               categoryName:=txtCategoryName.Text,
                               contenttypeids:=ddlContentType.ToXMLIdentifiers(),
                               mergeFieldIds:=ddlMergeFields.ToXMLIdentifiers(),
                               archived:=ToNegBool(chkArchived.Checked)
       )
        clCMSCategories.DataSource = cmsCategoryList
    End Sub

    Protected Sub clCMSCategories_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clCMSCategories.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem

        Dim keyId As Integer = item.GetDataKeyValue("CategoryID")

        'archive the user
        CMSCategoryManager.ToggleArchived(keyId)

        clCMSCategories.Rebind()
    End Sub

#End Region

#Region "Events"

    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindList()
    End Sub

    Protected Sub rdAjaxManager_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxManager.AjaxRequest
        BindList()
    End Sub

#End Region

   
End Class
