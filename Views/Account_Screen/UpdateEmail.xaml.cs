using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

/*
 * Name: Peter Skogman
 */
public partial class UpdateEmail : ContentPage {
    public UpdateEmail() {
        InitializeComponent();
    }

    private async void OnUpdateEmailButtonClicked(object sender, EventArgs e) {
        //Read in the email and the repeated email
        string emailAddress = emailInput.Text ?? string.Empty;
        string repeatEmail = repeatEmailInput.Text ?? string.Empty;

        //Check if they are the same 
        if (!emailAddress.Equals(repeatEmail)) {
            await DisplayAlert("Email", "The emails must match.", "OK");
            return;
        }

        //Attempt to update the email
        (UpdateEmailResult result, string? errorMessage) = await MauiProgram.BusinessLogic.UpdateEmail(emailAddress);

        //Return to account page or give pop up that error occured
        if (result == UpdateEmailResult.Success) {
            await Navigation.PopAsync();
        } else {
            await DisplayAlert("Email Update Failed", errorMessage, "OK");
        }
    }
}