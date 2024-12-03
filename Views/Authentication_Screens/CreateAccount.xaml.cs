using QuizzingApp341.Models;

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

        (AccountCreationResult result, string? errorMessage) = await MauiProgram.BusinessLogic.CreateNewUser(emailAddress, username, password);

        if (result == AccountCreationResult.Success) {
            await Navigation.PopAsync();
        } else {
            await DisplayAlert("User Creation Failed", errorMessage, "OK");
        }
    }
}