
Partial Class UserControls_CMSMergeFieldsList
    Inherits System.Web.UI.UserControl

#Region "Properties"

    Public Property CategoryID As Integer
        Get
            Return Val(ViewState("CategoryID"))
        End Get
        Set(value As Integer)
            ViewState("CategoryID") = value
        End Set
    End Property

    Public Property Required As Boolean
        Get
            Return ViewState("Required")
        End Get
        Set(value As Boolean)
            ViewState("Required") = value
            requiredLabel.Visible = value
            cvMergeField.Enabled = value
        End Set

    End Property

    'selected list is saved in viewstate
    Public Property SelectedMergeFields As List(Of CMSMergeField)
        Get
            If ViewState("SelectedMergeFields") Is Nothing Then
                ViewState("SelectedMergeFields") = New List(Of CMSMergeField)
            End If
            Return ViewState("SelectedMergeFields")
        End Get
        Set(value As List(Of CMSMergeField))
            ViewState("SelectedMergeFields") = value
        End Set
    End Property

#End Region

#Region "Page Functions"

    Protected Sub UserControls_CMSMergeFieldsList_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ' Populate()
        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub BindGrid()
        dgSelectList.DataSource = Nothing
        dgSelectList.AutoBind = True
        dgSelectList.Rebind()
    End Sub

    ''' <summary>
    ''' Load and populate data
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Populate()

        'populate selected items
        BindGrid()
        'populate ddl excluding items selected
        Dim list = CMSMergeFieldManager.GetList().Where(Function(x) _
                        Not (From mf In SelectedMergeFields).Where(Function(y) y.MergeFieldID = x.MergeFieldID).Any)

        ControlBinding.BindListControl(ddlList, list, "MergeField", "MergeFieldID", True)

    End Sub

    ''' <summary>
    ''' Save all selected merge fields at once
    ''' this function is used when saving merge fields for new categories (after retreiving categoryId from save function )
    ''' assign categoryId to this control before using this
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SaveMergeFields()
        If CategoryID > 0 Then
            CMSCategoryManager.SaveMergeFields(CategoryID, SelectedMergeFields)
        End If
    End Sub

#End Region

#Region "Events"

    Protected Sub dgSelectList_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles dgSelectList.ItemCommand
        If e.CommandName = "Remove" Then
            Dim mergeFieldId = DirectCast(e.Item, GridDataItem).GetDataKeyValue("MergeFieldID")
            If CategoryID > 0 Then
                'selected item is removed instantly if existing category record is being modified
                CMSCategoryManager.RemoveMergeField(CategoryID, mergeFieldId)
                'notify user that the info was saved
                JGrowl.ShowMessage(JGrowlMessageType.Notification, "Merge Field removed successfully")
            End If
            'remove from viewstate
            SelectedMergeFields.RemoveAt(SelectedMergeFields.FindIndex(Function(x) x.MergeFieldID = mergeFieldId))
            Populate()
            ddlList.Focus()
        End If
    End Sub

    Protected Sub dgSelectList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles dgSelectList.NeedDataSource

        'getlist from database for the first time and populate the viewstate
        If CategoryID > 0 And SelectedMergeFields.Count < 1 Then
            Dim objlist As List(Of CMSMergeField) = CMSCategoryManager.GetMergeField(CategoryID).ToList()
            objlist.ForEach(Sub(x) SelectedMergeFields.Add(New CMSMergeField With {.MergeFieldID = x.MergeFieldID, .MergeField = x.MergeField}))
        End If

        If SelectedMergeFields.Count = 0 Then dgSelectList.MasterTableView.NoMasterRecordsText = "<I>Select a Merge Field<I>"

        'assign datasource to viewstate list
        dgSelectList.DataSource = SelectedMergeFields

        'if only one item exists in the list and required is true, hide the remove column
        If Required AndAlso SelectedMergeFields IsNot Nothing AndAlso SelectedMergeFields.Count = 1 Then
            dgSelectList.MasterTableView.Columns().FindByUniqueName("RemoveCol").Display = False
        Else 'otherwise show the column
            dgSelectList.MasterTableView.Columns().FindByUniqueName("RemoveCol").Display = True
        End If
    End Sub

    Protected Sub ddlList_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlList.SelectedIndexChanged
        If CategoryID > 0 AndAlso ddlList.SelectedValue > 0 Then
            'selected item is saved instantly if existing category record is being modified
            CMSCategoryManager.SaveMergeField(CategoryID, ddlList.SelectedValue)

            'notify user that the info was saved
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Merge Field")
        End If
        'add to viewstate
        SelectedMergeFields.Add(New CMSMergeField With {.MergeFieldID = ddlList.SelectedValue.ToInteger(), .MergeField = ddlList.SelectedItem.Text})
        Populate()
        ddlList.Focus()
    End Sub

    Protected Sub rdAjaxPanel_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxPanel.AjaxRequest
        Populate()
    End Sub

#End Region

End Class
