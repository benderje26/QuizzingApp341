namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class FillBlank : ContentPage {
    public FillBlank() {
        InitializeComponent();
        BindingContext = MauiProgram.BusinessLogic;
    }

    private void OnSubmitClicked(object sender, EventArgs e) {
        // Navigate to Create account
        Navigation.PushAsync(new QuestionStats());
    }
}