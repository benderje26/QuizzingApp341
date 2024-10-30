using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class MultipleChoice : ContentPage
{
	public MultipleChoice()
	{
		InitializeComponent();
        BindingContext = MauiProgram.BusinessLogic;
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
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