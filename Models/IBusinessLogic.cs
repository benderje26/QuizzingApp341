﻿namespace QuizzingApp341.Models;
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
    /// Attempts to update the users email.
    /// </summary>
    /// <param name="emailAddress">The email address</param>
    /// <returns>The result and a nullable string showing the message if something went wrong</returns>
    Task<(UpdateEmailResult, string?)> UpdateEmail(string emailAddress);

    /// <summary>
    /// Attempts to update the users username.
    /// </summary>
    /// <param name="username">The username</param>
    /// <returns>The result and a nullable string showing the message if something went wrong</returns>
    Task<(UpdateUsernameResult, string?)> UpdateUsername(string username);

    /// <summary>
    /// Attempts to update the users password.
    /// </summary>
    /// <param name="password">The password</param>
    /// <returns>The result and a nullable string showing the message if something went wrong</returns>
    Task<(UpdatePasswordResult, string?)> UpdatePassword(string password);

    /// <summary>
    /// Attempts to log the user out.
    /// </summary>
    /// <returns>The result and a nullable string showing the message if something went wrong</returns>
    Task<(LogoutResult, string?)> Logout();

    /// <summary>
    /// Attempts to get the UserData object for a User ID.
    /// </summary>
    /// <param name="userId">The id of the user you need the UserData for</param>
    /// <returns>A UserData object for the user</returns>
    Task<UserData?> GetUserData(Guid userId);

    /// <summary>
    /// Attempts to delete the users account
    /// </summary>
    /// <returns>The result and a nullable string showing the message if something went wrong</returns>
    Task<(DeleteAccountResult, string?)> DeleteAccount();

    /// <summary>
    /// The current quiz manager.
    /// </summary>
    QuizManager? QuizManager { get; set; }

    Task<bool> ChangeQuizVisibility(bool isPublic);
    Task<long?> AddQuiz(Quiz quiz);

    /// <summary>
    /// Attempts to get a quiz.
    /// </summary>
    /// <param name="id">The given id of the quiz</param>
    /// <returns>The quiz if it is accessible, null if it is not or doesn't exist</returns>
    Task<Quiz?> GetQuiz(long id);

    Task<bool> DeleteQuiz(long quizId);

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
    Task<(DeleteQuestionResult, string?)> DeleteQuestion(long id);
    Task<ActiveQuiz?> GetActiveQuiz(long activeQuizId);

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
    Task<List<ActiveQuiz>> GetActiveQuizzesByActiveQuizIds(List<long> activeQuizIds);
    Task<bool> DeactivateQuiz();
    Task<bool> PrepareActiveQuiz(bool isLive = true);
    Task<bool> ActivateActiveQuiz();
    Task<bool> IncrementCurrentQuestion();

    /// <summary>
    /// Gets the current scores of the given active quiz
    /// </summary>
    /// <param name="activeQuizId">Current active quiz</param>
    /// <returns>List of all of the current scores for the active quiz and the total number of questions</returns>
    Task<(Dictionary<string, int>?, int)> GetQuizScoresForActiveQuizId(long activeQuizId);

    Task<List<Response>> GetResponses(long activeQuizId);

    Task<ObservableCollection<Quiz>?> GetAllPublicQuizzes();

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
    /// <param name="choices">The indexes of the choices the student selected</param>
    /// <returns></returns>
    Task<bool> GiveMultipleChoiceQuestionAnswer(ActiveQuestion question, int[]? choices);

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
    Task<bool> JoinActiveQuiz(ActiveQuiz quiz, NewActiveQuestionHandler questionHandler, QuizEndedHandler endedHandler);

    /// <summary>
    /// Stops listening for new questions for all active quizzes.
    /// </summary>
    void LeaveActiveQuiz();

    /// <summary>
    /// This makes sure that an access code given by the user is a valid access code by checking the db
    /// </summary>
    /// <param name="accessCode"></param>
    /// <returns>
    /// True if the access code is valid otherwise false
    /// </returns>
    Task<bool> ValidateAccessCode(string accessCode);
    Task<bool> EditQuizTitle(string newQuizTitle);
    public void RefreshQuestionNums();

    /// <summary>
    /// Deletes the quiz history data.
    /// </summary>
    /// <returns>
    /// True if it worked
    /// </returns>
    Task<bool> DeleteQuizFromActivationHistory(long activeQuizId);
}

public delegate void NewActiveQuestionHandler(ActiveQuestion newQuestion);

public delegate void QuizEndedHandler();