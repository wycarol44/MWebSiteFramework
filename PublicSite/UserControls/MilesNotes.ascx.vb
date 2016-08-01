

Partial Class UserControls_MilesNotes
    Inherits System.Web.UI.UserControl

#Region "Properties"

    Public Property ObjectID As MilesMetaObjects
        Get
            Return ViewState("ObjectID")
        End Get
        Set(value As MilesMetaObjects)
            ViewState("ObjectID") = CInt(value)
        End Set
    End Property

    Public Property KeyID As Integer
        Get
            Return ViewState("KeyID")
        End Get
        Set(value As Integer)
            ViewState("KeyID") = value
        End Set
    End Property

#End Region

#Region "Page Functions"

    Protected Sub UserControls_MilesNotes_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Page.SetFocus(txtNotes)
        End If
    End Sub

#End Region

#Region "Events"

    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        clNotes.Rebind()
    End Sub

    Protected Sub rdAjaxPanel_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxPanel.AjaxRequest
        clNotes.Rebind()
    End Sub

#End Region

#Region "Notes Card List"


    Protected Sub clNotes_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clNotes.ItemDataBound
        If TypeOf e.Item Is RadListViewDataItem Then



            Dim hdNoteID As HiddenField = e.Item.FindControl("hdNoteID")
            Dim hdNoteType As HiddenField = e.Item.FindControl("hdNoteType")
            Dim lnkHeaderLink As HyperLink = e.Item.FindControl("HeaderLink")
            Dim hdLinkURL As HiddenField = e.Item.FindControl("hdLinkURL")
            Dim ibEdit As ImageButton = e.Item.FindControl("ibEdit")
            Dim lblNotes As Label = e.Item.FindControl("lblNotes")
            Dim lnkViewNotes As LinkButton = e.Item.FindControl("lnkViewNotes")

            If ToInteger(hdNoteType.Value) = MilesMetaTypeItem.NotesTypeNote Then
                lnkHeaderLink.NavigateUrl = String.Format("javascript:openEditDialog('{0}');", ToInteger(hdNoteID.Value))
            Else
                lnkHeaderLink.Target = "_blank"
                lnkHeaderLink.NavigateUrl = hdLinkURL.Value
            End If


            If Regex.Replace(lblNotes.Text, "<.*?>", "").Length > 300 Then
                lblNotes.Text = Left(Regex.Replace(lblNotes.Text, "<.*?>", ""), 300)
                lnkViewNotes.Visible = True
                lnkViewNotes.Attributes.Add("onclick", String.Format("openEditDialog('{0}');", ToInteger(hdNoteID.Value)))
            Else
                lnkViewNotes.Visible = False
            End If

            ibEdit.Attributes.Add("onclick", String.Format("openEditDialog('{0}');", ToInteger(hdNoteID.Value)))

        End If
    End Sub


    Protected Sub clNotes_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clNotes.NeedDataSource

        clNotes.DataSource = NoteManager.GetList(ObjectID, KeyID, ddlTypes.ToXMLIdentifiers(), txtNotes.Text, ddlCreatedBy.UserID, dtDateFrom.SelectedDate, dtDateTo.SelectedDate, ToNegBool(chkArchived.Checked))

    End Sub

    Protected Sub clNotes_ToggleArchive(sender As Object, e As RadListViewCommandEventArgs) Handles clNotes.ToggleArchive
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("NoteID")

        NoteManager.ToggleArchived(keyId)

        clNotes.Rebind()

    End Sub

#End Region

End Class
