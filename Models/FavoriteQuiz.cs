using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace QuizzingApp341.Models;

// This class corresponds to the favorite_quizzes table in db
[Table("favorite_quizzes")]
    public class FavoriteQuiz : BaseModel {
        [PrimaryKey("user_id")]
        public Guid UserId { get; set; }

        [PrimaryKey("quiz_id")]
        public long QuizId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }