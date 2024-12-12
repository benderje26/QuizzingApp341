using QuizzingApp341.Models;
using System.Runtime.CompilerServices;

namespace QuizzingApp341.Views;
// This class displays the screen for the instructor/user who activated the quiz
public partial class ActivatorQuiz : ContentPage {
    public ActivatorQuiz() {
        InitializeComponent();
        BindingContext = MauiProgram.BusinessLogic;
    }

    // Event handler for button click
    private async void NextQuestionClicked(object sender, EventArgs e) {
        // Increment the current_question_no in the active quiz
        await MauiProgram.BusinessLogic.IncrementCurrentQuestion();
        if (MauiProgram.BusinessLogic.QuizManager?.CurrentQuestion == null) {
            // deactivate the quiz
            await MauiProgram.BusinessLogic.DeactivateQuiz();
            // pop off back to the edit quiz screen
            await Navigation.PopAsync();
        }
        base.OnBackButtonPressed();
    }

    protected override bool OnBackButtonPressed() {
        return false; // Disable going back
    }
}