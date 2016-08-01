
Public Class MetaCountryManager
#Region "Standard"

    ''' <summary>
    ''' Gets a record by its ID
    ''' </summary>
    ''' <param name="countryId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(countryId As Integer) As DataLibrary.MetaCountry
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.MetaCountries
                       Where t.CountryID = countryId
                       Select t).SingleOrDefault()

            Return obj

        End Using

    End Function

    ''' <summary>
    ''' Gets a list of records
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetList() As DataLibrary.MetaCountry()
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.MetaCountries
                       Where t.Include
                       Order By t.SortOrder
                       Select t).ToArray()

            Return obj

        End Using
    End Function

#End Region
End Class
