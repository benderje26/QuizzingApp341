﻿namespace QuizzingApp341.Models;

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
    Task<(LogoutResult, string?)> Logout();

    Task<Quiz?> GetQuiz(string id);
    bool SetQuiz(Quiz quiz);
    Question? NextQuestion();
    Question? PreviousQuestion();
    bool SetCurrentMultipleChoiceAnswer(int optionIndex);
    bool SetCurrentFillBlankAnswer(string value);
}
