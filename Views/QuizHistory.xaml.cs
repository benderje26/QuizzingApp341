namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
// This class initializes and displays the Quiz history.
public partial class QuizHistory : ContentPage {
    public List<Quiz> Quizzes { get; set; }

    public QuizHistory() {
        InitializeComponent();

        Quizzes = new List<Quiz>
        {
            new Quiz ("Quiz 1", DateTime.Parse("2024-01-01"), DateTime.Parse("2023-01-01"), 1000, new List<Question>()),
            new Quiz ("Quiz 2", DateTime.Parse("2024-01-02"), DateTime.Parse("2023-01-01"), 1000, new List<Question>()),
            new Quiz ("Quiz 3", DateTime.Parse("2024-01-03"), DateTime.Parse("2023-01-01"), 1000, new List<Question>()),
            new Quiz ("Quiz 4", DateTime.Parse("2024-01-04"), DateTime.Parse("2023-01-01"), 1000, new List<Question>())
        };

        BindingContext = this;
    }
}