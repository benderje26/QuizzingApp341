namespace QuizzingApp341.Views;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;

public partial class WaitScreen : Popup {
    public double ScreenWidth {get; set;}
    public double ScreenHeight {get; set;}
    public WaitScreen() {
        InitializeComponent();
        ScreenWidth = DeviceDisplay.Current.MainDisplayInfo.Width;
        ScreenHeight = DeviceDisplay.Current.MainDisplayInfo.Height;
        BindingContext = this;
    }
}