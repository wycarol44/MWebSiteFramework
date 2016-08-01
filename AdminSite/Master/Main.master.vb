
Partial Class Master_Main
    Inherits System.Web.UI.MasterPage
    Implements IEntityFooter
    Implements IPageHeader

#Region "Properties"


#End Region

    Protected Sub Master_Main_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindMenu()
            BindFavorites()
            SetAppDetails()
            SetUserDetails()
        End If

        SetSessionTimeoutScript()
    End Sub

#Region "Methods"

    Private Sub SetSessionTimeoutScript()
        Dim sessionExpiredLink = "/SystemPages/SessionExpired.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.RawUrl)
        Dim sessionTimeout = Session.Timeout * 60000

        'output script
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "SessionTimeOutScript",
                                                String.Format("sessionTimeout({0}, '{1}');", sessionTimeout, sessionExpiredLink), True)
    End Sub

    Private Sub BindFavorites()
        Dim mobileMenuString As New StringBuilder
        'get list of favorites
        Dim favorites = UserFavoriteManager.GetList(UserAuthentication.User.UserID)

        'bind icon bar
        BindFavoritesIconBar(favorites)

        'get the items to show in the toolbar
        Dim userFavorites = favorites.Where(Function(x) x.ShowInFavoritesMenu = True)

        ''create favorite dropdown
        'Dim rdFavoriteDropdown As RadToolBarDropDown

        'find the favorite drop down
        'rdFavoriteDropdown = rdActions.FindItem(Function(x)
        '                                            If TypeOf x Is RadToolBarDropDown Then
        '                                                Dim t As RadToolBarDropDown = x
        '                                                Return t.Attributes("value") = "favorites"
        '                                            Else
        '                                                Return False
        '                                            End If
        '                                        End Function)

        Dim rdFavoriteDropdown As RadToolBarDropDown = rdActions.Items(1)
        'clear all items
        rdFavoriteDropdown.Buttons.Clear()

        'If rdFavoriteDropdown Is Nothing Then
        '    'create new item
        '    rdFavoriteDropdown = New RadToolBarDropDown
        '    'set attributes
        '    rdFavoriteDropdown.Attributes.Add("value", "favorites")
        '    'rdFavoriteDropdown.Text = "&nbsp;" '"<span class='glyphicon glyphicon-star'></span>"
        '    rdFavoriteDropdown.ImageUrl = "~/Images/16x16/favorite.png"
        '    'rdFavoriteDropdown.ImagePosition = ToolBarImagePosition.Right
        '    rdFavoriteDropdown.ToolTip = "Favorites"
        '    rdFavoriteDropdown.DropDownWidth = "200"

        '    'add to the toolbar
        '    rdActions.Items.Add(rdFavoriteDropdown)

        'Else

        '    'clear all items
        '    rdFavoriteDropdown.Buttons.Clear()
        'End If


        'add the favorites to dropdown 
        If userFavorites.Count() Then

            For Each fav In userFavorites

                'add to the toolbar
                rdFavoriteDropdown.Buttons.Add(
                    New RadToolBarButton() With {
                        .Text = fav.DisplayName,
                        .NavigateUrl = fav.MenuPath,
                        .CausesValidation = False})

                'build the mobilefavorite menu
                mobileMenuString.Append((<li><a href=<%= fav.MenuPath %>><%= fav.DisplayName %></a></li>).ToString())

            Next

            'add the seperator
            rdFavoriteDropdown.Buttons.Add(New RadToolBarButton() With {.IsSeparator = True})
            mobileMenuString.Append((<li class="nav-divider"></li>).ToString())

        End If


        'configure add to favorite button, and its navigate url
        Dim currentPage As BasePage = Me.Page
        'get the metaform object
        Dim form = MetaFormManager.GetById(currentPage.FormID)

        'create the add to favorite menu if, the form can be favorite and is not already a favorite
        If form.CanBeFavorite Then
            Dim isUserFav As Boolean = UserFavoriteManager.CheckFavorite(UserAuthentication.User.UserID, currentPage.FormID)
            If Not isUserFav Then
                'add  Add to favorite menu item
                Dim addtoFavoriteMenuItem As New RadToolBarButton
                addtoFavoriteMenuItem.Text = "Add to Favorites"
                addtoFavoriteMenuItem.ImageUrl = "~/Images/16x16/favorite-add.png"
                addtoFavoriteMenuItem.NavigateUrl = String.Format("javascript:openManageFavorites('{0}');", form.FormID)
                rdFavoriteDropdown.Buttons.Add(addtoFavoriteMenuItem)

                mobileMenuString.Append((<li><a href=<%= String.Format("javascript:openManageFavorites({0})", form.FormID) %>>Add To Favorites</a></li>).ToString())
            End If
        End If

        'Add Manage Favorite Menu Item
        rdFavoriteDropdown.Buttons.Add(New RadToolBarButton() With {
                                       .Text = "Manage Favorites",
                                       .ImageUrl = "~/Images/16x16/favorite-settings.png",
                                       .NavigateUrl = "javascript:openManageFavorites(0)",
                                       .CausesValidation = False})

        mobileMenuString.Append((<li><a href='javascript:openManageFavorites(0)'>Manage Favorites</a></li>).ToString())
        ltMobileFavorites.Text = mobileMenuString.ToString()
    End Sub

    Private Sub SetAppDetails()
        lblAppName.Text = CompanyInfoManager.ApplicationName
    End Sub

    Private Sub SetUserDetails()
        If UserAuthentication.User IsNot Nothing Then
            'set the user picture
            milesPic.ObjectID = MilesMetaObjects.Users
            milesPic.KeyID = UserAuthentication.User.UserID

            milesMobilePic.ObjectID = MilesMetaObjects.Users
            milesMobilePic.KeyID = UserAuthentication.User.UserID

            DirectCast(rdActions.Items(0), RadToolBarDropDown).Text = UserAuthentication.User.Fullname
            '  DirectCast(rdActionsMobile.Items(0), RadToolBarDropDown).Text = UserAuthentication.User.Fullname
        End If
    End Sub

    Private Sub BindMenu()

        Dim menuList = MetaMenuManager.GetMenuList()

        rdMenu.DataSource = menuList
        rdMenu.DataTextField = "Title"
        rdMenu.DataNavigateUrlField = "MenuPath"
        rdMenu.DataFieldID = "MenuID"
        rdMenu.DataFieldParentID = "ParentID"
        rdMenu.DataBind()
        'bind the mobile menu
        ltMenu.Text = MetaMenuManager.GetMobileMenuHTML
    End Sub

    Private Sub BindFavoritesIconBar(favList As DataLibrary.UserFavorites_GetMenuList_Result())
        Dim favlistIconBar = favList.Where(Function(x) x.ShowInIconBar = True)
        rdFavs.DataSource = favlistIconBar
        rdFavs.DataTextField = "DisplayName"
        rdFavs.DataNavigateUrlField = "MenuPath"
        rdFavs.DataFieldID = "FavoriteID"
        rdFavs.DataBind()
    End Sub

