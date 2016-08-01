
Partial Class Meta_DialogTypeInfo
    Inherits BasePage

#Region "Properties"
    Protected Property TypeID As Integer
        Get
            Dim v As Object = ViewState("TypeID")
            If v Is Nothing Then
                v = ToInteger(Request("TypeID"))
                ViewState("TypeID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("TypeID") = value
        End Set
    End Property

#End Region

    Protected Sub Meta_DialogTypeInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If TypeID > 0 Then
                Populate()
            End If
        End If
    End Sub


#Region "Methods"
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
        Dim mt As MetaType = MetaTypeManager.GetByID(TypeID)
        lblTypeName.Text = mt.TypeName

        'populate selected items
        BindGrid()
    End Sub

#End Region

#Region "Events"
    Protected Sub btnAddItem_Click(sender As Object, e As EventArgs) Handles btnAddItem.Click
        'selected item is saved instantly if existing category record is being modified
        Dim mti As MetaTypeItem = New MetaTypeItem With {.ItemID = 0,
                                                         .ItemName = txtNewItemName.Text,
                                                         .TypeID = TypeID}
        If TypeID > 0 Then
            'save directly if its not a new type
            MetaTypeItemManager.Save(mti)

            'notify user that the info was saved
            JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Item")
        End If
        BindGrid()
        txtNewItemName.Text = ""
        txtNewItemName.Focus()
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub

#Region "grid"
    Protected Sub dgSelectList_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles dgSelectList.ItemCommand
        Dim itemId = DirectCast(e.Item, GridDataItem).GetDataKeyValue("ItemID")
        If e.CommandName = "Remove" Then
            If TypeID > 0 Then
                'selected item is removed instantly if existing category record is being modified
                MetaTypeItemManager.Delete(itemId)
                'notify user that the info was saved
                JGrowl.ShowMessage(JGrowlMessageType.Notification, "Item removed successfully")
            End If
        ElseIf e.CommandName = "Update" Then
            Dim mti As MetaTypeItem = MetaTypeItemManager.GetById(itemId)
            Dim txtItemName As TextBox = e.Item.FindControl("txtItemName")
            If txtItemName IsNot Nothing Then
                mti.ItemName = txtItemName.Text
                MetaTypeItemManager.Save(mti)
                e.Item.Edit = False
                JGrowl.ShowMessage(JGrowlMessageType.Success, "Item saved successfully")
            End If
        End If
        BindGrid()
        txtNewItemName.Focus()
    End Sub

    Protected Sub dgSelectList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles dgSelectList.NeedDataSource
        Dim objlist As List(Of MetaTypeItem) = MetaTypeItemManager.GetList(TypeID).ToList()
        dgSelectList.DataSource = objlist

        If Not objlist.Any() Then dgSelectList.MasterTableView.NoMasterRecordsText = "<I>Add an Item</I>"

    End Sub
#End Region

#End Region
End Class
