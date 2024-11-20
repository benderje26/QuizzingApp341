namespace QuizzingApp341.Views;
using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;
using QuizzingApp341.Models;

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
        (LoginResult result, string? errorMessage) = await MauiProgram.BusinessLogic.Login(email, password);

        if (result == LoginResult.Success) {
            // Set variables for user ahead of time
            await SetUser();
            // Navigate to HomeScreen and make TabBar visible
            await Shell.Current.GoToAsync("//HomeScreen");  //TODO: change HomeScreen to UserHome once UserHome got proved 
            Shell.SetTabBarIsVisible(this, true);
        } else {
            await DisplayAlert("Login Failed", errorMessage, "OK");
        }
    }

    private async void SkipLogin(object sender, EventArgs e) {
        await Shell.Current.GoToAsync("//HomeScreen");
    }

    // This method sets variables if needed for the User class
    private async Task SetUser() {
        // Get user ID
        UserInfo user = MauiProgram.BusinessLogic.UserInfo();
       
        // Set other variables ahead of time for the app
        ObservableCollection<Quiz>? userCreatedQuizzes = await MauiProgram.BusinessLogic.GetUserCreatedQuizzes(user.ID);
        if (userCreatedQuizzes != null) {
            user.CreatedQuizzes = userCreatedQuizzes;
        };
    }
}
