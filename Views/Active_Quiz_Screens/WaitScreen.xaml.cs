namespace QuizzingApp341.Views;
public partial class WaitScreen : ContentPage {
    public string Text { get; set; }
    public WaitScreen(string text) {
        Text = text;
        BindingContext = this;
        InitializeComponent();
    }
}