using System.Collections.ObjectModel;

namespace QuizzingApp341.Models;

public interface IDatabase {
    /// <summary>
    /// Skips the login so the user is logged in as anonymous.
    /// </summary>
    Task SkipLogin();
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
    /// Attempts to get the UserData object for a User ID.
    /// </summary>
    /// <param name="userId">The id of the user you need the UserData for</param>
    /// <returns>A UserData object for the user</returns>
    Task<UserData?> GetUserData(Guid userId);

    /// <summary>
    /// Attempts to get a quiz.
    /// </summary>
    /// <param name="id">The given id of the quiz</param>
    /// <returns>The quiz if it is accessible, null if it is not or doesn't exist</returns>
    Task<Quiz?> GetQuizById(long id);

    Task<bool> UpdateQuestionNo(long questionId, int newQuestionNo);

    /// <summary>
    /// Gets all of the questions from the database that matches the given id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// returns the observable collection if lookup is successful otherwise null
    /// </returns>
    Task<List<Question>?> GetQuestions(long id);

    /// <summary>
    /// Adds a question to the database.
    /// </summary>
    /// <param name="question"></param>
    /// <returns>
    /// Returns the id to that question after it gets added to the db otherwise null
    /// </returns>
    Task<long?> AddQuestion(Question question);

    /// <summary>
    /// Deletes a question from the database.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// Returns true if successfully deleted otherwise null
    /// </returns>
    Task<DeleteQuestionResult> DeleteQuestion(long id);

    /// <summary>
    /// Edits a question by updating it in the database.
    /// </summary>
    /// <param name="question"></param>
    /// <returns>
    /// returns true if successfully updated, otherwise false
    /// </returns>
    Task<bool> EditQuestion(Question question);

    /// <summary>
    /// Gets all the quizzes the current user has created from the quizzes table in the database.
    /// </summary>
    /// <param name="userID"></param>
    /// <returns>
    /// Returns a list of all the quizzes the user has created
    /// </returns>
    Task<List<Quiz>?> GetUserCreatedQuizzes(Guid? userID);
    Task<List<Quiz>?> GetUserCreatedQuizzes();

    /// <summary>
    /// Gets all the public quizzes from the database and the user's quizzes.
    /// </summary>
    /// <returns>All public and user's quizzes</returns>
    Task<List<Quiz>?> GetAllQuizzesAsync();
    Task<long?> AddQuiz(Quiz quiz);
    Task<bool> DeleteQuiz(long quizId);

    /// <summary>
    /// Gets the user's info for the currently logged in user. This should not request any data from the database,
    /// but should run in generally a very short amount of time.
    /// </summary>
    /// <returns>The user's info, or null if there is no logged in user</returns>
    UserInfo? GetUserInfo();

    /// <summary>
    /// Get's the current user's active quizzes' IDs.
    /// </summary>
    /// <returns>The IDs of the active quizzes</returns>
    /// 


    Task<bool> DeactivateQuestions(long id);
    Task<ActiveQuiz?> ActivateQuiz(Quiz quiz, String accessCode);
    Task<ActiveQuiz?> UpdateActiveQuiz(ActiveQuiz activeQuiz);
    Task<bool> ActivateQuestion(ActiveQuestion questions);
    Task<List<long>> GetActiveQuizIdsByUserId();

    /// <summary>
    /// Gets a list of the current user's active quizzes by their IDs.
    /// </summary>
    /// <param name="activeQuizIds">List of active quiz IDs.</param>
    /// <returns>A list of active quizzes, or null if there were none</returns>
    Task<List<ActiveQuiz>> GetActiveQuizzesByActiveQuizIds(List<long> activeQuizIds);

    /// <summary>
    /// Returns all of the users favorite quizzess from the database.
    /// </summary>
    /// <returns>
    /// Returns all of the quizzes that the user has favorited
    /// </returns>
    Task<ObservableCollection<Quiz>> GetFavoriteQuizzes();

    /// <summary>
    /// Adds a favorite quiz to the database.
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns>
    /// Returns the id to that favorite quiz after it gets added to the db otherwise null
    /// </returns>
    Task<long?> AddFavoriteQuiz(long quizId);

    /// <summary>
    /// Deletes a favorite quiz from the database.
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns>
    /// Returns whether the favorite quiz was successfully deleted
    /// </returns>
    Task<bool> DeleteFavoriteQuiz(long quizId);

    /// <summary>
    /// Gets an active quiz by its access code.
    /// </summary>
    /// <param name="accessCode">The access code of an active quiz</param>
    /// <returns>The active quiz</returns>
    Task<ActiveQuiz?> GetActiveQuiz(string accessCode);

    /// <summary>
    /// Submits a multiple choice question with its choice.
    /// </summary>
    /// <param name="question">The question you are submitting to</param>
    /// <param name="choice">The index of the choice the student selected</param>
    /// <returns></returns>
    Task<bool> SubmitMultipleChoiceQuestionAnswer(ActiveQuestion question, int choice);

    /// <summary>
    /// Submits a fill blank question with its answer.
    /// </summary>
    /// <param name="question">The question you are submitting to</param>
    /// <param name="response">The answer the student typed</param>
    /// <returns></returns>
    Task<bool> SubmitFillBlankQuestionAnswer(ActiveQuestion question, string response);

    /// <summary>
    /// Joins an active quiz, awaiting active questions to come in.
    /// </summary>
    /// <param name="quiz">The quiz the student is joining</param>
    /// <param name="handler">The handler for when a new active question comes in</param>
    /// <returns></returns>
    Task<bool> JoinActiveQuiz(ActiveQuiz quiz, NewActiveQuestionHandler handler);

    /// <summary>
    /// Validates that an access code is currently active.
    /// </summary>
    /// <param name="accessCode">The access code</param>
    /// <returns>Whether the access code is currently active</returns>
    Task<bool> ValidateAccessCode(string accessCode);
    Task<bool> UpdateQuiz(Quiz quiz);
}
