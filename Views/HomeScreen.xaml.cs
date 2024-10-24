namespace QuizzingApp341.Views;

public partial class HomeScreen : ContentPage {
    public HomeScreen() {
        InitializeComponent();
    }
    private void OnSearchClicked(object sender, EventArgs e) {
        // Navigate to SearchResultsPage
        Navigation.PushAsync(new Search());
    }
}