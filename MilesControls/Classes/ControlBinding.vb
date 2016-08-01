Imports System.Web.UI.WebControls
Imports Telerik.Web.UI

Public Class ControlBinding

#Region "List Controls"
    ''' <summary>
    ''' Binds a list control to a data source and optionally adds a default item
    ''' </summary>
    ''' <param name="list"></param>
    ''' <param name="source"></param>
    ''' <param name="dataTextField"></param>
    ''' <param name="dataValueField"></param>
    ''' <param name="defaultItem"></param>
    ''' <param name="defaultText"></param>
    ''' <param name="defaultValue"></param>
    ''' <remarks></remarks>
    Public Shared Sub BindListControl(list As ListControl, source As Object, dataTextField As String, dataValueField As String, Optional defaultItem As Boolean = False, Optional defaultText As String = "< Choose >", Optional defaultValue As String = "")
        'set datasource
        list.DataSource = source
        list.DataTextField = dataTextField
        list.DataValueField = dataValueField
        'bind
        list.DataBind()

        'add default option if enabled
        If defaultItem Then

            Dim item As New ListItem(defaultText, defaultValue)
            list.Items.Insert(0, item)

        End If


    End Sub

    Public Shared Sub BindListControl(list As RadComboBox, source As Object, dataTextField As String, dataValueField As String, Optional defaultItem As Boolean = False, Optional defaultText As String = "< Choose >", Optional defaultValue As String = "")
        'set datasource
        list.DataSource = source
        list.DataTextField = dataTextField
        list.DataValueField = dataValueField
        'bind
        list.DataBind()

        'add default option if enabled
        If defaultItem Then

            Dim item As New RadComboBoxItem(defaultText, defaultValue)
            list.Items.Insert(0, item)

        End If


    End Sub

#End Region

End Class
