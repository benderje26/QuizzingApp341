using QuizzingApp341.Views;
using Supabase;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace QuizzingApp341.Models;

// Implementing the business logic for user management and quiz operations
public class BusinessLogic(IDatabase database) : IBusinessLogic {

    // Creating constants for common error messages
    #region User and Auth
    private const string NETWORK_ERROR_MESSAGE = "There was a network error.";
    private const string OTHER_ERROR_MESSAGE = "An unknown error occurred.";

    // UserInfo to Retrieve the current user information
    public UserInfo? UserInfo => database.GetUserInfo();

    // Creating a new user account after validating email, username, and password
    public async Task<(AccountCreationResult, string?)> CreateNewUser(string emailAddress, string username, string password) {

        // Checking email format 
        if (!Regexes.EmailRegex().IsMatch(emailAddress)) {
            return (AccountCreationResult.BadEmail, "Email must be in the correct format (ex: example@example.com).");
        }

        // Checking username format
        if (!Regexes.UsernameRegex().IsMatch(username)) {
            return (AccountCreationResult.BadUsername, "Username must be 5 to 20 characters and only contain A-Z, a-z, 0-9, and _ (underscores).");
        }

        // Checking password strength
        if (!Regexes.PasswordRegex().IsMatch(password)) {
            return (AccountCreationResult.BadPassword, "Password is not strong enough or invalid. It must be 8 characters with a letter, number, and symbol, or at least 16 characters of any kind.");
        }

        // Create the new user in the database
        AccountCreationResult result = await database.CreateNewUser(emailAddress, username, password);

        // Notify subscribers with changes in user information
        NotifyPropertyChanged(nameof(UserInfo));

        // Map result to user-friendly messages
        string? message = result switch {
            AccountCreationResult.Success => null,
            AccountCreationResult.DuplicateEmail => "Email already used on another account.",
            AccountCreationResult.DuplicateUsername => "That username is already used, pick another one.",
            AccountCreationResult.NetworkError => NETWORK_ERROR_MESSAGE,
            AccountCreationResult.Other => OTHER_ERROR_MESSAGE,
            _ => OTHER_ERROR_MESSAGE
        };

        return (result, message);
    }
    // Logs in a user with email and password
    public async Task<(LoginResult, string?)> Login(string emailAddress, string password) {
        LoginResult result = await database.Login(emailAddress, password);

        // Notify user of changes in user information
        NotifyPropertyChanged(nameof(UserInfo));

        // Map result to user-friendly messages
        string? message = result switch {
            LoginResult.Success => null,
            LoginResult.BadCredentials => "The username or password are incorrect.",
            LoginResult.NetworkError => NETWORK_ERROR_MESSAGE,
            LoginResult.Other => OTHER_ERROR_MESSAGE,
            _ => OTHER_ERROR_MESSAGE
        };

        return (result, message);
    }

    // logs out the user  
    public async Task SkipLogin() {
        await database.SkipLogin();

        NotifyPropertyChanged(nameof(UserInfo));
    }

    // Notify user of changes in user information
    public async Task<(LogoutResult, string?)> Logout() {
        LogoutResult result = await database.Logout();

        NotifyPropertyChanged(nameof(UserInfo));

        // maps user to user friendly messages  
        string? message = result switch {
            LogoutResult.Success => null,
            LogoutResult.NetworkError => NETWORK_ERROR_MESSAGE,
            LogoutResult.Other => OTHER_ERROR_MESSAGE,
            _ => OTHER_ERROR_MESSAGE
        };

        return (result, message);
    }

    // Retrieves user's data by their ID
    public async Task<UserData?> GetUserData(Guid userId) {
        return await database.GetUserData(userId);
    }
    #endregion


    #region Quizzes
    public QuizManager? QuizManager { get; set; }

