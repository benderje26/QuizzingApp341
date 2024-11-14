using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuizzingApp341.Models {
    public abstract class Question(int questionNumber, bool isFinal, string questionText) : INotifyPropertyChanged {
        public virtual string Text { get; private set; } = questionText;
        public abstract bool IsCorrect();
        public abstract bool HasCorrectAnswer();
        public bool First { get; private set; } = questionNumber == 0;
        public bool Final { get => isFinal; }
        public bool NotFirst { get => !First; }
        public bool NotFinal { get => !isFinal; }
        public int QuestionNumber { get; private set; } = questionNumber;

        public abstract QuestionType Type { get; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public abstract void SetGivenAnswer(object givenAnswer);
    }

    public enum QuestionType {
        MultipleChoice, FillBlank
    }
}
