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
    public bool CanSubmit => !SelectedIndices.IsNullOrEmpty();
    public int[]? SelectedIndices { get; set; }
    public bool ShowSubmitAnswerButton => UserIsParticipant;
    public bool ShowNextButton => UserIsActivator; // TODO: also needs to not be final question
    public bool ShowFinishButton => UserIsActivator; // TODO: also needs to be final question

    private readonly ActiveQuestion currentQuestion;

    CheckBox checkBox = new CheckBox { IsChecked = true };

    public MultipleChoice(ActiveQuestion activeQuestion, bool isUserActivator, bool isUserParticipant) {
       

        QuestionText = activeQuestion.Question ?? string.Empty;
        Options = (activeQuestion.MultipleChoiceOptions ?? [])
            .Select((x, ind) => new IndexValuePair(ind, x))
            .ToObservableCollection();
        SelectedIndices = Array.Empty<int>();
        UserIsActivator = isUserActivator;
        UserIsParticipant = isUserParticipant;
        currentQuestion = activeQuestion;
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




    private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e) {
        if (sender is CheckBox checkBox && checkBox.BindingContext is IndexValuePair value) {
            if (e.Value) {
 
                SelectedIndices = SelectedIndices?.Concat(new[] { value.Index }).ToArray() ?? new[] { value.Index };
            } else {
      
                SelectedIndices = SelectedIndices?.Where(index => index != value.Index).ToArray();
            }

            System.Diagnostics.Debug.WriteLine("SelectedIndices: " + string.Join(", ", SelectedIndices ?? Array.Empty<int>()));

            // Notify property change for CanSubmit
            OnPropertyChanged(nameof(CanSubmit));
        }
    }


    private async void OnSubmitAnswerClicked(object sender, EventArgs e) {
        if (SelectedIndices.IsNullOrEmpty()) {
            return;
        }
        System.Diagnostics.Debug.WriteLine("Submitting Answer for SelectedIndices: " + string.Join(", ", SelectedIndices));

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


}


