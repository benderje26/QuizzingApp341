namespace QuizzingApp341.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

public interface IBusinessLogic : INotifyPropertyChanged {
    /// <summary>
    /// Gets the info for the current user.
    /// </summary>
    UserInfo? UserInfo { get; }
    /// <summary>
    /// Attempts to create a new user.
    /// </summary>
    /// <param name="emailAddress">The email address</param>
    /// <param name="username">The username</param>
    /// <param name="password">The password</param>
    /// <returns>The result and a nullable string showing the message if something went wrong</returns>
    Task<(AccountCreationResult, string?)> CreateNewUser(string emailAddress, string username, string password);
    /// <summary>
    /// Skips the login.
    /// </summary>
    Task SkipLogin();
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

    public QuizManager? getEditQuizManager();
    void SetEditQuestionQuizManager(QuizManager quizManager);

    /// <summary>
    /// Attempts to get a quiz.
    /// </summary>
    /// <param name="id">The given id of the quiz</param>
    /// <returns>The quiz if it is accessible, null if it is not or doesn't exist</returns>
    Task<Quiz?> GetQuiz(long id);

    /// <summary>
    /// Gets all of the questions from the questions table in db that matches the given id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// returns the observable collection if lookup is successful otherwise null
    /// </returns>
    Task<ObservableCollection<Question>?> GetQuestions(long id);

    /// <summary>
    /// Adds a question to the questions table 
    /// </summary>
    /// <param name="question"></param>
    /// <returns>
    /// Returns the id to that question after it gets added to the db otherwise null
    /// </returns>
    Task<long?> AddQuestion(Question question);

    /// <summary>
    /// Deletes a question in the questions table
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// Returns true if successfully deleted otherwise null
    /// </returns>
    Task<Exception?> DeleteQuestion(long id);

    /// <summary>
    /// Edits a question by updating it in the db
    /// </summary>
    /// <param name="question"></param>
    /// <returns>
    /// returns true if successfully updated, otherwise false
    /// </returns>
    Task<bool> EditQuestion(Question question);

    /// <summary>
    /// Gets all the quizzes the current user has created from the quizzes table in db
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>
    /// Returns an Observable Collection of all the quizzes the user has created
    /// </returns>
    Task<ObservableCollection<Quiz>?> GetUserCreatedQuizzes(Guid? userId);
    Task<ObservableCollection<Quiz>?> GetUserCreatedQuizzes();

    /// <summary>
    /// Gets quiz IDs based on a list of active quiz IDs.
    /// </summary>
    /// <param name="activeQuizIds">List of active quiz IDs</param>
    /// <returns>List of quiz IDs if found, otherwise null</returns>

    Task<List<(long quizId, DateTime? startTime)>?> GetQuizIdsAndStartTimesByActiveQuizIds(List<long> activeQuizIds);
    /// <summary>
    /// Gets the active quiz IDs for a given user from participants tables
    /// </summary>
    /// <returns>List of active quiz IDs</returns>
    Task<List<long>> GetActiveQuizIdsForUser();

    Task<ObservableCollection<Quiz>?> GetAllQuizzes();

    /// <summary>
    /// Adds a favorite quiz to the questions table 
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns>
    /// Returns the id to that favorite quiz after it gets added to the db otherwise null
    /// </returns>
    Task<long?> AddFavoriteQuiz(long quizId);

    /// <summary>
    /// Deletes a favorite quiz from the questions table 
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns>
    /// Returns whether or not the favorite quiz was successfully deleted
    /// </returns>
    Task<bool> DeleteFavoriteQuiz(long quizId);

    /// <summary>
    /// Gets an active quiz based off of an access code.
    /// </summary>
    /// <param name="accessCode">The code of the quiz</param>
    /// <returns></returns>
    Task<ActiveQuiz?> GetActiveQuiz(string accessCode);

    /// <summary>
    /// Submits a multiple choice question with its choice.
    /// </summary>
    /// <param name="question">The question you are submitting to</param>
    /// <param name="choice">The index of the choice the student selected</param>
    /// <returns></returns>
    Task<bool> GiveMultipleChoiceQuestionAnswer(ActiveQuestion question, int choice);

    /// <summary>
    /// Submits a fill blank question with its answer.
    /// </summary>
    /// <param name="question">The question you are submitting to</param>
    /// <param name="response">The answer the student typed</param>
    /// <returns></returns>
    Task<bool> GiveFillBlankQuestionAnswer(ActiveQuestion question, string response);

    /// <summary>
    /// Joins an active quiz, awaiting active questions to come in.
    /// </summary>
    /// <param name="quiz">The quiz the student is joining</param>
    /// <param name="handler">The handler for when a new active question comes in</param>
    /// <returns></returns>
    Task<bool> JoinActiveQuiz(ActiveQuiz quiz, NewActiveQuestionHandler handler);

    /// <summary>
    /// This makes sure that an access code given by the user is a valid access code by checking the db
    /// </summary>
    /// <param name="accessCode"></param>
    /// <returns>
    /// True if the access code is valid otherwise false
    /// </returns>
    Task<bool> ValidateAccessCode(string accessCode);
    Task<bool> EditQuizTitle(string newQuizTitle);
}

public delegate void NewActiveQuestionHandler(ActiveQuestion newQuestion);
