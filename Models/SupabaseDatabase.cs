namespace QuizzingApp341.Models;

using QuizzingApp341.Views;
using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using Supabase.Postgrest;
using Supabase.Postgrest.Exceptions;
using Supabase.Postgrest.Responses;
using Supabase.Realtime.PostgresChanges;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using Client = Supabase.Client;
using Constants = Supabase.Postgrest.Constants;

public class SupabaseDatabase : IDatabase {

    #region Auth and User
    // The Supabase url and key (is okay to be public)
    private const string REST_URL = "https://tcogwlqjinvzckjmnjhp.supabase.co";
    private const string API_KEY = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InRjb2d3bHFqaW52emNram1uamhwIiwicm9sZSI6ImFub24iLCJpYXQiOjE3MjkzNjc4NTIsImV4cCI6MjA0NDk0Mzg1Mn0.bzYJIRYJ3rtvEh3usrydy7M3ES1J6C5iMgPwzlqnTp8";

    /// <summary>
    /// The current Supabase client
    /// </summary>
    private Client Client { get; set; }

    // The current user's info
    private UserInfo? userInfo;

    /// <summary>
    /// The current Supabase session
    /// </summary>
    private Session? Session {
        get => _session;
    }
    private Session? _session;

    /// <summary>
    /// Sets the current session, or logs out.
    /// </summary>
    /// <param name="s">The new current session or null if logging out</param>
    private async Task SetSession(Session? s) {
        _session = s;
        await SetUser(s?.User);
    }

    /// <summary>
    /// Sets the current user.
    /// </summary>
    /// <param name="u">The new current user or null if logging out</param>
    private async Task SetUser(User? u) {
        _user = u;
        UserId = u?.Id == null ? Guid.Empty : new Guid(u.Id);
        await GenerateUserInfo();
    }

    /// <summary>
    /// The current Supabase User
    /// </summary>
    private User? User {
        get => _user;
    }
    private User? _user;

    /// <summary>
    /// The current user's ID
    /// </summary>
    private Guid UserId { get; set; }

    /// <summary>
    /// Regenerates the info for the current user.
    /// </summary>
    private async Task GenerateUserInfo() {
        UserData? data = await GetUserData(UserId);
        userInfo = new(UserId, User?.Email ?? string.Empty, data?.Username ?? string.Empty, User != null) {
            CreatedQuizzes = new ObservableCollection<Quiz>((User == null ? null : await GetUserCreatedQuizzes(UserId)) ?? []),
            FavoriteQuizzes = new ObservableCollection<Quiz>((User == null ? null : await GetFavoriteQuizzes()) ?? [])
        };
    }

    public async Task SkipLogin() {
        await SetSession(null); // setting the session to null sets things like UserId to null too
    }

    public UserInfo? GetUserInfo() {
        return userInfo;
    }

    public SupabaseDatabase() {
        Client = new(REST_URL, API_KEY, new SupabaseOptions {
            AutoConnectRealtime = true // connects now instead of when they start an active quiz
        });
        _ = Initialize(); // initializes in the background
    }

    public async Task Initialize() {
        // initialize Supabase
        await Client.InitializeAsync();
    }

