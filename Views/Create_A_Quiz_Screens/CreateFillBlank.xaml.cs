namespace QuizzingApp341.Views;

public partial class CreateFillBlank : ContentPage
{
	public CreateFillBlank()
	{
		InitializeComponent();
	}


    private void OnSaveClicked(object sender, EventArgs e) {
        // Retrieve data from user input
        //check for null, if null - replace with empty string
        string question = QuestionFillBlank.Text != null ? QuestionFillBlank.Text.Trim() : string.Empty;
        String answer = AnswerFillBlank.Text != null ? AnswerFillBlank.Text.Trim() : string.Empty;


        // Check for required fields not empty
        if (string.IsNullOrEmpty(question)) {
            DisplayAlert("Error", "Please enter a question.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(answer)) {
            DisplayAlert("Error", "Please enter a correct answer.", "OK");
            return;
        }

        // Do something with the retrieved data - make a question object - saving to Database

        // Navigate back to the CreateNewQuiz page
        var newPage = new CreateNewQuiz();
        Navigation.InsertPageBefore(newPage, this);
        Navigation.PopAsync();
    }

}