using System.Collections.ObjectModel;

namespace QuizzingApp341.Models;
public class UserInfo(Guid id, bool isSignedIn) {
    public Guid? Id { get; set; } = id;
    public bool IsSignedIn { get; private set; } = isSignedIn;
    public ObservableCollection<Quiz> CreatedQuizzes { get; set; } = [];
    public ObservableCollection<Quiz> FavoriteQuizzes { get; set; } = [];
    public ObservableCollection<Quiz> ActivatedQuizzes { get; set; } = [];
    public ObservableCollection<Quiz> QuizHistory { get; set; } = [];
    public ObservableCollection<Object>? QuizScores { get; set; }
}