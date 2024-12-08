using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

public partial class CreateFillBlank : ContentPage {
    public Question? FillBlankQuestionToChange { get; set; } // This is for editing a current question only if there is one to edit
    public bool? QuestionPresent { get; set; } = false;
    public bool? NoQuestionPresent { get; set; } = false;
    public bool? AnswerPresent { get; set; } = false;

    public string? Answers { get; set; }

    public string? QuestionText { get; set; }

    private bool isNewQuestion { get; set; }

    public CreateFillBlank(Question? question, bool isNewQuestion) {
        FillBlankQuestionToChange = question;
        this.isNewQuestion = isNewQuestion;
        // If there is a question present to edit
        if (FillBlankQuestionToChange != null) {
            NoQuestionPresent = false;
            QuestionPresent = true;
            QuestionText = question?.QuestionText;

            if (question?.AcceptableAnswers != null) { // If there are any answers
                Answers = string.Join(", ", question.AcceptableAnswers);
                AnswerPresent = true;
            }

        } else {
            NoQuestionPresent = true;
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
        FillBlankQuestionToChange.QuestionText = question;
        FillBlankQuestionToChange.AcceptableAnswers = [answer];
    }


    private async void OnSaveClicked(object sender, EventArgs e) {
        if (isNewQuestion) {
            await MauiProgram.BusinessLogic.AddQuestion(FillBlankQuestionToChange);
        } else {
            await MauiProgram.BusinessLogic.EditQuestion(FillBlankQuestionToChange);
        }
        // Navigate back to the CreateNewQuiz page
        await Navigation.PopAsync();
    }

    private async void OnDeleteQuestionClicked(object sender, EventArgs e) {
        RetrieveData();
        bool deleteQuestion = await DisplayAlert("Are you sure you would like to delete this question?", FillBlankQuestionToChange?.QuestionText, "Yes", "No");
        if (deleteQuestion) {
            await MauiProgram.BusinessLogic.DeleteQuestion(FillBlankQuestionToChange.Id);
        } 
        // Navigate back to the CreateNewQuiz page
        await Navigation.PopAsync();
    }
}