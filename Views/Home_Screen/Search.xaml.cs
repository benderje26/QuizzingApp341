using System.Collections.ObjectModel;
using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

public partial class Search : ContentPage {
    private readonly IBusinessLogic _businessLogic;

    public ObservableCollection<QuizSearch> Quizzes { get; set; }

    public Search(IBusinessLogic businessLogic) {
        InitializeComponent();
        _businessLogic = businessLogic ?? throw new ArgumentNullException(nameof(businessLogic));

        BindingContext = this;
        LoadQuizzesAsync();
    }

    private async void LoadQuizzesAsync() {
        try {
            // Fetch quizzes from the database
            var quizzes = await _businessLogic.GetAllQuizzes();
            if (quizzes != null) {
                Quizzes.Clear();
                foreach (var quiz in quizzes) {
                    Quizzes.Add(new QuizSearch(quiz.Title, $"Created by {quiz.CreatorId}"));
                }
            }
        } catch (Exception ex) {
            await DisplayAlert("Error", $"Failed to load quizzes: {ex.Message}", "OK");
        }
    }


    // Command for study button
    public Command<QuizSearch> StudyCommand => new Command<QuizSearch>(async (quiz) => {
        // Handle study button logic here
        await DisplayAlert("Study", $"Start studying {quiz.Title}?", "OK");
    });
}

// Quiz class
public class QuizSearch(string title, string creator) {
    public string? Title { get; set; } = title;
    public string? Creator { get; set; } = creator;
}
