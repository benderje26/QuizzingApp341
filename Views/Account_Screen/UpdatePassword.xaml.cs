using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class UpdatePassword : ContentPage {
    public UpdatePassword() {
        InitializeComponent();
    }

    private async void OnUpdatePasswordButtonClicked(object sender, EventArgs e) {
        //Read in the password and the repeated password
        string password = passwordInput.Text ?? string.Empty;
        string repeatPassword = repeatPasswordInput.Text ?? string.Empty;

        //Check if they are the same 
        if (!password.Equals(repeatPassword)) {
            await DisplayAlert("Password", "The passwords must match.", "OK");
            return;
        }

        //Attempt to update the username
        (UpdatePasswordResult result, string? errorMessage) = await MauiProgram.BusinessLogic.UpdatePassword(password);

        //Return to account page or give pop up that error occured
        if (result == UpdatePasswordResult.Success) {
            await Navigation.PopAsync();
        } else {
            await DisplayAlert("Password Update Failed", errorMessage, "OK");
        }
    }
}