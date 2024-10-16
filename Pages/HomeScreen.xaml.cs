namespace QuizzingApp341.Pages;

public partial class HomeScreen : ContentPage
{
	public HomeScreen()
	{
		InitializeComponent();
	}
    private void OnSearchClicked(object sender, EventArgs e)
    {
        // Navigate to SearchResultsPage
        Navigation.PushAsync(new SearchResultsPage());
    }
}