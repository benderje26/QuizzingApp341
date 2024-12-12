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
    public bool IsSignedIn { get; set; } = isSignedIn;

    private ObservableCollection<Quiz>? createdQuizzes;
    private ObservableCollection<Quiz>? favoriteQuizzes;
    private ObservableCollection<ActiveQuiz>? activatedQuizzes;
    private ObservableCollection<Participant>? participatedQuizzes;
    private ObservableCollection<object>? quizScores;
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
    public ObservableCollection<ActiveQuiz> ActivatedQuizzes {
        get => activatedQuizzes ?? [];
        set {
            activatedQuizzes = value;
            OnPropertyChanged();
        }
    }
    
    public ObservableCollection<Participant> ParticipatedQuizzes {
        get => participatedQuizzes ?? [];
        set {
            participatedQuizzes = value;
            OnPropertyChanged();
        }
    }
    public ObservableCollection<object>? QuizScores {
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