Imports BusinessLibrary
Imports CommonLibrary
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Imports System.IO

''' <summary>
''' Author: Sanjog Sharma
''' AuditLog Control for DashBoard
''' Displays Audit Logs in dashboard ordered by changed date
''' Number of items to display is configurable using property "NumberofDisplayItems", default value is 5
''' ObjectID and KeyID identifies which entity the audit log is for, "Populate" function populates the header and items for the control
''' AuditLog is rendered in collapsible panel but is configurable
''' Counter for AuditLogs is displayed in right, which is shown by default but can be turned off via property "Show Counter"
''' </summary>
''' <remarks></remarks>
Public Class MilesAuditLogDashboard
    Inherits Panel
    Implements INamingContainer



    Dim AuditLogPanelBodyWrapper As New Panel
    Dim AuditLogPanel As New Panel
    Dim AuditLogPanelBodyContent As New Panel

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
            If ViewState("NumberofDisplayItems") Is Nothing Then 'set 5 items as default 
                Return 5
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
            If ViewState("AuditLogHeaderText") Is Nothing Then
                Return "Audit Log"
            Else
                Return ViewState("AuditLogHeaderText")
            End If
        End Get
        Set(value As String)
            ViewState("AuditLogHeaderText") = value
        End Set
    End Property

    Public Property ShowCounter As Boolean
        Get
            If ViewState("ShowLogCounter") Is Nothing Then
                Return True
            Else
                Return ViewState("ShowLogCounter")
            End If
        End Get
        Set(value As Boolean)
            ViewState("ShowLogCounter") = value
        End Set
    End Property

    ''' <summary>
    ''' renders the control as collapsible panel if set to true
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Collapsible As Boolean
        Get
            If ViewState("Collapsible") Is Nothing Then
                Return True
            Else
                Return ViewState("Collapsible")
            End If
        End Get
        Set(value As Boolean)
            ViewState("Collapsible") = value
        End Set
    End Property

    ''' <summary>
    ''' expands the collapsible on load if set to true
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ExpandOnLoad As Boolean
        Get
            If ViewState("ExpandOnLoad") Is Nothing Then
                Return True
            Else
                Return ViewState("ExpandOnLoad")
            End If
        End Get
        Set(value As Boolean)
            ViewState("ExpandOnLoad") = value
        End Set
    End Property

    ''' <summary>
    ''' Set the option to show/hide the header bar, works if only collapsible is false
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowHeader As Boolean
        Get
            If ViewState("ShowHeader") Is Nothing Then
                Return True
            Else
                Return ViewState("ShowHeader")
            End If
        End Get
        Set(value As Boolean)
            ViewState("ShowHeader") = value
        End Set
    End Property

    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property AuditLogList As DataList
    <PersistenceMode(PersistenceMode.InnerProperty)>
    Public Property AuditLogHeader As HyperLink

    Private Property ViewMore As LinkButton
    Private Property AjaxPanel As Telerik.Web.UI.RadAjaxPanel

#End Region

#Region "Constructor"
    Public Sub New()
        AuditLogHeader = New HyperLink
        AuditLogList = New DataList
        AjaxPanel = New Telerik.Web.UI.RadAjaxPanel
        ViewMore = New LinkButton

    End Sub
#End Region

#Region "Overrides"

    Protected Overrides Sub OnPreRender(e As EventArgs)
        If Collapsible Then
            AuditLogHeader.Attributes.Add("data-target", String.Format("#{0}", AuditLogPanelBodyWrapper.ClientID))


            Dim scriptID As String = Me.ClientID & "ScriptKey"
            'register the client script to change collapse icon on the header on click
            Dim js = <script>
                     $(document).ready(function () {
                            $('#<%= AuditLogPanel.ClientID %> .accordion-toggle').click(function (e) {
                                var chevState = $("#<%= AuditLogPanel.ClientID %> a .indicator").toggleClass('glyphicon-chevron-down glyphicon-chevron-up');
                                $("#<%= AuditLogPanel.ClientID %> i.indicator").not(chevState).removeClass("glyphicon-chevron-down").addClass("glyphicon-chevron-up");
                            });
                        });
                 </script>

            'register the scripts with the script manager
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), scriptID, DecodeJS(js), True)
        End If
        MyBase.OnPreRender(e)

    End Sub

    Protected Overrides Sub OnInit(e As EventArgs)
        'assign the itemtemplate to the datalist
        AuditLogList.ItemTemplate = New MilesAuditLogDashboardItemTemplate

        AuditLogPanel.ID = "AuditLogPanel"
        AuditLogPanel.CssClass = "panel panel-default widget"

        Dim AuditLogPanelHeader As New Panel
        AuditLogPanelHeader.CssClass = "panel-heading anchorheader"

        AuditLogPanelBodyWrapper.ID = "AuditLogPanelBodyWrapper"

        If Collapsible Then
            'configure the header to make it collapsible
            'refer: http://getbootstrap.com/javascript/#collapse
            AuditLogHeader.CssClass = "accordion-toggle"
            AuditLogHeader.Attributes.Add("data-toggle", "collapse")
            AuditLogHeader.NavigateUrl = "javascript:void(0);"

            'define the area to collapse
            AuditLogPanelBodyWrapper.CssClass = "collapse"

            'add class to expand on default, if set to true
            If ExpandOnLoad Then AuditLogPanelBodyWrapper.CssClass += " in"
        End If

        Dim AuditLogPanelBody As New Panel
        AuditLogPanelBody.ID = "AuditLogPanelBody"
        AuditLogPanelBody.CssClass = "panel-body"


        AuditLogPanelBodyContent.ID = "AuditLogPanelBodyContent"
        AuditLogPanelBodyContent.CssClass = "contentarea"

        'add toggle to header
        AuditLogPanelHeader.Controls.Add(AuditLogHeader)

        ViewMore.ID = "ViewMore"
        AuditLogList.ID = "AuditLogList"


        AuditLogPanelBodyContent.Controls.Add(AuditLogList)
        AuditLogPanelBodyContent.Controls.Add(ViewMore)

        AuditLogPanelBody.Controls.Add(AuditLogPanelBodyContent)

        AuditLogPanelBodyWrapper.Controls.Add(AuditLogPanelBody)

        AuditLogPanel.Controls.Add(AuditLogPanelHeader)
        AuditLogPanel.Controls.Add(AuditLogPanelBodyWrapper)

        AjaxPanel.Controls.Add(AuditLogPanel)

        AddHandler ViewMore.Click, AddressOf ViewMore_Click


        Me.Controls.Add(AjaxPanel)

        'hide the header if not collapsible ShowHeader is set to false
        If Not Collapsible And Not ShowHeader Then AuditLogPanelHeader.Visible = False

        MyBase.OnInit(e)
    End Sub



