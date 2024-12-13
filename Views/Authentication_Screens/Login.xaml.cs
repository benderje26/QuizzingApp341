namespace QuizzingApp341.Views;
using Microsoft.Maui.Controls;
using QuizzingApp341.Models;
using System;

/*
* Name: Jeremiah Bender
*/
public partial class Login : ContentPage {
    public Login() {
        InitializeComponent();
        Shell.SetTabBarIsVisible(this, false);
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
        (LoginResult result, string? errorMessage) = await MauiProgram.BusinessLogic.Login(email, password);

        if (result == LoginResult.Success) {
            // Set variables for user ahead of time
            // Navigate to HomeScreen and make TabBar visible
            await Shell.Current.GoToAsync("//HomeScreen", true);
        } else {
            await DisplayAlert("Login Failed", errorMessage, "OK");
        }
    }

    private async void SkipLogin(object sender, EventArgs e) {
        Console.WriteLine("SKIP LOGIN CLICKED");
        await MauiProgram.BusinessLogic.SkipLogin();
        await Shell.Current.GoToAsync("//HomeScreen");
    }

}
