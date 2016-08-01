
Partial Class Test_TestSlider
    Inherits BasePage

    Protected Sub mpsPercentageDone_ValueChanged(sender As Object, e As EventArgs) Handles mpsPercentageDone.ValueChanged
        lblValue.Text = FormatNumber(mpsPercentageDone.Value, 0)
    End Sub
End Class
