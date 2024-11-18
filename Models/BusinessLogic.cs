using System.Collections.ObjectModel;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace QuizzingApp341.Models;

public class BusinessLogic(IDatabase database) : IBusinessLogic {

    private const string NETWORK_ERROR_MESSAGE = "There was a network error.";
    private const string OTHER_ERROR_MESSAGE = "An unknown error occurred.";

    private Quiz? CurrentQuiz {
        get => _currentQuiz;
        set {
            _currentQuiz = value;
            if (value != null) {
                value.CurrentIndex = 0;
            }
        }
    }

    public Question? CurrentQuestion => CurrentQuiz?.CurrentQuestion;
    public MultipleChoiceQuestion? CurrentMultipleChoiceQuestion { get => CurrentQuestion as MultipleChoiceQuestion; }
    public FillBlankQuestion? CurrentFillBlankQuestion { get => CurrentQuestion as FillBlankQuestion; }

    private Quiz? _currentQuiz;

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

    public async Task<Quiz?> GetQuiz(string id) {
        return await database.GetQuizById(id);
    }

    public bool SetQuiz(Quiz quiz) {
        CurrentQuiz = quiz;
        return true;
    }

    public Question? NextQuestion() {
        return CurrentQuiz?.NextQuestion();
    }

    public Question? PreviousQuestion() {
        return CurrentQuiz?.PreviousQuestion();
    }

    public bool SetCurrentMultipleChoiceAnswer(int optionIndex) {
        if (CurrentQuestion?.Type != QuestionType.MultipleChoice) {
            return false;
        }

        CurrentQuestion.SetGivenAnswer(optionIndex);
        return true;
    }

    public bool SetCurrentFillBlankAnswer(string value) {
        if (CurrentQuestion?.Type != QuestionType.FillBlank) {
            return false;
        }

        CurrentQuestion.SetGivenAnswer(value);
        return true;
    }

    public (int, int) GetScore() {
        if (CurrentQuiz == null) {
            return (0, 0);
        }

        int total = 0;
        int correct = 0;

        foreach (Question question in CurrentQuiz.Questions) {
            total++;
            if (question.IsCorrect()) {
                correct++;
            }
        }

        return (correct, total);
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
