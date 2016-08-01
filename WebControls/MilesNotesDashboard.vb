Imports BusinessLibrary
Imports CommonLibrary
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Imports System.IO
Imports Telerik.Web.UI

Public Class MilesNotesDashboard
    Inherits Panel

    ''' <summary>
    ''' Author: Sanjog Sharma
    ''' Notes List Control for DashBoard
    ''' Displays notes in dashboard ordered by pinned notes and created date
    ''' Number of items to display is configurable using property "NumberofDisplayItems", default value is 2
    ''' ObjectID and KeyID identifies which entity the documents is for, "Populate" function populates the header and items for the control
    ''' There is icon added for notes/link, if it is a link the icon is clickable and it navigates to the link URL
    ''' Counter for notes is displayed in right, which is shown by default but can be turned off via property "Show Counter"
    ''' HeaderText and NavigateURL is configurable.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>


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

    Public Property NumberofDisplayItems As Integer
        Get
            If ViewState("NumberofDisplayItems") Is Nothing Then 'set 2 items as default 
                Return 2
            Else
                Return ViewState("NumberofDisplayItems")
            End If
        End Get
        Set(value As Integer)
            ViewState("NumberofDisplayItems") = value
        End Set
    End Property

    Public Property HeaderText As String
        Get
            If ViewState("NotesHeaderText") Is Nothing Then
                Return "Notes"
            Else
                Return ViewState("NotesHeaderText")
            End If
        End Get
        Set(value As String)
            ViewState("NotesHeaderText") = value
        End Set
    End Property

    Public Property HeaderNavigationURL As String
        Get
            If ViewState("NotesHeaderNavigationURL") Is Nothing Then
                Return ""
            Else
                Return ViewState("NotesHeaderNavigationURL")
            End If
        End Get
        Set(value As String)
            ViewState("NotesHeaderNavigationURL") = value
        End Set
    End Property

    Public Property ShowCounter As Boolean
        Get
            If ViewState("ShowNotesCounter") Is Nothing Then
                Return True
            Else
                Return ViewState("ShowNotesCounter")
            End If
        End Get
        Set(value As Boolean)
            ViewState("ShowNotesCounter") = value
        End Set
    End Property

    Public Property EmptyText As String
        Get
            If ViewState("NotesEmptyText") Is Nothing Then
                Return "No Notes Added"
            Else
                Return ViewState("NotesEmptyText")
            End If
        End Get
        Set(value As String)
            ViewState("NotesEmptyText") = value
        End Set
    End Property

    ''' <summary>
    ''' gets or sets the info page for notes
    ''' Provide the full path to info page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InfoPage As String
        Get
            If ViewState("InfoPage") Is Nothing Then
                Return ""
            Else
                Return ViewState("InfoPage")
            End If
        End Get
        Set(value As String)
            ViewState("InfoPage") = value
        End Set
    End Property


    ''' <summary>
    ''' gets or sets the id field for notes
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IDField As String
        Get
            If ViewState("IDField") Is Nothing Then
                Return "NoteID"
            Else
                Return ViewState("IDField")
            End If
        End Get
        Set(value As String)
            ViewState("IDField") = value
        End Set
    End Property

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property NotesList As DataList
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property NotesHeader As HyperLink
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property AddNote As ImageButton

    Private Property AjaxPanel As Telerik.Web.UI.RadAjaxPanel
    Public Property EmptyTextLabel As Label

    Private Property ViewMore As HyperLink

#End Region

#Region "Constructor"
    Public Sub New()
        NotesHeader = New HyperLink
        NotesList = New DataList
        AjaxPanel = New Telerik.Web.UI.RadAjaxPanel
        AddHandler AjaxPanel.AjaxRequest, AddressOf AjaxPanel_AjaxRequest

        EmptyTextLabel = New Label
        EmptyTextLabel.Visible = False
        EmptyTextLabel.Font.Italic = True
        EmptyTextLabel.Text = EmptyText

        ViewMore = New HyperLink
        AddNote = New ImageButton
        AddNote.ImageUrl = "~/Images/24x24/document-add.png"
        AddNote.ToolTip = "Add New Note"
        AddNote.CommandName = "AddNew"
        AddNote.Attributes.Add("onclick", "openNoteEditDialog(0); return false;")
    End Sub
#End Region

#Region "Overrides"

    Protected Overrides Sub OnPreRender(e As EventArgs)
        Dim scriptID As String = Me.ClientID & "ScriptKey"

        Dim js = <script>
                      function openNoteEditDialog(id) {
                        openWindow('<%= InfoPage %>?<%= IDField %>=' + id + '&amp;ObjectID=<%= CInt(ObjectID) %>&amp;KeyID=<%= KeyID %>', 'Notes', WINDOW_LARGE, notesdialogClosed); 
                      }
                        function notesdialogClosed(sender, args) {
                                var arg = args.get_argument();
                                if (arg != null) {
                                    $find('<%= AjaxPanel.ClientID %>').ajaxRequest();}
                      }
                 </script>

        'register the scripts with the script manager
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), scriptID, DecodeJS(js), True)

        MyBase.OnPreRender(e)
    End Sub

    Protected Overrides Sub OnInit(e As EventArgs)
        NotesList.ItemTemplate = New MilesNotesDashboardItemTemplate
        Me.Controls.Add(AjaxPanel) 'add the controls inside ajaxpanel since they needs to be updated after note is saved
        AjaxPanel.Controls.Add(New LiteralControl("<div class='panel panel-default widget'><div class='panel-heading anchorheader'>"))
        AjaxPanel.Controls.Add(NotesHeader)
        AjaxPanel.Controls.Add(New LiteralControl("</div><div class='panel-body action-right'><div class='contentarea'>"))
        AjaxPanel.Controls.Add(EmptyTextLabel)
        AjaxPanel.Controls.Add(NotesList)
        AjaxPanel.Controls.Add(ViewMore)
        AjaxPanel.Controls.Add(New LiteralControl("</div><div class='action'>"))
        AjaxPanel.Controls.Add(AddNote)
        AjaxPanel.Controls.Add(New LiteralControl("</div></div></div>"))
        MyBase.OnInit(e)
    End Sub
