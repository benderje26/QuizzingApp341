
using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace QuizzingApp341.Models;

public class BusinessLogic(IDatabase database) : IBusinessLogic {
    // FOR AUTHENTICATION
    public UserInfo UserInfo() {
        return new UserInfo("ba08579e-8e08-43c5-bffc-612393113c28"); // Hardcoded for now..
    }
    private const string NETWORK_ERROR_MESSAGE = "There was a network error.";
    private const string OTHER_ERROR_MESSAGE = "An unknown error occurred.";

    public async Task<(AccountCreationResult, string?)> CreateNewUser(string emailAddress, string username, string password) {
        if (!Regexes.EmailRegex().IsMatch(emailAddress)) {
            return (AccountCreationResult.BadEmail, "Email must be in the correct format (ex: example@example.com).");
        }
        if (!Regexes.UsernameRegex().IsMatch(username)) {
            return (AccountCreationResult.BadUsername, "Username must be 4 to 32 characters and only contain A-Z, a-z, 0-9, and _ (underscores).");
        }
        if (!Regexes.PasswordRegex().IsMatch(password)) {
            return (AccountCreationResult.BadPassword, "Password is not strong enough or invalid. It must be 8 characters with a letter, number, and symbol, or at least 16 characters of any kind.");
        }

        AccountCreationResult result = await database.CreateNewUser(emailAddress, username, password);

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

        string? s = result switch {
            LoginResult.Success => null,
            LoginResult.BadCredentials => "The username or password are incorrect.",
            LoginResult.NetworkError => NETWORK_ERROR_MESSAGE,
            LoginResult.Other => OTHER_ERROR_MESSAGE,
            _ => OTHER_ERROR_MESSAGE
        };

        return (result, s);
    }

    public async Task<(LogoutResult, string?)> Logout() {
        LogoutResult result = await database.Logout();

        string? s = result switch {
            LogoutResult.Success => null,
            LogoutResult.NetworkError => NETWORK_ERROR_MESSAGE,
            LogoutResult.Other => OTHER_ERROR_MESSAGE,
            _ => OTHER_ERROR_MESSAGE
        };

        return (result, s);
    }


    // FOR QUIZ LOGIC

    public async Task<Quiz?> GetQuiz(long id) {
        return await database.GetQuizById(id);
    }

    public async Task<ObservableCollection<Question>?> GetQuestions(long id) {
        var result = await database.GetQuestions(id);
        if (result != null) {
            List<Question> questions = result;
            return new ObservableCollection<Question>(questions);
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

    public async Task<ObservableCollection<Quiz>?> GetUserCreatedQuizzes(string userID) {
        var result = await database.GetUserCreatedQuizzes(userID);
        if (result != null) {
            List<Quiz> quizzes = result;
            return new ObservableCollection<Quiz>(quizzes);
        }
        return null; 
    }
}

partial class Regexes {
    // must be a@b.c where a, b, and c are alphanumeric/underscore/"." but a little more complex
    [GeneratedRegex(@"^\w+(\.\w+)*@\w+(\.\w+)+$")]
    public static partial Regex EmailRegex();

    // must be 4-32 letters, numbers, or underscores
    [GeneratedRegex(@"^\w{4,32}$")]
    public static partial Regex UsernameRegex();

    // basically it must be at least 8 characters, and it must have a letter, number and symbol OR be at least 16 characters
    [GeneratedRegex(@"^(?=.*\w|.{16,})(?=.*\d|.{16,})(?=.*[\W_]|.{16,})[a-zA-z0-9!@#$%^&*()_\-=+\[\]{}<>\\|;:'"",.?/`~]{8,32}$")]
    public static partial Regex PasswordRegex();
}
