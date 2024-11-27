using QuizzingApp341.Models;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class FillBlank : Popup {
    public string QuestionText {get; set;}
    public bool UserIsActivator {get; set;}
    public bool UserIsParticipant {get; set;}
    public FillBlank(ActiveQuestion activeQuestion, bool isUserActivator) {
        QuestionText = activeQuestion.Question;
        UserIsActivator = isUserActivator;
        UserIsParticipant = !UserIsActivator;
        InitializeComponent();
        BindingContext = MauiProgram.BusinessLogic;
    }

    /*
     * Next button clicked so move to the next question in the quiz 
     */
    private void OnNextClicked(object sender, EventArgs e) {
        // string givenAnswer = textEntry.Text ?? string.Empty;
        // MauiProgram.BusinessLogic.SetCurrentFillBlankAnswer(givenAnswer);
        // bool success = MauiProgram.BusinessLogic.NextQuestion() != null;
        // if (success) {
        //     bool multipleChoice = MauiProgram.BusinessLogic.CurrentQuestion?.Type == Models.QuestionType.MultipleChoice;
        //     if (multipleChoice) {
        //         Navigation.PushModalAsync(new MultipleChoice());
        //     } else {
        //         Navigation.PushModalAsync(new FillBlank());
        //     }
        // }

        // TODO

    }

    private void OnAnswerSubmitClicked(object sender, EventArgs e) {
        
    }




    /*
    * Previous button clicked so move to the previous question in the quiz 
    */
    private void OnPreviousClicked(object sender, EventArgs e) {
        // string givenAnswer = textEntry.Text ?? string.Empty;
        // MauiProgram.BusinessLogic.SetCurrentFillBlankAnswer(givenAnswer);
        // bool success = MauiProgram.BusinessLogic.PreviousQuestion() != null;
        // if (success) {
        //     bool multipleChoice = MauiProgram.BusinessLogic.CurrentQuestion?.Type == Models.QuestionType.MultipleChoice;
        //     if (multipleChoice) {
        //         Navigation.PushModalAsync(new MultipleChoice());
        //     } else {
        //         Navigation.PushModalAsync(new FillBlank());
        //     }
        // }

        // TODO
    }

    /*
     * Submit button hit so close the quiz by going to the homescreen
     */
    private async void OnSubmitClicked(object sender, EventArgs e) {
        // string givenAnswer = textEntry.Text ?? string.Empty;
        // MauiProgram.BusinessLogic.SetCurrentFillBlankAnswer(givenAnswer);
        // (int correct, int total) = MauiProgram.BusinessLogic.GetScore();
        // await DisplayAlert("Quiz Over", "Congratulations! You got " + correct + " out of " + total + " correct", "OK");
        // Navigation.PushModalAsync(new HomeScreen());
        // TODO
    }
}