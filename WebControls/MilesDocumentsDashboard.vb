Imports BusinessLibrary
Imports CommonLibrary
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Imports System.IO

Public Class MilesDocumentsDashboard
    Inherits Panel

    ''' <summary>
    ''' Author: Sanjog Sharma
    ''' Documents List Control for DashBoard
    ''' Displays Documents in dashboard ordered by uploaded date
    ''' Number of items to display is configurable using property "NumberofDisplayItems", default value is 2
    ''' ObjectID and KeyID identifies which entity the documents is for, "Populate" function populates the header and items for the control
    ''' Corresponding document icon is displayed for mostly used documents, and text icon is displayed for others 
    ''' Counter for documents is displayed in right, which is shown by default but can be turned off via property "Show Counter"
    ''' HeaderText and HeaderNaviationURL is configurable. Provide the DocumentsList page for the object in the HeaderNavigationURL    ''' </summary>
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
            If ViewState("DocumentsHeaderText") Is Nothing Then
                Return "Documents"
            Else
                Return ViewState("DocumentsHeaderText")
            End If
        End Get
        Set(value As String)
            ViewState("DocumentsHeaderText") = value
        End Set
    End Property

    Public Property HeaderNavigationURL As String
        Get
            If ViewState("DocumentsHeaderNavigationURL") Is Nothing Then
                Return ""
            Else
                Return ViewState("DocumentsHeaderNavigationURL")
            End If
        End Get
        Set(value As String)
            ViewState("DocumentsHeaderNavigationURL") = value
        End Set
    End Property

    Public Property ShowCounter As Boolean
        Get
            If ViewState("ShowDocumentsCounter") Is Nothing Then
                Return True
            Else
                Return ViewState("ShowDocumentsCounter")
            End If
        End Get
        Set(value As Boolean)
            ViewState("ShowDocumentsCounter") = value
        End Set
    End Property

    Public Property EmptyText As String
        Get
            If ViewState("DocumentsEmptyText") Is Nothing Then
                Return "No Documents Added"
            Else
                Return ViewState("DocumentsEmptyText")
            End If
        End Get
        Set(value As String)
            ViewState("DocumentsEmptyText") = value
        End Set
    End Property

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property DocumentsList As DataList
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property DocumentsHeader As HyperLink

    Private Property EmptyTextLabel As Label
    Private Property ViewMore As HyperLink

#End Region

#Region "Constructor"
    Public Sub New()
        DocumentsHeader = New HyperLink
        DocumentsList = New DataList
        EmptyTextLabel = New Label
        ViewMore = New HyperLink

    End Sub
#End Region

#Region "Overrides"
    Protected Overrides Sub OnInit(e As EventArgs)
        'assign the itemtemplate to the datalist
        DocumentsList.ItemTemplate = New MilesDocumentsDashboardItemTemplate
        EmptyTextLabel.Visible = False
        EmptyTextLabel.Font.Italic = True
        EmptyTextLabel.Text = EmptyText

        Me.Controls.Add(New LiteralControl("<div class='panel panel-default widget'><div class='panel-heading anchorheader'>"))
        Me.Controls.Add(DocumentsHeader)
        Me.Controls.Add(New LiteralControl("</div><div class='panel-body'><div class='contentarea'>"))
        Me.Controls.Add(EmptyTextLabel)
        Me.Controls.Add(DocumentsList)
        Me.Controls.Add(ViewMore)
        Me.Controls.Add(New LiteralControl("</div></div></div>"))
        MyBase.OnInit(e)
    End Sub
#End Region

#Region "Methods"
    'Function call needed after assigning objectid and keyid
    Public Sub Populate()
        'set up the header text, add counter if showcounter is set to true and there is atleast one record
        Dim documentscount = DocumentManager.GetCount(ObjectID, KeyID)

        If ShowCounter And documentscount > 0 Then
            Dim documentscounter = <span class="badge pull-right"><%= documentscount %></span>
            DocumentsHeader.Text = HeaderText & documentscounter.ToString()
        Else
            DocumentsHeader.Text = HeaderText
        End If
        'set the navigation for header
        DocumentsHeader.NavigateUrl = HeaderNavigationURL

        If documentscount = 0 Then
            ViewMore.Visible = False
            EmptyTextLabel.Visible = True
            DocumentsList.Visible = False
        Else
            EmptyTextLabel.Visible = False
            DocumentsList.Visible = True
            'bind the datalist
            Dim objList = DocumentManager.GetListForDashBoard(ObjectID, KeyID, NumberofDisplayItems)
            DocumentsList.DataSource = objList
            DocumentsList.DataBind()

            'display view more link at bottom
            If documentscount > NumberofDisplayItems Then
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

End Class

'ItemTemplate Class controls needed for each item is added here
Class MilesDocumentsDashboardItemTemplate
    Implements ITemplate

    Public Sub New()

    End Sub

    Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn
        'create the controls we need
        Dim lnkDocumentName As New LinkButton
        Dim lblCreatedBy As New Label

        'set properties
        'assign 100% width so created by goes to second line
        lnkDocumentName.CssClass = "item"
        lblCreatedBy.CssClass = "itemfooter text-muted"

        AddHandler lnkDocumentName.DataBinding, AddressOf lnkDocumentName_DataBinding
        'handler for click event 
        AddHandler lnkDocumentName.Click, AddressOf lnkDocumentName_Click
        AddHandler lblCreatedBy.DataBinding, AddressOf lblCreatedBy_DataBinding

        'add all to the container
        container.Controls.Add(lnkDocumentName)
        container.Controls.Add(lblCreatedBy)

    End Sub

    Private Sub lnkDocumentName_DataBinding(sender As Object, e As EventArgs)
        Dim ctrl As LinkButton = sender

        Dim container As DataListItem = ctrl.NamingContainer
        'get the icon for mostly used files, otherwise text file icon is used
        Dim imageurl = Functions.GetDocIcon(Path.GetExtension(DataBinder.Eval(container.DataItem, "FilePath")))

        'append img tag infront of filename
        Dim imagetext = <img src=<%= imageurl %> style="padding-right:5px;"/>
        ctrl.Text = imagetext.ToString() & DataBinder.Eval(container.DataItem, "DocumentName")
        ctrl.Attributes.Add("ActionType", "None")
        'add the filepath as attribute to the button , so that it can be used in button_click
        ctrl.Attributes.Add("FilePath", DataBinder.Eval(container.DataItem, "FilePath"))
    End Sub

    Private Sub lblCreatedBy_DataBinding(sender As Object, e As EventArgs)
        Dim ctrl As Label = sender

        Dim container As DataListItem = ctrl.NamingContainer

        ctrl.Text = "- Uploaded By " & DataBinder.Eval(container.DataItem, "CreatedByName") &
                    " on " & ToFormattedShortDateString(DataBinder.Eval(container.DataItem, "DateCreated"))

    End Sub

    'flush the file when it is clicked
    Private Sub lnkDocumentName_Click(sender As Object, e As EventArgs)
        Dim lnk As LinkButton = sender
        Dim container As DataListItem = lnk.NamingContainer
        Dim filename As String = Path.GetFileName(lnk.Attributes("FilePath"))
        Dim fullfilepath As String = Path.Combine(AppSettings.UploadedDocumentsFolder, filename)
        FlushFile(filename, fullfilepath)
    End Sub

End Class