using System.Collections.ObjectModel;

namespace QuizzingApp341.Models;

public class BusinessLogic(IDatabase database) : IBusinessLogic {
    public int CurrentQuestionIndex {  get; set; }
    public ObservableCollection<Question> CurrentQuestion {
        get { 
            ObservableCollection<Question> currentQuestion = new ObservableCollection<Question>();
            currentQuestion.Add(Questions[CurrentQuestionIndex]);
            return currentQuestion;
            }
    }

    public List<Question> Questions {
        get { return GetAllQuestions(); }
    }
    public List<Question> GetAllQuestions() {
        return database.LoadQuestions();
    }
}
