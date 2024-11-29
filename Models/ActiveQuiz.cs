using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace QuizzingApp341.Models;

[Table("active_quizzes")]
public class ActiveQuiz : BaseModel {
    [PrimaryKey("id")]
    public long? Id { get; set; }

    [Column("quiz_id")]
    public long QuizId { get; set; }

    [Column("access_code")]
    public string? AccessCode { get; set; }

    [Column("is_active")]
    public bool? IsActive { get; set; }

    [Column("current_question_no")]
    public int? CurrentQuestionNo { get; set; }

    [Column("activator")]
    public Guid? Activator { get; set; }

    [Column("start_time")]
    public DateTime StartTime { get; set; }

    [Column("end_time")]
    public DateTime EndTime { get; set; }
}