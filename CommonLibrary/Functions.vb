Imports System.Globalization
Imports System.Web
Imports System.IO
Imports System.Text
Imports System.Web.UI
Imports System.Reflection
Imports System.Web.UI.WebControls


Public Module Functions

    Public Enum PhoneNumberType
        Work = 0
        Home = 1
        Mobile = 2
    End Enum


#Region "Image Functions"

    ''' <summary>
    ''' Resizes an image
    ''' </summary>
    ''' <param name="imageStream"></param>
    ''' <param name="maxWidth"></param>
    ''' <param name="maxHeight"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ResizeImage(imageStream As Stream, maxWidth As Integer, Optional maxHeight As Integer = 0) As Byte()
        Dim newwidth As Integer
        Dim origwidth As Integer
        Dim newheight As Integer
        Dim origheight As Integer
        Dim scalefactor As Decimal

        Using origImage = System.Drawing.Image.FromStream(imageStream)
            origwidth = origImage.Width
            origheight = origImage.Height

            If origwidth <= maxWidth Then
                newwidth = CInt((origwidth * 1))
                newheight = CInt((origheight * 1))
            Else
                newwidth = maxWidth
                scalefactor = newwidth / origwidth
                newheight = CInt((origImage.Height * scalefactor))
            End If

            If maxHeight > 0 Then

                If newheight >= maxHeight Then
                    scalefactor = maxHeight / newheight
                    newheight = maxHeight
                    newwidth = CInt((newwidth * scalefactor))

                End If

            End If


            Using thumbnailBitmap = New System.Drawing.Bitmap(newwidth, newheight),
                thumbnailGraph = System.Drawing.Graphics.FromImage(thumbnailBitmap)

                thumbnailGraph.CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality
                thumbnailGraph.SmoothingMode = Drawing.Drawing2D.SmoothingMode.HighQuality
                thumbnailGraph.InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic

                Dim imageRectangle = New Drawing.Rectangle(0, 0, newwidth, newheight)
                thumbnailGraph.DrawImage(origImage, imageRectangle)

                Using ms As New MemoryStream()
                    thumbnailBitmap.Save(ms, origImage.RawFormat)
                    Return ms.GetBuffer()
                End Using


            End Using


        End Using
    End Function

#End Region

#Region "File Functions"

    Public Function GetDocIcon(ByVal Ext As String) As String
        Dim IconUrl As String = ""
        Select Case Ext.ToLower
            Case ".doc", ".docx"
                IconUrl = "/images/fileicons/doc.png"
            Case ".xls", ".xlsx"
                IconUrl = "/images/fileicons/xls.png"
            Case ".pdf"
                IconUrl = "/images/fileicons/pdf.png"
            Case ".txt"
                IconUrl = "/images/fileicons/txt.png"
            Case ".ppt", ".pptx"
                IconUrl = "/images/fileicons/ppt.png"
            Case ".gif"
                IconUrl = "/images/fileicons/gif.png"
            Case ".jpg", ".jpeg", ".png"
                IconUrl = "/images/fileicons/jpg.png"
            Case ".bmp"
                IconUrl = "/images/fileicons/bmp.png"
            Case ".wav"
                IconUrl = "/images/fileicons/wav.png"
            Case ".tif"
                IconUrl = "/images/fileicons/tif.png"
            Case ".mp3"
                IconUrl = "/images/fileicons/mp3.png"
            Case Else
                IconUrl = "/images/fileicons/txt.png"
        End Select
        Return IconUrl
    End Function

    Public Function GetDocIcon64(ByVal Ext As String) As String
        Dim IconUrl As String = ""
        Select Case Ext.ToLower
            Case ".doc", ".docx"
                IconUrl = "/images/fileicons_64/word-64.png"
            Case ".xls", ".xlsx"
                IconUrl = "/images/fileicons_64/xls-64.png"
            Case ".pdf"
                IconUrl = "/images/fileicons_64/pdf-64.png"
            Case ".txt"
                IconUrl = "/images/fileicons_64/txt-64.png"
            Case ".ppt", ".pptx"
                IconUrl = "/images/fileicons_64/ppt-64.png"
            Case ".wav"
                IconUrl = "/images/fileicons_64/wav-64.png"
            Case ".mp3"
                IconUrl = "/images/fileicons_64/mp3-64.png"
            Case Else
                IconUrl = "/images/fileicons_64/plain.png"
        End Select
        Return IconUrl
    End Function

    Public Sub FlushFile(ByVal fileName As String, ByVal fullFileName As String, Optional ByVal BufferSize As Integer = 10000)
        Dim contentType As String = ""
        Dim iStream As System.IO.Stream = Nothing
        Dim buffer(BufferSize) As Byte
        Dim dataToRead As Long
        Dim length As Integer

        'clean filename ( if it is comma separated, some browser doesnt like it)
        fileName = Replace(fileName, ",", "")

        ' Determine Content Type
        Select Case System.IO.Path.GetExtension(fileName.ToLower())
            Case ".htm", ".html"
                contentType = "text/HTML"
            Case ".txt"
                contentType = "text/plain"
            Case ".doc", ".rtf"
                contentType = "application/msword"
            Case ".csv", ".xls"
                contentType = "application/x-msexcel"
            Case Else
                contentType = "application/octet-stream"
        End Select


        Try
            ' Open the file.
            iStream = New System.IO.FileStream(fullFileName, System.IO.FileMode.Open, _
                                                   IO.FileAccess.Read, IO.FileShare.Read)
            dataToRead = iStream.Length

            Dim currentContext As System.Web.HttpContext = System.Web.HttpContext.Current
            If Not currentContext Is Nothing Then

                currentContext.Response.ClearHeaders()
                currentContext.Response.ClearContent()
                currentContext.Response.ContentType = contentType
                currentContext.Response.AddHeader("content-disposition", "attachment; filename=" & fileName)

                While dataToRead > 0
                    ' Verify that the client is connected.

                    ' Read the data in buffer
                    length = iStream.Read(buffer, 0, BufferSize)

                    ' Write the data to the current output stream.
                    currentContext.Response.OutputStream.Write(buffer, 0, length)

                    ' Flush the data to the HTML output.
                    currentContext.Response.Flush()

                    ReDim buffer(BufferSize) ' Clear the buffer
                    dataToRead = dataToRead - length
                End While


                ' Get Current Context
                currentContext.Response.End()
            End If
        Catch ex As Exception
        Finally
            If IsNothing(iStream) = False Then
                ' Close the file.
                iStream.Close()
            End If

        End Try

    End Sub


