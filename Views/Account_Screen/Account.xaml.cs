namespace QuizzingApp341.Views {
    public partial class Account : ContentPage {
        public Account() {
            InitializeComponent();
        }

        private async void OnSignOutClicked(object sender, EventArgs e) {
            // Navigate back to the Login screen, and hide the TabBar
            await Shell.Current.GoToAsync("//Login", true);
        }

    }
}
