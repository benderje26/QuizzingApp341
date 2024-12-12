using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class UpdateUsername : ContentPage {
    public UpdateUsername() {
        InitializeComponent();
    }

    private async void OnUpdateUsernameButtonClicked(object sender, EventArgs e) {
        //Read in the username and the repeated username
        string username = usernameInput.Text ?? string.Empty;
        string repeatUsername = repeatUsernameInput.Text ?? string.Empty;

        //Check if they are the same 
        if (!username.Equals(repeatUsername)) {
            await DisplayAlert("Username", "The usernames must match.", "OK");
            return;
        }

        //Attempt to update the username
        (UpdateUsernameResult result, string? errorMessage) = await MauiProgram.BusinessLogic.UpdateUsername(username);

        //Return to account page or give pop up that error occured
        if (result == UpdateUsernameResult.Success) {
            await Navigation.PopAsync();
        } else {
            await DisplayAlert("Username Update Failed", errorMessage, "OK");
        }
    }
}