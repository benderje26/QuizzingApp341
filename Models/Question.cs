using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

// This class corresponds to the questions table in db
namespace QuizzingApp341.Models {
    [Table("questions")]
    public class Question : BaseModel {
        [PrimaryKey("id")]
        public long? Id { get; set; }

        [Column("question_no")]
        public int QuestionNum { get; set; }

        [Column("question_type")]
        public QuestionType QuestionType { get; set; } // TODO Convert to QuestionType Enum?? It's coming in as a string from the table.. Or we can just use this without the QuestionType enum

        [Column("question")]
        public string? QuestionText { get; set; }

        [Column("acceptable_answers")]
        public List<string>? acceptableAnswers { get; set; }

        [Column("multiple_choice_options")]
        public List<string>? MultipleChoiceOptions { get; set; }

        [Column("case_sensitive")]
        public bool? CaseSensitive { get; set; }

        [Column("quiz_id")]
        public long QuizId { get; set; }

        [Column("multiple_choice_correct_answers")]
        public List<string>? MultipleChoiceAnswers { get; set; }
    }

    public enum QuestionType : short {
        MultipleChoice = 0, FillBlank = 1
    }
}
