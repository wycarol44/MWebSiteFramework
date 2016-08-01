
Partial Class UserControls_MilesAddress
    Inherits System.Web.UI.UserControl

#Region "Properties"
    ''' <summary>
    ''' Gets or sets whether the required field validators are enabled
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Required As Boolean
        Get
            Return ViewState("Required")
        End Get
        Set(value As Boolean)
            ViewState("Required") = value

            rfvtxtAddressLine1.Enabled = value
            rfvtxtCity.Enabled = value
            rfvddlCountry.Enabled = value
            rfvddlState.Enabled = value
            rfvtxtState.Enabled = value
            rfvtxtPostalCode.Enabled = value

            lblAddressLine1Required.Visible = value
            lblCityRequired.Visible = value
            lblCountryRequired.Visible = value
            lblStateRequired.Visible = value
            lblPostalCodeRequired.Visible = value

        End Set
    End Property

    Public Property AddressID As Integer
        Get
            Return ViewState("AddressID")
        End Get
        Set(value As Integer)
            ViewState("AddressID") = value

            If value > 0 Then
                Populate()
            End If
        End Set
    End Property

#End Region

#Region "Methods"

    ''' <summary>
    ''' Determines if at least one field is entered
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsAddressEntered() As Boolean

        Return If(
            Not String.IsNullOrWhiteSpace(txtAddressLine1.Text) _
            Or Not String.IsNullOrWhiteSpace(txtAddressLine2.Text) _
            Or Not String.IsNullOrWhiteSpace(txtCity.Text) _
            Or Not String.IsNullOrWhiteSpace(txtState.Text) _
            Or Not String.IsNullOrWhiteSpace(txtPostalCode.Text) _
            Or ddlCountry.CountryID IsNot Nothing _
            Or ddlState.StateID IsNot Nothing,
            True,
            False)

    End Function

    Public Function Save() As Integer?

        'determine if the address is filled out (or at least one value is entered)
        If Not IsAddressEntered() Then Return Nothing

        Dim obj = AddressManager.GetById(AddressID)


        obj.Address1 = txtAddressLine1.Text
        obj.Address2 = txtAddressLine2.Text
        obj.City = txtCity.Text

        obj.CountryID = ddlCountry.CountryID


        If obj.CountryID = MilesMetaCountry.UnitedStates _
            Or obj.CountryID = MilesMetaCountry.Canada Then

            'save ID if us or canada
            obj.StateID = ddlState.StateID
        Else
            obj.State = txtState.Text
        End If

        obj.PostalCode = txtPostalCode.Text

        obj.FullAddress = FormatAddress(txtAddressLine1.Text,
                                        txtAddressLine2.Text,
                                        txtCity.Text,
                                        If(ddlCountry.CountryID = MilesMetaCountry.UnitedStates Or
                                           ddlCountry.CountryID = MilesMetaCountry.Canada, If(ddlState.SelectedValue = "", "", ddlState.SelectedItem.Text), txtState.Text),
                                       txtPostalCode.Text,
                                       If(ddlCountry.SelectedValue = "", "", ddlCountry.SelectedItem.Text))

        AddressID = AddressManager.Save(obj)

        Return AddressID
    End Function


    Public Sub Populate()
        Dim obj = AddressManager.GetById(AddressID)
        If obj.AddressID > 0 Then

            txtAddressLine1.Text = obj.Address1
            txtAddressLine2.Text = obj.Address2
            txtCity.Text = obj.City

            'set country
            ddlCountry.CountryID = obj.CountryID

            'load the states based on selected country
            LoadStates()

            'load field based on selected country
            If obj.StateID.HasValue Then
                ddlState.StateID = obj.StateID
            Else
                txtState.Text = obj.State
            End If

            txtPostalCode.Text = obj.PostalCode

        End If
    End Sub

    Private Sub LoadStates()
        Dim countryId As MilesMetaCountry = ddlCountry.CountryID.GetValueOrDefault

        'switch state view depending on selected country
        If countryId = MilesMetaCountry.UnitedStates Or countryId = MilesMetaCountry.Canada Then
            mvState.SetActiveView(vwStateComboxBox)
            ddlState.Country = countryId
        Else
            mvState.SetActiveView(vwStateTextBox)
        End If
    End Sub

#End Region

#Region "Events"
    Protected Sub ddlCountry_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles ddlCountry.SelectedIndexChanged
        LoadStates()

        If ddlState.Visible Then
            ddlState.Focus()
        Else
            txtState.Focus()
        End If
    End Sub
#End Region
    
End Class
