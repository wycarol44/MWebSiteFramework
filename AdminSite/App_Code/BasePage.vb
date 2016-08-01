Imports Microsoft.VisualBasic
Imports System.IO


Public Class BasePage
    Inherits Page

#Region "Public Properties"

    ''' <summary>
    ''' Gets the form ID of the current page
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FormID As Integer
        Get
            Return ToInteger(ViewState("FormID"))
        End Get
        Private Set(value As Integer)
            ViewState("FormID") = value
        End Set
    End Property


    Protected Property AccessType As AccessType
        Get
            Return ViewState("AccessType")
        End Get
        Private Set(value As AccessType)
            ViewState("AccessType") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets a flag indicating the user has made a change on the page, and saved it
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Property Saved As Boolean
        Get
            Return ViewState("Saved")
        End Get
        Set(value As Boolean)
            ViewState("Saved") = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets whether the master page on the page is dialog master
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DialogWindow As Boolean
        Get
            Return ViewState("DialogWindow")
        End Get
        Set(value As Boolean)
            ViewState("DialogWindow") = value
        End Set
    End Property

    Public Property ReferrerPageURL As Uri
        Get
            Return ViewState("ReferrerPageURL")
        End Get
        Set(value As Uri)
            ViewState("ReferrerPageURL") = value
        End Set
    End Property


#End Region

#Region "Override Properties"
    Public Overrides Property Theme As String
        Get
            Return "Default"
        End Get
        Set(value As String)
            MyBase.Theme = value
        End Set
    End Property


    ''' <summary>
    ''' Gets or sets the page header text
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PageHeader As String
        Get
            Dim header As String = ""

            'get master page
            If TypeOf Me.Master Is IPageHeader Then
                Dim m As IPageHeader = Me.Master

                header = m.PageHeader
            End If

            Return header
        End Get
        Set(value As String)
            If TypeOf Me.Master Is IPageHeader Then
                Dim m As IPageHeader = Me.Master

                m.PageHeader = value
            End If
        End Set
    End Property


#End Region

