
Public Class MetaStateManager
#Region "Standard"

    ''' <summary>
    ''' Gets a record by its ID
    ''' </summary>
    ''' <param name="stateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetById(stateId As Integer) As DataLibrary.MetaState
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.MetaStates
                       Where t.StateID = stateId
                       Select t).SingleOrDefault()

            Return obj

        End Using

    End Function

    ''' <summary>
    ''' Gets a list of records
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetList(Optional countryId As Integer? = Nothing) As DataLibrary.MetaState()
        Using ctx As New DataLibrary.ModelEntities
            Dim obj = (From t In ctx.MetaStates
                       Where (countryId Is Nothing OrElse t.CountryID = countryId)
                       Order By t.StateName
                       Select t).ToArray()

            Return obj

        End Using
    End Function

#End Region
End Class
