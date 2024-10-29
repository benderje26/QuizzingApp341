namespace QuizzingApp341.Models;

// This class represents a quiz that contains a title, date created, last date activated, questions and answers
public class Quiz(string title, DateTime? lastActivated, DateTime? dateCreated, long? time, List<Question> questions) {
    public string Title { get; set; } = title;
    public DateTime? LastActivated { get; set; } = lastActivated;
    public DateTime? DateCreated { get; set; } = dateCreated;
    public long? Time { get; set; } = time;
    public List<Question> Questions { get; set; } = questions;

    public Question? CurrentQuestion {
        get {
            if (currentIndex == null) {
                return null;
            }
            return Questions[currentIndex.Value];
        }
    }
    private int? currentIndex = null;

    public bool HasNextQuestion() {
        if (currentIndex == null) {
            return Questions.Count > 0;
        }
        return currentIndex < Questions.Count - 1;
    }

    public Question NextQuestion() {
        currentIndex = (currentIndex ?? 0) + 1;
        return Questions[currentIndex.Value];
    }
}