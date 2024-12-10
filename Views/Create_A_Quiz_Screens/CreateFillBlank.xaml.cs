using QuizzingApp341.Models;
using System.Text.Json;

namespace QuizzingApp341.Views;

public partial class CreateFillBlank : ContentPage {
    public string ScreenTitle => IsNewQuestion ? "Create Question" : "Edit Question";
    public Question Question { get; set; }
    public bool QuestionPresent { get; set; } = false;
    public bool NoQuestionPresent => !QuestionPresent;
    public bool AnswerPresent { get; set; } = false;
    public bool CaseSensitive { get; set; } = false;
    public string Answers { get; set; } = string.Empty;

    public string QuestionText { get; set; } = string.Empty;

    public bool IsNewQuestion { get; set; }

    public bool IsEditQuestion => !IsNewQuestion;

    public CreateFillBlank(Question question, bool isNewQuestion) {
        IsNewQuestion = isNewQuestion;
        // If there is a question present to edit
        if (question != null) {
            Question = question;
            QuestionPresent = true;
            QuestionText = question.QuestionText;
            CaseSensitive = question?.CaseSensitive ?? false;

            if (question?.AcceptableAnswers != null) { // If there are any answers
                Answers = string.Join(", ", question.AcceptableAnswers);
                AnswerPresent = true;
            }
        } else {
            QuestionPresent = false;
            Question = new() { QuestionType = QuestionType.FillBlank };
        }

        InitializeComponent();
        BindingContext = this;
    }

    private bool RetrieveData() {
        // Retrieve data from user input
        string question = questionName.Text.Trim();
        string[] answers = answerText.Text
            .Split(',')
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();

        // Check for required fields not empty
        if (string.IsNullOrEmpty(question)) {
            DisplayAlert("Error", "Please enter a question.", "OK");
            return false;
        }

        if (answers.Length == 0) {
            DisplayAlert("Error", "Please enter a correct answer.", "OK");
            return false;
        }

        // Do something with the retrieved data - make a question object - saving to Database
        Question.QuestionText = question;
        Question.AcceptableAnswers = answers;
        Question.CaseSensitive = CaseSensitive;
        return true;
    }


    private async void OnSaveClicked(object sender, EventArgs e) {
        bool goodToSave = RetrieveData();
        if (!goodToSave) {
            return;
        }

        if (IsNewQuestion) {
            await MauiProgram.BusinessLogic.AddQuestion(Question);
        } else {
            await MauiProgram.BusinessLogic.EditQuestion(Question);
        }
        // Navigate back to the CreateNewQuiz page
        await Navigation.PopAsync();
    }

    private async void OnDeleteQuestionClicked(object sender, EventArgs e) {
        bool deleteQuestion = await DisplayAlert("Are you sure you would like to delete this question?", Question?.QuestionText, "Yes", "No");
        if (deleteQuestion && Question != null) {
            var result = await MauiProgram.BusinessLogic.DeleteQuestion(Question.Id);
            if (result.Item1 != DeleteQuestionResult.Success) {
                await DisplayAlert("Error. Cannot delete question", result.Item2, "Ok");
            } else {
                // Navigate back to the CreateNewQuiz page
                await Navigation.PopAsync();
            }
        }
    }
}