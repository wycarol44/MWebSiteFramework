Imports System.Web
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Text

Public Class CheckBoxListValidator
    Inherits BaseValidator

    Public Sub New()
        MinCheckedRequired = 1
    End Sub

#Region "Properties"

    Public Property MinCheckedRequired As Integer
        Get
            Return Val(ViewState("MinCheckedRequired"))
        End Get
        Set(value As Integer)
            ViewState("MinCheckedRequired") = value
        End Set
    End Property
#End Region

    Protected Overrides Function EvaluateIsValid() As Boolean
        Dim ctrl As CheckBoxList = FindControl(ControlToValidate)
        If ctrl IsNot Nothing Then

            'check that at least 1 item is selected
            Dim checkedItems = From chk As ListItem In ctrl.Items
                               Where chk.Selected
                               Select chk

            Return (checkedItems.Count >= MinCheckedRequired)

        End If

        Return False

    End Function

    Protected Overrides Function ControlPropertiesValid() As Boolean

        'check whether the ControlToValdate propery is set or not
        If String.IsNullOrWhiteSpace(ControlToValidate) Then Return False

        Dim ctrl As Control = FindControl(ControlToValidate)
        'check whether the right type of control is selected to validate
        If (ctrl Is Nothing) Or (Not (TypeOf ctrl Is CheckBoxList)) Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Overrides Sub OnLoad(e As System.EventArgs)

        Dim ctrl As CheckBoxList = FindControl(ControlToValidate)
        If ControlPropertiesValid() = False Then
            Throw New Exception(String.Format( _
                                "ControlToValidate of {0} is either blank or not a valid CheckBoxList control", _
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
                    function validate_CBL(sender) {
                        var val = document.getElementById (sender.controltovalidate);
                        var col = val.getElementsByTagName("*");
                        if ( col != null ) {
                            for ( i = 0; i &lt; col.length; i++ ) {
                                if (col.item(i).tagName == "INPUT"){
                                    if ( col.item(i).checked ) {
                                        return true;
                                    }
                                }
                            }

                            return false;
                        }
                    }
                 </script>

        'Inject the script into the page
        ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "cblvalidator", HttpUtility.HtmlDecode(js.ToString()), False)
        'Registering validator clientside javascript function
        ScriptManager.RegisterExpandoAttribute(Me, ClientID, "evaluationfunction", "validate_CBL", False)

    End Sub


End Class
