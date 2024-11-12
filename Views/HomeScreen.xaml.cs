namespace QuizzingApp341.Views;

public partial class HomeScreen : ContentPage {
    public HomeScreen() {
        InitializeComponent();
    }
    private void OnSearchClicked(object sender, EventArgs e) {
        // Navigate to SearchResultsPage
        Navigation.PushAsync(new Search());
    }
   
    private async void OnStartClicked(object sender, EventArgs e)
    {
        string quizId = quizIdEntry.Text;

        if (quizId == "12345") 
        {
            await MauiProgram.BusinessLogic.SetQuiz();
            await Navigation.PushAsync(new MultipleChoice()); 
        }
        else
        {
            await DisplayAlert("Invalid Quiz ID", "Please enter a valid Quiz ID.", "OK");
        }
    }
}