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

    private void OnSubmitClicked(object sender, EventArgs e)
    {
        // Navigate to Create account
        Navigation.PushAsync(new QuestionStats());
    }
}