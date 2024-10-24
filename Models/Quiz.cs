namespace QuizzingApp341.Models;
// This class represents a quiz that contains a title, date created, last date activated, questions and answers
public class Quiz {
    public string Title { get; set; }
    public DateTime LastActivated { get; set; }
    public DateTime DateCreated { get; set; }
    public List<string> Questions { get; set; }
    public List<string> Answers { get; set; }

    public Quiz(string title, DateTime lastActivated) {
        this.Title = title;
        this.LastActivated = lastActivated;
    }
}