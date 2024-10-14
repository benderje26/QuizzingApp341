namespace QuizzingApp341.Pages;
using QuizzingApp341.Models;
public partial class QuizParticipants : ContentPage
{
    public List<Participant> Participants {get; set;}

    public QuizParticipants()
    {
        InitializeComponent();

        Participants = new List<Participant>
        {
            new Participant("Pachia", 10),
            new Participant("Jason", 10),
            new Participant("Zafeer", 10),
            new Participant("Peter", 10),
            new Participant("Jerimiah", 10)
        };

        BindingContext = this;
    }
}

public class Participant {
    public string User {get; set;}
    public int Score {get; set;}
    public Participant(string user, int score) {
        this.User = user;
        this.Score = score;
    }
}