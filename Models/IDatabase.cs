namespace QuizzingApp341.Models;

public interface IDatabase {
    public List<Question> LoadQuestions();
}
