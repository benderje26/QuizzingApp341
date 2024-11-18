using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;

namespace QuizzingApp341.Models;

public class MultipleChoiceQuestion(int questionNumber, bool isFinal, string text, List<string> options, int? correctAnswer) : Question(questionNumber, isFinal, text) {
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

    public ObservableCollection<IndexValuePair> Options {
        get { return options; }
        set {
            if (options != value) {
                options = value;
                OnPropertyChanged(nameof(Options));
            }
        }
    }

    public override QuestionType Type => QuestionType.MultipleChoice;

    ObservableCollection<IndexValuePair> options = options.Select((val, index) => new IndexValuePair(index, val)).ToObservableCollection();

    public override bool HasCorrectAnswer() => correctAnswer != null;

    public override bool IsCorrect() => givenAnswer == correctAnswer;

    public override void SetGivenAnswer(object givenAnswerParam) {
        givenAnswer = (int) givenAnswerParam;
    }
}

public class IndexValuePair(int index, string value) {
    public int Index { get { return index; } }
    public string Value { get { return value; } }
}
