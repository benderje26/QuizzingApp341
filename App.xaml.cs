﻿namespace QuizzingApp341 {
    public partial class App : Application {
        public App() {
            MainPage = new NavigationPage(new Views.HomeScreen());

            MainPage = new AppShell();
        }
    }
}