#End Region

#Region "Methods"
    'Function call needed after assigning objectid and keyid
    Public Sub Populate()
        Dim notescount = NoteManager.GetCount(ObjectID, KeyID)

        'set up the header text, add counter if showcounter is set to true and there is atleast one record
        If ShowCounter And notescount > 0 Then
            Dim notescounter = <span class="badge pull-right"><%= notescount %></span>
            NotesHeader.Text = HeaderText & notescounter.ToString()
        Else
            NotesHeader.Text = HeaderText
        End If
        'set the navigation for header
        NotesHeader.NavigateUrl = HeaderNavigationURL


        If notescount = 0 Then
            ViewMore.Visible = False
            EmptyTextLabel.Visible = True
            NotesList.Visible = False
        Else
            EmptyTextLabel.Visible = False
            NotesList.Visible = True
            'bind the datalist
            Dim objList = NoteManager.GetListForDashBoard(ObjectID, KeyID, NumberofDisplayItems)
            NotesList.DataSource = objList
            NotesList.DataBind()

            'display view more link at bottom
            If notescount > NumberofDisplayItems Then
                ViewMore.Text = "View More..."
                ViewMore.Font.Italic = True
                ViewMore.NavigateUrl = HeaderNavigationURL
                ViewMore.Visible = True
            Else
                ViewMore.Visible = False
            End If
        End If
    End Sub
#End Region

    Private Sub AjaxPanel_AjaxRequest(sender As Object, e As AjaxRequestEventArgs)
        'populate the notes again after saving
        Populate()
    End Sub


End Class

'ItemTemplate Class controls needed for each item is added here
Class MilesNotesDashboardItemTemplate
    Implements ITemplate

    Public Sub New()

    End Sub

    Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn
        'create the controls we need
        Dim lblNoteIndicator As New Label
        Dim lnkLinkIndicator As New HyperLink
        Dim lnkNoteName As New HyperLink
        Dim lblCreatedBy As New Label

        'set properties
        lnkNoteName.CssClass = "item"
        lblCreatedBy.CssClass = "itemfooter text-muted"
        lblNoteIndicator.CssClass = "itemindicator"
        lnkLinkIndicator.CssClass = "itemindicator"

        lnkLinkIndicator.Target = "_blank"
        lnkLinkIndicator.Font.Underline = False


        AddHandler lblNoteIndicator.DataBinding, AddressOf Indicator_DataBinding
        AddHandler lnkLinkIndicator.DataBinding, AddressOf Indicator_DataBinding
        AddHandler lnkNoteName.DataBinding, AddressOf lnkNoteName_DataBinding
        AddHandler lblCreatedBy.DataBinding, AddressOf lblCreatedBy_DataBinding

        'add all to the container
        container.Controls.Add(lblNoteIndicator)
        container.Controls.Add(lnkLinkIndicator)
        container.Controls.Add(lnkNoteName)
        container.Controls.Add(lblCreatedBy)
    End Sub

    'bind the note/link indicator
    Private Sub Indicator_DataBinding(sender As Object, e As EventArgs)
        Dim ctrl As Control = sender
        Dim container As DataListItem = ctrl.NamingContainer
        Dim notetype As New MilesMetaTypeItem
        notetype = DataBinder.Eval(container.DataItem, "NoteTypeID")
        If notetype = MilesMetaTypeItem.NotesTypeLink Then 'bind hyperlink if it is a link and hide the label
            If TypeOf sender Is HyperLink Then
                ctrl.Visible = True
                DirectCast(ctrl, HyperLink).CssClass += " glyphicon glyphicon-link"
                DirectCast(ctrl, HyperLink).NavigateUrl = DataBinder.Eval(container.DataItem, "LinkUrl")
                DirectCast(ctrl, HyperLink).ToolTip = DataBinder.Eval(container.DataItem, "LinkUrl")
            Else
                ctrl.Visible = False
            End If
        ElseIf notetype = MilesMetaTypeItem.NotesTypeNote Then 'bind label if it is note and hide the hyperlink
            If TypeOf sender Is Label Then
                ctrl.Visible = True
                DirectCast(ctrl, Label).CssClass += " glyphicon glyphicon-list-alt"
            Else
                ctrl.Visible = False
            End If
        End If
    End Sub

    'bind the notetitle
    Private Sub lnkNoteName_DataBinding(sender As Object, e As EventArgs)
        Dim ctrl As HyperLink = sender

        Dim container As DataListItem = ctrl.NamingContainer
        ctrl.Text = DataBinder.Eval(container.DataItem, "Title")
        Dim NoteID As Integer = DataBinder.Eval(container.DataItem, "NoteID")
        ctrl.NavigateUrl = "javascript:openNoteEditDialog(" & NoteID & ");"
    End Sub

    'bind createdby information
    Private Sub lblCreatedBy_DataBinding(sender As Object, e As EventArgs)
        Dim ctrl As Label = sender

        Dim container As DataListItem = ctrl.NamingContainer

        ctrl.Text = "- Created By " & DataBinder.Eval(container.DataItem, "CreatedByName") &
                    " on " & ToFormattedShortDateString(DataBinder.Eval(container.DataItem, "DateCreated"))
    End Sub

End Class