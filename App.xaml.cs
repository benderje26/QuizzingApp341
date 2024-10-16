namespace QuizzingApp341 {
    public partial class App : Application {
        public App() {
            MainPage = new NavigationPage(new Pages.HomeScreen());

            MainPage = new AppShell();
        }
    }
}
