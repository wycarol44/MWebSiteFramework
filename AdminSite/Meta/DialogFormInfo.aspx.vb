
Partial Class Meta_DialogFormInfo
    Inherits BasePage
    Protected Property MetaFormID As Integer
        Get
            Dim v As Object = ViewState("MetaFormID")
            If v Is Nothing Then
                v = ToInteger(Request("MetaFormID"))
                ViewState("MetaFormID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("MetaFormID") = value
        End Set
    End Property

    Protected Sub Meta_DialogFormInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If MetaFormID > 0 Then
                Populate()
            End If
        End If
    End Sub

#Region "Methods"
    Private Sub Populate()
        PopulateDDL()
        Dim f As MetaForm = MetaFormManager.GetById(MetaFormID)
        txtFormName.Text = f.FormName
        txtFormPath.Text = f.FormPath
        ddlModule.SelectedValue = f.ModuleID.ToString().ToInteger()
        chkCanBeFavorite.Checked = f.CanBeFavorite
    End Sub

    Private Sub PopulateDDL()
        ddlModule.DataTextField = "ModuleName"
        ddlModule.DataValueField = "ModuleID"
        ddlModule.DataFieldID = "ModuleID"
        ddlModule.DataFieldParentID = "ParentID"

        Dim objlist() = MetaModuleManager.GetList()
        ddlModule.DataSource = objlist
        ddlModule.DataBind()
        ddlModule.ExpandAllDropDownNodes()
    End Sub
    Private Function Save(Optional close As Boolean = False) As Boolean

        Dim obj = MetaFormManager.GetById(MetaFormID)

        obj.ModuleID = ToNullableInteger(ddlModule.SelectedValue)
        obj.CanBeFavorite = chkCanBeFavorite.Checked

        MetaFormID = MetaFormManager.Save(obj)

        Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Form", useParent:=close And Not delay, isDelayed:=close And delay)

        Return True
    End Function
#End Region

#Region "Events"

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            CloseDialogWindow(MetaFormID)
        End If

    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub

#End Region
End Class
