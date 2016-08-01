
Partial Class Meta_DialogCMSMergeFieldInfo
    Inherits BasePage

#Region "Properties"

    Public Property MergeFieldID As Integer
        Get
            If ViewState("MergeFieldID") Is Nothing Then
                ViewState("MergeFieldID") = Val(Request("MergeFieldID"))
            End If

            Return Val(ViewState("MergeFieldID"))
        End Get
        Set(value As Integer)
            ViewState("MergeFieldID") = value
        End Set
    End Property

#End Region

#Region "Page Functions"

    Protected Sub Meta_DialogCMSMergeFieldInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Page.SetFocus(txtMergeField)

            If MergeFieldID > 0 Then
                Populate()
            End If
        End If
    End Sub

#End Region

#Region "Methods"

    Private Sub Populate()

        Dim obj = CMSMergeFieldManager.GetByID(MergeFieldID)

        'populate fields
        txtMergeField.Text = obj.MergeField

        'set entity footer
        'SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)

    End Sub

    ''' <summary>
    ''' Saves or updates the record
    ''' </summary>
    ''' <param name="close"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Save(Optional close As Boolean = False) As Boolean

        Try

            Dim obj = CMSMergeFieldManager.GetByID(MergeFieldID)

            obj.MergeField = txtMergeField.Text

            'save cms category
            MergeFieldID = CMSMergeFieldManager.Save(obj)

            'set entity footer
            'SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)

            Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
            'show success
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="CMS Merge Field", useParent:=close And Not delay, isDelayed:=close And delay)

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
            CloseDialogWindow(MergeFieldID)
        End If
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub

    Protected Sub rdAjaxPanel_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxPanel.AjaxRequest

    End Sub

#End Region

   
End Class
