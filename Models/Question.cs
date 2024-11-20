using System.ComponentModel;
using System.Runtime.CompilerServices;
using Android.App.AppSearch;
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

        [Column("question_no")]
        public int QuestionNumber { get; set; }

        [Column("question_type")]
        public QuestionType QuestionType {get; set;}

        [Column("question_text")]
        public string? QuestionText { get; set; }

        [Column("acceptable_answers")]
        public List<string>? acceptableAnswers { get; set; }

        [Column("multiple_choice_options")]
        public List<string>? MultipleChoiceOptions { get; set; }

        [Column("case_sensitive")]
        public bool? CaseSensitive { get; set; }
    }

    public enum QuestionType : int {
        MultipleChoice = 0, FillBlank = 1
    }
}
