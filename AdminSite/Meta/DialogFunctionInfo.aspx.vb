
Partial Class Meta_DialogFunctionInfo
    Inherits BasePage

#Region "Properties"

    Protected Property FunctionID As Integer
        Get
            Dim v As Object = ViewState("FunctionID")
            If v Is Nothing Then
                v = ToInteger(Request("FunctionID"))
                ViewState("FunctionID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("FunctionID") = value
        End Set
    End Property

    'selected list is saved in viewstate
    Property _selectedForm As New List(Of MetaForm)
    Public Property SelectedForm As List(Of MetaForm)
        Get
            If ViewState("SelectedForm") Is Nothing Then
                ViewState("SelectedForm") = _selectedForm
            End If
            Return ViewState("SelectedForm")
        End Get
        Set(value As List(Of MetaForm))
            ViewState("SelectedForm") = value
        End Set
    End Property
#End Region

    Protected Sub Meta_DialogFunctionInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If FunctionID > 0 Then
                Populate()
            Else
                Page.SetFocus(txtFunctionName)
                PopulateDDls()
                dgSelectList.MasterTableView.NoMasterRecordsText = "No Forms Assigned"
            End If
        End If
    End Sub

#Region "Methods"
    Private Sub PopulateDDls()
        ddlModule.DataTextField = "ModuleName"
        ddlModule.DataValueField = "ModuleID"
        ddlModule.DataFieldID = "ModuleID"
        ddlModule.DataFieldParentID = "ParentID"

        Dim objlist() = MetaModuleManager.GetList()
        ddlModule.DataSource = objlist
        ddlModule.DataBind()
        ddlModule.ExpandAllDropDownNodes()

        PopulateRelatedForm()
    End Sub

    Private Sub PopulateRelatedForm()
        'populate items which are not already assigned
        Dim formList = MetaFormManager.GetList().Where(Function(x) _
                                        Not (From ur In SelectedForm).Where(Function(y) _
                                                                                 y.FormID = x.FormID).Any)
        ControlBinding.BindListControl(ddlRelatedForms, formList, "FormName", "FormID", True)
    End Sub

    Private Sub Populate()
        Dim obj = MetaFunctionManager.GetById(FunctionID)
        txtFunctionName.Text = obj.FunctionName
        'load assigned forms in viewstate
        SelectedForm = obj.MetaForms.ToList()
        BindGrid()
        PopulateDDls()

        ddlModule.SelectedValue = obj.ModuleID
        chkViewOnly.Checked = obj.ViewOnly
        SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)
    End Sub

    Private Sub BindGrid()
        dgSelectList.DataSource = Nothing
        dgSelectList.AutoBind = True
        dgSelectList.Rebind()
    End Sub

    Private Function Save(Optional close As Boolean = False) As Boolean

        Dim obj = MetaFunctionManager.GetById(FunctionID)
        Dim newFunction As Boolean = (FunctionID = 0)

        obj.FunctionName = txtFunctionName.Text
        obj.ModuleID = ddlModule.SelectedValue
        obj.ViewOnly = chkViewOnly.Checked

        FunctionID = MetaFunctionManager.Save(obj)

        'save related forms if new record
        MetaFunctionManager.SaveForms(FunctionID, SelectedForm)

        SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)

        Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Function", useParent:=close And Not delay, isDelayed:=close And delay)

        Return True
    End Function
#End Region

#Region "Events"

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            CloseDialogWindow(FunctionID)
        End If

    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub

#End Region

    Protected Sub dgSelectList_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles dgSelectList.ItemCommand
        If e.CommandName = "Remove" Then
            Dim formid = DirectCast(e.Item, GridDataItem).GetDataKeyValue("FormID")
            If FunctionID > 0 Then
                'selected item is removed instantly if existing user record is being modified
                MetaFunctionManager.RemoveForm(FunctionID, formid)
                'notify user that the info was removed
                JGrowl.ShowMessage(JGrowlMessageType.Notification, "Form removed successfully")
            End If
            'remove from viewstate
            SelectedForm.RemoveAt(SelectedForm.FindIndex(Function(x) x.FormID = formid))
            PopulateRelatedForm()
            BindGrid()
        End If
    End Sub

    Protected Sub dgSelectList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles dgSelectList.NeedDataSource
        dgSelectList.DataSource = SelectedForm
        If SelectedForm.Count = 0 Then
            dgSelectList.MasterTableView.NoMasterRecordsText = "No Forms Assigned"
        End If
    End Sub

    Protected Sub ddlRelatedForms_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlRelatedForms.SelectedIndexChanged
        If FunctionID > 0 AndAlso ddlRelatedForms.SelectedValue > 0 Then
            'selected item is saved instantly if existing user record is being modified
            MetaFunctionManager.SaveForm(FunctionID, ddlRelatedForms.SelectedValue)

            'notify user that the info was removed
            JGrowl.ShowMessage(JGrowlMessageType.Success, "Form added successfully")
        End If

        'add to viewstate
        SelectedForm.Add(New MetaForm With {.FormID = ddlRelatedForms.SelectedValue, .FormName = ddlRelatedForms.SelectedItem.Text})
        PopulateRelatedForm()
        BindGrid()
    End Sub
End Class