#Region "Override Methods"

    Protected Overrides Sub OnPreInit(e As EventArgs)

        'check security
        If UserAuthentication.User Is Nothing Then
            UserAuthentication.IsUserAuthenticated()
        End If

        'If Fullmode then change the master page
        If ToInteger(Request("Fullmode")) = 1 AndAlso TypeOf Me.Master Is IDialogMaster Then
            Me.MasterPageFile = "/Master/Main.master"
        End If

        If TypeOf Me.Master Is IDialogMaster Then
            DialogWindow = True
        Else
            DialogWindow = False
        End If

        MyBase.OnPreInit(e)
    End Sub

    Protected Overrides Sub OnInit(e As EventArgs)

        Dim currentPage = SiteMap.CurrentNode
        If currentPage IsNot Nothing Then
            'add form if in sitemap and not in db
            FormID = MetaFormManager.AddForm(currentPage.Url, currentPage.Title)

            'set page header
            If Not TypeOf Me.Master Is IDialogMaster Then
                'Title = AppSettings.ApplicationName + " - " + currentPage.Title
                Title = CompanyInfoManager.ApplicationName + " - " + currentPage.Title
            End If

            PageHeader = currentPage.Title
        End If

        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub OnInitComplete(e As EventArgs)
        If Not Page.IsPostBack Then
            If FormID > 0 AndAlso UserAuthentication.User IsNot Nothing AndAlso UserAuthentication.User.UserID <> AppSettings.ADImpersonateUserID Then
                SetPageSecurity()
            End If
        End If
        MyBase.OnInitComplete(e)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
    End Sub

    Public Sub SetReferrer()
        ReferrerPageURL = HttpContext.Current.Request.UrlReferrer
    End Sub

    Protected Overrides Sub OnPreRenderComplete(e As EventArgs)

        'dispatch any delayed messages
        JGrowl.DispatchMessages()
        If Me.AccessType = BusinessLibrary.AccessType.ReadOnlyAccess Then

            MakeControlsReadOnly(Me)

        End If

        MyBase.OnPreRenderComplete(e)
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Sets the entity footer for created by and modified by
    ''' </summary>
    ''' <param name="createdBy"></param>
    ''' <param name="dateCreated"></param>
    ''' <param name="modifiedBy"></param>
    ''' <param name="dateModified"></param>
    ''' <remarks></remarks>
    Public Sub SetEntityFooter(createdBy As Integer?, dateCreated As Date?, modifiedBy As Integer?, dateModified As Date?)
        If TypeOf Master Is IEntityFooter Then
            Dim m As IEntityFooter = Master

            'get the label
            Dim lblEntityFooter = m.GetEntityFooter()
            If lblEntityFooter IsNot Nothing Then

                'set the entity footer text
                Dim parts As New List(Of String)

                Dim createdByUser = UserManager.GetById(createdBy.GetValueOrDefault())
                If createdByUser.UserID > 0 Then
                    parts.Add("Created by: " + createdByUser.Fullname)
                End If

                If dateCreated.HasValue Then
                    parts.Add("Date created: " + dateCreated.ToString())
                End If

                Dim modifiedByUser = UserManager.GetById(modifiedBy.GetValueOrDefault())
                If modifiedByUser.UserID > 0 Then
                    parts.Add("Modified by: " + modifiedByUser.Fullname)
                End If

                If dateModified.HasValue Then
                    parts.Add("Date modified: " + dateModified.ToString())
                End If

                Dim text As String = String.Join(" | ", parts)

                'update via javascript since footer might be outside ajax panel
                Dim js = <script>
                     setTimeout(function(){miles.updateEntityFooter('<%= text %>');}, 0);
                 </script>

                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "updateEntityFooter", DecodeJS(js), True)

                'set the label so its stored in viewstate
                lblEntityFooter.Text = text

            End If
        End If



    End Sub


    ''' <summary>
    ''' Closes the page if the page is a dialog window
    ''' </summary>
    ''' <param name="returnValue"></param>
    ''' <remarks></remarks>
    Public Sub CloseDialogWindow(Optional returnValue As String = Nothing)
        If returnValue IsNot Nothing Then
            'sanitize
            returnValue = returnValue.Replace("'", "\'")
        End If

        'create script
        Dim js = <script>

                     (function(){
                        var val = <%= If(Not String.IsNullOrWhiteSpace(returnValue), String.Format("'{0}'", returnValue), "null") %>;
                        //close window and pass value
                        closeWindow(val);
                        
                     })();


                 </script>

        'register script
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "closeDialog", DecodeJS(js), True)
    End Sub

    ''' <summary>
    ''' Navigates to another page
    ''' </summary>
    ''' <param name="url"></param>
    ''' <remarks></remarks>
    Public Sub Navigate(url As String)
        Response.Redirect(url)
    End Sub



    Private Sub SetPageSecurity()

        'check to see if the user has access to this page by its associated function

        Dim access = UserManager.HasPageAccess(FormID, UserAuthentication.User.UserID)
        Me.AccessType = access
        If access = BusinessLibrary.AccessType.NoAccess Then
            'no page access
            Response.Redirect("~/SystemPages/NoAccess.aspx")

        End If

    End Sub




#End Region

