namespace QuizzingApp341.Models;
using Supabase;
using Supabase.Gotrue;
using Client = Supabase.Client;
using Supabase.Gotrue.Exceptions;
//using AndroidX.Activity;
using System.Collections.ObjectModel;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Responses;
using CommunityToolkit.Maui.Core.Extensions;

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
        if (User == null) {
            userInfo = null;
        }
        UserInfo info = new(UserId) {
            CreatedQuizzes = new ObservableCollection<Quiz>(await GetUserCreatedQuizzes(UserId) ?? []),
            FavoriteQuizzes = new ObservableCollection<Quiz>(await GetFavoriteQuizzes() ?? [])
        };
        userInfo = info;
    }

    public async Task SkipLogin() {
        await GenerateUserInfo(); // TODO DELETE THIS METHOD WHEN LOGIN WORKS
    }

    public UserInfo? GetUserInfo() {
        return userInfo;
    }

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
            await SetUser(null);
            return LogoutResult.Success;
        } catch (Exception) {
            return LogoutResult.NetworkError;
        }
    }

    #endregion

    public async Task<List<Quiz>?> GetAllQuizzesAsync() {
        try {
            var result = await Client.From<Quiz>().Get();
            return result?.Models;
        } catch {
            return null;
        }
    }

    #region Quizzes
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
                .Where(q => q.Id == id).Get();

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
        } catch (Exception e){
            Console.Write("ERRORRRRR" + e);
            return null;
        }
    }

    public async Task<ObservableCollection<Quiz>> GetFavoriteQuizzes() {
        try {
            //Quiz first = new Quiz();
            //first.Title = "first";
            //first.Id = 0;
            //Quiz two = new Quiz();
            //two.Title = "two";
            //two.Id = 1;
            //ObservableCollection<Quiz> fq = new ObservableCollection<Quiz>();
            //fq.Add(first);
            //fq.Add(two);
            //return fq;

            //This is the code that will be used to no longer hard code the data but currently not working

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
            FavoriteQuiz f = new FavoriteQuiz();
            f.QuizId = quizId;
            f.UserId = UserId;
            f.CreatedAt = DateTime.Now;
            var result = await Client
                .From<FavoriteQuiz>()
                .Insert(f);

            if (result == null || result.Model == null) {
                return null;
            }

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
        return true;
    }
    #endregion
}
