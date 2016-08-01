Imports Telerik.Web.UI
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports CommonLibrary
Public Class MilesPercentageSlider
    Inherits RadSlider

#Region "Properties"

#End Region

    Public Sub New()
        'set initial properties
        Me.MinimumValue = 0
        Me.MaximumValue = 100
        Me.Height = Unit.Pixel(45)
        Me.Width = Unit.Pixel(200)
        Me.ShowDecreaseHandle = False
        Me.ShowIncreaseHandle = False
        Me.ItemType = SliderItemType.Tick
        Me.LargeChange = 10
        Me.SmallChange = 5
        Me.TrackPosition = SliderTrackPosition.BottomRight
        Me.LiveDrag = True
        Me.TrackMouseWheel = False
        Me.OnClientSlideStart = "slider.MilesPercentageSliderOnClientSlideStart"
        Me.OnClientSlideEnd = "slider.MilesPercentageSliderOnClientSlideEnd"
        Me.OnClientValueChanging = "slider.MilesPercentageSliderOnClientValueChanging"

    End Sub

    Protected Overrides Sub OnPreRender(e As EventArgs)

        Dim js = <script>
                    (function(global){
                        
                        var slider = global.slider = global.slider || {};

                        slider.cancelMilesPercentageSliderValueChange = true;

                        slider.MilesPercentageSliderOnClientSlideStart = function(sender, args) {
                            slider.cancelMilesPercentageSliderValueChange = false;
                        }

                        slider.MilesPercentageSliderOnClientSlideEnd = function(sender, args) {
                            slider.cancelMilesPercentageSliderValueChange = true;
                        }

                        slider.MilesPercentageSliderOnClientValueChanging = function(sender, args) {
                            args.set_cancel(slider.cancelMilesPercentageSliderValueChange)
                        }
                    })(window);

        
                 </script>

        'Inject the script into the page
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "MilesPercentageSlider", DecodeJS(js), True)

        MyBase.OnPreRender(e)
    End Sub

End Class
