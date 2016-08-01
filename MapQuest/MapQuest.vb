Imports System
Imports System.Collections.Generic
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Xml


Namespace MapQuest
    '<remarks>
    '<see cref="http://open.mapquestapi.com/geocoding/"/>
    '<seealso cref="http://developer.mapquest.com/"/>
    '</remarks>
    Public Class Geocoder

#Region "Properties"
        Private MyAppKey As String
        Public Property key() As String
            Get
                Return MyAppKey
            End Get
            Set(value As String)
                MyAppKey = value
            End Set
        End Property

        Private OSM As Boolean
        Public Property UseOSM() As Boolean
            Get
                Return OSM
            End Get
            Set(value As Boolean)
                OSM = value
            End Set
        End Property

        Private m_Proxy As IWebProxy
        Public Property Proxy() As IWebProxy
            Get
                Return m_Proxy
            End Get
            Set(value As IWebProxy)
                m_Proxy = value
            End Set
        End Property
#End Region

#Region "Methods"

        Public Sub New(key As String)
            If String.IsNullOrWhiteSpace(key) Then
                Throw New ArgumentException("key can not be null or blank")
            End If
            Me.key = key
        End Sub

        Public Function Geocode(Address As Address) As MapQuest.Coordinates
            Dim cor = New MapQuest.Coordinates With {.Longitude = 0.0, .Latitude = 0.0}

            'Check Empty
            If Address Is Nothing Then
                Throw New ArgumentException("address can not be null or empty!")
            End If

            Dim FormatAddress As String = Address.Street & "," & Address.State & "," & Address.Postalcode & "," & Address.Country

            'Send request
            Dim request As WebRequest = WebRequest.Create("http://open.mapquestapi.com/geocoding/v1/address?key=" & key & "&location=" & FormatAddress & "&outFormat=xml")
            Dim response As WebResponse = request.GetResponse()

            Dim document As New XmlDocument()
            Dim stream As Stream = response.GetResponseStream()
            Dim sr = New StreamReader(stream)
            Dim reader As XmlReader = XmlReader.Create(sr)
            document.Load(reader)

            Dim xdoc As XDocument = XDocument.Load(New XmlNodeReader(document))

            'There could be mutiple return location, take the first one
            Dim latlng = xdoc...<latLng>.FirstOrDefault()

            'Get the value in xml note <lat> and <lng>
            Dim lat = latlng.<lat>
            Dim lng = latlng.<lng>

            cor.Latitude = Convert.ToDouble(lat.Value)
            cor.Longitude = Convert.ToDouble(lng.Value)


            Return cor
        End Function

        Public Function ReverseGeocode(latitute As Double, longitute As Double) As Address
            Dim add = New Address()

            If latitute = Nothing Or longitute = Nothing Then
                Throw New ArgumentException("Lattitute or Longitute can not be null!")
            End If

            'Send request
            Dim request As WebRequest = WebRequest.Create("http://open.mapquestapi.com/geocoding/v1/reverse?key=" & key & "&callback=renderReverse&location=" & latitute.ToString() & "," & longitute.ToString() & "&outFormat=xml")
            Dim response As WebResponse = request.GetResponse()

            Dim document As New XmlDocument()
            Dim stream As Stream = response.GetResponseStream()
            Dim sr = New StreamReader(stream)
            Dim reader As XmlReader = XmlReader.Create(sr)
            document.Load(reader)

            Dim xdoc As XDocument = XDocument.Load(New XmlNodeReader(document))

            Dim location = xdoc...<location>.FirstOrDefault()

            add.Street = location.<street>.Value
            add.City = location.<adminArea5>.Value
            add.State = location.<adminArea3>.Value
            add.County = location.<adminArea4>.Value
            add.Postalcode = location.<postalCode>.Value
            add.Country = location.<adminArea1>.Value

            Return add
        End Function

        Public Function BatchGeocode(Addresses As List(Of Address)) As List(Of Coordinates)
            Dim List As New List(Of Coordinates)

            Dim requestUrl As String = "http://open.mapquestapi.com/geocoding/v1/batch?key=" & key
            Dim FormatAddress As String = ""

            For Each Str As Address In Addresses
                FormatAddress = Str.Street & "," & Str.State & "," & Str.Postalcode & "," & Str.Country
                requestUrl = requestUrl & "&location=" & FormatAddress
            Next

            requestUrl = requestUrl & "&outFormat=xml"

            'Send request
            Dim request As WebRequest = WebRequest.Create(requestUrl)
            Dim response As WebResponse = request.GetResponse()

            Dim document As New XmlDocument()
            Dim stream As Stream = response.GetResponseStream()
            Dim sr = New StreamReader(stream)
            Dim reader As XmlReader = XmlReader.Create(sr)
            document.Load(reader)

            Dim xdoc As XDocument = XDocument.Load(New XmlNodeReader(document))

            'Get the value from <results><result> node
            Dim result As IEnumerable(Of XElement) = xdoc...<result>


            For Each re As XElement In result
                Dim cor = New MapQuest.Coordinates
                Dim latLng = re...<latLng>.FirstOrDefault()
                Dim lat = latLng.<lat>
                Dim lng = latLng.<lng>

                cor.Latitude = lat.Value
                cor.Longitude = lng.Value

                List.Add(cor)
            Next

            Return List
        End Function
#End Region


    End Class

    Public Class Coordinates

        'Private add As Address
        'Public Property Address() As Address
        '    Get
        '        Return add
        '    End Get
        '    Set(value As Address)
        '        add = value
        '    End Set
        'End Property

        Private lng As Double
        Public Property Longitude() As Double
            Get
                Return lng
            End Get
            Set(value As Double)
                lng = value
            End Set
        End Property

        Private lat As Double
        Public Property Latitude() As Double
            Get
                Return lat
            End Get
            Set(value As Double)
                lat = value
            End Set
        End Property
    End Class

    Public Class Address
        Private str As String = ""
        Public Property Street() As String
            Get
                Return str
            End Get
            Set(value As String)
                str = value
            End Set
        End Property

        Private cit As String = ""
        Public Property City() As String
            Get
                Return cit
            End Get
            Set(value As String)
                cit = value
            End Set
        End Property

        Private sta As String = ""
        Public Property State() As String
            Get
                Return sta
            End Get
            Set(value As String)
                sta = value
            End Set
        End Property

        Private con As String = ""
        Public Property County() As String
            Get
                Return con
            End Get
            Set(value As String)
                con = value
            End Set
        End Property

        Private pos As String = ""
        Public Property Postalcode() As String
            Get
                Return pos
            End Get
            Set(value As String)
                pos = value
            End Set
        End Property

        Private cont As String = ""
        Public Property Country() As String
            Get
                Return cont
            End Get
            Set(value As String)
                cont = value
            End Set
        End Property
    End Class

End Namespace


