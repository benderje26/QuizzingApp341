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

    private async void OnLoginButtonClicked(object sender, EventArgs e) {
        bool loginSuccessful = true; // set true for now, later should check for username and password

        if (loginSuccessful) {
            // Navigate to HomeScreen and make TabBar visible
            await Shell.Current.GoToAsync("//HomeScreen");
            Shell.SetTabBarIsVisible(this, true);
        } else {
            await DisplayAlert("Login Failed", "Invalid username or password.", "OK");
        }
    }

}
