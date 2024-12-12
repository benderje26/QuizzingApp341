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
    public bool ShowSubmitAnswerButton => UserIsParticipant;
    public bool ShowNextButton => UserIsActivator && !LastQuestion; // TODO: also needs to not be final question
    public bool ShowFinishButton => UserIsActivator && LastQuestion; // TODO: also needs to be final question

    public bool LastQuestion { get; set; }

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
    private async void OnFinishClicked(object sender, EventArgs e) {
        await Navigation.PopToRootAsync();

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
            bool success = await MauiProgram.BusinessLogic.GiveMultipleChoiceQuestionAnswer(
                currentQuestion,
                SelectedIndices
            );
            await UserInterfaceUtil.ProcessResponseResult(success, this);
        } catch (Exception ex) {
            System.Diagnostics.Debug.WriteLine($"Error submitting answer: {ex.Message}");
        }
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


