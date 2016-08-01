Imports System.Runtime.CompilerServices
Imports System.Web.UI.WebControls

Public Module Extensions

    ''' <summary>
    ''' Takes the checked items of a radcombobox and turns them into an xml identifiers object
    ''' </summary>
    ''' <param name="source"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ToXMLIdentifiers(source As Telerik.Web.UI.RadComboBox, Optional searchMode As Boolean = True) As XElement

        'add object to the xml
        'in search panel, when all is checked then it means all , so when not in search mode and when all are not selected , build list
        If Not searchMode OrElse source.Items.Count <> source.CheckedItems.Count Then

            Dim res = <list>
                          <%= From s In source.CheckedItems.Select(Function(n) n.Value)
                       Select <id><%= s %></id> %>
                      </list>

            'return the xml
            Return res

        Else

            Dim res = <list>
                      </list>

            Return res

        End If

    End Function

    ''' <summary>
    ''' Selects a value from the list if it exists
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    <Extension()>
    Public Sub SelectedValue(source As DropDownList, value As Integer?)
        If value.HasValue Then
            Dim item = source.Items.FindByValue(value.ToString())
            If item IsNot Nothing Then
                source.SelectedValue = value.ToString()
            End If
        End If
    End Sub

End Module
