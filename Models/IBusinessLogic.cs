namespace QuizzingApp341.Models;

public interface IBusinessLogic {
    Task<List<Question>> GetAllQuestions();
    Task SetQuiz();
    bool IncrementCurrentQuestion(int givenAnswer);
    bool DecrementCurrentQuestion(int givenAnswer);
    bool IncrementCurrentQuestion(string givenAnswer);
    bool DecrementCurrentQuestion(string givenAnswer);
    public int GetTotalCorrect();
    bool IsCurrentQuestionMultipleChoice();
    /// <summary>
    /// Attempts to create a new user.
    /// </summary>
    /// <param name="emailAddress">The email address</param>
    /// <param name="username">The username</param>
    /// <param name="password">The password</param>
    /// <returns>The result and a nullable string showing the message if something went wrong</returns>
    Task<(AccountCreationResult, string?)> CreateNewUser(string emailAddress, string username, string password);

    /// <summary>
    /// Attempts to log in.
    /// </summary>
    /// <param name="emailAddress">The email address</param>
    /// <param name="password">The password</param>
    /// <returns>The result and a nullable string showing the message if something went wrong</returns>
    Task<(LoginResult, string?)> LogIn(string emailAddress, string password);
}
