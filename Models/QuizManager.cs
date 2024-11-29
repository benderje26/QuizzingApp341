using System.Collections.ObjectModel;

namespace QuizzingApp341.Models;

// This class represents a quiz that contains a title, date created, last date activated, questions and answers
public class QuizManager {

    // The quiz table from supabase that has columns Id, created_at, creator, title
    public Quiz? Quiz { get; set; }
    public ObservableCollection<Question> Questions { get; set; } = [];

    public ActiveQuiz? ActiveQuiz { get; set; }

    static int CurrentQuestion { get; set; } = 0;

    // Constructor for setting a quiz
    public QuizManager(Quiz quiz) {
        Quiz = quiz;
        Questions = [];
    }

    /// <summary>
    /// Sets an active quiz, if a user is about to take a quiz, it also needs to set the Quiz and Questions
    /// Call SetActiveQuizInfo() after this constructor
    /// Quiz quiz = new Quiz(ActiveQuiz);
    /// await quiz.SetActiveQuizInfo();
    /// </summary>
    /// <param name="activeQuiz"></param>
    public QuizManager(ActiveQuiz activeQuiz) {
        ActiveQuiz = activeQuiz;
    }

    // Overloaded constructor for taking a quiz
    public QuizManager(Quiz quiz, ObservableCollection<Question> questions) {
        Quiz = quiz;
        Questions = questions;
    }

    /// <summary>
    /// Sets the Quiz and questions from an activated quiz
    /// Should be called after the Quiz is activated passing in the active quiz to the constructor
    /// </summary>
    /// <returns>
    /// returns true if it successfully updated Quiz and Questions otherwise false
    /// </returns>
    public async Task<bool> SetActiveQuizInfo() {
        if (ActiveQuiz != null) {
            Quiz = await MauiProgram.BusinessLogic.GetQuiz(ActiveQuiz.QuizId);
            Questions = await MauiProgram.BusinessLogic.GetQuestions(ActiveQuiz.QuizId) ?? [];
            return true;
        }
        return false;
    }

    //TODO
    /// <summary>
    /// This starts a quiz for a user, which needs an id
    /// </summary>
    /// <param name="accessCode"></param>
    public void StartQuiz(string accessCode) {// Can either take a LiveQuiz Id or an Access code
        // ActiveQuiz = BusinessLogic.GetActiveQuizFromAccessCode(accessCode);
        // Get the Quiz with the quiz id
        // Get the supabasequestions with the quiz id
    }

    /// <summary>
    /// Gets all of the questions for THIS quiz 
    /// </summary>
    /// <returns>
    /// returns true if it successfully gets all the questions for this quiz
    /// </returns>
    public async Task<bool> GetQuestions() {
        try {
            // Get all the questions from db using Quiz.Id
            var result = await MauiProgram.BusinessLogic.GetQuestions(Quiz.Id);

            if (result == null) {
                return false;
            }

            Questions = result;
            return true;
        } catch {
            return false;
        }
    }

    /// <summary>
    /// Adds a question to the database and if successful, adds it to this Questions list.
    /// Mainly used for when the user is creating a quiz
    /// </summary>
    /// <param name="question"></param>
    /// <returns>
    /// Returns true if its successfully added to the db otherwise false... could return question id instead...
    /// </returns>
    //
    public async Task<bool> AddQuestion(Question question) {
        try {
            // Add the question to the db
            long? id = await MauiProgram.BusinessLogic.AddQuestion(question); // returns null if the question was not added, otherwise returns the id assigned to the question
            if (id == null) { // If the question was not added to the db, return false
                return false;
            }

            // If the question was added to the db set the id of the question to the one returned from the db and add it to the questions list 
            question.Id = id ?? 0;
            Questions.Add(question);
        } catch {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Deletes a question in the db using a questionId
    /// </summary>
    /// <param name="questionId"></param>
    /// <returns>
    /// returns true if successfully deleted in db otherwise false
    /// </returns>
    public async Task<bool> DeleteQuestion(long questionId) {
        try {
            // Delete from the table in db using the question id
            if (await MauiProgram.BusinessLogic.DeleteQuestion(questionId)) {
                return true;
            }

            // If successful with db, delete/reset it in the Questions list
            await GetQuestions();
        } catch {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Replaces all of the contents of the question in db using the question id
    /// </summary>
    /// <param name="question"></param>
    /// <returns>
    /// returns true if successfully edited in db otherwise false
    /// </returns>
    public async Task<bool> EditQuestion(Question question) {
        try {
            // Update the question 
            if (await MauiProgram.BusinessLogic.EditQuestion(question)) {
                // Fix the question in THIS Questions list
                for (int i = 0; i < Questions.Count; i++) {
                    if (Questions[i].Id == question.Id) {
                        Questions[i] = question;
                        return true;
                    }
                }

            }
        } catch {
            return false;
        }
        return true;
    }

    public async Task<bool> NextQuestion() {
        // TODO
        // Change the current question in DB
        CurrentQuestion++;

        // Return true if successful
        return true;
    }

    public async Task<bool> PreviousQuestion() {
        // TODO
        // Change the current question in DB
        CurrentQuestion--;

        // Return true if successful
        return true;
    }
}