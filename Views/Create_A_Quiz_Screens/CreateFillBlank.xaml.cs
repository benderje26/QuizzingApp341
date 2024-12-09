using QuizzingApp341.Models;
using System.Text.Json;

namespace QuizzingApp341.Views;

public partial class CreateFillBlank : ContentPage {
    public Question Question { get; set; }
    public bool? QuestionPresent { get; set; } = false;
    public bool? NoQuestionPresent { get; set; } = false;
    public bool? AnswerPresent { get; set; } = false;

    public string? Answers { get; set; }

    public string? QuestionText { get; set; }

    public bool IsNewQuestion { get; set; }

    public bool IsEditQuestion { get; set; }

    public CreateFillBlank(Question? question) {
        IsNewQuestion = question == null;
        IsEditQuestion = !IsNewQuestion;
        // If there is a question present to edit
        if (question != null) {
            Question = question;
            NoQuestionPresent = false;
            QuestionPresent = true;
            QuestionText = question?.QuestionText;

            if (question?.AcceptableAnswers != null) { // If there are any answers
                Answers = string.Join(", ", question.AcceptableAnswers);
                AnswerPresent = true;
            }
        } else {
            NoQuestionPresent = true;
            Question = new() { QuestionType = QuestionType.FillBlank };
        }

        InitializeComponent();
        BindingContext = this;
    }

    private void RetrieveData() {
        // Retrieve data from user input
        //check for null, if null - replace with empty string
        string question = QuestionFillBlank.Text != null ? QuestionFillBlank.Text.Trim() : string.Empty;
        string answer = AnswerFillBlank.Text != null ? AnswerFillBlank.Text.Trim() : string.Empty;

        // Check for required fields not empty
        if (string.IsNullOrEmpty(question)) {
            DisplayAlert("Error", "Please enter a question.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(answer)) {
            DisplayAlert("Error", "Please enter a correct answer.", "OK");
            return;
        }

        // Do something with the retrieved data - make a question object - saving to Database
        Question.QuestionText = question;
        Question.AcceptableAnswers = [answer];
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
        bool deleteQuestion = await DisplayAlert("Are you sure you would like to delete this question?", Question?.QuestionText, "Yes", "No");
        if (deleteQuestion && Question != null) {
            var result = await MauiProgram.BusinessLogic.DeleteQuestion(Question.Id);
            if (result.Item1 != DeleteQuestionResult.Success) {
                await DisplayAlert("Error. Cannot delete question", result.Item2, "Ok");
            }
        } 

        // Navigate back to the CreateNewQuiz page
        await Navigation.PopAsync();
    }
}