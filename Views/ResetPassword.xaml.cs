using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

/*
 * Name: Jeremiah Bender
 */
public partial class ResetPassword : ContentPage {
    public ResetPassword() {
        InitializeComponent();
    }

    private async void OnResetButtonClicked(object sender, EventArgs e) {
        string email = emailInput.Text ?? string.Empty;
        (ResetPasswordResult result, string? errorMessage) = await MauiProgram.BusinessLogic.ResetPassword(email);
        
        if (result == ResetPasswordResult.EmailSent) {
            await DisplayAlert("Email Sent", errorMessage, "OK");
        } else {
            await DisplayAlert("Login Failed", errorMessage, "OK");
        }
    }
}