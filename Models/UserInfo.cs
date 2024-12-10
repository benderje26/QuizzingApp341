using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;


namespace QuizzingApp341.Models;
public class UserInfo(Guid id, string email, string username, bool isSignedIn) : INotifyPropertyChanged {
    public Guid? Id { get; set; } = id;
    public string Email { get; set; } = email;
    public string Username { get; set; } = username;
    public bool IsSignedIn { get; private set; } = isSignedIn;

    public ObservableCollection<Quiz>? createdQuizzes;
    public ObservableCollection<Quiz>? favoriteQuizzes;
    public ObservableCollection<Quiz>? activatedQuizzes;
    public ObservableCollection<Quiz>? quizHistory;
    public ObservableCollection<object>? quizScores;
    public ObservableCollection<Quiz> CreatedQuizzes {
        get => createdQuizzes ?? [];
        set {
            createdQuizzes = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<Quiz> FavoriteQuizzes {
        get => favoriteQuizzes ?? [];
        set {
            favoriteQuizzes = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<Quiz> ActivatedQuizzes { 
        get => activatedQuizzes ?? [];
        set {
            activatedQuizzes = value;
            OnPropertyChanged();
        }
     }
    public ObservableCollection<Quiz> QuizHistory {
        get => quizHistory ?? [];
        set {
            quizHistory = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<Object>? QuizScores {
        get => quizScores;
        set {
            quizScores = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}