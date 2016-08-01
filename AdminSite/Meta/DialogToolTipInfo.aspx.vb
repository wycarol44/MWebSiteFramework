
Partial Class Meta_DialogToolTipinfo
    Inherits BasePage

#Region "Properties"
    Protected Property ToolTipID As Integer
        Get
            Dim v As Object = ViewState("ToolTipID")
            If v Is Nothing Then
                v = ToInteger(Request("ToolTipID"))
                ViewState("ToolTipID") = v
            End If
            Return v
        End Get
        Set(value As Integer)
            ViewState("ToolTipID") = value
        End Set
    End Property
#End Region


    Protected Sub Meta_DialogToolTipinfo_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtToolTipDesc.SetEditorPaths()
            Populate()
        End If
    End Sub

#Region "Methods"
    Private Sub Populate()
        Dim tt As MetaToolTip = MetaToolTipManager.GetByID(ToolTipID)

        txtToolTipName.Text = tt.ToolTipName
        txtToolTipDesc.Content = tt.ToolTipDesc

    End Sub

    Private Function Save(Optional close As Boolean = False) As Boolean

        Dim tt As MetaToolTip = MetaToolTipManager.GetByID(ToolTipID)

        tt.ToolTipName = txtToolTipName.Text
        tt.ToolTipDesc = txtToolTipDesc.Content

        MetaToolTipManager.Save(tt)

        Dim delay As Boolean = Not TypeOf Me.Master Is IDialogMaster
        'show success
        JGrowl.ShowMessage(JGrowlMessageType.Success, objectName:="ToolTip", useParent:=close And Not delay, isDelayed:=close And delay)

        Return True
    End Function
#End Region

#Region "Events"
    Protected Sub btnSaveClose_Click(sender As Object, e As EventArgs) Handles btnSaveClose.Click
        If Save(True) Then
            CloseDialogWindow(ToolTipID)
        End If
    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        CloseDialogWindow()
    End Sub
#End Region

End Class
