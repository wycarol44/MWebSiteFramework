
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports Telerik.Web.UI

<ParseChildren(True)>
<ValidationProperty("Text")>
Public Class MilesPostalCode
    Inherits RadTextBox

#Region "Properties"


#End Region

    Public Sub New()
        Me.ClientEvents.OnLoad = "MilesPostalCode_Load"
        Me.ClientEvents.OnBlur = "MilesPostalCode_Blur"
    End Sub

#Region "Overrides"
    Protected Overrides Sub OnPreRender(e As EventArgs)

        'output some javascript
        Dim milesPostalCode = Page.ClientScript.GetWebResourceUrl(Me.GetType(), "MilesControls.MilesPostalCode.js")

        'link javascript
        ScriptManager.RegisterClientScriptInclude(Me.Page, Me.GetType(), "MilesPostalCode", milesPostalCode)

        MyBase.OnPreRender(e)
    End Sub

#End Region
End Class
