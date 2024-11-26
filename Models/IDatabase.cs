﻿namespace QuizzingApp341.Models;

public interface IDatabase {
    Task SkipLogin(); // TODO DELETE THIS WHEN LOGIN WORKS
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
    Task<Quiz?> GetQuizById(long id);

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
    Task<bool> DeleteQuestion(long id);

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
    /// <param name="userId"></param>
    /// <returns>
    /// Returns a list of all the quizzes the user has created
    /// </returns>
    Task<List<Quiz>?> GetUserCreatedQuizzes(Guid? userId);

    /// <summary>
    /// Gets the user's info for the currently logged in user. This should not request any data from the database,
    /// but should run in generally a very short amount of time.
    /// </summary>
    /// <returns>The user's info, or null if there is no logged in user</returns>
    UserInfo? GetUserInfo();

    Task<List<long?>> GetActiveQuizIdsByUserId();


    Task<List<ActiveQuiz>?> GetQuizIdsByActiveQuizIds(List<long?> activeQuizIds);
}
