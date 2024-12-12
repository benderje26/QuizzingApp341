﻿using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace QuizzingApp341.Models; 

[Table("participants")]
public class Participant : BaseModel {
    [PrimaryKey("active_quiz_id")]
    public long ActiveQuizId { get; set; }

    [PrimaryKey("user_id")]
    public Guid UserId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public ActiveQuiz? ActiveQuiz { get; set; }

    public bool IsUserOwner { get; set; }
}