#Region "Read Only Page"

    Private Sub DisableControl(ByVal c2 As System.Web.UI.Control)

        Dim btnTextPattern As String = AppSettings.MilesActions.ToUpper

        ' HttpContext.Current.Response.Write(c2.GetType.ToString.ToUpper & "<br/>")

        'Miles Card List ( Action Template( Archive, Edit ) , Add New)
        If TypeOf c2 Is MilesCardList Then
            'Dim cardList As MilesCardList = c2
            MakeControlsReadOnly(c2)
            'Miles Notes Dashboard ( Add New)
        ElseIf TypeOf c2 Is MilesNotesDashboard Then

            MakeControlsReadOnly(c2)

            'Miles Document Dashboard
        ElseIf TypeOf c2 Is MilesDocumentsDashboard Then
            MakeControlsReadOnly(c2)
            'GRID/MILES GRID
        ElseIf TypeOf c2 Is RadGrid Then
            Dim grid As RadGrid = c2

            'LOOP THRU GRID COLUMNS
            For Each gc As GridColumn In grid.Columns
                If TypeOf gc Is GridEditCommandColumn Or
                    TypeOf gc Is GridArchiveColumn Then

                    gc.Visible = False

                End If
            Next
        ElseIf TypeOf c2 Is MilesGrid Then
            Dim grid As MilesGrid = c2

            'LOOP THRU GRID COLUMNS
            For Each gc As GridColumn In grid.Columns
                If TypeOf gc Is GridEditCommandColumn Or
                    TypeOf gc Is GridArchiveColumn Then

                    gc.Visible = False

                End If
            Next
        ElseIf TypeOf c2 Is RadFileExplorer Then
            Try
                Dim explorer As RadFileExplorer = c2
                explorer.EnableCopy = False
                explorer.EnableCreateNewFolder = False
                explorer.GridContextMenu.Items.FindItemByText("Delete").Visible = False
                explorer.GridContextMenu.Items.FindItemByText("Rename").Visible = False
                explorer.GridContextMenu.Items.FindItemByText("Upload").Visible = False
                explorer.ToolBar.Items.FindItemByText("Upload").Visible = False
                explorer.ToolBar.Items.FindItemByValue("Delete").Visible = False
            Catch ex As Exception

            End Try
        ElseIf TypeOf c2 Is Button Then
            Dim btn As Button = c2
            If Regex.IsMatch(btn.CommandName.ToUpper, btnTextPattern) Or
                Regex.IsMatch(btn.Text.ToUpper, btnTextPattern) Then

                btn.Visible = False

            End If

        ElseIf TypeOf c2 Is RadToolBarButton Then

            Dim btn As RadToolBarButton = c2

            If Regex.IsMatch(btn.Text.ToUpper, btnTextPattern) Or _
                Regex.IsMatch(btn.CommandName.ToUpper, btnTextPattern) Then

                btn.Visible = False

            End If

        ElseIf TypeOf c2 Is LinkButton Then
            Dim btn As LinkButton = c2
            If IsNothing(btn.Attributes("ActionType")) Or
                   (btn.Attributes("ActionType") = "Primary" Or
                    btn.Attributes("ActionType") = "Negative" Or
                    DirectCast(ToInteger(btn.Attributes("ActionType")), ActionType) = ActionType.Primary Or
                    DirectCast(ToInteger(btn.Attributes("ActionType")), ActionType) = ActionType.Negative) Then
                If System.Text.RegularExpressions.Regex.IsMatch(btn.CommandName.ToUpper, btnTextPattern) Or _
                        System.Text.RegularExpressions.Regex.IsMatch(btn.Text.ToUpper, btnTextPattern) Or _
                        System.Text.RegularExpressions.Regex.IsMatch(btn.ToolTip.ToUpper, btnTextPattern) Then
                    btn.Visible = False
                End If
            End If
        ElseIf TypeOf c2 Is Panel Then

            Dim pnl As Panel = c2
            If pnl.Attributes("ActionType") = "Primary" Then
                pnl.Visible = False
            End If

        ElseIf TypeOf c2 Is DropDownList Then
            Dim btn As DropDownList = c2

            If IsNothing(btn.Attributes("ActionType")) Or
                (btn.Attributes("ActionType") = "Primary" Or
                 btn.Attributes("ActionType") = "Negative" Or
                 DirectCast(ToInteger(btn.Attributes("ActionType")), ActionType) = ActionType.Primary Or
                 DirectCast(ToInteger(btn.Attributes("ActionType")), ActionType) = ActionType.Negative) Then
                btn.AutoPostBack = False
            End If


        ElseIf TypeOf c2 Is RadComboBox Then
            Dim btn As RadComboBox = c2
            If IsNothing(btn.Attributes("ActionType")) Or
                (btn.Attributes("ActionType") = "Primary" Or
                 btn.Attributes("ActionType") = "Negative" Or
                 DirectCast(ToInteger(btn.Attributes("ActionType")), ActionType) = ActionType.Primary Or
                 DirectCast(ToInteger(btn.Attributes("ActionType")), ActionType) = ActionType.Negative) Then
                btn.AutoPostBack = False
            End If

        ElseIf TypeOf c2 Is RadMenuItem Then
            Dim btn As RadMenuItem = c2

            If (Regex.IsMatch(btn.Text.ToUpper, btnTextPattern) Or _
                Regex.IsMatch(btn.Value.ToUpper, btnTextPattern)) Then
                'Response.Write(btn.Text.ToUpper & "<br/>")
                btn.Visible = False
            End If

        ElseIf TypeOf c2 Is HyperLink Then
            Dim btn As HyperLink = c2
            If Regex.IsMatch(btn.Text.ToUpper, btnTextPattern) Then
                btn.Visible = False
            End If
        ElseIf TypeOf c2 Is ImageButton Then
            Dim btn As ImageButton = c2

            If Regex.IsMatch(btn.CommandName.ToUpper, btnTextPattern) Then
                btn.Visible = False
            End If
        ElseIf TypeOf c2 Is MilesButton Then
            Dim btn As MilesButton = c2

            If Regex.IsMatch(btn.Text.ToUpper, btnTextPattern) Or
               Regex.IsMatch(btn.CommandName.ToUpper, btnTextPattern) Then

                btn.Visible = False

            End If

        ElseIf TypeOf c2 Is MilesToggleArchiveImageButton Then

            Dim btn As MilesToggleArchiveImageButton = c2

            If Regex.IsMatch(btn.CommandName.ToUpper, btnTextPattern) Then
                btn.Visible = False
            End If

        End If



    End Sub

    Private Sub DisableGrid(ByVal c2 As System.Web.UI.Control)
        Dim btnTextPattern As String = AppSettings.MilesActions.ToUpper

        'GRID/MILES GRID
        If TypeOf c2 Is RadGrid Then
            Dim grid As RadGrid = c2
            grid.MasterTableView.CommandItemDisplay = Telerik.Web.UI.GridCommandItemDisplay.None
            'grid.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = False
        ElseIf TypeOf c2 Is MilesGrid Then
            Dim grid As MilesGrid = c2
            grid.MasterTableView.CommandItemDisplay = Telerik.Web.UI.GridCommandItemDisplay.None

            For Each gc As Telerik.Web.UI.GridColumn In grid.Columns
                If TypeOf gc Is GridEditCommandColumn Or
                    TypeOf gc Is GridArchiveColumn Then
                    gc.Visible = False

                ElseIf TypeOf gc Is GridTemplateColumn Then
                    Dim tc As GridTemplateColumn = gc
                    If Regex.IsMatch(tc.UniqueName.ToUpper, btnTextPattern) Then
                        gc.Visible = False
                    End If

                End If
            Next

        End If
    End Sub

    ' loop thru buttons and disable if access type is read only
    Public Sub MakeControlsReadOnly(ByVal c1 As System.Web.UI.Control)

        For Each c2 As System.Web.UI.Control In c1.Controls

            If c2.UniqueID.Contains("mainContent") And Not c2.UniqueID.Contains("pnlSearch") Then
                DisableControl(c2)
            End If

            If c2.HasControls Then
                MakeControlsReadOnly(c2)
            End If

        Next

    End Sub

    Public Sub MakeGridReadOnly(ByVal c1 As System.Web.UI.Control)
        For Each c2 As System.Web.UI.Control In c1.Controls
            DisableGrid(c2)
            If c2.HasControls Then
                MakeGridReadOnly(c2)
            End If
        Next
    End Sub

