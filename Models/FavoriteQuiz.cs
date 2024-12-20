﻿using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace QuizzingApp341.Models;

[Table("favorite_quizzes")]
public class FavoriteQuiz : BaseModel {
    [PrimaryKey("user_id", true)]
    public Guid UserId { get; set; }

    [PrimaryKey("quiz_id", true)]
    public long QuizId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
}