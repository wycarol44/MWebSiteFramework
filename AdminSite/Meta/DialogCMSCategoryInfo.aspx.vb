
Partial Class Meta_DialogCMSCategoryInfo
    Inherits BasePage

#Region "Properties"

    Public Property CategoryID As Integer
        Get
            If ViewState("CategoryID") Is Nothing Then
                ViewState("CategoryID") = Val(Request("CategoryID"))
            End If

            Return Val(ViewState("CategoryID"))
        End Get
        Set(value As Integer)
            ViewState("CategoryID") = value
        End Set
    End Property

#End Region

#Region "Page Functions"

    Protected Sub Meta_DialogCMSCategoryInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            PopulateMergeFields()

            Page.SetFocus(txtCategoryName)

            If CategoryID > 0 Then
                Populate()
            End If
        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub PopulateMergeFields()
        Dim obj = CMSMergeFieldManager.GetList()

        milesCMSMergeFields.CategoryID = CategoryID
        milesCMSMergeFields.Populate()
    End Sub

    Private Sub Populate()

        Dim obj = CMSCategoryManager.GetByID(CategoryID)

        'populate fields
        txtCategoryName.Text = obj.CategoryName
        ddlContentType.SelectedValue = obj.ContentTypeID.GetValueOrDefault()

        If obj.ContentTypeID.GetValueOrDefault = MilesMetaTypeItem.CMSContentTypeEmail Then
            pnlEmailFromType.Visible = True
            ddlEmailFromType.SelectedValue = obj.EmailFromTypeID.GetValueOrDefault
        Else
            pnlEmailFromType.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' Saves or updates the record
    ''' </summary>
    ''' <param name="close"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Save(Optional close As Boolean = False) As Boolean

        Try

            Dim newCategory As Boolean = (CategoryID = 0)

            Dim obj = CMSCategoryManager.GetByID(CategoryID)

            obj.CategoryName = txtCategoryName.Text
            obj.ContentTypeID = ddlContentType.SelectedValue
            If ddlContentType.SelectedValue = MilesMetaTypeItem.CMSContentTypeEmail Then
                obj.EmailFromTypeID = ddlEmailFromType.SelectedValue
            End If


            'save cms category
            CategoryID = CMSCategoryManager.Save(obj)

            'if saving a new user, save the userroles too
            If newCategory Then
                milesCMSMergeFields.CategoryID = CategoryID
                milesCMSMergeFields.SaveMergeFields()
            End If

            Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
            'show success
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="CMS Category", useParent:=close And Not delay, isDelayed:=close And delay)

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

    Protected Sub ddlContentType_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlContentType.SelectedIndexChanged
        If ddlContentType.SelectedValue = MilesMetaTypeItem.CMSContentTypeEmail Then
            pnlEmailFromType.Visible = True
        Else
            pnlEmailFromType.Visible = False
        End If
    End Sub

#End Region


   
End Class
