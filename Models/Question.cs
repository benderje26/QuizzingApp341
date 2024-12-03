using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace QuizzingApp341.Models {
    [Table("questions")]
    public class Question : BaseModel {
        [PrimaryKey("id")]
        public long Id { get; set; }

        [Column("question_no")]
        public int QuestionNo { get; set; }

        [Column("question_type")]
        public QuestionType QuestionType { get; set; }

        [Column("question")]
        public string? QuestionText { get; set; } = null;

        [Column("acceptable_answers")]
        public string[]? AcceptableAnswers { get; set; }

        [Column("multiple_choice_options")]
        public string[]? MultipleChoiceOptions { get; set; }

        [Column("case_sensitive")]
        public bool? CaseSensitive { get; set; }

        [Column("quiz_id")]
        public long QuizId { get; set; }

        [Column("multiple_choice_correct_answers")]
        public int[]? MultipleChoiceCorrectAnswers { get; set; }
    }

    public enum QuestionType : short {
        MultipleChoice = 0, FillBlank = 1
    }
}
