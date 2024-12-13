using CommunityToolkit.Maui.Core.Extensions;
using Microsoft.IdentityModel.Tokens;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;

namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class MultipleChoice : ContentPage {
    public string QuestionText { get; set; }
    public ObservableCollection<IndexValuePair> Options { get; set; }
    public bool UserIsActivator { get; set; }
    public bool UserIsParticipant { get; set; }
    public bool CanSubmit => SelectedIndices.Length > 0;
    public bool Multiselect { get; set; }
    public bool NotMultiselect => !Multiselect;
    public int[] SelectedIndices { get; set; }
    public bool ShowSubmitAnswerButton => UserIsParticipant && !UserIsActivator;
    public bool ShowNextButton => UserIsActivator && !LastQuestion;
    public bool ShowFinishButton => UserIsActivator && LastQuestion;
    public bool LastQuestion => MauiProgram.BusinessLogic.QuizManager?.CurrentQuestion?.QuestionNo == MauiProgram.BusinessLogic.QuizManager?.Questions.Count;

    private readonly ActiveQuestion currentQuestion;

    public MultipleChoice(ActiveQuestion activeQuestion, bool isUserActivator, bool isUserParticipant) {
        QuestionText = activeQuestion.Question ?? string.Empty;
        Options = (activeQuestion.MultipleChoiceOptions ?? [])
            .Select((x, ind) => new IndexValuePair(ind, x))
            .ToObservableCollection();
        Multiselect = activeQuestion.Multiselect ?? false;
        SelectedIndices = [];
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

    private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e) {
        if (sender is CheckBox checkBox && checkBox.BindingContext is IndexValuePair value) {
            if (e.Value) {
                SelectedIndices = [.. SelectedIndices, value.Index];
            } else {
                SelectedIndices = SelectedIndices.Where(index => index != value.Index).ToArray();
            }

            // Notify property change for CanSubmit
            OnPropertyChanged(nameof(CanSubmit));
        }
    }

    private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e) {
        if (e.Value) {
            RadioButton? rb = sender as RadioButton;

            if (rb?.BindingContext is IndexValuePair value) {
                SelectedIndices = [value.Index];
            }

            OnPropertyChanged(nameof(CanSubmit));
        }
    }

    private async void OnSubmitAnswerClicked(object sender, EventArgs e) {
        if (SelectedIndices.IsNullOrEmpty()) {
            return;
        }

        try {
            bool success = await SubmitAnswer();
            await UserInterfaceUtil.ProcessResponseResult(success, this);
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($"Error submitting answer: {ex.Message}");
        }
    }

    private async Task<bool> SubmitAnswer() {
        return await MauiProgram.BusinessLogic.GiveMultipleChoiceQuestionAnswer(
            currentQuestion,
            SelectedIndices
        );
    }

    protected override bool OnBackButtonPressed() {
        _ = UserInterfaceUtil.ProcessActiveQuizEnded(Navigation);

        return true;
    }
}


