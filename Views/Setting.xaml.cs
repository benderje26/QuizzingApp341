namespace QuizzingApp341.Views {
    public partial class Setting : ContentPage {
        public Setting() {
            InitializeComponent();
        }

        private void OnSignOutClicked(object sender, EventArgs e) {

            // Navigate back to the Login screen, and hide the TabBar
            Shell.Current.GoToAsync("//Login");
        }

    }
}
