using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QuizzingApp341.Models {
    public abstract class Question(int questionNumber, string questionText) : INotifyPropertyChanged {
        public virtual string Text { get; private set; } = questionText;
        public abstract bool IsCorrect();
        public abstract bool HasCorrectAnswer();
        public bool First { get; set; }
        public bool Final { get; set; }
        public bool NotFinal { get; set; }
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
