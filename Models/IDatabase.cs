namespace QuizzingApp341.Models;

public interface IDatabase {
    Task<bool> CreateNewUser(string emailAddress, string username, string password);
    Task<bool> LogIn(string emailAddress, string password);
    public Task<List<Question>> LoadQuestions();
}
