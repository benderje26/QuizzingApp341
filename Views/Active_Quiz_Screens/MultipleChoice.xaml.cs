using CommunityToolkit.Maui.Core.Extensions;
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
    public bool CanSubmit => SelectedIndex != null;
    public int? SelectedIndex { get; set; }
    public bool ShowSubmitAnswerButton => UserIsParticipant;
    public bool ShowNextButton => UserIsActivator; // TODO also needs to not be final question
    public bool ShowFinishButton => UserIsActivator; // TODO also needs to be final question

    private readonly ActiveQuestion currentQuestion;

    public MultipleChoice(ActiveQuestion activeQuestion, bool isUserActivator, bool isUserParticipant) {
        QuestionText = activeQuestion.Question ?? string.Empty;
        Options = (activeQuestion.MultipleChoiceOptions ?? []).Select((x, ind) => new IndexValuePair(ind, x)).ToObservableCollection();
        UserIsActivator = isUserActivator;
        UserIsParticipant = isUserParticipant;
        currentQuestion = activeQuestion;
        SelectedIndex = null;
        BindingContext = this;
        InitializeComponent();
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

    private void OnFinishClicked(object sender, EventArgs e) {
        // Finish button hit so close the quiz by going to the homescreen
    }

    private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e) {
        if (e.Value) {
            RadioButton? rb = sender as RadioButton;

            if (rb?.BindingContext is IndexValuePair value) {
                SelectedIndex = value.Index;
            }

            OnPropertyChanged(nameof(CanSubmit));
        }
    }

    private async void OnSubmitAnswerClicked(object sender, EventArgs e) {
        if (SelectedIndex == null) {
            return;
        }
        bool success = await MauiProgram.BusinessLogic.GiveMultipleChoiceQuestionAnswer(currentQuestion, SelectedIndex ?? 0);
        await UserInterfaceUtil.ProcessResponseResult(success, this);
    }
}