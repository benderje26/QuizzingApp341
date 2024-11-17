namespace QuizzingApp341.Models;

public interface IBusinessLogic {
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
    Task<(LoginResult, string?)> Login(string emailAddress, string password);
    /// <summary>
    /// Attempts to log the user out.
    /// </summary>
    /// <returns>The result and a nullable string showing the message if something went wrong</returns>
    Task<(LogoutResult, string?)> Logout();

    /// <summary>
    /// Attempts to send user email to reset password
    /// </summary>
    /// <returns>The result and a nullable string showing the message if something went wrong</returns>
    Task<(ResetPasswordResult, string?)> ResetPassword(string emailAddress);

    Question? CurrentQuestion { get; }
    List<Quiz> FavoriteQuizzes { get; }
    MultipleChoiceQuestion? CurrentMultipleChoiceQuestion { get; }
    FillBlankQuestion? CurrentFillBlankQuestion { get; }
    /// <summary>
    /// Attempts to get a quiz.
    /// </summary>
    /// <param name="id">The given id of the quiz</param>
    /// <returns>The quiz if it is accessible, null if it is not or doesn't exist</returns>
    Task<Quiz?> GetQuiz(string id);
    /// <summary>
    /// Sets the current quiz.
    /// </summary>
    /// <param name="quiz">The quiz to set as the current</param>
    /// <returns>true if success, false if failure</returns>
    bool SetQuiz(Quiz quiz);
    /// <summary>
    /// Gets the next question.
    /// </summary>
    /// <returns>The next question or null if there is no more</returns>
    Question? NextQuestion();
    /// <summary>
    /// Gets the previous question.
    /// </summary>
    /// <returns>The previous question or null if it is the first</returns>
    Question? PreviousQuestion();
    /// <summary>
    /// Sets the index of the option selected for the current multiple choice question.
    /// </summary>
    /// <param name="optionIndex">The index of the option picked</param>
    /// <returns>true if it worked, false if it didn't</returns>
    bool SetCurrentMultipleChoiceAnswer(int optionIndex);
    /// <summary>
    /// Sets the input for the current fill blank question.
    /// </summary>
    /// <param name="value">The text inputted</param>
    /// <returns>true if it worked, false if it didn't</returns>
    bool SetCurrentFillBlankAnswer(string value);
    /// <summary>
    /// Gets the score of the current quiz.
    /// </summary>
    /// <returns>The amount correct and the total in the format (correct, total)</returns>
    (int, int) GetScore();

}