#End Region

#Region "Events"

    Protected Sub lnkLogout_Click(sender As Object, e As EventArgs) Handles lnkLogout.Click
        UserAuthentication.SignOut()
    End Sub

    Protected Sub rdActions_ButtonClick(sender As Object, e As RadToolBarEventArgs) Handles rdActions.ButtonClick
        If TypeOf e.Item Is RadToolBarButton Then
            Dim btn As RadToolBarButton = e.Item

            If btn.CommandName = "LogOut" Then
                UserAuthentication.SignOut()
            End If
        End If
    End Sub

    Protected Sub rdAjaxPanel_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxPanel.AjaxRequest
        If e.Argument = "UserInfo" Then
            'assign the new pictureid
            milesPic.ObjectID = MilesMetaObjects.Users
            milesPic.KeyID = UserAuthentication.User.UserID
        ElseIf e.Argument = "Favorites" Then

            BindFavorites()
        End If
    End Sub
#End Region

#Region "IEntityFooter"
    Public Function GetEntityFooter() As Label Implements IEntityFooter.GetEntityFooter
        Return lblEntityFooter
    End Function
#End Region

#Region "IPageHeader"
    ''' <summary>
    ''' Gets or sets the title of the page
    ''' </summary>
    ''' <remarks></remarks>
    Public Property PageHeader As String Implements IPageHeader.PageHeader
        Get
            Return lblPageTitle.Text
        End Get
        Set(value As String)
            lblPageTitle.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the application name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AppName As String Implements IPageHeader.AppName
        Get
            Return lblAppName.Text
        End Get
        Set(value As String)
            lblAppName.Text = value
        End Set
    End Property

#End Region

End Class

