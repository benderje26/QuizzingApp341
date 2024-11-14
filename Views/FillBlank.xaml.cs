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
        string givenAnswer = "test";
        bool success = MauiProgram.BusinessLogic.IncrementCurrentQuestion(givenAnswer);
        if (success) {
            bool multipleChoice = MauiProgram.BusinessLogic.IsCurrentQuestionMultipleChoice();
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
        string givenAnswer = "test";
        bool success = MauiProgram.BusinessLogic.DecrementCurrentQuestion(givenAnswer);
        if (success) {
            bool multipleChoice = MauiProgram.BusinessLogic.IsCurrentQuestionMultipleChoice();
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
    private void OnSubmitClicked(object sender, EventArgs e) {
        DisplayAlert("Quiz Over", "Congratulations! You got " + MauiProgram.BusinessLogic.GetTotalCorrect().ToString() + " correct", "OK");
        Navigation.PushModalAsync(new HomeScreen());
    }
}