using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace QuizzingApp341.Models;

public class BusinessLogic(IDatabase database) : IBusinessLogic {

    #region User and Auth
    private const string NETWORK_ERROR_MESSAGE = "There was a network error.";
    private const string OTHER_ERROR_MESSAGE = "An unknown error occurred.";

    public UserInfo? UserInfo => database.GetUserInfo();

    public async Task<(AccountCreationResult, string?)> CreateNewUser(string emailAddress, string username, string password) {
        if (!Regexes.EmailRegex().IsMatch(emailAddress)) {
            return (AccountCreationResult.BadEmail, "Email must be in the correct format (ex: example@example.com).");
        }
        if (!Regexes.UsernameRegex().IsMatch(username)) {
            return (AccountCreationResult.BadUsername, "Username must be 5 to 20 characters and only contain A-Z, a-z, 0-9, and _ (underscores).");
        }
        if (!Regexes.PasswordRegex().IsMatch(password)) {
            return (AccountCreationResult.BadPassword, "Password is not strong enough or invalid. It must be 8 characters with a letter, number, and symbol, or at least 16 characters of any kind.");
        }

        AccountCreationResult result = await database.CreateNewUser(emailAddress, username, password);

        NotifyPropertyChanged(nameof(UserInfo));

        string? s = result switch {
            AccountCreationResult.Success => null,
            AccountCreationResult.DuplicateEmail => "Email already used on another account.",
            AccountCreationResult.DuplicateUsername => "That username is already used, pick another one.",
            AccountCreationResult.NetworkError => NETWORK_ERROR_MESSAGE,
            AccountCreationResult.Other => OTHER_ERROR_MESSAGE,
            _ => OTHER_ERROR_MESSAGE
        };

        return (result, s);
    }

    public async Task<(LoginResult, string?)> Login(string emailAddress, string password) {
        LoginResult result = await database.Login(emailAddress, password);

        NotifyPropertyChanged(nameof(UserInfo));

        string? s = result switch {
            LoginResult.Success => null,
            LoginResult.BadCredentials => "The username or password are incorrect.",
            LoginResult.NetworkError => NETWORK_ERROR_MESSAGE,
            LoginResult.Other => OTHER_ERROR_MESSAGE,
            _ => OTHER_ERROR_MESSAGE
        };

        return (result, s);
    }

    public async Task SkipLogin() {
        await database.SkipLogin();

        NotifyPropertyChanged(nameof(UserInfo));
    }

    public async Task<(LogoutResult, string?)> Logout() {
        LogoutResult result = await database.Logout();

        NotifyPropertyChanged(nameof(UserInfo));

        string? s = result switch {
            LogoutResult.Success => null,
            LogoutResult.NetworkError => NETWORK_ERROR_MESSAGE,
            LogoutResult.Other => OTHER_ERROR_MESSAGE,
            _ => OTHER_ERROR_MESSAGE
        };

        return (result, s);
    }

    public async Task<UserData?> GetUserData(Guid userId) {
        return await database.GetUserData(userId);
    }
    #endregion


    #region Quizzes
    public async Task<Quiz?> GetQuiz(long id) {
        return await database.GetQuizById(id);
    }

    public async Task<ObservableCollection<Question>?> GetQuestions(long id) {
        var result = await database.GetQuestions(id);
        if (result != null) {
            List<Question> questions = result;
            return new ObservableCollection<Question>(questions.OrderBy(q => q.QuestionNo));
        }
        return null;
    }

    public async Task<long?> AddQuestion(Question question) {
        var result = await database.AddQuestion(question);
        return result;
    }

    public async Task<bool> DeleteQuestion(long id) {
        return await database.DeleteQuestion(id);
    }

    public async Task<bool> EditQuestion(Question question) {
        return await database.EditQuestion(question);
    }

    public async Task<ObservableCollection<Quiz>?> GetUserCreatedQuizzes(Guid? userId) {
        var result = await database.GetUserCreatedQuizzes(userId);
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
            var activeQuizzes = await database.GetQuizIdsByActiveQuizIds(activeQuizIds);

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

    public async Task<ObservableCollection<Quiz>?> GetAllQuizzes() {
        var result = await database.GetAllQuizzesAsync();
        return result == null ? null : new ObservableCollection<Quiz>(result);
    }

    #region Favorite Quizzes

    /// <summary>
    /// Adds a favorite quiz to the questions table 
    /// </summary>
    /// <param name="quizId"></param>
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
    /// <param name="quizId"></param>
    /// <returns>
    /// Returns whether or not the favorite quiz was successfully deleted
    /// </returns>
    public async Task<bool> DeleteFavoriteQuiz(long quizId) {
        //Call the database to delete the favorite
        var result = await database.DeleteFavoriteQuiz(quizId);
        return result;
    }

    #region Active Quizzes
    public async Task<ActiveQuiz?> GetActiveQuiz(string accessCode) {
        return await database.GetActiveQuiz(accessCode);
    }

    public async Task<bool> GiveMultipleChoiceQuestionAnswer(ActiveQuestion question, int choice) {
        return await database.SubmitMultipleChoiceQuestionAnswer(question, choice);
    }

    public async Task<bool> GiveFillBlankQuestionAnswer(ActiveQuestion question, string response) {
        return await database.SubmitFillBlankQuestionAnswer(question, response);
    }

    public async Task<bool> JoinActiveQuiz(ActiveQuiz quiz, NewActiveQuestionHandler handler) {
        return await database.JoinActiveQuiz(quiz, handler);
    }

    public async Task<bool> ValidateAccessCode(string accessCode) {
        return await database.ValidateAccessCode(accessCode);
    }
    #endregion
    #endregion

    #region INotifyPropertyChanged Stuff
    public event PropertyChangedEventHandler? PropertyChanged;

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
