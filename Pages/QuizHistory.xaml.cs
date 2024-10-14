namespace QuizzingApp341.Pages;
using QuizzingApp341.Models;
// This class initializes and displays the Quiz history.
public partial class QuizHistory : ContentPage
{
    public List<Quiz> Quizzes {get; set;}

    public QuizHistory()
    {
        InitializeComponent();

        Quizzes = new List<Quiz>
        {
            new Quiz ("Quiz 1", DateTime.Parse("2024-01-01") ),
            new Quiz ("Quiz 2", DateTime.Parse("2024-01-02") ),
            new Quiz ("Quiz 3", DateTime.Parse("2024-01-03") ),
            new Quiz ("Quiz 4", DateTime.Parse("2024-01-04") )
        };

        BindingContext = this;
    }
}