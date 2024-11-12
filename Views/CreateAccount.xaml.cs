using System.Text.RegularExpressions;

namespace QuizzingApp341.Views;

/*
 * Name: Jeremiah Bender
 */
public partial class CreateAccount : ContentPage {
    public CreateAccount() {
        InitializeComponent();
    }

    private async void OnCreateAccountButtonClicked(object sender, EventArgs e) {
        string emailAddress = emailInput.Text ?? string.Empty;
        string username = usernameInput.Text ?? string.Empty;
        string password = passwordInput.Text ?? string.Empty;
        string repeatPassword = repeatPasswordInput.Text ?? string.Empty;

        if (!password.Equals(repeatPassword)) {
            await DisplayAlert("Password", "The passwords must match.", "OK");
            return;
        }

        string? result = await MauiProgram.BusinessLogic.CreateNewUser(emailAddress, username, password);

        if (result == null) {
            await Navigation.PopAsync();
        } else {
            await DisplayAlert("User Creation Failed", result, "OK");
        }
    }
}