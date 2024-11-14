using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

public partial class HomeScreen : ContentPage {
    public HomeScreen() {
        InitializeComponent();
    }
    private void OnSearchClicked(object sender, EventArgs e) {
        // Navigate to SearchResultsPage
        Navigation.PushAsync(new Search());
    }
   
    private async void OnStartClicked(object sender, EventArgs e)
    {
        string quizId = quizIdEntry.Text;

        if (await MauiProgram.BusinessLogic.GetQuiz(quizId) is Quiz quiz) {
            MauiProgram.BusinessLogic.SetQuiz(quiz);
            bool multipleChoice = MauiProgram.BusinessLogic.CurrentQuestion?.Type == Models.QuestionType.MultipleChoice;
            if (multipleChoice) {
                await Navigation.PushModalAsync(new MultipleChoice());
            } else {
                await Navigation.PushModalAsync(new FillBlank());
            }
        } else {
            await DisplayAlert("Invalid Quiz ID", "Please enter a valid Quiz ID.", "OK");
        }
    }
}