namespace QuizzingApp341.Models;

public interface IBusinessLogic {
    Task<List<Question>> GetAllQuestions();
    Task SetQuiz();
    bool IncrementCurrentQuestion();
    bool DecrementCurrentQuestion();
    bool IsCurrentQuestionMultipleChoice();
}
