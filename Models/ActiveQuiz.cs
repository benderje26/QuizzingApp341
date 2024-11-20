using Java.Sql;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace QuizzingApp341.Models;

// This class corresponds to the active_quizzes table in db

[Table ("active_quizzes")]
    public class ActiveQuiz : BaseModel {
        [PrimaryKey("id")]
        public long Id {get; set;}

        [Column("quiz_id")]
        public long QuizId {get; set;}

        [Column("access_code")]
        public string? AccessCode {get; set;}

        [Column("is_active")]
        public bool IsActive{get; set;}

        [Column("current_question_no")]
        public int CurrentQuestionNum {get; set;}

        [Column("activator")]
        public string? Activator {get; set;}

        [Column("start_time")]
        public DateTime startTime {get; set;}

        [Column("end_time")]
        public DateTime endTime {get; set;}
    }