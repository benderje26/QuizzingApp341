namespace QuizzingApp341.Models;
using Supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;
using Supabase.Gotrue.Exceptions;
//using AndroidX.Activity;
using System.Collections.ObjectModel;
//using AVFoundation;
using QuizzingApp341.Views;
using System.Linq;
using System.Diagnostics;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Responses;
using CommunityToolkit.Maui.Core.Extensions;
using Supabase.Realtime.PostgresChanges;
using Supabase.Realtime.Interfaces;
using Supabase.Postgrest;
using Newtonsoft.Json.Linq;

public class SupabaseDatabase : IDatabase {

    #region Auth and User
    private const string REST_URL = "https://tcogwlqjinvzckjmnjhp.supabase.co";
    private const string API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InRjb2d3bHFqaW52emNram1uamhwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjkzNjc4NTIsImV4cCI6MjA0NDk0Mzg1Mn0.bzYJIRYJ3rtvEh3usrydy7M3ES1J6C5iMgPwzlqnTp8";

    private Client Client { get; set; }

    private UserInfo? userInfo;

    private Session? Session {
        get => _session;
    }
    private Session? _session;

    private async Task SetSession(Session? s) {
        _session = s;
        await SetUser(s?.User);
    }

    private async Task SetUser(User? u) {
        _user = u;
        UserId = u?.Id == null ? Guid.Empty : new Guid(u.Id);
        await GenerateUserInfo();
    }

    private User? User {
        get => _user;
    }
    private User? _user;

    private Guid UserId { get; set; }

    private async Task GenerateUserInfo() {
        userInfo = new(UserId, User != null) {
            CreatedQuizzes = new ObservableCollection<Quiz>((User == null ? null : await GetUserCreatedQuizzes(UserId)) ?? []),
            FavoriteQuizzes = new ObservableCollection<Quiz>((User == null ? null : await GetFavoriteQuizzes()) ?? [])
        };
    }

    public async Task SkipLogin() {
        await SetSession(null);
    }

    public UserInfo? GetUserInfo() {
        return userInfo;
    }

    public SupabaseDatabase() {
        Client = new(REST_URL, API_KEY, new SupabaseOptions {
            AutoConnectRealtime = true
        });
        _ = Initialize();
    }

    public async Task Initialize() {
        // initialize Supabase
        await Client.InitializeAsync();
    }

    public async Task<AccountCreationResult> CreateNewUser(string emailAddress, string username, string password) {
        try {
            await SetSession(await Client.Auth.SignUp(emailAddress, password,
                new SignUpOptions() {
                    Data = new Dictionary<string, object> {
                        { "username", username }
                    }
                }));
            return Session == null ? AccountCreationResult.NetworkError : AccountCreationResult.Success;
        } catch (Exception) {
            return AccountCreationResult.Other;
        }
    }

    public async Task<LoginResult> Login(string emailAddress, string password) {
        try {
            await SetSession(await Client.Auth.SignInWithPassword(emailAddress, password));
            return Session == null ? LoginResult.Other : LoginResult.Success;
        } catch (Exception e) {
            if (e is GotrueException ge && ge.Reason == FailureHint.Reason.UserBadLogin) {
                return LoginResult.BadCredentials;
            }
            return LoginResult.Other;
        }
    }

    public async Task<LogoutResult> Logout() {
        if (Client == null) {
            return LogoutResult.Other;
        }
        try {
            await Client.Auth.SignOut();
            await SetSession(null);
            return LogoutResult.Success;
        } catch (Exception) {
            return LogoutResult.NetworkError;
        }
    }

    #endregion

    #region Quizzes 

    public async Task<List<Quiz>?> GetAllQuizzesAsync() {
        try {
            var result = await Client.From<Quiz>().Get();
            return result?.Models;
        } catch {
            return null;
        }
    }

    public async Task<Quiz?> GetQuizById(long id) {
        try {
            Quiz? quiz = await Client
                .From<Quiz>()
                .Where(q => q.Id == id)
                .Single();
            if (quiz == null) {
                return null;
            } else {
                return quiz;
            }
        } catch (Exception) {
            return null;
        }
    }

    public async Task<List<Question>?> GetQuestions(long id) {
        try {
            var result = await Client
                .From<Question>()
                .Where(q => q.QuizId == id)
                .Get();
                
            if (result == null) {
                return null;
            }
            return result.Models; // Return the list of supabase questions
        } catch {
            return null;
        }
    }


