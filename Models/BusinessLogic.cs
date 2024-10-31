namespace QuizzingApp341.Models;

public class BusinessLogic(IDatabase database) : IBusinessLogic {
    private readonly IDatabase database = database;

    public List<Question> GetAllQuestions() {
        return Task.Run(() => database.LoadQuestions()).Result;
    }
}
