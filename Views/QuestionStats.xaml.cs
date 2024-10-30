namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class QuestionStats : ContentPage {
    public QuestionStats() {
        InitializeComponent();
    }

    private void OnNextQuestionClicked(object sender, EventArgs e) {
        // Navigate to Create account
        Navigation.PushAsync(new FillBlank());
    }
}