namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class FillBlank : ContentPage
{
	public FillBlank()
	{
		InitializeComponent();
        BindingContext = MauiProgram.BusinessLogic;
    }

    /*
     * Next button clicked so move to the next question in the quiz 
     */
    private void OnNextClicked(object sender, EventArgs e) {
        string givenAnswer = textEntry.Text ?? string.Empty;
        MauiProgram.BusinessLogic.SetCurrentFillBlankAnswer(givenAnswer);
        bool success = MauiProgram.BusinessLogic.NextQuestion() != null;
        if (success) {
            bool multipleChoice = MauiProgram.BusinessLogic.CurrentQuestion?.Type == Models.QuestionType.MultipleChoice;
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
        string givenAnswer = textEntry.Text ?? string.Empty;
        MauiProgram.BusinessLogic.SetCurrentFillBlankAnswer(givenAnswer);
        bool success = MauiProgram.BusinessLogic.PreviousQuestion() != null;
        if (success) {
            bool multipleChoice = MauiProgram.BusinessLogic.CurrentQuestion?.Type == Models.QuestionType.MultipleChoice;
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
        string givenAnswer = textEntry.Text ?? string.Empty;
        MauiProgram.BusinessLogic.SetCurrentFillBlankAnswer(givenAnswer);
        (int correct, int total) = MauiProgram.BusinessLogic.GetScore();
        await DisplayAlert("Quiz Over", "Congratulations! You got " + correct + " out of " + total + " correct", "OK");
        Navigation.PushModalAsync(new HomeScreen());
    }
}