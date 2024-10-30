namespace QuizzingApp341.Views;

/*
 * Name: Jeremiah Bender
 */
public partial class Login : ContentPage {
    public Login() {
        InitializeComponent();
    }

    private void LoginButtonClicked(object sender, EventArgs e) {
        Application.Current.MainPage = new NavigationPage(new UserHome()); // Go to user's home page
    }
}