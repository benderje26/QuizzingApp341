using System.Collections.ObjectModel;
using QuizzingApp341.Models;
using System.Linq;

namespace QuizzingApp341.Views;

public partial class Search : ContentPage {
    private readonly IBusinessLogic _businessLogic;
    private ObservableCollection<QuizSearch> _allQuizzes;

    public ObservableCollection<QuizSearch> Quizzes { get; set; }

    public Search(IBusinessLogic businessLogic) {
        InitializeComponent();
        _businessLogic = businessLogic ?? throw new ArgumentNullException(nameof(businessLogic));
        Quizzes = new ObservableCollection<QuizSearch>();
        _allQuizzes = new ObservableCollection<QuizSearch>();

        BindingContext = this;
        LoadQuizzesAsync();
    }

    private async void LoadQuizzesAsync() {
        try {
            // Fetch quizzes from the database
            var quizzes = await _businessLogic.GetAllQuizzes();
            if (quizzes != null) {
                _allQuizzes.Clear();
                foreach (var quiz in quizzes) {
                    _allQuizzes.Add(new QuizSearch(quiz.Title, $"Created by {quiz.CreatorId}"));
                }
                FilterQuizzes(string.Empty); // Load all initially
            }
        } catch (Exception ex) {
            await DisplayAlert("Error", $"Failed to load quizzes: {ex.Message}", "OK");
        }
    }

    private void FilterQuizzes(string searchText) {
        var filteredQuizzes = string.IsNullOrWhiteSpace(searchText)
            ? _allQuizzes
            : new ObservableCollection<QuizSearch>(
                _allQuizzes.Where(q => q.Title != null && q.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)));

        Quizzes.Clear();
        foreach (var quiz in filteredQuizzes) {
            Quizzes.Add(quiz);
        }
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e) {
        FilterQuizzes(e.NewTextValue);
    }

    // Command for study button
    public Command<QuizSearch> StudyCommand => new Command<QuizSearch>(async (quiz) => {
        await DisplayAlert("Study", $"Start studying {quiz.Title}?", "OK");
        // Navigate to the study page here if needed
    });
}

public class QuizSearch {
    public string? Title { get; set; }
    public string? Creator { get; set; }

    public QuizSearch(string title, string creator) {
        Title = title;
        Creator = creator;
    }
}