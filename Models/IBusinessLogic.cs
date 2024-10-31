﻿namespace QuizzingApp341.Models;

public interface IBusinessLogic {
    List<Question> GetAllQuestions();
    bool IncrementCurrentQuestion();
    bool DecrementCurrentQuestion();
    bool IsCurrentQuestionMultipleChoice();
}