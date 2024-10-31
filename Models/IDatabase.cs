namespace QuizzingApp341.Models;

public interface IDatabase {
    public Task<List<Question>> LoadQuestions();
}
