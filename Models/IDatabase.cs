namespace QuizzingApp341.Models;

public interface IDatabase {
    Task<AccountCreationResult> CreateNewUser(string emailAddress, string username, string password);
    Task<LoginResult> Login(string emailAddress, string password);
    Task<LogoutResult> Logout();
    public Task<Quiz?> GetQuizById(string quizId);
}
