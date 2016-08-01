
Partial Class Admin_DialogManageFavorites
    Inherits BasePage

#Region "Properties"
    Protected Property MetaFormID As Integer
        Get
            Dim v As Object = ViewState("MetaFormID")
            If v Is Nothing Then
                v = ToInteger(Request("MetaFormID"))
                ViewState("MetaFormID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("MetaFormID") = value
        End Set
    End Property
    Protected Property FavoriteID As Integer
        Get
            Return ToInteger(ViewState("FavoriteID"))
            
        End Get
        Set(value As Integer)
            ViewState("FavoriteID") = value
        End Set
    End Property
#End Region

    Protected Sub Admin_DialogManageFavorites_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If MetaFormID > 0 Then
                'show the info panel
                PopulateInfo()
            Else
                Saved = False
                'show the grid
                BindGrid()
            End If
        End If
    End Sub

    Private Sub PopulateInfo()
        pnlFavoriteInfo.Visible = True
        pnlManageFavorites.Visible = False
        btnSaveClose.Visible = True

        If FavoriteID > 0 Then
            Dim obj = UserFavoriteManager.GetById(FavoriteID)
            txtFavorite.Text = obj.DisplayName
            chkDisplaySettings.Items.FindByValue("ShowInIconBar").Selected = obj.ShowInIconBar
            If obj.ShowInIconBar Then
                pnlIconColor.Visible = True
                rdIconColorPicker.SelectedColor = System.Drawing.ColorTranslator.FromHtml(obj.IconColor)
            End If
            chkDisplaySettings.Items.FindByValue("ShowInFavoriteMenu").Selected = obj.ShowInFavoritesMenu
            chkLandingPage.Checked = obj.IsLandingPage
        Else
            txtFavorite.Text = MetaFormManager.GetById(MetaFormID).FormName
        End If

    End Sub

    Private Function Save(Optional close As Boolean = False) As Boolean
        Dim obj = UserFavoriteManager.GetById(FavoriteID)
        If MetaFormID > 0 Then obj.FormID = MetaFormID
        obj.UserID = UserAuthentication.User.UserID
        obj.DisplayName = txtFavorite.Text
        obj.ShowInIconBar = chkDisplaySettings.Items.FindByValue("ShowInIconBar").Selected
        If obj.ShowInIconBar Then
            obj.IconColor = System.Drawing.ColorTranslator.ToHtml(rdIconColorPicker.SelectedColor)
        Else
            If obj.IconColor Is Nothing Then obj.IconColor = "#FFFFFF" 'default to White, if 'show in iconbar' is not selected
        End If
        obj.ShowInFavoritesMenu = chkDisplaySettings.Items.FindByValue("ShowInFavoriteMenu").Selected
        obj.IsLandingPage = chkLandingPage.Checked
        UserFavoriteManager.Save(obj)

        'indicate the record is saved, so the menu can be refreshed
        Saved = True

        Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Favorite", useParent:=close And Not delay, isDelayed:=close And delay)

        Return True
    End Function

    Private Sub BindGrid()
        pnlFavoriteInfo.Visible = False
        pnlManageFavorites.Visible = True
        btnSaveClose.Visible = False

        FavoriteID = 0

        clFavorites.DataSource = Nothing
        clFavorites.AutoBind = True
        clFavorites.Rebind()
    End Sub

    Protected Sub chkDisplaySettings_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkDisplaySettings.SelectedIndexChanged
        If chkDisplaySettings.Items.FindByValue("ShowInIconBar").Selected Then
            pnlIconColor.Visible = True
        Else
            pnlIconColor.Visible = False
        End If
    End Sub

#Region "Events"
    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            If FavoriteID > 0 Then
                'go back to grid
                BindGrid()
            Else
                'close the window
                CloseDialogWindow(Saved)
            End If
        End If
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        If FavoriteID > 0 Then
            BindGrid()
        Else
            CloseDialogWindow(Saved)
        End If
    End Sub
#End Region

#Region "Grid"
    Protected Sub clFavorites_ItemCommand(sender As Object, e As Telerik.Web.UI.RadListViewCommandEventArgs) Handles clFavorites.ItemCommand
        Dim item As RadListViewDataItem = e.ListViewItem
        Dim keyId As Integer = item.GetDataKeyValue("FavoriteID")

        If e.CommandName = "ViewFavorite" Then
            FavoriteID = keyId
            PopulateInfo()
            Exit Sub
        ElseIf e.CommandName = "Remove" Then
            UserFavoriteManager.RemoveFavorite(keyId)

            BindGrid()
            Saved = True
        End If
    End Sub

    Protected Sub clFavorites_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clFavorites.ItemDataBound
        If TypeOf e.Item Is Telerik.Web.UI.RadListViewDataItem Then
            Dim item As RadListViewDataItem = e.Item
            Dim q As UserFavorites_GetMenuList_Result = item.DataItem
            Dim lblVisibility As Label = item.FindControl("lblVisibility")

            If q.ShowInIconBar And q.ShowInFavoritesMenu Then
                lblVisibility.Text = "Visibility: Icon Bar, Favorites Menu"
            ElseIf q.ShowInFavoritesMenu Then
                lblVisibility.Text = "Visibility: Favorites Menu"
            ElseIf q.ShowInIconBar Then
                lblVisibility.Text = "Visibility: Icon Bar"
            End If
        End If

    End Sub

    Protected Sub clFavorites_ItemDrop(sender As Object, e As RadListViewItemDragDropEventArgs) Handles clFavorites.ItemDrop
        Dim a = e.DestinationHtmlElement

        Dim lvi As Telerik.Web.UI.RadListViewDataItem = Nothing

        lvi = clFavorites.FindControl(e.DestinationHtmlElement)

        'check if any dataitem is found
        If lvi IsNot Nothing Then

            Dim favList = UserFavoriteManager.GetList(UserAuthentication.User.UserID).ToList()
            'dragged item
            Dim draggeditem = favList.Where(Function(x) x.FavoriteID = e.DraggedItem.GetDataKeyValue("FavoriteID")).FirstOrDefault
            'destination item
            Dim destinationitem = favList.Where(Function(x) x.FavoriteID = lvi.GetDataKeyValue("FavoriteID")).FirstOrDefault

            'extract the destination item index before removing the dragged item
            Dim destinationitemindex As Integer = favList.IndexOf(destinationitem)

            'reorder
            'remove draggeditem from its position
            favList.Remove(draggeditem)
            'and insert where the destination item was
            favList.Insert(destinationitemindex, draggeditem)

            UserFavoriteManager.UpdateSortOrder(favList)

            clFavorites.Rebind()
            Saved = True

        End If
    End Sub

    Protected Sub clFavorites_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clFavorites.NeedDataSource
        clFavorites.DataSource = UserFavoriteManager.GetList(UserAuthentication.User.UserID)

    End Sub
#End Region


End Class
