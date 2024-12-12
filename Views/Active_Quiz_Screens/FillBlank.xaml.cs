using QuizzingApp341.Models;
using System.Collections.ObjectModel;

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
    public bool ShowNextButton => UserIsActivator && !LastQuestion; // TODO: also needs to not be final question
    public bool ShowFinishButton => UserIsActivator && LastQuestion; // TODO: also needs to be final question

    public bool LastQuestion { get; set; }
    private readonly ActiveQuestion currentQuestion;
    public FillBlank(ActiveQuestion activeQuestion, bool isUserActivator, bool isUserParticipant) {
        QuestionText = activeQuestion.Question ?? string.Empty;
        UserIsActivator = isUserActivator;
        UserIsParticipant = !UserIsActivator;
        currentQuestion = activeQuestion;
        LastQuestion = MauiProgram.BusinessLogic.QuizManager.CurrentQuestion.QuestionNo == MauiProgram.BusinessLogic.QuizManager.Questions.Count;
        BindingContext = this;
        InitializeComponent();
    }

    /*
     * Next button clicked so move to the next question in the quiz 
     */

    private async void OnNextClicked(object sender, EventArgs e) {
        // Increment the current question number
        long activeQuizId = MauiProgram.BusinessLogic.QuizManager.ActiveQuiz.Id;
        MauiProgram.BusinessLogic.IncrementCurrentQuestion();

        Question nextQuestion = MauiProgram.BusinessLogic.QuizManager.CurrentQuestion;
        
        // Get next question
        // Make it an active question
        ActiveQuestion activeQuestion = new ActiveQuestion(nextQuestion, activeQuizId);

        // call ProcessNextResult()
        await UserInterfaceUtil.ProcessNextResult(activeQuestion, this, false, true);
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

    private async void OnFinishClicked(object sender, EventArgs e) {
        await Navigation.PopToRootAsync();

        await MauiProgram.BusinessLogic.DeactivateQuiz();
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