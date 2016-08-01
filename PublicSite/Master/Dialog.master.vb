
Partial Class Master_Dialog
    Inherits System.Web.UI.MasterPage
    Implements IEntityFooter
    Implements IDialogMaster


    Protected Sub Master_Dialog_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender

        Dim js = <script>
                     setTimeout(function(){miles.adjustDialogContentHeight();}, 0);
                 </script>

        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "sizeContent", DecodeJS(js), True)

    End Sub

#Region "IEntityFooter"
    Public Function GetEntityFooter() As Label Implements IEntityFooter.GetEntityFooter
        Return lblEntityFooter
    End Function
#End Region

   
End Class