    public async Task<long?> AddQuestion(Question question) {
        try {
            var result = await Client
                .From<Question>()
                .Insert(question);

            if (result == null || result.Model == null) {
                return null;
            }

            return result.Model.Id;
        } catch {
            return null;
        }
    }

    public async Task<bool> DeleteQuestion(long id) {
        try {
            await Client
            .From<Question>()
            .Where(q => q.Id == id)
            .Delete();
        } catch {
            return false;
        }
        return true;
    }

    public async Task<bool> EditQuestion(Question question) {
        try {
            await Client
            .From<Question>()
            .Upsert(question);
        } catch {
            return false;
        }
        return true;
    }

    public async Task<List<Quiz>?> GetUserCreatedQuizzes(Guid? userId) {
        try {
            var result = await Client
                .From<Quiz>()
                .Where(q => q.CreatorId == userId)
                .Get();

            return result?.Models;
        } catch (Exception e) {
            Console.Write("ERRORRRRR" + e);
            return null;
        }
    }

    #region Active Quizzes
    public async Task<ActiveQuiz?> GetActiveQuiz(string accessCode) {
        try {
            return await Client
                .From<ActiveQuiz>()
                .Where(q => q.AccessCode == accessCode)
                .Single();
        } catch (Exception) {
            return null;
        }
    }

    // Get active_quiz_id by pass in user_id to participants table
    public async Task<List<long?>> GetActiveQuizIdsByUserId() {
        try {
            // Log the userId being passed to the method
            Console.WriteLine($"Fetching active quiz IDs for user: {UserId}");

            // Query the participants table for the provided userId
            var participants = await Client
                .From<Participants>()
                .Where(p => p.UserId == UserId)
                .Get();

            Console.WriteLine($"Supabase Response: {participants?.Models?.Count} records returned");
            Console.WriteLine($"active_quiz_id:{participants?.Models}");
            if (participants?.Models == null || !participants.Models.Any()) {
                Console.WriteLine($"No participants found for user {UserId}");
                return null; // No active quizzes found for the user
            }

            // Return the list of ActiveQuizIds
            return participants.Models.Select(p => p.ActiveQuizId).ToList();
        } catch (Exception ex) {
            Console.WriteLine($"Error fetching active quiz ids for user {UserId}: {ex.Message}");
            return null;
        }
    }


