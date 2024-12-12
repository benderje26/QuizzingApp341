namespace QuizzingApp341.Views;
public partial class WaitScreen : ContentPage {
    public string Text { get; set; }
    public WaitScreen(string text) {
        Text = text;
        BindingContext = this;
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed() {
        // Leave the quiz (you can still get back in by re-entering the code)
        MauiProgram.BusinessLogic.LeaveActiveQuiz();

        if (base.OnBackButtonPressed()) {
            Navigation.PopToRootAsync();

            return true;
        }

        return false;
    }
}