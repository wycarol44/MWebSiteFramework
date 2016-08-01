
Partial Class Meta_DialogMenuInfo
    Inherits BasePage
#Region "Properties"
    ''' <summary>
    ''' Gets or set the record id
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Property MenuID As Integer
        Get
            Dim v As Object = ViewState("MenuID")
            If v Is Nothing Then
                v = ToInteger(Request("MenuID"))
                ViewState("MenuID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("MenuID") = value
        End Set
    End Property

    Protected Property ParentID As Integer
        Get
            Dim v As Object = ViewState("ParentID")
            If v Is Nothing Then
                v = ToInteger(Request("ParentID"))
                ViewState("ParentID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("ParentID") = value
        End Set
    End Property
#End Region

    Protected Sub Meta_DialogMenuInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PopulateChoices()
            Populate()
        End If
    End Sub

#Region "Methods"

    Private Sub PopulateChoices()
        Dim formsList = MetaFormManager.GetList()

        ControlBinding.BindListControl(ddlForm, formsList, "FormName", "FormID", True)
    End Sub

    Private Sub Populate()

        Dim m = MetaMenuManager.GetByID(MenuID)

        txtTitle.Text = m.Title
        ddlForm.SelectedValue = m.FormID.GetValueOrDefault()
        txtNavigateUrl.Text = m.NavigateUrl
        chkIsInternalOnly.Checked = m.IsInternalOnly

        'display form input items
        pnlEdit.Visible = True
      
        txtTitle.Focus()
    End Sub

    Private Function Save(Optional ByVal close As Boolean = False) As Boolean
        Dim m = MetaMenuManager.GetByID(MenuID)

        m.Title = txtTitle.Text
        m.FormID = ToNullableInteger(ddlForm.SelectedValue)
        If MenuID = 0 And ParentID > 0 Then 'new record and has parentid
            m.ParentID = ParentID
        End If
        m.NavigateUrl = txtNavigateUrl.Text
        m.IsInternalOnly = chkIsInternalOnly.Checked

        MenuID = MetaMenuManager.Save(m)

        'clear cache
        MetaMenuManager.ClearMenuCache()

        Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Manu Item", useParent:=close And Not delay, isDelayed:=close And delay)

        Return True
    End Function

 
#End Region

#Region "Events"

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            CloseDialogWindow(MenuID)
        End If

    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub

#End Region

End Class
