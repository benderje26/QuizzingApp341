using System.Collections.ObjectModel;

namespace QuizzingApp341.Models;
public class UserInfo(Guid id) {
    public Guid? Id { get; set; } = id;
    public ObservableCollection<Quiz> CreatedQuizzes { get; set; } = [];
    public ObservableCollection<Quiz> FavoriteQuizzes { get; set; } = [];
    public ObservableCollection<Quiz> ActivatedQuizzes { get; set; } = [];
    public ObservableCollection<Quiz> QuizHistory { get; set; } = [];
    public ObservableCollection<Object>? QuizScores { get; set; }
}