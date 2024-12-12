using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace QuizzingApp341.Models;

[Table("active_questions")]
public class ActiveQuestion : BaseModel {
    [PrimaryKey("id")]
    public long Id { get; set; }

    [Column("active_quiz_id")]
    public long ActiveQuizId { get; set; }

    [Column("question_id")]
    public long QuestionId { get; set; }

    [Column("question_no")]
    public int QuestionNo { get; set; }

    [Column("question_type")]
    public QuestionType QuestionType { get; set; }

    [Column("question")]
    public string Question { get; set; } = string.Empty;

    [Column("multiple_choice_options")]
    public string[]? MultipleChoiceOptions { get; set; }
    // public bool IsStudying { get; set; } COMMENTED OUT BECAUSE I COULDN'T ADD TO active_questions TABLE

    public ActiveQuestion() {}
    public ActiveQuestion(Question question, long activeQuizId) {
        ActiveQuizId = activeQuizId;
        QuestionId = question.Id;
        QuestionNo = question.QuestionNo;
        QuestionType = question.QuestionType;
        Question = question.QuestionText;
        MultipleChoiceOptions = question.MultipleChoiceOptions;
    }

    [Column("multiselect")]
    public bool? Multiselect { get; set; }
    public bool IsStudying { get; set; }
}