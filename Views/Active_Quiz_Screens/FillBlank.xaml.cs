using QuizzingApp341.Models;
namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class FillBlank : ContentPage {
    public string QuestionText { get; set; }
    public bool UserIsActivator { get; set; }
    public bool UserIsParticipant { get; set; }
    public bool CanSubmit => true;
    public bool ShowSubmitAnswerButton => UserIsParticipant;
    public bool ShowNextButton => UserIsActivator; // TODO also needs to not be final question
    public bool ShowFinishButton => UserIsActivator; // TODO also needs to be final question
    private readonly ActiveQuestion currentQuestion;
    public FillBlank(ActiveQuestion activeQuestion, bool isUserActivator, bool isUserParticipant) {
        QuestionText = activeQuestion.Question ?? string.Empty;
        UserIsActivator = isUserActivator;
        UserIsParticipant = !UserIsActivator;
        currentQuestion = activeQuestion;
        BindingContext = this;
        InitializeComponent();
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

    private void OnFinishClicked(object sender, EventArgs e) {
        // Finish button hit so close the quiz by going to the homescreen
    }

    private async void OnSubmitAnswerClicked(object sender, EventArgs e) {
        string answerGiven = textEntry.Text;
        bool success = await MauiProgram.BusinessLogic.GiveFillBlankQuestionAnswer(currentQuestion, answerGiven);
        await UserInterfaceUtil.ProcessResponseResult(success, this);
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