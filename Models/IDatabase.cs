namespace QuizzingApp341.Models;

public interface IDatabase {
    Task<AccountCreationResult> CreateNewUser(string emailAddress, string username, string password);
    Task<LoginResult> LogIn(string emailAddress, string password);
    public Task<List<Question>> LoadQuestions();
}
