Imports MapQuest


Module Module1

    Sub Main()
        Dim maprequest As New MapQuest.Geocoder("Fmjtd%7Cluub2huanl%2C20%3Do5-9uzwdz")

        Dim address As MapQuest.Address = New MapQuest.Address With {.Street = "111 montgomery st", .City = "Highland Park", .State = "NJ", .Postalcode = "08904"}
        Dim cor As MapQuest.Coordinates = maprequest.Geocode(address)

        Console.WriteLine("Latitude:" & cor.Latitude)
        Console.WriteLine("longitude: " & cor.Longitude)

        Console.WriteLine("****************************************************************")

        Dim add As MapQuest.Address = maprequest.ReverseGeocode(40.499798, -74.432801)
        Console.WriteLine(add.Street)
        Console.WriteLine(add.City & "," & add.State & "," & add.Postalcode & "," & add.Country)


        Console.WriteLine("****************************************************************")

        Dim address2 As MapQuest.Address = New MapQuest.Address With {.Street = "111 montgomery st", .City = "Highland Park", .State = "NJ", .Postalcode = "08904"}
        Dim address3 As MapQuest.Address = New MapQuest.Address With {.Street = "2801 Merrywood Dr", .City = "Edison", .State = "NJ", .Postalcode = "08817"}
        Dim address4 As MapQuest.Address = New MapQuest.Address With {.Street = "18 College Ave", .City = "New Brunswick", .State = "NJ", .Postalcode = "08901"}
        Dim addList = New List(Of MapQuest.Address)
        addList.Add(address2)
        addList.Add(address3)
        addList.Add(address4)

        Dim corList As List(Of MapQuest.Coordinates) = maprequest.BatchGeocode(addList)

        For Each cor2 As MapQuest.Coordinates In corList
            Console.WriteLine("----------------------------------------------------------")
            Console.WriteLine(cor2.Latitude)
            Console.WriteLine(cor2.Longitude)
        Next

        Console.ReadLine()
    End Sub

End Module
