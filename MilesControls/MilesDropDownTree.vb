Imports Telerik.Web.UI
Imports System.Web.UI

Public Class MilesDropDownTree
    Inherits RadDropDownTree

#Region "Properties"
    Public Property AutoCollapseDropDown As Boolean
        Get
            Return ViewState("AutoCollapseDropDown")
        End Get
        Set(value As Boolean)
            ViewState("AutoCollapseDropDown") = value

        End Set
    End Property

#End Region

    Public Sub New()
        Me.OnClientDropDownOpening = "MilesDropDownTree_OnDropDownOpening"
       
    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        'can only be done after property is initialized
        If AutoCollapseDropDown Then
            Me.OnClientEntryAdded = "MilesDropDownTree_OnClientEntryAdded"
        End If

        'output some javascript
        Dim jsFile = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesDropDownTree.js")

        'link javascript
        ScriptManager.RegisterClientScriptInclude(Page, Page.GetType(), "MilesDropDownTree", jsFile)


    End Sub

End Class
