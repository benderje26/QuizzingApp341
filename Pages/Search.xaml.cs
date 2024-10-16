using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace QuizzingApp341.Pages
{
    public partial class SearchResultsPage : ContentPage
    {
        public ObservableCollection<Quiz> Quizzes { get; set; }

        public SearchResultsPage()
        {
            InitializeComponent();
            Quizzes = new ObservableCollection<Quiz>
            {
                new Quiz { Title = "Anatomy Quiz 1", Creator = "Created by user1" },
                new Quiz { Title = "Anatomy Quiz 2", Creator = "Created by user2" }
            };

            BindingContext = this;
        }

        // Command for study button
        public Command<Quiz> StudyCommand => new Command<Quiz>(async (quiz) =>
        {
            // Handle study button logic here
            await DisplayAlert("Study", $"Start studying {quiz.Title}?", "OK");
        });
    }

    // Quiz class
    public class Quiz
    {
        public string Title { get; set; }
        public string Creator { get; set; }
    }
}
