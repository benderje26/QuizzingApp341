namespace QuizzingApp341.Models;

public class BusinessLogic(IDatabase database) : IBusinessLogic {
    public List<Question> GetAllQuestions() {
        return database.LoadQuestions();
    }
}