#End Region

#Region "Conversion Functions"

    ''' <summary>
    ''' Checks if object is empty and return nothing or the object
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ToNull(s As Object) As Object
        If TypeOf s Is String Then
            Return If(Not String.IsNullOrWhiteSpace(DirectCast(s, String)), s, Nothing)
        ElseIf TypeOf s Is XElement Then
            Return If(s IsNot Nothing, s.ToString(), Nothing)
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Converts a string to an integer?, no matter what the string contains
    ''' </summary>
    ''' <param name="s"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ToNullableInteger(s As String) As Integer?

        If String.IsNullOrWhiteSpace(s) Then
            Return Nothing
        ElseIf IsNumeric(s) AndAlso Not Double.IsNaN(s) Then
            Dim d As Double = Convert.ToDouble(s)

            If d > Integer.MaxValue Then d = Integer.MaxValue
            If d < Integer.MinValue Then d = Integer.MinValue

            Return d
        Else
            Return Nothing
        End If

    End Function

    ''' <summary>
    ''' Converts a negative or zero value to null
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function PositiveOrNull(value As Integer) As Integer?
        Return If(value > 0, value, Nothing)
    End Function

    ''' <summary>
    ''' Converts a true value to Nothing, indicating all inclusive. A false value will return false
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ToNegBool(value As Boolean) As Boolean?
        Return If(value, Nothing, CObj(False))
    End Function

    ''' <summary>
    ''' Converts a false value to nothing, indicating all inclusive. A true value will return true
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ToPosBool(value As Boolean) As Boolean?
        Return If(value, CObj(True), Nothing)
    End Function

    ''' <summary>
    ''' Converts a hex string to an array of bytes
    ''' </summary>
    ''' <param name="hex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HexToByteArray(hex As String) As Byte()
        'must be in multiples of 2
        If (Not hex.Length Mod 2 = 0) Then
            Throw New ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hex))
        End If

        'create byte array
        Dim hexBytes((hex.Length / 2) - 1) As Byte

        'loop through each byte, and get the corresponding hex value from the string
        For x As Integer = 0 To hexBytes.Length - 1
            'get value from string
            Dim byteValue = hex.Substring(x * 2, 2)
            'parse hex value to a byte
            hexBytes(x) = Byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture)
        Next

        Return hexBytes
    End Function


    Public Function ByteArrayToHexString(ba As Byte()) As String
        Dim hex As New StringBuilder(ba.Length * 2)
        For Each b As Byte In ba
            hex.AppendFormat("{0:x2}", b)
        Next
        Return hex.ToString()
    End Function

    ''' <summary>
    ''' HTML decodes a javascript xml literal
    ''' </summary>
    ''' <param name="js"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DecodeJS(js As XElement) As String
        Return HttpUtility.HtmlDecode(js.Value)
    End Function

