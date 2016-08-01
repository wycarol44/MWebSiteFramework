
Partial Class Master_Global
    Inherits System.Web.UI.MasterPage



    Protected Sub Master_Global_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim js = <script>
                     setTimeout(function(){
                        ValidatorHighlight_Init();

                        miles.initValidationSummary();
                     });
                 </script>

        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.GetType(), "validator", DecodeJS(js), True)
    End Sub
End Class

