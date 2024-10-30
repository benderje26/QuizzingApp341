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
        // Navigate to Create account
        Navigation.PushAsync(new QuestionStats());
    }

    private void OnPreviousClicked(object sender, EventArgs e) {
        // Navigate to Create account
        Navigation.PushAsync(new QuestionStats());
    }
}