#End Region

#Region "Methods"
    'Function call needed after assigning objectid and keyid
    Public Sub Populate(Optional ByVal ViewMoreClicked As Boolean = False)

        Dim logcount = AuditLogManager.GetCount(ObjectID, KeyID)

        'set up the header text, add counter if showcounter is set to true and there is atleast one record
        If ShowCounter And logcount > 0 Then
            Dim logcounter = <span class="badge pull-right"><%= logcount %></span>
            AuditLogHeader.Text = HeaderText & logcounter.ToString()
        Else
            AuditLogHeader.Text = HeaderText
        End If

        'add the collapse icon if the panel is collapsible
        If Collapsible Then
            'header icon to indicate the collapsible panel
            Dim chevIconClass As String = ""
            If ExpandOnLoad Or ViewMoreClicked Then
                chevIconClass = "glyphicon-chevron-up"
            Else
                chevIconClass = "glyphicon-chevron-down"
            End If

            Dim headerChev = <i class=<%= String.Format("indicator glyphicon {0}", chevIconClass) %> style="padding-right:20px;"></i>
            AuditLogHeader.Text = headerChev.ToString & AuditLogHeader.Text
        End If

        'Bind datalist
        Dim objList = AuditLogManager.GetListForDashBoard(ObjectID, KeyID, NumberofDisplayItems)
        AuditLogList.DataSource = objList
        AuditLogList.DataBind()

        If logcount > NumberofDisplayItems Then
            ViewMore.Text = "View More..."
            ViewMore.Font.Italic = True
            ViewMore.Visible = True
        Else
            ViewMore.Visible = False
        End If

    End Sub
#End Region

    Private Sub ViewMore_Click(sender As Object, e As EventArgs)
        NumberofDisplayItems = 0
        Populate()

        If Collapsible And Not ExpandOnLoad Then
            'since the collpse element is collapsed by default on postback, we need to add client script to show the div
            Dim js = <script>
                     $(document).ready(function () {
                           $('#<%= AuditLogPanelBodyWrapper.ClientID %>').collapse('show');
                    });
                </script>

            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), "showCollapse_" & Me.ClientID, DecodeJS(js), True)
        End If
        ViewMore.Visible = False
    End Sub

End Class

'ItemTemplate Class controls needed for each item is added here
Class MilesAuditLogDashboardItemTemplate
    Implements ITemplate

    Public Sub New()

    End Sub

    Public Sub InstantiateIn(container As Control) Implements ITemplate.InstantiateIn
        'create the controls we need
        Dim lblChange As New Label
        Dim lblChangedBy As New Label

        'set properties
        'assign 100% width so created by goes to second line
        lblChange.CssClass = "item"
        lblChangedBy.CssClass = "itemfooter text-muted"

        AddHandler lblChange.DataBinding, AddressOf lblChange_DataBinding
        AddHandler lblChangedBy.DataBinding, AddressOf lblChangedBy_DataBinding

        'add all to the container
        container.Controls.Add(lblChange)
        container.Controls.Add(lblChangedBy)
    End Sub

    Private Sub lblChange_DataBinding(sender As Object, e As EventArgs)
        Dim ctrl As Label = sender

        Dim container As DataListItem = ctrl.NamingContainer

        ctrl.Text = DataBinder.Eval(container.DataItem, "AuditLogText")
    End Sub

    Private Sub lblChangedBy_DataBinding(sender As Object, e As EventArgs)
        Dim ctrl As Label = sender

        Dim container As DataListItem = ctrl.NamingContainer

        ctrl.Text = "- Changed By " & DataBinder.Eval(container.DataItem, "CreatedByName") &
                    " on " & ToFormattedString(DataBinder.Eval(container.DataItem, "DateCreated"))

    End Sub
End Class