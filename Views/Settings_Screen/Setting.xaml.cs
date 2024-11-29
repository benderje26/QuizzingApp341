namespace QuizzingApp341.Views {
    public partial class Setting : ContentPage {
        public Setting() {
            InitializeComponent();
        }

        private async void OnSignOutClicked(object sender, EventArgs e) {
            // Navigate back to the Login screen, and hide the TabBar
            await Shell.Current.GoToAsync("//Login", true);
            Shell.SetTabBarIsVisible(this, false);
        }

    }
}