    public async Task<bool> ChangeQuizVisibility(bool isPublic) {
        bool originalVisibility = QuizManager.Quiz.IsPublic;
        QuizManager.Quiz.IsPublic = isPublic;
        var result = await database.UpdateQuiz(QuizManager.Quiz);

        if (!result) {
            QuizManager.Quiz.IsPublic = originalVisibility;
        }
        return result;
    }

    public async Task<long?> AddQuiz(Quiz quiz) {
        quiz.CreatorId = (Guid)UserInfo.Id;
        var result = await database.AddQuiz(quiz);
        QuizManager.Quiz.Id = (long)result;
        UserInfo.CreatedQuizzes = await GetUserCreatedQuizzes();
        QuizManager = new QuizManager(UserInfo.CreatedQuizzes.FirstOrDefault(q => q.Id == (long)result));
        return result;
    }

    // Get quiz by ID
    public async Task<Quiz?> GetQuiz(long id) {
        return await database.GetQuizById(id);
    }

    public async Task<bool> DeleteQuiz(long quizId) {
        bool result = await database.DeleteQuiz(quizId);
        UserInfo.CreatedQuizzes = await GetUserCreatedQuizzes();
        return result;
    }

    public async Task<bool> EditQuizTitle(string newQuizTitle) {
        if (QuizManager?.Quiz != null) {
            QuizManager.Quiz.Title = newQuizTitle;
            return await database.UpdateQuiz(QuizManager.Quiz);
        }
        UserInfo.CreatedQuizzes = await GetUserCreatedQuizzes();
        return false;
    }

    public async void RefreshQuestionNums() {
        for (int i = 0; i < QuizManager?.Questions.Count; i++) {
            await database.UpdateQuestionNo(QuizManager.Questions[i].Id, i + 1);
        }
    }

    // Retrieves questions for a specific quiz with the id given, ordered by question number
    public async Task<ObservableCollection<Question>?> GetQuestions(long id) {
        // Refresh the question numbers, because if one question was deleted, the question numbers needs to be renumbered
        RefreshQuestionNums();

        var result = await database.GetQuestions(id);
        if (result != null) {
            ObservableCollection<Question> questions = new ObservableCollection<Question>(result.OrderBy(q => q.QuestionNo));
            if (QuizManager != null) {
                QuizManager.Questions = questions;
            }
            return questions;
        }
        return null;
    }

    // Adds a question in the database
    public async Task<long?> AddQuestion(Question question) {
        if (QuizManager?.Quiz != null) {
            long? result = await database.AddQuestion(question);
            await GetQuestions(QuizManager.Quiz.Id);
            return result;
        }
        return null;
    }

    public async Task<(DeleteQuestionResult, string?)> DeleteQuestion(long id) {
        DeleteQuestionResult result = await database.DeleteQuestion(id);
        string? message = result switch {
            DeleteQuestionResult.Success => null,
            DeleteQuestionResult.QuizStillActive => "",
            DeleteQuestionResult.NetworkError => NETWORK_ERROR_MESSAGE,
            DeleteQuestionResult.Other => OTHER_ERROR_MESSAGE,
            _ => OTHER_ERROR_MESSAGE
        };

        if (QuizManager?.Quiz != null) {
            await GetQuestions(QuizManager.Quiz.Id);
        }

        return (result, message);
    }

    // edit questions in the database
    public async Task<bool> EditQuestion(Question question) {
        for (int i = 0; i < QuizManager?.Questions.Count(); i++) {
            var currentQuestion = QuizManager.Questions[i];
            if (currentQuestion.Id == question.Id) {
                QuizManager.Questions[i] = question;
            }
        }
        return await database.EditQuestion(question);
    }

    // Retrieves quizzes created by a specific user
    public async Task<ObservableCollection<Quiz>?> GetUserCreatedQuizzes(Guid? userId) {
        var result = await database.GetUserCreatedQuizzes(userId);
        if (result != null) {
            return new ObservableCollection<Quiz>(result);
        }
        return null;
    }

    public async Task<ObservableCollection<Quiz>?> GetUserCreatedQuizzes() {
        var result = await database.GetUserCreatedQuizzes();
        if (result != null) {
            return new ObservableCollection<Quiz>(result);
        }
        return null;
    }