#End Region

#Region "Formatting Functions"
    ''' <summary>
    ''' format the navigate url of the phone to have a tel link
    ''' </summary>
    ''' <param name="phone"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FormatPhoneLink(phone As String) As String
        'check to see if we have a phone number to format
        If String.IsNullOrWhiteSpace(phone) Then
            Return String.Empty
        Else
            Return "tel:" + phone
        End If
    End Function

    ''' <summary>
    ''' Formats a phone number
    ''' </summary>
    ''' <param name="phone"></param>
    ''' <param name="ext"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function FormatPhone(phoneType As PhoneNumberType, phone As String, Optional ext As String = Nothing) As String
        Dim formattedPhone As String = String.Empty

        'check to see if we have a phone number to format
        If String.IsNullOrWhiteSpace(phone) Then Return formattedPhone

        'if the number has ten digits, format as a phone with area code
        If phone.Length = 10 Then
            'format with area code
            formattedPhone = String.Format("({0}) {1}-{2}",
                                           phone.Substring(0, 3),
                                           phone.Substring(3, 3),
                                           phone.Substring(6, 4)
                                          )

        ElseIf phone.Length = 7 Then
            'format witout area code
            formattedPhone = String.Format("{0}-{1}",
                                           phone.Substring(0, 3),
                                           phone.Substring(3, 4)
                                          )
        Else
            formattedPhone = phone
        End If

        'if there is an extension, then add it to the phone number

        Dim phoneTypeValue As String = String.Empty
        Select Case phoneType
            Case PhoneNumberType.Work
                phoneTypeValue = "W"

                'add the extension, if it exists
                If Not String.IsNullOrWhiteSpace(ext) Then
                    formattedPhone += " x" + ext
                End If

            Case PhoneNumberType.Home
                phoneTypeValue = "H"
            Case PhoneNumberType.Mobile
                phoneTypeValue = "M"
        End Select

        'add the prepend
        formattedPhone = phoneTypeValue + " " + formattedPhone

        Return formattedPhone

    End Function

    Public Function FormatAddress(address1 As String, address2 As String, city As String, state As String, zip As String, country As String) As String
        Dim FullAddress As New StringBuilder

        If Not String.IsNullOrEmpty(address1) Then FullAddress.Append(address1)
        If Not String.IsNullOrEmpty(address2) Then
            If Not String.IsNullOrEmpty(address1) Then FullAddress.Append(",")
            FullAddress.Append(address2)
        End If
        If Not String.IsNullOrEmpty(city) Then FullAddress.Append("<br />" & city)
        If Not String.IsNullOrEmpty(state) Then
            If String.IsNullOrEmpty(city) Then
                FullAddress.Append("<br />")
            Else
                FullAddress.Append(", ")
            End If
            FullAddress.Append(state)
        End If
        If Not String.IsNullOrEmpty(zip) Then FullAddress.Append(" " & zip)
        If Not String.IsNullOrEmpty(country) Then FullAddress.Append("<br />" & country)

        Return FullAddress.ToString()
    End Function

#End Region

#Region "Email Functions"
    Public Function IsEmail(ByVal str As String) As Boolean
        Dim emailValidationString As String = "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
        If System.Text.RegularExpressions.Regex.IsMatch(str, emailValidationString) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Sends an email
    ''' </summary>
    ''' <param name="emailFrom"></param>
    ''' <param name="emailTo"></param>
    ''' <param name="emailSubject"></param>
    ''' <param name="emailBody"></param>
    ''' <param name="emailCC"></param>
    ''' <param name="emailBCC"></param>
    ''' <param name="emailFromName"></param>
    ''' <param name="isHtml"></param>
    ''' <param name="attachments"></param>
    ''' <remarks></remarks>
    Public Sub SendEmail(ByVal emailFrom As String, ByVal emailTo As String, ByVal emailSubject As String, ByVal emailBody As String, Optional emailCC As String = Nothing, Optional emailBCC As String = Nothing, Optional emailFromName As String = Nothing, Optional ByVal isHtml As Boolean = True, Optional ByVal attachments As ArrayList = Nothing)

        Using m As New System.Net.Mail.MailMessage


            Dim smtp As New System.Net.Mail.SmtpClient

            If AppSettings.EmailTestMode Then
                emailTo = AppSettings.EmailTestModeSendTo
            End If

            m.Subject = emailSubject
            m.Body = emailBody

            If emailFromName = Nothing Then
                m.From = New System.Net.Mail.MailAddress(emailFrom)
            Else
                m.From = New System.Net.Mail.MailAddress(emailFrom, emailFromName)
            End If
            m.To.Add(emailTo)
            If Not AppSettings.EmailTestMode Then
                If emailCC IsNot Nothing Then m.CC.Add(emailCC)
                If emailBCC IsNot Nothing Then m.Bcc.Add(emailBCC)
            End If
            'attachments
            If attachments IsNot Nothing Then
                For Each a As System.Net.Mail.Attachment In attachments
                    m.Attachments.Add(a)
                Next
            End If
            m.IsBodyHtml = isHtml
            smtp.Send(m)
        End Using


    End Sub
