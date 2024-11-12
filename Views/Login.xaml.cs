namespace QuizzingApp341.Views;
using System;
using Microsoft.Maui.Controls;
/*
 * Name: Jeremiah Bender
 */
public partial class Login : ContentPage {
    public Login() {
        InitializeComponent();
    }

    private void OnCreateClicked(object sender, EventArgs e) {
        // Navigate to Create Account
        Navigation.PushAsync(new CreateAccount());
    }

    private void OnResetPasswordClicked(object sender, EventArgs e) {
        // Navigate to Reset Password
        Navigation.PushAsync(new ResetPassword());
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e) {
        string email = emailInput.Text ?? string.Empty;
        string password = passwordInput.Text ?? string.Empty;
        bool loginSuccessful = await MauiProgram.BusinessLogic.LogIn(email, password);

        if (loginSuccessful) {
            // Navigate to HomeScreen and make TabBar visible
            await Shell.Current.GoToAsync("//HomeScreen");  //TODO: change HomeScreen to UserHome once UserHome got proved 
            Shell.SetTabBarIsVisible(this, true);
        } else {
            await DisplayAlert("Login Failed", "Invalid email or password.", "OK");
        }
    }

}