    public async Task<AccountCreationResult> CreateNewUser(string emailAddress, string username, string password) {
        try {
            bool? alreadyExists = await HasUserData(username);
            if (alreadyExists == null) {
                return AccountCreationResult.Other;
            } else if (alreadyExists.Value) {
                return AccountCreationResult.DuplicateUsername;
            }

            // Calls Supabase to sign up and then sets the current session to that new user
            await Client.Auth.SignUp(emailAddress, password);
            await SetSession(await Client.Auth.SignIn(emailAddress, password));
        } catch (GotrueException e) {
            if (e.Reason == Supabase.Gotrue.Exceptions.FailureHint.Reason.UserAlreadyRegistered) {
                return AccountCreationResult.DuplicateEmail;
            }
            return AccountCreationResult.Other;
        } catch (Exception e) {
            Console.WriteLine("ERRORRRR:" + e);
            return AccountCreationResult.Other;
        }

        if (Session != null) {
            // If it worked, put the user's data into a UserData object
            UserData myData = new() {
                UserId = UserId,
                Username = username
            };

            // Puts it in the USER_DATA table, which is used for storing usernames
            try {
                var result = await Client
                    .From<UserData>()
                    .Insert(myData);

                // Returns if it was a success
                return result?.Model == null ? AccountCreationResult.Other : AccountCreationResult.Success;
            } catch (Exception e) {
                Console.WriteLine("ERRORRRR:" + e);

                // If we were unable to assign the username, delete the account
                await DeleteAccount();

                // Checks to see if it was a uniqueness problem. This would only happen if someone else created
                // an account inbetween the last check and the time that we inserted into the table
                if (e is PostgrestException pe) {
                    if (pe.Reason == Supabase.Postgrest.Exceptions.FailureHint.Reason.UniquenessViolation) {
                        return AccountCreationResult.DuplicateUsername;
                    }
                }
            }
        }

        return AccountCreationResult.Other;
    }

    public async Task<LoginResult> Login(string emailAddress, string password) {
        try {
            // Attempts to log in and set the session to what the user logged in as
            await SetSession(await Client.Auth.SignInWithPassword(emailAddress, password));

            // Retuns if it was a success
            return Session == null ? LoginResult.Other : LoginResult.Success;
        } catch (Exception e) {
            // Sees if it was a bad credentials error
            if (e is GotrueException ge && ge.Reason == Supabase.Gotrue.Exceptions.FailureHint.Reason.UserBadLogin) {
                return LoginResult.BadCredentials;
            }
            return LoginResult.Other;
        }
    }

    public async Task<LogoutResult> Logout() {
        if (Session == null) {
            // Can't log out if already logged out
            return LogoutResult.NotSignedIn;
        }
        try {
            // Signs out with supabase
            await Client.Auth.SignOut();

            // Sets variables like UserId to null
            await SetSession(null);
            return LogoutResult.Success;
        } catch (Exception) {
            return LogoutResult.NetworkError;
        }
    }

    /// <summary>
    /// Attempts to update the users email.
    /// </summary>
    /// <param name="emailAddress">The email address</param>
    /// <returns>The result of attempting to update the users email</returns>
    public async Task<UpdateEmailResult> UpdateEmail(string emailAddress) {
        if (Session == null) {
            return UpdateEmailResult.NotSignedIn;
        }
        try {
            //Update the users email
            var newEmail = new UserAttributes { Email = emailAddress };
            var result = await Client.Auth.Update(newEmail);

            if (userInfo != null) {
                userInfo.Email = emailAddress;
            }
            //Return that email was updated successfully
            return UpdateEmailResult.Success;
        } catch (GotrueException e) {
            // Sadly, the exception that is returned when there is a duplicate email
            // has the reason Reason.Unknown, so it is likely a duplicate email
            if (e.Reason == Supabase.Gotrue.Exceptions.FailureHint.Reason.Unknown) {
                return UpdateEmailResult.DuplicateEmail;
            }
        } catch (Exception e) {
            //Write out the error if if occurred
            Console.Write("ERRORRRRR" + e);
        }
        return UpdateEmailResult.Other;
    }

