Imports Microsoft.VisualBasic


Public Class BasePage
    Inherits Page

#Region "Public Properties"

#End Region

#Region "Override Properties"
    Public Overrides Property Theme As String
        Get
            Return "Default"
        End Get
        Set(value As String)
            MyBase.Theme = value
        End Set
    End Property

#End Region

#Region "Override Methods"

    Protected Overrides Sub OnPreInit(e As EventArgs)
        MyBase.OnPreInit(e)
    End Sub

    Protected Overrides Sub OnInit(e As EventArgs)
        MyBase.OnInit(e)
    End Sub

    Protected Overrides Sub OnInitComplete(e As EventArgs)
        MyBase.OnInitComplete(e)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
    End Sub

    Protected Overrides Sub OnPreRenderComplete(e As EventArgs)

        'dispatch any delayed messages
        JGrowl.DispatchMessages()

        MyBase.OnPreRenderComplete(e)
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Closes the page if the page is a dialog window
    ''' </summary>
    ''' <param name="returnValue"></param>
    ''' <remarks></remarks>
    Public Sub CloseDialogWindow(Optional returnValue As String = Nothing)
        If returnValue IsNot Nothing Then
            'sanitize
            returnValue = returnValue.Replace("'", "\'")
        End If

        'create script
        Dim js = <script>

                     (function(){
                        var val = <%= If(Not String.IsNullOrWhiteSpace(returnValue), String.Format("'{0}'", returnValue), "null") %>;
                        //close window and pass value
                        closeWindow(val);
                        
                     })();


                 </script>

        'register script
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "closeDialog", DecodeJS(js), True)
    End Sub

    ''' <summary>
    ''' Navigates to another page
    ''' </summary>
    ''' <param name="url"></param>
    ''' <remarks></remarks>
    Public Sub Navigate(url As String)
        Response.Redirect(url)
    End Sub


#End Region

End Class
