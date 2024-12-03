using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace QuizzingApp341.Models;

[Table("quizzes")]
public class Quiz : BaseModel {
    [PrimaryKey("id")]
    public long Id { get; set; }

    [Column("creator_id")]
    public Guid CreatorId { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;
}