using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

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
        get { return questions ?? GetAllQuestions().Result; }
    }
    private List<Question>? questions;

    /*
     * Returns all of the questions from the database
     * 
     * @return - all the questions from the database
     */
    public async Task<List<Question>> GetAllQuestions() {
        return questions = await database.LoadQuestions();
    }

    public async Task SetQuiz() {
        await GetAllQuestions();
    }

    public Quiz CurrentQuiz {
        get {
            currentQuiz ??= new Quiz("First Quiz", new DateTime(), new DateTime(), 1000, Questions);
            return currentQuiz;
        }
    }
    private Quiz? currentQuiz;

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

    public async Task<string?> CreateNewUser(string emailAddress, string username, string password) {
        if (!Regexes.EmailRegex().IsMatch(emailAddress)) {
            return "Email must be in the correct format (ex: example@example.com).";
        }
        if (!Regexes.UsernameRegex().IsMatch(username)) {
            return "Username must be 4 to 32 characters and only contain A-Z, a-z, 0-9, and _ (underscores).";
        }
        if (!Regexes.PasswordRegex().IsMatch(password)) {
            return "Password is not strong enough or invalid. It must be 8 characters with a letter, number, and symbol, or at least 16 characters of any kind.";
        }

        bool success = await database.CreateNewUser(emailAddress, username, password);

        if (!success) {
            return "Something went wrong, possibly dupliacte email/username.";
        }

        return null;
    }

    public async Task<bool> LogIn(string emailAddress, string password) {
        return await database.LogIn(emailAddress, password);
    }
}

partial class Regexes {
    // must be a@b.c where a, b, and c are alphanumeric/underscore/"." but a little more complex
    [GeneratedRegex(@"^\w+(\.\w+)*@\w+(\.\w+)+$")]
    public static partial Regex EmailRegex();

    // must be 4-32 letters, numbers, or underscores
    [GeneratedRegex(@"^\w{4,32}$")]
    public static partial Regex UsernameRegex();

    // basically it must be at least 8 characters, and it must have a letter, number and symbol OR be at least 16 characters
    [GeneratedRegex(@"^(?=.*\w|.{16,})(?=.*\d|.{16,})(?=.*[\W_]|.{16,})[a-zA-z0-9!@#$%^&*()_\-=+\[\]{}<>\\|;:'"",.?/`~]{8,32}$")]
    public static partial Regex PasswordRegex();
}
