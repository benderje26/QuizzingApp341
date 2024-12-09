namespace QuizzingApp341.Views;
using System;
using Microsoft.Maui.Controls;
using QuizzingApp341.Models;

public partial class CreateMultipleChoice : ContentPage {
    public Question Question { get; set; }
    public bool? QuestionPresent { get; set; } = false;
    public bool? NoQuestionPresent { get; set; } = false;
    public bool? AnswerPresent { get; set; } = false;
    public string? Answers { get; set; }
    public string? OptionA { get; set; }
    public string? OptionB { get; set; }
    public string? OptionC { get; set; }
    public string? OptionD { get; set; }

    public int? CorrectOption { get; set; }

    public string? QuestionText { get; set; }

    public bool IsNewQuestion { get; set; }

    public bool IsEditQuestion { get; set; }
    public CreateMultipleChoice(Question? question, bool isNewQuestion) {
        IsNewQuestion = isNewQuestion;
        IsEditQuestion = !IsNewQuestion;

        // If there is a question present to edit
        if (question != null) {
            Question = question;
            NoQuestionPresent = false;
            QuestionPresent = true;
            QuestionText = question.QuestionText;

            if (question.AcceptableAnswers != null) { // If there are any answers
                Answers = string.Join(", ", question.AcceptableAnswers);
                AnswerPresent = true;
            }

            // Set all the options with the question's current answer options
            OptionA = question.MultipleChoiceOptions?[0];
            OptionB = question.MultipleChoiceOptions?[1];
            OptionC = question.MultipleChoiceOptions?[2];
            OptionD = question.MultipleChoiceOptions?[3];
            CorrectOption = question.MultipleChoiceCorrectAnswers?[0] ?? 0;
        } else {
            NoQuestionPresent = true;
            Question = new() { QuestionType = QuestionType.MultipleChoice };
        }

        InitializeComponent();
        BindingContext = this;
    }

    private int getCorrectAnswerNumber(string correctAnswer) {
        correctAnswer = correctAnswer.Substring(correctAnswer.Length - 1);
        switch (correctAnswer) {
            case "A":
                return 1;
            case "B":
                return 2;
            case "C":
                return 3;
            case "D":
                return 4;
            default:
                return 0;
        }
    }

    private void RetrieveData() {
        // Retrieve data from user input
        //check for null, if null - replace with empty string
        string question = questionMultipleChoice.Text != null ? questionMultipleChoice.Text.Trim() : string.Empty;
        string optionA = optionAEntry.Text != null ? optionAEntry.Text.Trim() : string.Empty;
        string optionB = optionBEntry.Text != null ? optionBEntry.Text.Trim() : string.Empty;
        string optionC = optionCEntry.Text != null ? optionCEntry.Text.Trim() : string.Empty;
        string optionD = optionDEntry.Text != null ? optionDEntry.Text.Trim() : string.Empty;
        int correctOption = correctAnswerFromUser.SelectedIndex;
        // int correctAnswer = getCorrectQuestion(correctOption);
        string[] options = [optionA, optionB, optionC, optionD];

        // Check for required fields not empty
        if (string.IsNullOrEmpty(question)) {
            DisplayAlert("Error", "Please enter a question.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(optionA) || string.IsNullOrEmpty(optionB) ||
            string.IsNullOrEmpty(optionC) || string.IsNullOrEmpty(optionD)) {
            DisplayAlert("Error", "Please provide all options.", "OK");
            return;
        }

        Question.QuestionText = question;
        Question.MultipleChoiceOptions = options;
        Question.MultipleChoiceCorrectAnswers = [correctOption];
    }
    private async void OnSaveClicked(object sender, EventArgs e) {
        RetrieveData();

        if (IsNewQuestion) {
            await MauiProgram.BusinessLogic.AddQuestion(Question);
        } else {
            await MauiProgram.BusinessLogic.EditQuestion(Question);
        }

        // Navigate back to the CreateNewQuiz page
        await Navigation.PopAsync();
    }

    private async void OnDeleteQuestionClicked(object sender, EventArgs e) {
        RetrieveData();
        bool deleteQuestion = await DisplayAlert("Are you sure you would like to delete this question?", Question.QuestionText, "Yes", "No");
        if (deleteQuestion) {
            await MauiProgram.BusinessLogic.DeleteQuestion(Question.Id);
        }
        // Navigate back to the CreateNewQuiz page
        await Navigation.PopAsync();
    }
}
