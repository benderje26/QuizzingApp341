//using AuthenticationServices;
using System.Collections.ObjectModel;

namespace QuizzingApp341.Models;

public class BusinessLogic(IDatabase database) : IBusinessLogic {
    //public int CurrentQuestionIndex {  get; set; }
    public ObservableCollection<Question> CurrentQuestion {
        get {
            ObservableCollection<Question> currentQuestion = new ObservableCollection<Question>();
            currentQuestion.Add(CurrentQuiz.CurrentQuestion);
            return currentQuestion;
        }
    }
    
    public List<Question> Questions {
        get { return GetAllQuestions(); }
    }
    public List<Question> GetAllQuestions() {
        return database.LoadQuestions();
    }

    public Quiz CurrentQuiz {
        get {
            return currentQuiz;
        }
    }
    private Quiz currentQuiz = new Quiz("First Quiz", new DateTime(), new DateTime(), 1000, [new MultipleChoiceQuestion(0, "How many CS students does it take to screw in a lightbulb?", ["1", "3", "10", "30"], 3),
            new FillBlankQuestion(1, "What is our professor's name?", ["Dr. Rogers", "Professor Rogers"], false)]);

    public bool IncrementCurrentQuestion() {
        if (CurrentQuiz.HasNextQuestion()) {
            CurrentQuiz.NextQuestion();
            return true;
        }
        return false;
    }

    public bool DecrementCurrentQuestion() {
        if (CurrentQuiz.HasPreviousQuestion()) {
            CurrentQuiz.PreviousQuestion();
            return true;
        }
        return false;
    }

    public bool IsCurrentQuestionMultipleChoice() {
        if (CurrentQuestion[0] is MultipleChoiceQuestion) {
            return true;
        } else {
            return false;
        }
    }
}
