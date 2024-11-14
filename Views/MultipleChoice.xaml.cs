using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class MultipleChoice : ContentPage
{
    int? selectedIndex;

	public MultipleChoice()
	{
		InitializeComponent();
        BindingContext = MauiProgram.BusinessLogic;
    }

    /*
     * Next button clicked so move to the next question in the quiz 
     */
    private void OnNextClicked(object sender, EventArgs e) {
        int? selected = selectedIndex;
        if (selected == null) {
            return;
        }
        MauiProgram.BusinessLogic.SetCurrentMultipleChoiceAnswer(selected.Value);
        bool success = MauiProgram.BusinessLogic.NextQuestion() != null;
        if (success) {
            bool multipleChoice = MauiProgram.BusinessLogic.CurrentQuestion?.Type == QuestionType.MultipleChoice;
            if (multipleChoice) {
                Navigation.PushModalAsync(new MultipleChoice());
            } else {
                Navigation.PushModalAsync(new FillBlank());
            }
        }
    }

    /*
    * Previous button clicked so move to the previous question in the quiz 
    */
    private void OnPreviousClicked(object sender, EventArgs e) {
        int? selected = selectedIndex;
        if (selected == null) {
            return;
        }
        MauiProgram.BusinessLogic.SetCurrentMultipleChoiceAnswer(selected.Value);
        bool success = MauiProgram.BusinessLogic.PreviousQuestion() != null;
        if (success) {
            bool multipleChoice = MauiProgram.BusinessLogic.CurrentQuestion?.Type == QuestionType.MultipleChoice;
            if (multipleChoice) {
                Navigation.PushModalAsync(new MultipleChoice());
            } else {
                Navigation.PushModalAsync(new FillBlank());
            }
        }
    }

    /*
     * Submit button hit so close the quiz by going to the homescreen
     */
    private async void OnSubmitClicked(object sender, EventArgs e) {
        int? selected = selectedIndex;
        if (selected == null) {
            return;
        }
        MauiProgram.BusinessLogic.SetCurrentMultipleChoiceAnswer(selected.Value);
        (int correct, int total) = MauiProgram.BusinessLogic.GetScore();
        await DisplayAlert("Quiz Over", "Congratulations! You got " + correct + " out of " + total + " correct", "OK");
        await Navigation.PushModalAsync(new HomeScreen());
    }

    private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e) {
        if (e.Value) {
            RadioButton? rb = sender as RadioButton;

            if (rb?.BindingContext is IndexValuePair value) {
                selectedIndex = value.Index;
            }
        }
    }

}