    // Fetch quiz_id by use active_quiz_id from active_quizzes table
    public async Task<List<ActiveQuiz>?> GetQuizIdsByActiveQuizIds(List<long?> activeQuizIds) {
        try {
            Console.WriteLine($"print user ID (GetActiveQuizzesByActiveQuizIds): {UserId}");
            Console.WriteLine($"print input activeQuizIds (GetActiveQuizzesByActiveQuizIds): {string.Join(", ", activeQuizIds)}");

            if (activeQuizIds == null || !activeQuizIds.Any()) {
                Console.WriteLine("No quiz IDs provided.");
                return null;
            }
            var result = await Client
                .From<ActiveQuiz>()
                .Where(x => x.Activator == UserId)
                .Filter(x => x.Id, Supabase.Postgrest.Constants.Operator.In, activeQuizIds)
                .Get();

            Console.WriteLine($"Total records matching Activator == UserId: {result?.Models?.Count ?? 0}");
            Console.WriteLine($"Fetched active quizzes (GetActiveQuizzesByActiveQuizIds): {result?.Models?.Count ?? 0} entries");

            if (result?.Models == null || !result.Models.Any()) {
                Console.WriteLine("No matching active quizzes found.");
                return null;
            }

            return result.Models;
        } catch (Exception ex) {
            Console.WriteLine($"Error fetching active quizzes: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> SubmitMultipleChoiceQuestionAnswer(ActiveQuestion question, int choice) {
        try {
            await Client
                .From<Response>()
                .Upsert(new Response() {
                    ActiveQuizId = question.ActiveQuizId,
                    UserId = UserId,
                    QuestionNo = question.QuestionNo,
                    MultipleChoiceResponse = [choice],
                    FillBlankResponse = null
                }, new QueryOptions() { Returning = QueryOptions.ReturnType.Minimal });
            return true;
        } catch (Exception) {
            return false;
        }
    }

    public async Task<bool> SubmitFillBlankQuestionAnswer(ActiveQuestion question, string response) {
        try {
            await Client
                .From<Response>()
                .Upsert(new Response() {
                    ActiveQuizId = question.ActiveQuizId,
                    UserId = UserId,
                    QuestionNo = question.QuestionNo,
                    MultipleChoiceResponse = null,
                    FillBlankResponse = response
                }, new QueryOptions() { Returning = QueryOptions.ReturnType.Minimal });
            return true;
        } catch (Exception) {
            return false;
        }
    }

    public async Task<bool> JoinActiveQuiz(ActiveQuiz quiz, NewActiveQuestionHandler handler) {
        try {
            // TODO update participants table that we participated!
            // TODO get current question if it exists
            Client.Realtime.Channel("realtime", "public", "active_quizzes", null, "id=eq." + quiz.Id)
                .AddPostgresChangeHandler(PostgresChangesOptions.ListenType.Updates, async (channel, response) => {
                    ActiveQuiz? quiz = response.Model<ActiveQuiz>();
                    ActiveQuestion? question = quiz == null ? null : await GetCurrentActiveQuestion(quiz);

                    if (question == null) {
                        return;
                    }

                    handler(question);
                });
            return true;
        } catch (Exception) {
            return false;
        }
    }

    public async Task<ActiveQuestion?> GetCurrentActiveQuestion(ActiveQuiz quiz) {
        if (!quiz.IsActive ?? true || quiz.CurrentQuestionNo < 0) {
            return null;
        }
        long activeQuizId = quiz.Id ?? 0;
        int questionNo = quiz.CurrentQuestionNo;
        try {
            return await Client
                .From<ActiveQuestion>()
                .Where(x => x.ActiveQuizId == activeQuizId)
                .Where(x => x.QuestionNo == questionNo)
                .Single();
        } catch (Exception) {
            return null;
        }
    }
    public async Task<bool> ValidateAccessCode(string accessCode) {
        try {
            var result = await Client
                .From<ActiveQuiz>()
                .Where(a => a.AccessCode == accessCode)
                .Single();

            Console.WriteLine("****************************");
            Console.WriteLine(result?.IsActive ?? false);

            return result?.IsActive ?? false;
        } catch (Exception e) {
            Console.WriteLine("Error: " + e.Message);
            return false;
        }
    }

    #endregion
    #endregion

    #region Favorite Quizzes
    public async Task<ObservableCollection<Quiz>> GetFavoriteQuizzes() {
        try {
            ModeledResponse<FavoriteQuiz> result = await Client
                .From<FavoriteQuiz>()
                .Where(q => q.UserId == UserId)
                .Get();

            List<FavoriteQuiz> favQuizzes = result.Models;
            ObservableCollection<Quiz> quizzes = [];

            foreach (FavoriteQuiz favQuiz in favQuizzes) {
                Quiz? q = await GetQuizById(favQuiz.QuizId);
                if (q != null) {
                    quizzes.Add(q);
                }
            }

            return quizzes;
        } catch (Exception e) {
            Console.Write("ERRORRRRR" + e);
            return [];
        }
    }

    public async Task<long?> AddFavoriteQuiz(long quizId) {
        try {
            FavoriteQuiz f = new FavoriteQuiz {
                QuizId = quizId,
                UserId = UserId,
                CreatedAt = DateTime.Now
            };

            var result = await Client
                .From<FavoriteQuiz>()
                .Insert(f);


            if (result == null || result.Model == null) {
                return null;
            }

            var newQuiz = await GetQuizById(quizId);
            userInfo.FavoriteQuizzes.Add(newQuiz);
            return result.Model.QuizId;
        } catch (Exception e) {
            Console.Write("ERRORRRRR" + e);

            return null;
        }
    }

    public async Task<bool> DeleteFavoriteQuiz(long quizId) {
        try {
            await Client
            .From<FavoriteQuiz>()
            .Where(q => q.QuizId == quizId)
            .Delete();
        } catch (Exception e) {
            Console.Write("ERRORRRRR" + e);
            return false;
        }

        ObservableCollection<Quiz> favQuizzess = await GetFavoriteQuizzes();
        userInfo.FavoriteQuizzes.Clear();
        foreach (Quiz quiz in favQuizzess) {
            userInfo.FavoriteQuizzes.Add(quiz);
        }
        return true;
    }
    #endregion

}
