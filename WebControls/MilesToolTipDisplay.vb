Imports BusinessLibrary
Imports CommonLibrary
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web
Imports System.IO
Imports Telerik.Web.UI

Public Class MilesToolTipDisplay
    Inherits Panel
    Implements INamingContainer

#Region "Properties"
    Private lnkToolTip As HyperLink

    Public WriteOnly Property ToolTipID As MilesMetaToolTips
        Set(value As MilesMetaToolTips)
            Dim _tooltipid As Integer = CInt(value)
            lnkToolTip.Attributes.Add("title", MetaToolTipManager.GetByID(_tooltipid).ToolTipDesc)
        End Set
    End Property
#End Region

#Region "Constructor"
    Public Sub New()
        lnkToolTip = New HyperLink
    End Sub
#End Region

#Region "Overrides"
    Protected Overrides Sub OnPreRender(e As EventArgs)
        Dim scriptID As String = Me.ClientID & "ScriptKey"
        'register the client script to change collapse icon on the header on click
        Dim js = <script>
                     $(document).ready(function () {
                            $('#<%= lnkToolTip.ClientID %>').tooltip({ placement: 'auto', html:'true', trigger:'hover' });
                        });                 
                 </script>

        'register the scripts with the script manager
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.Page.GetType(), scriptID, DecodeJS(js), True)

        MyBase.OnPreRender(e)
    End Sub
#End Region

    Protected Overrides Sub OnInit(e As EventArgs)
        lnkToolTip.ID = "lnkToolTip"
        lnkToolTip.NavigateUrl = "javascript:void(0)"
        lnkToolTip.Style.Add("text-decoration", "none")
        lnkToolTip.CssClass = "glyphicon glyphicon-info-sign"

        Me.Style.Add("display", "inline")
        Me.Controls.Add(lnkToolTip)
    End Sub

End Class
