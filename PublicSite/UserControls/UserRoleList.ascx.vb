
Partial Class UserControls_UserRoleList
    Inherits System.Web.UI.UserControl

#Region "Properties"
    Public Property UserID As Integer
        Get
            Return ViewState("UserID")
        End Get
        Set(value As Integer)
            ViewState("UserID") = value
        End Set
    End Property

    Public Property Required As Boolean
        Get
            Return ViewState("Required")
        End Get
        Set(value As Boolean)
            ViewState("Required") = value
            requiredLabel.Visible = value
            cvUserRole.Enabled = value
        End Set

    End Property

    'selected list is saved in viewstate

    Public Property SelectedUserRole As List(Of UserRole)
        Get
            If ViewState("SelectedUserRole") Is Nothing Then
                ViewState("SelectedUserRole") = New List(Of UserRole)
            End If
            Return ViewState("SelectedUserRole")
        End Get
        Set(value As List(Of UserRole))
            ViewState("SelectedUserRole") = value
        End Set
    End Property
#End Region

    Protected Sub UserControls_UserRoleList_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ucToolTip.ToolTipID = MilesMetaToolTips.UserRoleDescription
            ' Populate()
        End If
    End Sub


    Private Sub BindGrid()
        dgSelectList.DataSource = Nothing
        dgSelectList.AutoBind = True
        dgSelectList.Rebind()
    End Sub
    ''' <summary>
    ''' Load and populate data
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Populate()

        'populate selected items
        BindGrid()
        'populate ddl excluding items selected
        Dim list = UserRoleManager.GetList().Where(Function(x) _
                                                Not (From ur In SelectedUserRole).Where(Function(y) _
                                                                                         y.UserRoleID = x.UserRoleID).Any)
        ControlBinding.BindListControl(ddlList, list, "UserRoleName", "UserRoleID", True)
    End Sub

    ''' <summary>
    ''' Save all selected roles at once
    ''' this function is used when saving userroles for new user (after retreiving userid from save function )
    ''' assign userid to this control before using this
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SaveRoles()
        If UserID > 0 Then
            UserManager.SaveRoles(UserID, SelectedUserRole)
        End If
    End Sub

#Region "Events"
    Protected Sub dgSelectList_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles dgSelectList.ItemCommand
        If e.CommandName = "Remove" Then
            Dim userroleid = DirectCast(e.Item, GridDataItem).GetDataKeyValue("UserRoleID")
            If UserID > 0 Then
                'selected item is removed instantly if existing user record is being modified
                UserManager.RemoveRole(UserID, userroleid)
                'notify user that the info was saved
                JGrowl.ShowMessage(JGrowlMessageType.Notification, "User role removed successfully")
            End If
            'remove from viewstate
            SelectedUserRole.RemoveAt(SelectedUserRole.FindIndex(Function(x) x.UserRoleID = userroleid))
            Populate()
            ddlList.Focus()
        End If
    End Sub

    Protected Sub dgSelectList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles dgSelectList.NeedDataSource

        'getlist from database for the first time and populate the viewstate
        If UserID > 0 And SelectedUserRole.Count < 1 Then
            Dim objlist As List(Of UserRole) = UserManager.GetUserRole(UserID).ToList()
            objlist.ForEach(Sub(x) SelectedUserRole.Add(New UserRole With {.UserRoleID = x.UserRoleID, .UserRoleName = x.UserRoleName}))
        End If

        If SelectedUserRole.Count = 0 Then dgSelectList.MasterTableView.NoMasterRecordsText = "<I>Select a User Role<I>"

        'assign datasource to viewstate list
        dgSelectList.DataSource = SelectedUserRole

        'if only one item exists in the list and required is true, hide the remove column
        If Required AndAlso SelectedUserRole IsNot Nothing AndAlso SelectedUserRole.Count = 1 Then
            dgSelectList.MasterTableView.Columns().FindByUniqueName("RemoveCol").Display = False
        Else 'otherwise show the column
            dgSelectList.MasterTableView.Columns().FindByUniqueName("RemoveCol").Display = True
        End If
    End Sub

    Protected Sub ddlList_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlList.SelectedIndexChanged
        If UserID > 0 AndAlso ddlList.SelectedValue > 0 Then
            'selected item is saved instantly if existing user record is being modified
            UserManager.SaveRole(UserID, ddlList.SelectedValue)

            'notify user that the info was saved
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="User Role")
        End If
        'add to viewstate
        SelectedUserRole.Add(New UserRole With {.UserRoleID = ddlList.SelectedValue.ToInteger(), .UserRoleName = ddlList.SelectedItem.Text})
        Populate()
        ddlList.Focus()
    End Sub
#End Region

End Class
