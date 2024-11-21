using QuizzingApp341.Models;
using System.Collections.ObjectModel;

public class UserInfo {
    public string? ID {get; set;}
    public ObservableCollection<Quiz> CreatedQuizzes {get; set;} = [];
    public ObservableCollection<Quiz> ActivatedQuizzes {get; set;} = [];
    public ObservableCollection<Quiz> QuizHistory {get; set;} = [];
    public ObservableCollection<Object>? QuizScores {get; set;}

    // Sets the user's id
    public UserInfo(string id) {
        ID = "ba08579e-8e08-43c5-bffc-612393113c28";
    }
}