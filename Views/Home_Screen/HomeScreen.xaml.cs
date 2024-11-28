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

        // If the accessCode is a valid access code
        bool valid = await MauiProgram.BusinessLogic.ValidateAccessCode(accessCode);

        if (valid) {
            ActiveQuiz? activeQuiz = await MauiProgram.BusinessLogic.GetActiveQuiz(accessCode);

            if (activeQuiz == null) {
                await DisplayAlert("Error", "Something went wrong, try again.", "OK");
                return;
            }
            WaitScreen blankScreen = new(string.Empty);
            await Navigation.PushAsync(blankScreen, false);
            // Display wait
            WaitScreen waitScreen = new("Waiting for quiz to be started...");
            await Navigation.PushAsync(waitScreen, false);

            // Join Quiz
            await MauiProgram.BusinessLogic.JoinActiveQuiz(activeQuiz, async activeQuestion => {
                // Pop off screen
                await MainThread.InvokeOnMainThreadAsync(async () => await Navigation.PopAsync(false));

                // Display current active question according to its question type
                if (activeQuestion.QuestionType == QuestionType.MultipleChoice) {
                    await MainThread.InvokeOnMainThreadAsync(async () => await Navigation.PushAsync(new MultipleChoice(activeQuestion, false, true), false));
                } else {
                    await MainThread.InvokeOnMainThreadAsync(async () => await Navigation.PushAsync(new FillBlank(activeQuestion, false, true), false));
                }
            });
        } else {
            await DisplayAlert("Error", "Invalid access code.", "OK");
        }
    }
}