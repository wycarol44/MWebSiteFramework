
Partial Class Admin_DialogContentInfo
    Inherits BasePage

#Region "Properties"
    Protected Property CategoryID As Integer
        Get
            Dim v As Object = ViewState("CategoryID")
            If v Is Nothing Then
                v = ToInteger(Request("CategoryID"))
                ViewState("CategoryID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("CategoryID") = value
        End Set
    End Property
#End Region

    Protected Sub Admin_DialogContentInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtBody.SetEditorPaths()
            Populate()

        End If
    End Sub

#Region "Methods"
    Private Sub Populate()
        Dim cms As CMSCategory = CMSCategoryManager.GetByID(CategoryID)

        txtSubject.Text = cms.ContentSubject
        txtBody.Content = cms.ContentBody

        If cms.ContentTypeID = MilesMetaTypeItem.CMSContentTypeEmail Then
            If cms.EmailFromTypeID = MilesMetaTypeItem.CMSEmailFromManual Then
                pnlEmailFromManual.Visible = True
                txtEmailFromName.Text = cms.EmailFromName
                txtEmail.Text = cms.EmailFromEmail
            ElseIf cms.EmailFromTypeID = MilesMetaTypeItem.CMSEmailFromSelectUser Then
                pnlEmailFromUser.Visible = True
                ucUserCombo.SelectedValue = ToInteger(cms.EmailFromID.ToString())
            End If
            txtBody.ToolsFile = "~/App_Data/EditorToolFiles/EmailToolsFile.xml"

            'since this is an email - make the urls absolute
            txtBody.EnableFilter(EditorFilters.MakeUrlsAbsolute)
        ElseIf cms.ContentTypeID = MilesMetaTypeItem.CMSContentTypeWebsite Then
            pnlSubject.Visible = False

        ElseIf cms.ContentTypeID = MilesMetaTypeItem.CMSContentTypeHyperlink Then
            txtHyperlinkContent.Text = cms.ContentBody
            pnlEmailWebsite.Visible = False
            pnlHyperLink.Visible = True
        End If
        If pnlEmailWebsite.Visible Then CreateCMSFields()
    End Sub

    Private Sub CreateCMSFields()

        Dim mList As CMSMergeField() = CMSCategoryManager.GetMergeFields(CategoryID)
        If mList.Count > 0 Then
            'add a new Toolbar dynamically
            Dim dynamicToolbar As New EditorToolGroup()
            txtBody.Tools.Add(dynamicToolbar)

            'add a custom dropdown and set its items and dimension attributes
            Dim ddn As New EditorDropDown("MergeFields")
            ddn.Text = "Merge Fields"

            'Set the popup width and height
            ddn.Attributes("width") = "110px"
            ddn.Attributes("popupwidth") = "240px"
            ddn.Attributes("popupheight") = "100px"

            'Add items
            For Each li As CMSMergeField In mList
                ddn.Items.Add(li.MergeField, li.MergeField)
            Next

            'Add tool to toolbar
            dynamicToolbar.Tools.Add(ddn)
        End If
    End Sub

    Private Function Save(Optional close As Boolean = False) As Boolean

        Dim cms = CMSCategoryManager.GetByID(CategoryID)

        If cms.ContentTypeID = MilesMetaTypeItem.CMSContentTypeHyperlink Then
            cms.ContentBody = txtHyperlinkContent.Text
        Else
            cms.ContentBody = txtBody.Content
        End If
        If cms.ContentTypeID = MilesMetaTypeItem.CMSContentTypeEmail Then
            cms.ContentSubject = txtSubject.Text
            If cms.EmailFromTypeID = MilesMetaTypeItem.CMSEmailFromManual Then
                cms.EmailFromName = txtEmailFromName.Text
                cms.EmailFromEmail = txtEmail.Text
            ElseIf cms.EmailFromTypeID = MilesMetaTypeItem.CMSEmailFromSelectUser Then
                cms.EmailFromID = ucUserCombo.SelectedValue.ToInteger()
            End If
        End If


        CMSCategoryManager.Save(cms)

        Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Content", useParent:=close And Not delay, isDelayed:=close And delay)

        Return True
    End Function
#End Region

#Region "Events"
    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            CloseDialogWindow(CategoryID)
        End If
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub
#End Region

End Class
