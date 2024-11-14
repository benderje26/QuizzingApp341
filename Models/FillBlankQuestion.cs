namespace QuizzingApp341.Models;

public class FillBlankQuestion(int questionNumber, bool isFinal, string text, List<string>? correntAnswers, bool caseSensitive) : Question(questionNumber, isFinal, text) {
    public string GivenAnswer {
        get { return givenAnswer; }
        set {
            givenAnswer = value;
            OnPropertyChanged(nameof(GivenAnswer));
        }
    }

    public override QuestionType Type => QuestionType.FillBlank;

    string givenAnswer = string.Empty;

    public override bool HasCorrectAnswer() => correntAnswers != null;

    public override bool IsCorrect() {
        List<string> possibilities = correntAnswers ?? [];
        if (!caseSensitive) {
            string toLower = givenAnswer.ToLower();
            return possibilities.Where(x => x.ToLower().Equals(toLower)).Any();
        }
        return possibilities.Contains(givenAnswer);
    }

    public override void SetGivenAnswer(object givenAnswerParam) {
        givenAnswer = (string) givenAnswerParam;
    }
}
