Imports System.Xml

Partial Class CommonDialogs_DialogAddEditNotes
    Inherits BasePage

#Region "Properties"

    Public Property NoteID As Integer
        Get
            Dim v As Object = ViewState("NoteID")
            If v Is Nothing Then
                v = ToInteger(Request("NoteID"))
                ViewState("NoteID") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("NoteID") = value
        End Set
    End Property

    Public Property ObjectID As Integer
        Get
            Dim v As Object = ViewState("ObjectID")
            If v Is Nothing Then
                v = ToInteger(Request("ObjectID"))
                ViewState("ObjectID") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("ObjectID") = value
        End Set
    End Property

    Public Property KeyID As Integer
        Get
            Dim v As Object = ViewState("KeyID")
            If v Is Nothing Then
                v = ToInteger(Request("KeyID"))
                ViewState("KeyID") = v
            End If

            Return v
        End Get
        Set(value As Integer)
            ViewState("KeyID") = value
        End Set
    End Property


#End Region

#Region "Page Events"

    Protected Sub CommonDialogs_DialogAddEditNotes_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If NoteID > 0 Then
                Populate()
            Else
                Page.SetFocus(txtTitle)
            End If
        End If
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Populate the form with Notes Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Populate()

        Dim obj = NoteManager.GetById(NoteID)

        'populate fields
        txtTitle.Text = obj.Title
        ddlType.SelectedValue = obj.NoteTypeID

        If obj.NoteTypeID = MilesMetaTypeItem.NotesTypeLink Then
            pnlLinkURL.Visible = True
            txtLinkURL.Text = obj.LinkURL
        End If
        chkPinNote.Checked = obj.Pinned
        txtNotes.Content = obj.Notes

        'set entity footer
        SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)
    End Sub

    ''' <summary>
    ''' Saves or updates the record
    ''' </summary>
    ''' <param name="close"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Save(Optional close As Boolean = False) As Boolean

        Try
            Dim obj = NoteManager.GetById(NoteID)
            obj.ObjectID = ObjectID
            obj.KeyID = KeyID
            obj.Title = txtTitle.Text
            obj.NoteTypeID = ddlType.SelectedValue.ToInteger
            If ddlType.SelectedValue = MilesMetaTypeItem.NotesTypeLink Then
                obj.LinkURL = txtLinkURL.Text
            Else
                obj.LinkURL = ""
            End If
            obj.Pinned = chkPinNote.Checked
            obj.Notes = txtNotes.Content
            obj.NotesText = txtNotes.Text


            'save customer
            NoteID = NoteManager.Save(obj)

            'set entity footer
            SetEntityFooter(obj.CreatedBy, obj.DateCreated, obj.ModifiedBy, obj.DateModified)

            Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
            'show success
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Note", useParent:=close And Not delay, isDelayed:=close And delay)

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
    Protected Sub ddlType_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlType.SelectedIndexChanged

        Page.SetFocus(chkPinNote)
        If Not ddlType.SelectedValue.ToInteger() = 0 Then
            If ddlType.SelectedValue.ToInteger() = MilesMetaTypeItem.NotesTypeLink Then
                pnlLinkURL.Visible = True

                Page.SetFocus(txtLinkURL)
            Else
                pnlLinkURL.Visible = False
            End If

        Else
            pnlLinkURL.Visible = False
        End If

    End Sub

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then

            CloseDialogWindow(NoteID)


        End If
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        CloseDialogWindow()

    End Sub

#End Region


End Class