    // Get active quiz IDs for a user from participants table
    // Fetch the active_quiz_ids for a user by querying the participants table.
    public async Task<List<long>> GetActiveQuizIdsForUser() {
        try {
            var activeQuizIds = await database.GetActiveQuizIdsByUserId(); // Get from participants table

            if (activeQuizIds == null || activeQuizIds.Count() == 0) {
                return [];
            }

            return activeQuizIds;
        } catch (Exception ex) {
            Console.WriteLine($"Error in business logic: {ex.Message}");
            return null;
        }
    }



    // Get quiz IDs from active quiz IDs
    // Fetch the quiz_id for each active_quiz_id.
    public async Task<List<(long quizId, DateTime? startTime)>?> GetQuizIdsAndStartTimesByActiveQuizIds(List<long> activeQuizIds) {
        try {
            Console.WriteLine($"Fetching quiz data for activeQuizIds: {string.Join(", ", activeQuizIds)}");

            // Fetch the active quizzes based on the provided IDs
            var activeQuizzes = await database.GetActiveQuizzesByActiveQuizIds(activeQuizIds);

            // Check if there are no active quizzes
            if (activeQuizzes == null || !activeQuizzes.Any()) {
                Console.WriteLine("No active quizzes found.");
                return null;
            }

            var quizList = activeQuizzes.Select(q => (q.QuizId, q.StartTime)).ToList();

            return quizList;
        } catch (Exception ex) {
            Console.WriteLine($"Error fetching active quizzes: {ex.Message}");
            return null;
        }
    }

    // Retrieves all quizes from the database
    public async Task<ObservableCollection<Quiz>?> GetAllQuizzes() {
        var result = await database.GetAllQuizzesAsync();
        return result == null ? null : new ObservableCollection<Quiz>(result);
    }

    #region Favorite Quizzes

    /// <summary>
    /// Adds a favorite quiz to the questions table 
    /// </summary>
    /// <param name="quizId">The quiz's ID</param>
    /// <returns>
    /// Returns the id to that favorite quiz after it gets added to the db otherwise null
    /// </returns>
    public async Task<long?> AddFavoriteQuiz(long quizId) {
        //Call the database to add the quiz to favorites
        var result = await database.AddFavoriteQuiz(quizId);
        return result;
    }

    /// <summary>
    /// Deletes a favorite quiz from the questions table 
    /// </summary>
    /// <param name="quizId">The quiz's ID</param>
    /// <returns>
    /// Returns whether or not the favorite quiz was successfully deleted
    /// </returns>
    public async Task<bool> DeleteFavoriteQuiz(long quizId) {
        //Call the database to delete the favorite
        var result = await database.DeleteFavoriteQuiz(quizId);
        return result;
    }

    #region Active Quizzes

    public async Task<bool> DeactivateQuiz() {
        if (QuizManager?.ActiveQuiz == null) {
            return false;
        }
        var oldActiveQuiz = QuizManager.ActiveQuiz;

        try {
            // Deactivate the active quiz
            QuizManager.ActiveQuiz.IsActive = false;
            QuizManager.ActiveQuiz.AccessCode = null;
            QuizManager.ActiveQuiz.CurrentQuestionNo = null;
            QuizManager.ActiveQuiz.EndTime = DateTime.Now;
            var result = await database.UpdateActiveQuiz(QuizManager.ActiveQuiz);
            if (result == null) {
                QuizManager.ActiveQuiz = oldActiveQuiz;
                return false;
            }
            QuizManager.ActiveQuiz = null;
            QuizManager.CurrentQuestion = null;
            // remove all the questions from the active questions table
            await database.DeactivateQuestions(oldActiveQuiz.Id);
        } catch {
            QuizManager.ActiveQuiz = oldActiveQuiz;
            return false;
        }
        return true;
    }

