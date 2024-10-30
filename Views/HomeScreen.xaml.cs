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
        var quizIdEntry = this.FindByName<Entry>("quizIdEntry");
        string quizId = quizIdEntry?.Text;

        if (quizId == "12345") 
        {
            await Navigation.PushAsync(new TestQuiz("12345")); 
        }
        else
        {
            await DisplayAlert("Invalid Quiz ID", "Please enter a valid Quiz ID.", "OK");
        }
    }
}