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
            if (CurrentIndex == null) {
                return null;
            }
            return Questions[CurrentIndex.Value];
        }
    }

    public int? CurrentIndex {
        get { return currentIndex; }
        set {
            currentIndex = value;
        }
    }
    private int? currentIndex = 0;

    public int? TotalCorrect {
        get { return totalCorrect; }
        set {
            totalCorrect = value;
        }
    }
    private int? totalCorrect = 0;

    public int IncrementTotalCorrect() {
        TotalCorrect = (TotalCorrect ?? 0) + 1;
        return TotalCorrect.Value;
    }

    public int DecrementTotalCorrect() {
        TotalCorrect = (TotalCorrect ?? 1) - 1;
        return TotalCorrect.Value;
    }

    public bool HasNextQuestion() {
        if (CurrentIndex == null) {
            return Questions.Count > 0;
        }
        return CurrentIndex < Questions.Count - 1;
    }

    public Question NextQuestion() {
        CurrentIndex = (CurrentIndex ?? 0) + 1;
        return Questions[CurrentIndex.Value];
    }

    public bool HasPreviousQuestion() {
        if (CurrentIndex == null) {
            return Questions.Count > 0;
        }
        return CurrentIndex > 0;
    }

    public Question PreviousQuestion() {
        CurrentIndex = (CurrentIndex ?? Questions.Count-1) - 1;
        return Questions[CurrentIndex.Value];
    }
}