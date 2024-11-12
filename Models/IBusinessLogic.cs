namespace QuizzingApp341.Models;

public interface IBusinessLogic {
    Task<List<Question>> GetAllQuestions();
    Task SetQuiz();
    bool IncrementCurrentQuestion();
    bool DecrementCurrentQuestion();
    bool IsCurrentQuestionMultipleChoice();
    /// <summary>
    /// Attempts to create a new user.
    /// </summary>
    /// <param name="emailAddress">The email address</param>
    /// <param name="username">The username</param>
    /// <param name="password">The password</param>
    /// <returns>null if it was a success, otherwise the error message</returns>
    Task<string?> CreateNewUser(string emailAddress, string username, string password);
    /// <summary>
    /// Attempts to log in.
    /// </summary>
    /// <param name="emailAddress">The email address</param>
    /// <param name="password">The password</param>
    /// <returns>true if success, false if failure</returns>
    Task<bool> LogIn(string emailAddress, string password);
}