    public async Task<bool> ActivateQuiz() {
        if (QuizManager?.Quiz == null) {
            return false;
        }
        try {

            ActiveQuiz? result = null;
            int maxRetries = 5;
            for (int i = 0; i < maxRetries && result == null; i++) {
                result = await database.ActivateQuiz(QuizManager.Quiz, GenerateRandomAccessCode(6));
            }

            if (result == null) {
                return false;
            }

            QuizManager.ActiveQuiz = result;
            QuizManager.CurrentQuestion = QuizManager.Questions.FirstOrDefault(q => q.QuestionNo == QuizManager.ActiveQuiz.CurrentQuestionNo);

            List<Task<bool>> tasks = [];

            for (int i = 0; i < QuizManager.Questions.Count; i++) {
                // Add questions to the active questions table
                tasks.Add(database.ActivateQuestion(new ActiveQuestion(QuizManager.Questions[i], result.Id)));
            }

            await Task.WhenAll(tasks);
        } catch {
            return false;
        }
        return true;
    }

    private static string GenerateRandomAccessCode(int length) {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();

        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public async Task<bool> IncrementCurrentQuestion() {
        var currentQuestionNo = QuizManager.ActiveQuiz.CurrentQuestionNo;
        try {
            // update the active quiz
            QuizManager.ActiveQuiz.CurrentQuestionNo += 1;
            var result = await database.UpdateActiveQuiz(QuizManager.ActiveQuiz);
            if (result == null) {
                QuizManager.ActiveQuiz.CurrentQuestionNo = currentQuestionNo;
                return false;
            }
            QuizManager.CurrentQuestion = QuizManager.Questions.FirstOrDefault(q => q.QuestionNo == QuizManager.ActiveQuiz.CurrentQuestionNo);
        } catch {
            QuizManager.ActiveQuiz.CurrentQuestionNo = currentQuestionNo;
            return false;
        }
        return true;
    }

    // retrieves active quizzes from its access code
    public async Task<ActiveQuiz?> GetActiveQuiz(string accessCode) {
        return await database.GetActiveQuiz(accessCode);
    }

    // Submits an answer for a multiple-choice question
    public async Task<bool> GiveMultipleChoiceQuestionAnswer(ActiveQuestion question, int choice) {
        return await database.SubmitMultipleChoiceQuestionAnswer(question, choice);
    }

    // Submits an answer for a fill-in-the-blank question
    public async Task<bool> GiveFillBlankQuestionAnswer(ActiveQuestion question, string response) {
        return await database.SubmitFillBlankQuestionAnswer(question, response);
    }

    // Joins an active quiz using a handler for new questions
    public async Task<bool> JoinActiveQuiz(ActiveQuiz quiz, NewActiveQuestionHandler handler) {
        return await database.JoinActiveQuiz(quiz, handler);
    }

    // validates the access code for the quiz
    public async Task<bool> ValidateAccessCode(string accessCode) {
        return await database.ValidateAccessCode(accessCode);
    }
    #endregion
    #endregion

    // Event to notify property changes for data binding
    #region INotifyPropertyChanged Stuff
    public event PropertyChangedEventHandler? PropertyChanged;

    // Helper method to raise the PropertyChanged event
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}

partial class Regexes {
    // must be a@b.c where a, b, and c are alphanumeric/underscore/"." but a little more complex
    [GeneratedRegex(@"^\w+(\.\w+)*@\w+(\.\w+)+$")]
    public static partial Regex EmailRegex();

    // must be 4-32 letters, numbers, or underscores
    [GeneratedRegex(@"^\w{5,20}$")]
    public static partial Regex UsernameRegex();

    // basically it must be at least 8 characters, and it must have a letter, number and symbol OR be at least 16 characters
    [GeneratedRegex(@"^(?=.*\w|.{16,})(?=.*\d|.{16,})(?=.*[\W_]|.{16,})[a-zA-z0-9!@#$%^&*()_\-=+\[\]{}<>\\|;:'"",.?/`~]{8,32}$")]
    public static partial Regex PasswordRegex();
}
#endregion