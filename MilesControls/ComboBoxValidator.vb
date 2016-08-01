Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Text
Imports Telerik.Web.UI

Public Class ComboBoxValidator
    Inherits BaseValidator

    Public Sub New()

    End Sub

    Protected Overrides Function EvaluateIsValid() As Boolean
        Dim ctrl As RadComboBox = FindControl(ControlToValidate)
        If ctrl IsNot Nothing Then
            Dim selectedValue = ctrl.SelectedValue
            'check that the selected value is not Null or Whitespace
            'this will validate no matter if the value is numeric or
            'a combination value (i.e. objectID "-" keyID)
            If Not String.IsNullOrWhiteSpace(selectedValue) AndAlso Not selectedValue = "0" Then
                Return True
            End If
        End If

        Return False

    End Function

    Protected Overrides Function ControlPropertiesValid() As Boolean

        'check whether the ControlToValdate propery is set or not
        If String.IsNullOrWhiteSpace(ControlToValidate) Then Return False

        Dim ctrl As Control = FindControl(ControlToValidate)
        'check whether the right type of control is selected to validate
        If (ctrl Is Nothing) Or (Not (TypeOf ctrl Is RadComboBox)) Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Overrides Sub OnLoad(e As System.EventArgs)

        Dim ctrl As RadComboBox = FindControl(ControlToValidate)
        If ControlPropertiesValid() = False Then
            Throw New Exception(String.Format( _
                                "ControlToValidate of {0} is either blank or not a valid RadComboBox control", _
                                Me.ID))
        End If


    End Sub

    Protected Overrides Sub OnPreRender(e As System.EventArgs)
        MyBase.OnPreRender(e)

        If ControlPropertiesValid() And EnableClientScript Then
            InjectClientValidationScript()
        End If

        If String.IsNullOrWhiteSpace(ErrorMessage) Then
            ErrorMessage = Me.ID
        End If

    End Sub

    'code to generate clients javascript for validation
    Private Sub InjectClientValidationScript()

        Dim js = <script language="javascript">
                    function validate_RCB(sender) {
                        var combo = $find(sender.controltovalidate);
                        var comboItem = combo.findItemByText(combo.get_text())
                        if (comboItem) {
                            var value = comboItem.get_value();
                            if (value) {
                                if (value.length > 0 &amp;&amp; value!=="0") {
                                    return true;
                                }
                            }
                        }
                        else {
                            var value = combo.get_value();
                            if (value) {
                                if (value.length > 0 &amp;&amp; value!=="0") {
                                    return true;
                                }
                            }
                        }                                
                        return false;
                    }

                 </script>

        'Inject the script into the page
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "rcbvalidator", HttpUtility.HtmlDecode(js.ToString()), False)
        'Registering validator clientside javascript function
        ScriptManager.RegisterExpandoAttribute(Me, ClientID, "evaluationfunction", "validate_RCB", False)

    End Sub


End Class
