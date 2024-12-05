namespace QuizzingApp341.Views {
    public partial class Account : ContentPage {
        public Account() {
            InitializeComponent();
        }

        private async void OnSignOutClicked(object sender, EventArgs e) {
            // Navigate back to the Login screen
            await Shell.Current.GoToAsync("//Login", true);
        }
        private void OnUpdateEmailClicked(object sender, EventArgs e) {
            // Navigate to the Update Email screen
            Navigation.PushAsync(new UpdateEmail());
        }

        private void OnUpdateUsernameClicked(object sender, EventArgs e) {
            // Navigate to the Update Username screen
            Navigation.PushAsync(new UpdateUsername());
        }

        private void OnUpdatePasswordClicked(object sender, EventArgs e) {
            // Navigate to the Update Password screen
            Navigation.PushAsync(new UpdatePassword());
        }


    }
}