#End Region


#Region "Viewstate"

    Protected Overrides Function LoadPageStateFromPersistenceMedium() As Object
        Dim alteredViewState As [String]
        Dim bytes As Byte()
        Dim rawViewState As [Object]
        Dim fomatter As New LosFormatter()
        Me.PageStatePersister.Load()
        alteredViewState = Me.PageStatePersister.ViewState.ToString()
        bytes = Convert.FromBase64String(alteredViewState)
        bytes = Compressor.Decompress(bytes)
        rawViewState = fomatter.Deserialize(Convert.ToBase64String(bytes))
        Return New Pair(Me.PageStatePersister.ControlState, rawViewState)
    End Function
    ''' <summary>
    ''' takes the view state compresses it and then saves it back in the page state
    ''' </summary>
    ''' <param name="viewStateIn"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub SavePageStateToPersistenceMedium(viewStateIn As Object)
        Dim fomatter As New LosFormatter()
        Dim writer As New StringWriter()
        Dim rawPair As Pair
        Dim rawViewState As [Object]
        Dim rawViewStateStr As [String]
        Dim alteredViewState As [String]
        Dim bytes As Byte()
        If TypeOf viewStateIn Is Pair Then
            rawPair = DirectCast(viewStateIn, Pair)
            Me.PageStatePersister.ControlState = rawPair.First
            rawViewState = rawPair.Second
        Else
            rawViewState = viewStateIn
        End If
        fomatter.Serialize(writer, rawViewState)
        rawViewStateStr = writer.ToString()
        bytes = Convert.FromBase64String(rawViewStateStr)
        bytes = Compressor.Compress(bytes)
        alteredViewState = Convert.ToBase64String(bytes)
        Me.PageStatePersister.ViewState = alteredViewState
        Me.PageStatePersister.Save()
    End Sub

#End Region



End Class
