namespace QuizzingApp341.Models;
using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using Supabase.Postgrest;
using Supabase.Postgrest.Responses;
using Supabase.Realtime.PostgresChanges;
using System.Collections.ObjectModel;
using System.Linq;
using Client = Supabase.Client;
using Constants = Supabase.Postgrest.Constants;

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
            await SetSession(await Client.Auth.SignUp(emailAddress, password));

            if (Session != null) {
                UserData myData = new() {
                    UserId = UserId,
                    Username = username
                };

                var result = await Client
                    .From<UserData>()
                    .Insert(myData);

                return result?.Model == null ? AccountCreationResult.Other : AccountCreationResult.Success;
            }
            return AccountCreationResult.Other;
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

    public async Task<UserData?> GetUserData(Guid userId) {
        try {
            return await Client
                .From<UserData>()
                .Where(x => x.UserId == userId)
                .Single();
        } catch (Exception e) {
            Console.Write("ERORRRR" + e.Message);
            return null;
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
            // Get an active quiz from the table based on the access code
            return await Client
                .From<ActiveQuiz>()
                .Where(q => q.AccessCode == accessCode) // Make sure the access code matches
                .Single(); // Return the first active quiz

        } catch (Exception) {
            return null; // There was no active quiz
        }
    }

    // Get active_quiz_id by pass in user_id to participants table
    public async Task<List<long>> GetActiveQuizIdsByUserId() {
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
                return []; // No active quizzes found for the user
            }

            // Return the list of ActiveQuizIds
            return participants.Models.Select(p => p.ActiveQuizId).ToList();
        } catch (Exception ex) {
            Console.WriteLine($"Error fetching active quiz ids for user {UserId}: {ex.Message}");
            return null;
        }
    }


    // Fetch quiz_id by use active_quiz_id from active_quizzes table
    public async Task<List<ActiveQuiz>?> GetQuizIdsByActiveQuizIds(List<long> activeQuizIds) {
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
            await Client // Insert into the response table according to the choice selected
                .From<Response>()
                .Insert(new Response() {
                    ActiveQuizId = question.ActiveQuizId,
                    UserId = UserId,
                    QuestionNo = question.QuestionNo,
                    MultipleChoiceResponse = [choice],
                    FillBlankResponse = null // Null because this is multiple choice
                }, new QueryOptions() { Returning = QueryOptions.ReturnType.Minimal });
            return true;
        } catch (Exception e) {
            Console.WriteLine("ERRORRRR" + e.Message);
            return false; // Inserting into responses failed
        }
    }

    public async Task<bool> SubmitFillBlankQuestionAnswer(ActiveQuestion question, string response) {
        try {
            await Client // Insert into the responses table according to the response given
                .From<Response>()
                .Insert(new Response() {
                    ActiveQuizId = question.ActiveQuizId,
                    UserId = UserId,
                    QuestionNo = question.QuestionNo,
                    MultipleChoiceResponse = null, // Null because this is a fill in the blank question
                    FillBlankResponse = response
                }, new QueryOptions() { Returning = QueryOptions.ReturnType.Minimal });
            return true;
        } catch (Exception e) {
            return false; // Inserting into responses failed
        }
    }

    public async Task<bool> JoinActiveQuiz(ActiveQuiz quiz, NewActiveQuestionHandler handler) {
        try {
            // TODO update participants table that we participated!
            // TODO get current question if it exists
            // Create a channel for the active quiz
            var channel = Client.Realtime.Channel("realtime", "public", "active_quizzes", null, "id=eq." + quiz.Id);

            // Add a handler to the channel
            channel.AddPostgresChangeHandler(PostgresChangesOptions.ListenType.Updates, async (channel, response) => {
                ActiveQuiz? quiz = response.Model<ActiveQuiz>(); // Update the quiz with the new active question from the response

                // Get the current active question for the current quiz
                ActiveQuestion? question = quiz == null ? null : await GetCurrentActiveQuestion(quiz);

                if (question == null) {
                    return;
                }

                // Populate the question for the users
                handler(question);
            });

            await channel.Subscribe(); // Listen to any changes
            return true;
        } catch (Exception) {
            return false;
        }
    }

    public async Task<ActiveQuestion?> GetCurrentActiveQuestion(ActiveQuiz quiz) {
        // Check if the active question from the active quiz IS active
        long activeQuizId = quiz.Id;
        int questionNo = quiz.CurrentQuestionNo ?? -1;
        if (!quiz.IsActive ?? true || quiz.CurrentQuestionNo < 0) {
            return null;
        }

        try { // If the active question is active in db, return the active question
            return await Client
                .From<ActiveQuestion>()
                .Where(x => x.ActiveQuizId == activeQuizId) // Make sure the active quiz Id matches
                .Where(x => x.QuestionNo == questionNo) // Make sure the question number matches
                .Single();
        } catch (Exception) {
            return null;
        }
    }
    public async Task<bool> ValidateAccessCode(string accessCode) {
        try {
            // Check if the access code exists in the active quiz table
            int count = await Client
                .From<ActiveQuiz>()
                .Where(a => a.AccessCode == accessCode) // Make sure the access code matches
                .Where(a => a.IsActive == true) // Make sure it is active
                .Limit(1)
                .Count(Constants.CountType.Exact); // If count is more than one it exists

            return count > 0;
        } catch (Exception e) {
            Console.WriteLine("Error: " + e.Message);
            return false;
        }
    }
    #endregion
    #endregion

    #region Favorite Quizzes

    /// <summary>
    /// Returns all of the users favorite quizzess from the database.
    /// </summary>
    /// <returns>
    /// Returns all of the quizzes that the user has favorited
    /// </returns>
    public async Task<ObservableCollection<Quiz>> GetFavoriteQuizzes() {
        try {
            //Get all the favorited quizzess from the database
            ModeledResponse<FavoriteQuiz> result = await Client
                .From<FavoriteQuiz>()
                .Where(q => q.UserId == UserId)
                .Get();

            //Store the favorited quizzess into a variable
            List<FavoriteQuiz> favQuizzes = result.Models;
            //Create a new collection of Quiz, so that the actual information of the favorited quizzess can be accessed
            ObservableCollection<Quiz> quizzes = [];

            //Loop through all of the favorited quizzess
            foreach (FavoriteQuiz favQuiz in favQuizzes) {
                //Get all of the quiz information for the favorited quiz
                Quiz? q = await GetQuizById(favQuiz.QuizId);
                //Make sure that the quiz returned is valid
                if (q != null) { 
                    //Add it to the array to be returned
                    quizzes.Add(q);
                }
            }
            //Return all the quiz information for the favorited quizzess
            return quizzes;
        } catch (Exception e) {
            //Write out the error if if occurred and return an empty collection
            Console.Write("ERRORRRRR" + e);
            return [];
        }
    }

    /// <summary>
    /// Adds a favorite quiz to the database.
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns>
    /// Returns the id to that favorite quiz after it gets added to the db otherwise null
    /// </returns>
    public async Task<long?> AddFavoriteQuiz(long quizId) {
        try {
            //Create the new favorite quiz to be added to the database
            FavoriteQuiz f = new FavoriteQuiz {
                QuizId = quizId,
                UserId = UserId,
                CreatedAt = DateTime.Now
            };

            //Add the favorited quiz to the database
            var result = await Client
                .From<FavoriteQuiz>()
                .Insert(f);

            //Check to see that the favorited quiz added successfully
            if (result == null || result.Model == null) {
                return null;
            }

            //Get all the information about the new favorited quiz
            var newQuiz = await GetQuizById(quizId);
            //Update the users info that they added the favorite quiz
            userInfo.FavoriteQuizzes.Add(newQuiz);
            return result.Model.QuizId;
        } catch (Exception e) {
            //Write out the error if if occurred and return null to represent a failed add
            Console.Write("ERRORRRRR" + e);
            return null;
        }
    }

    /// <summary>
    /// Deletes a favorite quiz from the database.
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns>
    /// Returns whether the favorite quiz was successfully deleted
    /// </returns>
    public async Task<bool> DeleteFavoriteQuiz(long quizId) {
        try {
            //Delete the favorited quiz from the database
            await Client
            .From<FavoriteQuiz>()
            .Where(q => q.QuizId == quizId)
            .Delete();

            //Get the users favorite quizzes after the deletion
            ObservableCollection<Quiz> favQuizzess = await GetFavoriteQuizzes();
            //Clear the users favorited quizzes and readd the current list of favorites
            userInfo.FavoriteQuizzes.Clear();
            foreach (Quiz quiz in favQuizzess) {
                userInfo.FavoriteQuizzes.Add(quiz);
            }
            return true;
        } catch (Exception e) {
            //Write out the error if if occurred and return null to represent a failed add
            Console.Write("ERRORRRRR" + e);
            return false;
        }
    }
    #endregion
}