    /// <summary>
    /// Attempts to update the users username.
    /// </summary>
    /// <param name="username">The username</param>
    /// <returns>The result of attempting to update the users username</returns>
    public async Task<UpdateUsernameResult> UpdateUsername(string username) {
        if (Session == null) {
            return UpdateUsernameResult.NotSignedIn;
        }
        try {
            //Update the users username
            var result = await Client
                .From<UserData>()
                .Where(x => x.UserId == UserId)
                .Set(x => x.Username, username)
                .Update();

            if (userInfo != null) {
                userInfo.Username = username;
            }
            //Return that it was updated successfully
            return UpdateUsernameResult.Success;
        } catch (PostgrestException e) {
            if (e.Reason == Supabase.Postgrest.Exceptions.FailureHint.Reason.UniquenessViolation) {
                return UpdateUsernameResult.DuplicateUsername;
            }
        } catch (Exception e) {
            // Write out the error if if occurred
            Console.Write("ERRORRRRR" + e);
        }
        return UpdateUsernameResult.Other;
    }

    /// <summary>
    /// Attempts to update the users password.
    /// </summary>
    /// <param name="password">The password</param>
    /// <returns>The result of attempting to update the users password</returns>
    public async Task<UpdatePasswordResult> UpdatePassword(string password) {
        if (Session == null) {
            return UpdatePasswordResult.NotSignedIn;
        }
        try {
            //Update the users password
            var newPassword = new UserAttributes { Password = password };
            var result = await Client.Auth.Update(newPassword);

            //Return that is was updated successfully
            return UpdatePasswordResult.Success;
        } catch (Exception e) {
            //Write out the error if if occurred and return NetworkError to represent a failed update
            Console.Write("ERRORRRRR" + e);
            return UpdatePasswordResult.NetworkError;
        }
    }

    public async Task<UserData?> GetUserData(Guid userId) {
        try {
            // Gets a user's data (used for retrieving usernames currently)
            var user = await Client
                .From<UserData>()
                .Where(x => x.UserId == userId)
                .Single();
            return user;
        } catch (Exception e) {
            Console.Write("ERORRRR" + e.Message);
            return null;
        }
    }

    public async Task<bool?> HasUserData(string username) {
        try {
            // Gets a user's data by username
            UserData? data = await Client
                .From<UserData>()
                .Where(x => x.Username == username)
                .Single();

            return data != null;
        } catch (Exception e) {
            Console.Write("ERORRRR" + e.Message);
            return null;
        }
    }

    /// <summary>
    /// Attempts to delete the current users account
    /// </summary>
    /// <returns>The result of attempting to delete the account</returns>
    public async Task<DeleteAccountResult> DeleteAccount() {
        if (User == null) {
            return DeleteAccountResult.NotSignedIn;
        }
        try {
            // Make the delete_account stored procedure call
            await Client.Rpc("delete_account", null);
            await SetSession(null);
            return DeleteAccountResult.Success;
        } catch (Exception e) {
            Console.Write("ERRORRRRR" + e);
            return DeleteAccountResult.Other;
        }
    }

    #endregion

    #region Quizzes 

    #region Normal Quizzes
    //Get All the Quizzes for user
    //From Quiz Models to quizzes table in supabase
    //return List of Quiz 
    public async Task<List<Quiz>?> GetAllQuizzesAsync() {
        try {
            var result = await Client.From<Quiz>().Get();    //get all quizzes 
            return result?.Models;
        } catch {
            return null;                                // failed
        }
    }
  
