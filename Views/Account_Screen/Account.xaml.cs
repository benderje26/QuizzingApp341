using QuizzingApp341.Models;

namespace QuizzingApp341.Views {
    public partial class Account : ContentPage {
        public Account() {
            InitializeComponent();
        }

        private async void OnSignOutClicked(object sender, EventArgs e) {
            // Navigate back to the Login screen
            await Shell.Current.GoToAsync("//Login", true);
        }
        private async void OnUpdateEmailClicked(object sender, EventArgs e) {
            // Check to see if the user is signed in
            if (MauiProgram.BusinessLogic.UserInfo.IsSignedIn) {
                // Navigate to the Update Email screen
                Navigation.PushAsync(new UpdateEmail());
            } else {
                // If the user isn't signed in see if they want to sign in
                bool result = await DisplayAlert("Update Email", "You are not signed in. Would you like to return to the home screen to login?", "Yes", "No");
                if (result) {
                    // Navigate back to the Login screen
                    await Shell.Current.GoToAsync("//Login", true);
                }
            }
        }

        private async void OnUpdateUsernameClicked(object sender, EventArgs e) {
            // Check to see if the user is signed in
            if (MauiProgram.BusinessLogic.UserInfo.IsSignedIn) {
                // Navigate to the Update Username screen
                Navigation.PushAsync(new UpdateUsername());
            } else {
                // If the user isn't signed in see if they want to sign in
                bool result = await DisplayAlert("Update Username", "You are not signed in. Would you like to return to the home screen to login?", "Yes", "No");
                if (result) {
                    // Navigate back to the Login screen
                    await Shell.Current.GoToAsync("//Login", true);
                }
            }
        }
        
        private async void OnUpdatePasswordClicked(object sender, EventArgs e) {
            // Check to see if the user is signed in
            if (MauiProgram.BusinessLogic.UserInfo.IsSignedIn) {
                // Navigate to the Update Password screen
                Navigation.PushAsync(new UpdatePassword());
            } else {
                // If the user isn't signed in see if they want to sign in
                bool result = await DisplayAlert("Update Password", "You are not signed in. Would you like to return to the home screen to login?", "Yes", "No");
                if (result) {
                    // Navigate back to the Login screen
                    await Shell.Current.GoToAsync("//Login", true);
                }
            }
        }

        private async void OnDeleteAccountClicked(object sender, EventArgs e) {
            // Check to see if the user is signed in
            if (MauiProgram.BusinessLogic.UserInfo.IsSignedIn) {
                // Confirm that the user wants to delete their account
                bool result = await DisplayAlert("Delete Account", "Are you sure that you want to delete your account? Press Confirm to delete your account or Cancel exit.", "Confirm", "Cancel");
                if (result) {
                    //Call business logic to delete the account
                    var deleted = await MauiProgram.BusinessLogic.DeleteAccount();
                    if (deleted.Item1.Equals(DeleteAccountResult.Success)) {
                        await Shell.Current.GoToAsync("//Login", true);
                    } else {
                        await DisplayAlert("Delete Account", "An error has occured while deleting the account. Please try again", "OK");
                    }
                }
            } else {
                // If the user isn't signed in see if they want to sign in
                bool result = await DisplayAlert("Delete Account", "You are not signed in. Would you like to return to the home screen to login?", "Yes", "No");
                if (result) {
                    // Navigate back to the Login screen
                    await Shell.Current.GoToAsync("//Login", true);
                }
            }
        }
    }
}
