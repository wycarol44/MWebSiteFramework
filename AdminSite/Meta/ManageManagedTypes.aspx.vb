﻿
Partial Class Meta_ManageManagedTypes
    Inherits BasePage

    Protected Sub Meta_ManageManagedTypes_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtType.Focus()
        End If
    End Sub


#Region "Methods"

    Private Sub BindList()
        clTypes.DataSource = Nothing
        clTypes.AutoBind = True
        clTypes.Rebind()
    End Sub
#End Region

#Region "Grid"

    Protected Sub clJobTitle_ItemCommand(sender As Object, e As RadListViewCommandEventArgs) Handles clTypes.ItemCommand
        If e.CommandName = "PerformInsert" Then 'RadListView.PerformInsertCommandName
            If TypeOf e.ListViewItem Is RadListViewInsertItem Then
                InsertUpdate(e, 0)
            ElseIf TypeOf e.ListViewItem Is RadListViewEditableItem Then
                Dim item As RadListViewDataItem = e.ListViewItem
                Dim keyID = item.GetDataKeyValue("TypeID")
                InsertUpdate(e, keyID)
            End If

        End If
    End Sub

    Protected Sub clJobTitle_ItemCreated(sender As Object, e As RadListViewItemEventArgs) Handles clTypes.ItemCreated
        If e.Item.IsInEditMode Then
            Dim txtFriendlyName As TextBox = e.Item.FindControl("txtFriendlyName")
            If txtFriendlyName IsNot Nothing Then
                txtFriendlyName.Focus()
            End If
        End If
    End Sub

    Protected Sub clTypes_ItemDataBound(sender As Object, e As RadListViewItemEventArgs) Handles clTypes.ItemDataBound
        If TypeOf e.Item Is RadListViewDataItem And e.Item.IsInEditMode = False Then
        
        End If
    End Sub

    Protected Sub clTypes_NeedDataSource(sender As Object, e As RadListViewNeedDataSourceEventArgs) Handles clTypes.NeedDataSource
        Dim objlist = ManagedTypeManager.GetList(ToNull(txtType.Text))
        clTypes.DataSource = objlist
    End Sub

    Protected Sub InsertUpdate(ByVal e As Telerik.Web.UI.RadListViewCommandEventArgs, ByVal KeyID As Integer)
        If TypeOf e.ListViewItem Is RadListViewEditableItem Or TypeOf e.ListViewItem Is RadListViewInsertItem Then
            'get the edit item
            Dim editedItem As Telerik.Web.UI.RadListViewEditableItem = CType(e.ListViewItem, Telerik.Web.UI.RadListViewEditableItem)
            'Get the edit fields
            Dim txtfriendlyname As TextBox = editedItem.FindControl("txtFriendlyName")
            Dim txttypename As TextBox = editedItem.FindControl("txtTypeName")
            Dim chkIsHierarchy As CheckBox = editedItem.FindControl("chkIsHierarchy")

            Dim mt = ManagedTypeManager.GetByID(KeyID)
            mt.TypeName = txtTypeName.Text
            mt.FriendlyName = txtfriendlyname.Text
            mt.IsHierarchy = chkIsHierarchy.Checked

            ManagedTypeManager.Save(mt)

            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Managed Type")

            clTypes.Rebind()
        End If
    End Sub

#End Region

#Region "Events"
    Protected Sub pnlSearch_Search(sender As Object, e As EventArgs) Handles pnlSearch.Search
        BindList()
    End Sub

    Protected Sub rdAjaxManager_AjaxRequest(sender As Object, e As AjaxRequestEventArgs) Handles rdAjaxManager.AjaxRequest
        BindList()
    End Sub
#End Region
End Class