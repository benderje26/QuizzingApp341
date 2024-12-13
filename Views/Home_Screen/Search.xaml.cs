using QuizzingApp341.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuizzingApp341.Views;

public partial class Search : ContentPage {
    private readonly IBusinessLogic _businessLogic;

    private ObservableCollection<QuizSearch> _allQuizzes = [];
    public ObservableCollection<QuizSearch> Quizzes { get; set; } = [];
    public string SearchText { get; set; }

    public Search(IBusinessLogic businessLogic) {
        InitializeComponent();
        _businessLogic = businessLogic ?? throw new ArgumentNullException(nameof(businessLogic));
        SearchText = string.Empty;

        BindingContext = this;
        LoadQuizzesAsync();
    }

    private async void LoadQuizzesAsync() {
        try {
            // Fetch quizzes from the database
            var quizzes = await _businessLogic.GetAllPublicQuizzes();
            if (quizzes != null) {
                _allQuizzes.Clear();
                foreach (var quiz in quizzes) {
                    var favQuizzes = _businessLogic.UserInfo.FavoriteQuizzes;
                    var creatorName = "Unknown";
                    var userData = await _businessLogic.GetUserData(quiz.CreatorId);
                    if (userData != null && !string.IsNullOrWhiteSpace(userData.Username)) {
                        creatorName = userData.Username;
                    }
                    bool isFavorite = favQuizzes.Any(f => f.Id == quiz.Id);
                    _allQuizzes.Add(new QuizSearch(quiz.Id, quiz.Title, $"Created by {creatorName}", isFavorite));
                }

                // Sort quizzes so favorites are first
                var sortedQuizzes = new ObservableCollection<QuizSearch>(
                    _allQuizzes.OrderByDescending(q => q.Favorite)
                );

                _allQuizzes = sortedQuizzes; // Update the internal list
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
        SearchText = e.NewTextValue;
    }

    // Command for study button
    public Command<QuizSearch> StudyCommand => new(async quiz => {
        await UserInterfaceUtil.StudyQuiz(quiz.Id, Navigation);
    });

    // Command for Favorite button
    public Command<QuizSearch> FavoriteCommand => new(quiz => {
        quiz.Favorite = !quiz.Favorite;
        if (quiz.Favorite) {
            _businessLogic.AddFavoriteQuiz(quiz.Id);
        } else {
            _businessLogic.DeleteFavoriteQuiz(quiz.Id);
        }
    });

    public Command<Search> SearchCommand => new((quiz) => {

    });
}

public class QuizSearch : INotifyPropertyChanged {
    public long Id { get; set; }
    public string Title { get; set; }
    public string Creator { get; set; }
    public bool Favorite {
        get => favorite;
        set {
            if (favorite != value) {
                favorite = value;
                NotifyPropertyChanged(nameof(Favorite));
                NotifyPropertyChanged(nameof(NotFavorite));
            }
        }
    }
    private bool favorite;
    public bool NotFavorite => !Favorite;

    public QuizSearch(long id, string title, string creator, bool favorite) {
        Id = id;
        Title = title;
        Creator = creator;
        Favorite = favorite;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    // Helper method to raise the PropertyChanged event
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}