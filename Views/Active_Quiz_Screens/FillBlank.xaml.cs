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
    public bool ShowSubmitAnswerButton => UserIsParticipant && !UserIsActivator;
    public bool ShowNextButton => UserIsActivator && !LastQuestion;
    public bool ShowFinishButton => UserIsActivator && LastQuestion;
    public bool LastQuestion => MauiProgram.BusinessLogic.QuizManager?.CurrentQuestion?.QuestionNo == MauiProgram.BusinessLogic.QuizManager?.Questions.Count;

    private readonly ActiveQuestion currentQuestion;
    public FillBlank(ActiveQuestion activeQuestion, bool isUserActivator, bool isUserParticipant) {
        QuestionText = activeQuestion.Question ?? string.Empty;
        UserIsActivator = isUserActivator;
        UserIsParticipant = isUserParticipant;
        currentQuestion = activeQuestion;
        BindingContext = this;
        InitializeComponent();
    }

    /*
     * Next button clicked so move to the next question in the quiz 
     */

    private async void OnNextClicked(object sender, EventArgs e) {
        if (MauiProgram.BusinessLogic.QuizManager?.ActiveQuiz == null) {
            return;
        }

        if (UserIsParticipant) {
            bool result = await SubmitAnswer();
            if (!result) {
                return;
            }
        }

        // Increment the current question number
        long activeQuizId = MauiProgram.BusinessLogic.QuizManager.ActiveQuiz.Id;
        await MauiProgram.BusinessLogic.IncrementCurrentQuestion();

        Question? nextQuestion = MauiProgram.BusinessLogic.QuizManager.CurrentQuestion;

        if (nextQuestion == null) {
            return;
        }
        
        // Get next question
        // Make it an active question
        ActiveQuestion activeQuestion = new(nextQuestion, activeQuizId);

        // call ProcessNextResult()
        await UserInterfaceUtil.ProcessNextResult(activeQuestion, this, UserIsActivator, UserIsParticipant);
    }

    private async void OnFinishClicked(object sender, EventArgs e) {
        if (UserIsParticipant) {
            bool result = await SubmitAnswer();
            if (!result) {
                return;
            }
        }

        if (MauiProgram.BusinessLogic.QuizManager?.ActiveQuiz is ActiveQuiz aq) {
            await UserInterfaceUtil.ShowQuizResults(aq.Id, this, true);
        }

        await MauiProgram.BusinessLogic.DeactivateQuiz();
    }

    private async void OnSubmitAnswerClicked(object sender, EventArgs e) {
        bool success = await SubmitAnswer();
        await UserInterfaceUtil.ProcessResponseResult(success, this);
    }

    private async Task<bool> SubmitAnswer() {
        string answerGiven = textEntry.Text;
        return await MauiProgram.BusinessLogic.GiveFillBlankQuestionAnswer(currentQuestion, answerGiven);
    }

    protected override bool OnBackButtonPressed() {
        _ = UserInterfaceUtil.ProcessActiveQuizEnded(Navigation);

        return true;
    }
}