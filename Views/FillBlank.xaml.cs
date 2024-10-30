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

    private void OnSubmitClicked(object sender, EventArgs e)
    {
        // Navigate to Create account
        Navigation.PushAsync(new QuestionStats());
    }

    private void OnNextClicked(object sender, EventArgs e) {
        bool success = MauiProgram.BusinessLogic.IncrementCurrentQuestion();
        if (success) {
            bool multipleChoice = MauiProgram.BusinessLogic.IsCurrentQuestionMultipleChoice();
            if (multipleChoice) {
                Navigation.PushModalAsync(new MultipleChoice());
            } else {
                Navigation.PushModalAsync(new FillBlank());
            }
        }
    }

    private void OnPreviousClicked(object sender, EventArgs e) {
        bool success = MauiProgram.BusinessLogic.DecrementCurrentQuestion();
        if (success) {
            bool multipleChoice = MauiProgram.BusinessLogic.IsCurrentQuestionMultipleChoice();
            if (multipleChoice) {
                Navigation.PushModalAsync(new MultipleChoice());
            } else {
                Navigation.PushModalAsync(new FillBlank());
            }
        }
    }
}