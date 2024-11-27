using QuizzingApp341.Models;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class MultipleChoice : Popup {
    int? selectedIndex;
    public string QuestionText {get; set;}
    public string[]? Options {get; set;}
    public bool UserIsActivator {get; set;}
    public bool UserIsParticipant {get; set;}

    public static int UserAnswer {get; set;}

    private ActiveQuestion currentQuestion;

    public MultipleChoice(ActiveQuestion activeQuestion, bool isUserActivator) {
        InitializeComponent();
        QuestionText = activeQuestion.Question;
        Options = activeQuestion.MultipleChoiceOptions;
        UserIsActivator = isUserActivator;
        UserIsParticipant = !UserIsActivator;
        currentQuestion = activeQuestion;
        UserAnswer = -1;
        BindingContext = this;
    }

    /*
     * Next button clicked so move to the next question in the quiz 
     */
    private void OnNextClicked(object sender, EventArgs e) {
        // int? selected = selectedIndex;
        // if (selected == null) {
        //     return;
        // }
        // MauiProgram.BusinessLogic.SetCurrentMultipleChoiceAnswer(selected.Value);
        // bool success = MauiProgram.BusinessLogic.NextQuestion() != null;
        // if (success) {
        //     bool multipleChoice = MauiProgram.BusinessLogic.CurrentQuestion?.Type == QuestionType.MultipleChoice;
        //     if (multipleChoice) {
        //         Navigation.PushModalAsync(new MultipleChoice());
        //     } else {
        //         Navigation.PushModalAsync(new FillBlank());
        //     }
        // }
        //TODO
    }

    /*
    * Previous button clicked so move to the previous question in the quiz 
    */
    private void OnPreviousClicked(object sender, EventArgs e) {
        // int? selected = selectedIndex;
        // if (selected == null) {
        //     return;
        // }
        // MauiProgram.BusinessLogic.SetCurrentMultipleChoiceAnswer(selected.Value);
        // bool success = MauiProgram.BusinessLogic.PreviousQuestion() != null;
        // if (success) {
        //     bool multipleChoice = MauiProgram.BusinessLogic.CurrentQuestion?.Type == QuestionType.MultipleChoice;
        //     if (multipleChoice) {
        //         Navigation.PushModalAsync(new MultipleChoice());
        //     } else {
        //         Navigation.PushModalAsync(new FillBlank());
        //     }
        // }
        //TODO
    }

    /*
     * Submit button hit so close the quiz by going to the homescreen
     */
    private async void OnSubmitClicked(object sender, EventArgs e) {
        // int? selected = selectedIndex;
        // if (selected == null) {
        //     return;
        // }
        // MauiProgram.BusinessLogic.SetCurrentMultipleChoiceAnswer(selected.Value);
        // (int correct, int total) = MauiProgram.BusinessLogic.GetScore();
        // await DisplayAlert("Quiz Over", "Congratulations! You got " + correct + " out of " + total + " correct", "OK");
        // await Navigation.PushModalAsync(new HomeScreen());
        // TODO
    }

    private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e) {
        if (e.Value) {
            RadioButton? rb = sender as RadioButton;

            if (rb != null) {
                //TODO get the index of the selected radio button and set it equal to UserAnswer
            }
        }
    }

    private void OnAnswerSubmitClicked(object sender, EventArgs e) {
        // Make a response
        Response response = new Response();
        response.ActiveQuizId = currentQuestion.ActiveQuizId;
        response.QuestionNo = currentQuestion.QuestionNo;
        response.MultipleChoiceResponse = [UserAnswer];

        // Send response to db
        MauiProgram.BusinessLogic.AddResponse(response);
    }

}