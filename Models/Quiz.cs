using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace QuizzingApp341.Models;

// This class corresponds to the quizzes table in db
[Table("quizzes")]
    public class Quiz : BaseModel {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("creator_id")]

        public string? CreatorId { get; set; }

        [Column("title")]
        public string? Title { get; set; }
    }