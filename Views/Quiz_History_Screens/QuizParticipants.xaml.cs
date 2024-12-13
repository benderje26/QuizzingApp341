namespace QuizzingApp341.Views;
// This screen initializes and displays the list of participants of a quiz
public partial class QuizParticipants : ContentPage {
    public List<Participant> Participants { get; set; }

    public QuizParticipants(Dictionary<string, int> quizStats) {
        InitializeComponent();
        Participants = new List<Participant>();
        foreach (KeyValuePair<string, int> user in quizStats) {
            Participants.Add(new Participant(user.Key, user.Value));
        }
        BindingContext = this;
    }
}

// Represents a participant of a quiz with a User name and a score
public class Participant {
    public string User { get; set; }
    public int Score { get; set; }
    public Participant(string user, int score) {
        this.User = user;
        this.Score = score;
    }
}