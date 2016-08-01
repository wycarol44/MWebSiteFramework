
Partial Class Admin_Utility
    Inherits BasePage

#Region "Methods"
    Protected Sub btnCleanDeletedForms_Click(sender As Object, e As EventArgs) Handles btnCleanDeletedForms.Click
        MetaFormManager.CleanDeletedForms()

        JGrowl.ShowMessage(JGrowlMessageType.Notification, "Cleaned deleted forms")

    End Sub

    Protected Sub btnClearCache_Click(sender As Object, e As EventArgs) Handles btnClearCache.Click

        Dim webCache As Cache = System.Web.HttpRuntime.Cache
        Dim CacheEnum As IDictionaryEnumerator = webCache.GetEnumerator()

        While CacheEnum.MoveNext()

            webCache.Remove(CacheEnum.Key)



        End While

        JGrowl.ShowMessage(JGrowlMessageType.Success, "All cache has been cleared.")
    End Sub

#End Region
    
   
End Class
