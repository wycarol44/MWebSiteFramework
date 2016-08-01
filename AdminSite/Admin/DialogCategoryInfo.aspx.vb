
Partial Class Admin_DialogCategoryInfo
    Inherits BasePage

#Region "Properties"

    Protected Property CategoryID As Integer
        Get
            Dim v As Object = ViewState("CategoryID")
            If v Is Nothing Then
                v = ToInteger(Request("CategoryID"))
                ViewState("CategoryID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("CategoryID") = value
        End Set
    End Property

    Protected Property ParentID As Integer
        Get
            Dim v As Object = ViewState("ParentID")
            If v Is Nothing Then
                v = ToInteger(Request("ParentID"))
                ViewState("ParentID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("ParentID") = value
        End Set
    End Property
#End Region

    Protected Sub Meta_DialogMenuInfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Populate(CategoryID, ParentID)
        End If
    End Sub

#Region "Methods"

    Private Sub Populate(CategoryID As Integer, ParentID As Integer)
        Dim m = CategoryMenuManager.GetById(CategoryID)
        txtCategory.Text = m.CategoryName
        txtDescription.Text = m.Description
    End Sub

    Private Function Save(Optional ByVal close As Boolean = False) As Boolean
        Dim m = CategoryMenuManager.GetById(CategoryID)

        m.CategoryName = txtCategory.Text
        m.Description = txtDescription.Text
        m.Archived = False

        If CategoryID = 0 And ParentID > 0 Then 'new record and has parentid
            m.ParentID = ParentID
        End If

        CategoryID = CategoryMenuManager.Save(m)
        Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="Manu Item", useParent:=close And Not delay, isDelayed:=close And delay)

        Return True
    End Function

#End Region

#Region "Events"

    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            CloseDialogWindow(CategoryID)
        End If

    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub

#End Region


End Class
