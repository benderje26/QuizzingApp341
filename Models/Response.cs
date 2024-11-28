using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace QuizzingApp341.Models;

[Table("responses")]
public class Response : BaseModel {
    [PrimaryKey("active_quiz_id", true)]
    public long ActiveQuizId { get; set; }

    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("question_no")]
    public int QuestionNo { get; set; }

    [Column("multiple_choice_response")]
    public int[]? MultipleChoiceResponse { get; set; }

    [Column("fill_blank_response")]
    public string? FillBlankResponse { get; set; }
}