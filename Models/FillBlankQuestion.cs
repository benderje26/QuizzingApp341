namespace QuizzingApp341.Models;

public class FillBlankQuestion(int questionNumber, string text, string[]? correctAnswers, bool caseSensitive) : Question(questionNumber, text) {
    public string GivenAnswer {
        get { return givenAnswer; }
        set {
            givenAnswer = value;
            OnPropertyChanged(nameof(GivenAnswer));
        }
    }
    string givenAnswer = string.Empty;

    public override bool HasCorrectAnswer() => correctAnswers != null;

    public override bool IsCorrect() {
        string[] possibilities = correctAnswers ?? [];
        if (caseSensitive) {
            string toLower = givenAnswer.ToLower();
            return possibilities.Where(x => x.ToLower().Equals(toLower)).Any();
        }
        return possibilities.Contains(givenAnswer);
    }
}
