Imports System.Runtime.CompilerServices
Imports System.Web.UI

Public Module Extensions

    ''' <summary>
    ''' Converts a string to an integer, no matter what the string contains
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ToInteger(s As String) As Integer
        If IsNumeric(s) AndAlso Not Double.IsNaN(s) Then
            Dim d As Double = Convert.ToDouble(s)

            If d > Integer.MaxValue Then d = Integer.MaxValue
            If d < Integer.MinValue Then d = Integer.MinValue

            Return d
        Else
            Return 0
        End If
    End Function


    ''' <summary>
    ''' Converts a string to a decimal, no matter what the string contains
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ToDecimal(s As String) As Decimal
        If IsNumeric(s) AndAlso Not Double.IsNaN(s) Then
            Dim d As Double = Convert.ToDouble(s)

            If d > Decimal.MaxValue Then d = Decimal.MaxValue
            If d < Decimal.MinValue Then d = Decimal.MinValue

            Return d
        Else
            Return 0
        End If
    End Function

#Region "DateTime Extensions"

    ''' <summary>
    ''' Extension Method for DateTime.Now 
    ''' This will return the date of the supplied day based on the current date
    ''' Usage: Date/DateTime .Now.StartOfWeek(DayOfWeek Enum)
    ''' Return Value: Date of particular day within the current date's timeframe'
    ''' </summary>
    ''' <param name="currentDate"></param>
    ''' <param name="defineStartOfWeek"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()> _
    Public Function StartOfWeek(ByVal currentDate As DateTime, defineStartOfWeek As DayOfWeek)
        Dim diff As Integer = currentDate.DayOfWeek - defineStartOfWeek
        If (diff < 0) Then
            diff += 7
        End If

        Return currentDate.AddDays(-1 * diff).Date
    End Function

    ''' <summary>
    ''' Converts a datetime object to a string. If date is null, returns an empty string
    ''' </summary>
    ''' <param name="d"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ToFormattedShortDateString(d As Date) As String
        Return If(Not d = Nothing, d.ToString("MM/dd/yyyy"), "")
    End Function

    <Extension()>
    Public Function ToFormattedShortTimeString(d As Date) As String
        Return If(Not d = Nothing, d.ToString("hh:mm tt"), "")
    End Function

    <Extension()>
    Public Function ToFormattedString(d As Date) As String
        Return If(Not d = Nothing, d.ToString("MM/dd/yyyy hh:mm tt"), "")
    End Function

    Public Function ToFormattedShortDateString(d As Object) As String
        If IsDate(d) Then
            Dim dt As Date = Convert.ToDateTime(d)
            Return If(Not dt = Nothing, dt.ToString("MM/dd/yyyy"), "")
        Else
            Return ""
        End If
    End Function

#End Region

#Region "XML"

    ''' <summary>
    ''' Converts an Integer to an Identifiers xml
    ''' </summary>
    ''' <param name="source"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ToXMLIdentifiers(source As Integer) As XElement

        'add object to the xml
        Dim res = <list>
                      <id><%= source %></id>
                  </list>

        'return the xml
        Return res
    End Function

    ''' <summary>
    ''' Converts an IEnumerable to an Identifiers xml
    ''' </summary>
    ''' <param name="source"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ToXMLIdentifiers(source As IEnumerable) As XElement

        'add object to the xml
        Dim res = <list>
                      <%= From s In source
                          Select <id><%= s %></id> %>
                  </list>

        'return the xml
        Return res
    End Function

    ''' <summary>
    ''' Converts an IEnumerable to an Identifiers xml
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="selector"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ToXMLIdentifiers(Of TSource)(source As IEnumerable(Of TSource), selector As Func(Of TSource, Object)) As XElement

        'add object to the xml
        Dim res = <list>
                      <%= From s In source
                          Select <id><%= selector(s) %></id> %>
                  </list>

        'return the xml
        Return res
    End Function

    ''' <summary>
    ''' Converts an IEnumerable to an Identifiers xml
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="selector"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ToXMLIdentifiers(Of TSource)(source As IEnumerable, selector As Func(Of TSource, Object)) As XElement

        'add object to the xml
        Dim res = <list>
                      <%= From s In source
                          Select <id><%= selector(s) %></id> %>
                  </list>

        'return the xml
        Return res
    End Function

    ''' <summary>
    ''' Converts an IEnumerable to a KeyPairs xml
    ''' </summary>
    ''' <param name="source"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function ToXMLKeyPairs(Of T)(source As IEnumerable(Of T)) As XElement
        Dim res = <list></list>

        'select the source and the properties of source
        Dim props = (From s In source
                     Select Src = s, Properties = s.GetType().GetProperties())

        'for each element, get the source and its properties
        For Each prop In props
            If Not prop.Properties.Count = 2 Then
                Throw New Exception("Key pairs data source must have exactly two columns. Please select exactly two.")
            End If


            'create objects list for data row
            Dim objects As New List(Of Object)

            'add each property to a list (maximum of two properties)
            For Each propInfo In prop.Properties
                Dim obj = propInfo.GetValue(prop.Src, Nothing)
                'add the object to an array of objects to create a table row
                objects.Add(obj)
            Next

            'add the new row
            If objects.Count = 2 Then
                Dim pair = <pair>
                               <keyId1><%= objects(0) %></keyId1>
                               <keyId2><%= objects(1) %></keyId2>
                           </pair>

                'add to the xml
                res.Add(pair)

            End If
        Next


        'return the xml
        Return res
    End Function


#End Region

#Region "Controls"
    ''' <summary>
    ''' Recursively looks for controls in a collection of a specific type
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="c"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Extension()>
    Public Function FindControls(Of T)(c As Control, Optional ID As String = Nothing) As List(Of T)

        'list of all the matching controls
        Dim foundControls As New List(Of T)

        'look through the controls collection
        For Each ctrl As Control In c.Controls

            'is the control of the type we want?
            If (TypeOf ctrl Is T AndAlso ID Is Nothing) OrElse
                (ID IsNot Nothing AndAlso ctrl.ID = ID) Then
                'found the control, add it
                foundControls.Add(CTypeDynamic(Of T)(ctrl))
            End If

            'does the control have any controls of its own?
            foundControls.AddRange(FindControls(Of T)(ctrl, ID))

        Next

        'return the list
        Return foundControls

    End Function




#End Region

End Module
