namespace QuizzingApp341.Models;

public class MultipleChoiceQuestion(int questionNumber, string text, List<string> options, int? correctAnswer) : Question(questionNumber, text) {
    public int? GivenAnswer {
        get { return givenAnswer; }
        set {
            if (givenAnswer != value) {
                givenAnswer = value;
                OnPropertyChanged(nameof(GivenAnswer));
            }
        }
    }
    int? givenAnswer;

    public List<string> Options {
        get { return options; }
        set {
            if (options != value) {
                options = value;
                OnPropertyChanged(nameof(Options));
            }
        }
    }

    public override QuestionType Type => QuestionType.MultipleChoice;

    List<string> options = options;

    public override bool HasCorrectAnswer() => correctAnswer != null;

    public override bool IsCorrect() => givenAnswer == correctAnswer;

    public override void SetGivenAnswer(object givenAnswerParam) {
        givenAnswer = (int) givenAnswerParam;
    }
}