#End Region

#Region "Export to Excel Functions"

    ''' <summary>
    ''' Exports a list of objects to excel
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="objList"></param>
    ''' <param name="colsToInclude"></param>
    ''' <param name="colHeaderNames"></param>
    ''' <param name="filename"></param>
    ''' <remarks></remarks>
    Public Sub ExportToExcel(Of T)(objList As List(Of T), colsToInclude As String(), colHeaderNames As String(), Optional filename As String = "Export")
        'The Clear method erases any buffered HTML output.
        HttpContext.Current.Response.Clear()
        'The AddHeader method adds a new HTML header and value to the response sent to the client.
        HttpContext.Current.Response.AddHeader("content-disposition", String.Format("attachment; filename={0}.xls", filename))
        'The ContentType property specifies the HTTP content type for the response.
        HttpContext.Current.Response.ContentType = "application/ms-excel"
        'Implements a TextWriter for writing information to a string. The information is stored in an underlying StringBuilder.
        Using sw As New StringWriter()
            'Writes markup characters and text to an ASP.NET server control output stream. This class provides formatting capabilities that ASP.NET server controls use when rendering markup to clients.
            Using htw As New HtmlTextWriter(sw)
                Dim propertyInfos As PropertyInfo() = GetType(T).GetProperties()
                Dim tbl As New Table()
                tbl.GridLines = GridLines.Both
                GetExcelHeaders(propertyInfos, colsToInclude, colHeaderNames, tbl)
                objList.ForEach(Sub(d) GetExcelData(d, propertyInfos, colsToInclude, tbl))
                tbl.RenderControl(htw)
                '  render the htmlwriter into the response
                HttpContext.Current.Response.Write(sw.ToString())
                HttpContext.Current.Response.End()

            End Using
        End Using
    End Sub

    Private Sub GetExcelHeaders(propertyInfos As PropertyInfo(), colsToInclude As String(), colHeaderNames() As String, ByRef tbl As Table)
        Dim newRow As New TableRow()


        Dim query = From pif As PropertyInfo In propertyInfos
                    Select pif Where colsToInclude.Contains(pif.Name)
                    Order By Array.IndexOf(colsToInclude, pif.Name)

        For Each proinfo As PropertyInfo In query.ToList
            Dim hcell As New TableHeaderCell()
            hcell.Text = colHeaderNames.GetValue(Array.IndexOf(colsToInclude, proinfo.Name))
            newRow.Cells.Add(hcell)
        Next
        tbl.Rows.Add(newRow)
    End Sub

    Private Sub GetExcelData(Of T)(obj As T, propertyInfos As PropertyInfo(), colsToInclude As String(), ByRef tbl As Table)
        Dim newRow As New TableRow()

        Dim query = From pif As PropertyInfo In propertyInfos
                  Select pif Where colsToInclude.Contains(pif.Name)
                  Order By Array.IndexOf(colsToInclude, pif.Name)

        For Each proinfo As PropertyInfo In query.ToList()

            Dim NewCell As New TableCell()
            If Not IsNothing(proinfo.GetValue(obj, Nothing)) Then
                NewCell.Text = proinfo.GetValue(obj, Nothing).ToString()
            End If
            newRow.Cells.Add(NewCell)

        Next
        tbl.Rows.Add(newRow)
    End Sub

#End Region

End Module
