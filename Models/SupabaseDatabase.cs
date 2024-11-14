namespace QuizzingApp341.Models;
using Supabase;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Responses;
using Supabase.Gotrue;
using Client = Supabase.Client;
using Supabase.Gotrue.Exceptions;
using System.Net.Mail;

public class SupabaseDatabase : IDatabase {

    private const string REST_URL = "https://tcogwlqjinvzckjmnjhp.supabase.co";
    private const string API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InRjb2d3bHFqaW52emNram1uamhwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjkzNjc4NTIsImV4cCI6MjA0NDk0Mzg1Mn0.bzYJIRYJ3rtvEh3usrydy7M3ES1J6C5iMgPwzlqnTp8";

    private Client Client { get; set; }

    private Session? Session {
        get => _session; 
        set {
            _session = value;
            User = value?.User;
        }
    }
    private Session? _session;

    private User? User {
        get => _user;
        set {
            _user = value;
            UserId = value?.Id == null ? Guid.Empty : new Guid(value.Id);
        }
    }
    private User? _user;

    private Guid UserId { get; set; }

    public SupabaseDatabase() {
        Client = new(REST_URL, API_KEY, new SupabaseOptions {
            AutoConnectRealtime = false // may need to be changed
        });
        _ = Initialize();
    }

    public async Task Initialize() {
        // initialize Supabase
        await Client.InitializeAsync();
    }

    public async Task<AccountCreationResult> CreateNewUser(string emailAddress, string username, string password) {
        try {
            Session = await Client.Auth.SignUp(emailAddress, password,
                new SignUpOptions() {
                    Data = new Dictionary<string, object> {
                        { "username", username }
                    }
                });
            return Session == null ? AccountCreationResult.NetworkError : AccountCreationResult.Success;
        } catch (Exception) {
            return AccountCreationResult.NetworkError;
        }
    }

    public async Task<LoginResult> Login(string emailAddress, string password) {
        try {
            Session = await Client.Auth.SignInWithPassword(emailAddress, password);
            return Session == null ? LoginResult.Other : LoginResult.Success;
        } catch (Exception e) {
            if (e is GotrueException ge && ge.Reason == FailureHint.Reason.UserBadLogin) {
                return LoginResult.BadCredentials;
            }
            return LoginResult.NetworkError;
        }
    }

    public async Task<LogoutResult> Logout() {
        if (Client == null) {
            return LogoutResult.Other;
        }
        try {
            await Client.Auth.SignOut();
            return LogoutResult.Success;
        } catch (Exception) {
            return LogoutResult.NetworkError;
        }
    }

    public async Task<Quiz?> GetQuizById(string id) {
        try {
            SupabaseQuiz? quiz = await Client
                .From<SupabaseQuiz>()
                .Where(q => q.Identifier == id)
                .Single();
            if (quiz == null) {
                return null;
            }
            ModeledResponse<SupabaseQuestion> questionsResult = await Client
                .From<SupabaseQuestion>()
                .Where(q => q.QuizId == quiz.Identifier)
                .Get();
            List<Question> questions = [];
            for (int i = 0, length = questionsResult.Models.Count; i < length; i++) {
                SupabaseQuestion sq = questionsResult.Models[i];
                if (sq.Choices != null) {
                    questions.Add(new MultipleChoiceQuestion(sq.QuestionNumber, i == length - 1, sq.Title ?? string.Empty, sq.Choices, sq.CorrectAnswer));
                } else {
                    questions.Add(new FillBlankQuestion(sq.QuestionNumber, i == length - 1, sq.Title ?? string.Empty, sq.AcceptedTextAnswers, sq.CaseSensitive ?? false));
                }
            }
            questions.Sort((x, y) => x.QuestionNumber.CompareTo(y.QuestionNumber));

            Quiz result = new(quiz.Title ?? string.Empty, DateTime.Now, DateTime.Now, null, questions);
            return result;
        } catch (Exception) {
            return null;
        }
    }

    [Table("quizzes")]
    public class SupabaseQuiz : BaseModel {
        [PrimaryKey("id")]
        public long Id { get; set; }

        [Column("identifier")]
        public string? Identifier { get; set; }

        [Column("creator_id")]
        public Guid CreatorId { get; set; }

        [Column("title")]
        public string? Title { get; set; }
    }

    [Table("questions")]
    public class SupabaseQuestion : BaseModel {
        [PrimaryKey("id")]
        public long Id { get; set; }

        [Column("quiz_identifier")]
        public string? QuizId { get; set; }

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
