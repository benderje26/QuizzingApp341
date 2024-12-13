using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;



namespace QuizzingApp341.Models {
    [Table("questions")]
    public class Question : BaseModel, INotifyPropertyChanged {
        private long id;
        private int questionNo;
        private QuestionType questionType;
        private string questionText = string.Empty;
        private string[]? acceptableAnswers;
        private string[]? multipleChoiceOptions;
        private bool? caseSensitive;
        private bool? multiselect;
        private long quizId;
        private int[]? multipleChoiceCorrectAnswers;

        [PrimaryKey("id")]
        public long Id {
            get => id;
            set {
                id = value;
                OnPropertyChanged();
            }
        }

        [Column("question_no")]
        public int QuestionNo {
            get => questionNo;
            set {
                questionNo = value;
                OnPropertyChanged();
            }
        }


        [Column("question_type")]
        public QuestionType QuestionType {
            get => questionType;
            set {
                questionType = value;
                OnPropertyChanged();
            }
        }

        [Column("question")]
        public string QuestionText {
            get => questionText;
            set {
                questionText = value ?? string.Empty;
                OnPropertyChanged();
            }
        }

        [Column("acceptable_answers")]
        public string[]? AcceptableAnswers {
            get => acceptableAnswers;
            set {
                acceptableAnswers = value;
                OnPropertyChanged();
            }
        }

        [Column("multiple_choice_options")]
        public string[]? MultipleChoiceOptions {
            get => multipleChoiceOptions;
            set {
                multipleChoiceOptions = value;
                OnPropertyChanged();
            }
        }

        [Column("case_sensitive")]
        public bool? CaseSensitive {
            get => caseSensitive;
            set {
                caseSensitive = value;
                OnPropertyChanged();
            }
        }

        [Column("multiselect")]
        public bool? Multiselect {
            get => multiselect;
            set {
                multiselect = value;
                OnPropertyChanged();
            }
        }

        [Column("quiz_id")]
        public long QuizId {
            get => quizId;
            set {
                quizId = value;
                OnPropertyChanged();
            }
        }

        [Column("multiple_choice_correct_answers")]
        public int[]? MultipleChoiceCorrectAnswers {
            get => multipleChoiceCorrectAnswers;
            set {
                multipleChoiceCorrectAnswers = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public enum QuestionType : short {
        MultipleChoice = 0, FillBlank = 1
    }
}
