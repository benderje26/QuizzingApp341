namespace QuizzingApp341.Views;

public partial class CreateQuiz : ContentPage {
    public CreateQuiz() {
        InitializeComponent();
    }


    // Event handler for button click
    private void StudyButtonClickedCreateQuiz(object sender, EventArgs e) {
        // pull out the quize that got clicked
    }


    private void OnCreateNewQuizButtonClicked(object sender, EventArgs e) {
        Navigation.PushAsync(new CreateNewQuiz());
    }

}