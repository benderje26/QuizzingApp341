
namespace QuizzingApp341.Models;

public interface IDatabase {
    /// <summary>
    /// Attempts to create a new user.
    /// </summary>
    /// <param name="emailAddress">The email address</param>
    /// <param name="username">The username</param>
    /// <param name="password">The password</param>
    /// <returns>The result of attempting to create the user</returns>
    Task<AccountCreationResult> CreateNewUser(string emailAddress, string username, string password);
    /// <summary>
    /// Attempts to log in.
    /// </summary>
    /// <param name="emailAddress">The email address</param>
    /// <param name="password">The password</param>
    /// <returns>The result of attempting to log in</returns>
    Task<LoginResult> Login(string emailAddress, string password);
    /// <summary>
    /// Attempts to log the user out.
    /// </summary>
    /// <returns>The result of attempting to log out</returns>
    Task<LogoutResult> Logout();

    /// <summary>
    /// Attempts to get a quiz.
    /// </summary>
    /// <param name="id">The given id of the quiz</param>
    /// <returns>The quiz if it is accessible, null if it is not or doesn't exist</returns>
    public Task<Quiz?> GetQuizById(long id);

    /// <summary>
    /// Gets all of the questions from the questions table in db that matches the given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// returns the observable collection if lookup is successful otherwise null
    /// </returns>
    public Task<List<Question>?> GetQuestions(long id);

    /// <summary>
    /// Adds a question to the questions table 
    /// </summary>
    /// <param name="question"></param>
    /// <returns>
    /// Returns the id to that question after it gets added to the db otherwise null
    /// </returns>
    public Task<long?> AddQuestion(Question question);

    /// <summary>
    /// Deletes a question in the questions table
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// Returns true if successfully deleted otherwise null
    /// </returns>
    public Task<bool> DeleteQuestion(long id);

    /// <summary>
    /// Edits a question by updating it in the db
    /// </summary>
    /// <param name="question"></param>
    /// <returns>
    /// returns true if successfully updated, otherwise false
    /// </returns>
    public Task<bool> EditQuestion(Question question);

    /// <summary>
    /// Gets all the quizzes the current user has created from the quizzes table in db
    /// </summary>
    /// <param name="userID"></param>
    /// <returns>
    /// Returns a list of all the quizzes the user has created
    /// </returns>
    public Task<List<Quiz>?> GetUserCreatedQuizzes(string userID);
    Task<List<Quiz>?> GetAllQuizzesAsync();
}
