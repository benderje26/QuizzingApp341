namespace QuizzingApp341.Models;
public class ActiveQuiz {
    public List<Question> Questions { get; set; }
    public DateTime ActivationTime {get; set;}
    public ActiveQuiz() {

    }

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
    private int? currentIndex;

    public int TotalCorrect {
        get { return totalCorrect; }
        set {
            totalCorrect = value;
        }
    }
    private int totalCorrect = 0;

    public int IncrementTotalCorrect() {
        TotalCorrect++;
        return TotalCorrect;
    }

    public int DecrementTotalCorrect() {
        TotalCorrect--;
        return TotalCorrect;
    }

    public bool HasNextQuestion() {
        if (CurrentIndex == null) {
            return Questions.Count > 0;
        }
        return CurrentIndex < Questions.Count - 1;
    }

    public Question? NextQuestion() {
        if (!HasNextQuestion()) {
            return null;
        }
        CurrentIndex = (CurrentIndex ?? -1) + 1;
        return Questions[CurrentIndex.Value];
    }

    public bool HasPreviousQuestion() {
        if (CurrentIndex == null) {
            return Questions.Count > 0;
        }
        return CurrentIndex > 0;
    }

    public Question? PreviousQuestion() {
        if (!HasPreviousQuestion()) {
            return null;
        }
        CurrentIndex = (CurrentIndex ?? Questions.Count - 1) - 1;
        return Questions[CurrentIndex.Value];
    }
}