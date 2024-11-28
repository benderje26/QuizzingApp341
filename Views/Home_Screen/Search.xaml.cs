using System.Collections.ObjectModel;
using QuizzingApp341.Models;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuizzingApp341.Views;

public partial class Search : ContentPage {
    private readonly IBusinessLogic _businessLogic;
    private ObservableCollection<QuizSearch> _allQuizzes;

    public ObservableCollection<QuizSearch> Quizzes { get; set; }
    public string searchText { get; set; }

    public Search(IBusinessLogic businessLogic) {
        InitializeComponent();
        _businessLogic = businessLogic ?? throw new ArgumentNullException(nameof(businessLogic));
        Quizzes = new ObservableCollection<QuizSearch>();
        _allQuizzes = new ObservableCollection<QuizSearch>();
        searchText = string.Empty;

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
                    var favQuizzes = _businessLogic.UserInfo.FavoriteQuizzes;
                    if (favQuizzes.Any(f => f.Id == quiz.Id)) {
                        _allQuizzes.Add(new QuizSearch(quiz.Id, quiz.Title, $"Created by {quiz.CreatorId}", true));
                    } else {
                        _allQuizzes.Add(new QuizSearch(quiz.Id, quiz.Title, $"Created by {quiz.CreatorId}", false));
                    }
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
        searchText = e.NewTextValue;
    }

    // Command for study button
    public Command<QuizSearch> StudyCommand => new Command<QuizSearch>(async (quiz) => {
        await DisplayAlert("Study", $"Start studying {quiz.Title}?", "OK");
        // Navigate to the study page here if needed
    });

    // Command for Favorite button
    public Command<QuizSearch> FavoriteCommand => new Command<QuizSearch>(async (quiz) => {
        if (quiz.Favorite == true) {
            await _businessLogic.DeleteFavoriteQuiz(quiz.Id);
            //quiz.Favorite = false;
            //quiz.NotFavorite = true;
            var index = Quizzes.IndexOf(quiz);
            Quizzes.Remove(quiz);
            Quizzes.Insert(index, new QuizSearch(quiz.Id, quiz.Title, quiz.Creator, false));
        } else {
            await _businessLogic.AddFavoriteQuiz(quiz.Id);
            //quiz.Favorite = true;
            //quiz.NotFavorite = false;
            var index = Quizzes.IndexOf(quiz);
            Quizzes.Remove(quiz);
            Quizzes.Insert(index, new QuizSearch(quiz.Id, quiz.Title, quiz.Creator, true));
        }
        //OnPropertyChanged(nameof(Quizzes));
    });


}

public class QuizSearch {
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Creator { get; set; }
    public bool? Favorite { get; set; }
    public bool? NotFavorite { get; set; }

    public QuizSearch(long id, string title, string creator, bool? favorite) {
        Id = id;
        Title = title;
        Creator = creator;
        Favorite = favorite;
        NotFavorite = !favorite;
    }
}