    public async Task<bool> DeleteQuiz(long quizId) {
        try {
            await Client
                .From<Quiz>()
                .Where(q => q.Id == quizId)
                .Delete();
        } catch (Exception e) {
            Console.WriteLine("DELETE QUIZ ERROR:");
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }
    public async Task<bool> UpdateQuiz(Quiz quiz) {
        try {
            var result = await Client
                .From<Quiz>()
                .Upsert(quiz);

        } catch (Exception e) {
            return false;
        }

        return true;
    }

    public async Task<long?> AddQuiz(Quiz quiz) {
        try {
            var result = await Client
            .From<Quiz>()
            .Insert(quiz);

            return result.Model?.Id;
        } catch (Exception e) {
            Console.WriteLine("ERROR: " + e.Message);
            return null;
        }
    }

    //Get Quiz by Quiz id
    // From Quiz Models to quizzes table in supabase
    //<param name="id"></param>
    //return one Quiz that matched the id, otherwise return null
    public async Task<Quiz?> GetQuizById(long id) {
        try {
            Quiz? quiz = await Client
                .From<Quiz>()
                .Where(q => q.Id == id)  // check id
                .Single();
            if (quiz == null) {  // if not found
                return null;
            } else {
                return quiz;  // return quiz
            }
        } catch (Exception) {
            return null;
        }
    }

    public async Task<bool> UpdateQuestionNo(long questionId, int newQuestionNo) {
        try {
            var result = await Client
            .From<Question>()
            .Where(q => q.Id == questionId)
            .Set(q => q.QuestionNo, newQuestionNo)
            .Update();

        } catch (Exception e) {
            Console.WriteLine("Error: " + e.Message);
            return false;
        }
        return true;
    }

    //Get all the questions from that quiz id
    // From Question Model to question table in supabse 
    //  <param name="id"></param>
    // return List of questions corresponding to that quiz id
    public async Task<List<Question>?> GetQuestions(long id) {
        try {
            var result = await Client
                .From<Question>()
                .Where(q => q.QuizId == id)   // check if the quiz id exist
                .Get();                         // get all questions

            if (result == null) {            //no questions found
                return null;
            }
            return result.Models; // Return the list of supabase questions
        } catch {
            return null;
        }
    }


    // Add question 
    // From Question Model to question table in supabse 
    //<param name="question"></param>
    // return question id
    public async Task<long?> AddQuestion(Question question) {
        try {
            var result = await Client
                .From<Question>()
                .Insert(question);     // insert new question to supabase

            if (result == null || result.Model == null) {
                return null;
            }

            return result.Model.Id;      // return question id 
        } catch {
            return null;
        }
    }


    // Delete question by question id
    //From Question Model to question table in supabse 
    //<param name="id"></param>
    //return true is question got deleted, otherwise false
    public async Task<DeleteQuestionResult> DeleteQuestion(long id) {
        try {
            await Client
                .From<Question>()
                .Where(q => q.Id == id)  // if id match, delete that id
                .Delete();

            return DeleteQuestionResult.Success;
        } catch (Exception e) {
            return DeleteQuestionResult.Other;
        }
    }

    //Edit question by question object
    //From Question Model to question table in supabse 
    //<param name="question"></param>
    //return true is question got updated, otherwise false
    public async Task<bool> EditQuestion(Question question) {
        try {
            await Client
            .From<Question>() 
            .Upsert(question);     //add new question to supabase
        } catch {
            return false;
        }
        return true;
    }

    //Get all the user quizzes by user id
    //From Quiz model to quiz table in supabse
    //<param name="userId"></param>
    //return list of quiz corresponding to user id 
    public async Task<List<Quiz>?> GetUserCreatedQuizzes(Guid? userId) {
        try {
            var result = await Client
                .From<Quiz>()
                .Where(q => q.CreatorId == userId)   //check for id
                .Get();                              //get all quizzes

            return result?.Models;                 //return list of quizzes
        } catch (Exception e) {
            Console.Write("ERRORRRRR" + e);          //failed to get quizzes
            return null;
        }
    }

    public async Task<List<Quiz>?> GetUserCreatedQuizzes() {
        try {
            var result = await Client
                .From<Quiz>()
                .Where(q => q.CreatorId == UserId)
                .Get();

            userInfo.CreatedQuizzes = new ObservableCollection<Quiz>(result?.Models);
            return result?.Models;
        } catch (Exception e) {
            Console.Write("ERRORRRRR" + e);
            return null;
        }
    }

    #endregion

    #region Active Quizzes

    public async Task<bool> DeactivateQuestions(long id) {
        try {
            await Client
            .From<ActiveQuestion>()
            .Where(q => q.ActiveQuizId == id)
            .Delete();
        } catch {
            return false;
        }
        return true;
    }


    public async Task<ActiveQuiz?> ActivateQuiz(Quiz quiz, String accessCode) {
        ActiveQuiz activeQuiz = new ActiveQuiz();
        activeQuiz.StartTime = DateTime.Now;
        activeQuiz.QuizId = quiz.Id;
        activeQuiz.IsActive = true;
        activeQuiz.CurrentQuestionNo = 1;
        activeQuiz.Activator = UserId;
        activeQuiz.AccessCode = accessCode;
        try {
            var result = await Client
            .From<ActiveQuiz>()
            .Insert(activeQuiz);
            // var response = await Client.Rpc("activate_quiz", activeQuiz);
            return result.Model;
        } catch {
            return null;
        }
    }

    public async Task<ActiveQuiz?> UpdateActiveQuiz(ActiveQuiz activeQuiz) {
        try {
            var result = await Client
            .From<ActiveQuiz>()
            .Update(activeQuiz);

            return result.Model;
        } catch {
            return null;
        }
    }

    public async Task<bool> ActivateQuestion(ActiveQuestion question) {
        try {
            var result = await Client
                .From<ActiveQuestion>()
                .Insert(question);
        } catch (Exception e){
            Console.WriteLine("Error: " + e.Message);
            return false;
        }
        return true;
    }
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

    public async Task<List<ActiveQuiz>> GetActiveQuizzesByActiveQuizIds(List<long> activeQuizIds) {
        try {
            if (activeQuizIds == null || activeQuizIds.Count == 0) { // if the list is empty no need requesting to db
                return [];
            }
            var result = await Client
                .From<ActiveQuiz>()
                .Where(x => x.Activator == UserId) // only activated by current user
                .Filter(x => x.Id, Supabase.Postgrest.Constants.Operator.In, activeQuizIds) // they are in the list of the IDs
                .Get();

            return result.Models;
        } catch (Exception ex) {
            Console.WriteLine($"Error fetching active quizzes: {ex.Message}");
            return [];
        }
    }

    public async Task<bool> SubmitMultipleChoiceQuestionAnswer(ActiveQuestion question, int[]? choices) {
        try {
            await Client // Insert into the response table according to the choice selected
                .From<Response>()
                .Insert(new Response() {
                    ActiveQuizId = question.ActiveQuizId,
                    UserId = UserId,
                    QuestionNo = question.QuestionNo,
                    MultipleChoiceResponse = choices,
                    FillBlankResponse = null // Null because this is multiple choice
                }, new QueryOptions() { Returning = QueryOptions.ReturnType.Minimal });

            return true;
        } catch (Exception e) {

            Console.WriteLine($"Error occurred while submitting multiple choice answer: {e.Message}");
            return false;
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
                ActiveQuiz? quiz = response.Model<ActiveQuiz>(); // Gets the active quiz with the new change (new question)

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

    public async Task<List<Question>> GetQuizQuestionsByActiveQuizId(long activeQuizId) {
        try {
            //Get the quiz that corresonds to the active quiz id
            var activeQuiz = await Client.From<ActiveQuiz>().Where(x => x.Id == activeQuizId).Single();
            //Get the questions for the quiz
            var questions = await Client.From<Question>().Where(x => x.QuizId == activeQuiz.QuizId).Get();
            //Return the questions
            return questions.Models; 
        } catch (Exception e) {
            //Write out the error if if occurred and return an empty collection
            Console.WriteLine("Error: " + e.Message);
            return new List<Question>();
        }
    }

    public async Task<List<Response>> GetRepsonsesByActiveQuizId(long activeQuizId) {
        try {
            //Get the response for the active quiz and return them
            var responses = await Client.From<Response>().Where(x => x.ActiveQuizId == activeQuizId).Get();
            return responses.Models;
        } catch (Exception e) {
            //Write out the error if if occurred and return an empty collection
            Console.WriteLine("Error: " + e.Message);
            return new List<Response>();
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
    
    #endregion
}
