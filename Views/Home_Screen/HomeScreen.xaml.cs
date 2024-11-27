using CommunityToolkit.Maui.Views;
using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

public partial class HomeScreen : ContentPage {
    public HomeScreen() {
        InitializeComponent();
    }
    private void OnSearchClicked(object sender, EventArgs e) {
        // Navigate to SearchResultsPage
        Navigation.PushAsync(new Search(MauiProgram.BusinessLogic));
    }

    private async void OnStartClicked(object sender, EventArgs e) {
        string accessCode = quizIdEntry.Text;
        //TODO

        // If the accessCode is a valid access code
        bool valid = await MauiProgram.BusinessLogic.ValidateAccessCode(accessCode);

        if (valid) {
            // Display wait
            WaitScreen waitScreen = new WaitScreen();
            this.ShowPopup(waitScreen);

            // Join Quiz
            ActiveQuiz activeQuiz = await MauiProgram.BusinessLogic.GetActiveQuiz(accessCode);
            await MauiProgram.BusinessLogic.JoinActiveQuiz(activeQuiz, activeQuestion => {
                // Pop off screen
                waitScreen.Close();

                // Display current active question according to its question type
                if(activeQuestion.QuestionType == QuestionType.MultipleChoice) {
                    this.ShowPopup(new MultipleChoice(activeQuestion));
                } else {
                    this.ShowPopup(new FillBlank(activeQuestion));
                }
            });
        } else {
            DisplayAlert("Error", "Invalid access code.", "OK");
        }
    }
}