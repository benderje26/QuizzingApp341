namespace QuizzingApp341.Models;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Responses;

class SupabaseDatabase : IDatabase {

    private const string REST_URL = "https://tcogwlqjinvzckjmnjhp.supabase.co";
    private const string API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InRjb2d3bHFqa"
        + "W52emNram1uamhwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjkzNjc4NTIsImV4cCI6MjA0NDk0Mzg1Mn0.bzYJIRYJ3rtvEh3usrydy7M3"
        + "ES1J6C5iMgPwzlqnTp8";

    private Client Client { get; set; }

    public SupabaseDatabase() {
        Client = new(REST_URL, API_KEY, new SupabaseOptions {
            AutoConnectRealtime = false // may need to be changed
        });
    }

    public async void Initialize() {
        // initialize Supabase
        await Client.InitializeAsync();
    }

    public async Task<List<Question>> LoadQuestions() {
        SupabaseQuiz? quiz = await Client
            .From<SupabaseQuiz>()
            .Where(x => x.Id == 0)
            .Single();
        ModeledResponse<SupabaseQuestion> questionsResult = await Client
            .From<SupabaseQuestion>()
            .Where(x => x.Quiz == quiz)
            .Get();
        List<Question> questions = [];
        foreach (SupabaseQuestion sq in questionsResult.Models) {
            if (sq.Choices != null) {
                questions.Add(new MultipleChoiceQuestion(sq.QuestionNumber, sq.Title ?? string.Empty, sq.Choices, sq.CorrectAnswer));
            } else {
                questions.Add(new FillBlankQuestion(sq.QuestionNumber, sq.Title ?? string.Empty, sq.AcceptedTextAnswers, sq.CaseSensitive ?? false));
            }
        }
        questions.Sort((x, y) => x.QuestionNumber.CompareTo(y.QuestionNumber));
        return questions;
    }

    [Table("USER")]
    public class SupabaseUser : BaseModel {
        [PrimaryKey("id")]
        public long? Id { get; set; }

        [Column("email_address")]
        public string? EmailAddress { get; set; }
    }

    [Table("QUIZ")]
    public class SupabaseQuiz : BaseModel {
        [PrimaryKey("id")]
        public long? Id { get; set; }

        [Column("creator")]
        public SupabaseUser? Creator { get; set; }

        [Column("title")]
        public string? Title { get; set; }
    }

    [Table("QUESTION")]
    public class SupabaseQuestion : BaseModel {
        [PrimaryKey("id")]
        public long? Id { get; set; }

        [Column("quiz")]
        public SupabaseQuiz? Quiz { get; set; }

        [Column("question_no")]
        public int QuestionNumber { get; set; }

        [Column("title")]
        public string? Title { get; set; }

        [Column("choices")]
        public List<string>? Choices { get; set; }

        [Column("correct_answer")]
        public int? CorrectAnswer { get; set; }

        [Column("accepted_text_answers")]
        public List<string>? AcceptedTextAnswers { get; set; }

        [Column("case_sensitive")]
        public bool? CaseSensitive { get; set; }
    }
}
