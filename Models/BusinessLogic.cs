//using AuthenticationServices;
using System.Collections.ObjectModel;

namespace QuizzingApp341.Models;

public class BusinessLogic(IDatabase database) : IBusinessLogic {

    /*
     * Get the current question to be displayed
     * 
     * @return - the question to be displayed
     */
    public ObservableCollection<Question> CurrentQuestion {
        get {
            ObservableCollection<Question> currentQuestion = new ObservableCollection<Question>();
            Question question = CurrentQuiz.CurrentQuestion;
            question.First = CurrentQuiz.HasPreviousQuestion();
            question.Final = CurrentQuiz.HasNextQuestion();
            question.NotFinal = !question.Final;
            currentQuestion.Add(question);
            return currentQuestion;
        }
    }
    
    /*
     * The current questions of the quiz being taken
     * 
     * @return - all the questions of the quiz
     */
    public List<Question> Questions {
        get { return GetAllQuestions(); }
    }

    /*
     * Returns all of the questions from the database
     * 
     * @return - all the questions from the database
     */
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

    /*
     * Moves to the next question of the quiz if there is one
     * 
     * @return - whether or not it moved to the next question
     */
    public bool IncrementCurrentQuestion() {
        if (CurrentQuiz.HasNextQuestion()) {
            CurrentQuiz.NextQuestion();
            return true;
        }
        return false;
    }

    /*
     * Moves to the previous question of the quiz if there is one
     * 
     * @return - whether or not it moved to the previous question
     */
    public bool DecrementCurrentQuestion() {
        if (CurrentQuiz.HasPreviousQuestion()) {
            CurrentQuiz.PreviousQuestion();
            return true;
        }
        return false;
    }

    /*
     * Checks to see if the current questions is a multiple choice question
     * and if not it means the question is a fill blank questions
     * 
     * @return - whether or not the question is multiple choice
     */
    public bool IsCurrentQuestionMultipleChoice() {
        if (CurrentQuestion[0] is MultipleChoiceQuestion) {
            return true;
        } else {
            return false;
        }
    }
}
