namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using System.ComponentModel;

public partial class EditQuiz : ContentPage {
    public ICommand QuestionClickedCommand { get; set; }
    public EditQuiz(QuizManager quizManager) {
        InitializeComponent();
        MauiProgram.BusinessLogic.EditQuizManager = quizManager;
        QuestionClickedCommand = new Command<Question>(QuestionClicked);
        BindingContext = MauiProgram.BusinessLogic;
    }

    private async void QuestionClicked(Question question) {
        // If the question is a multiple choice question
        if (question.QuestionType == 0) {
            await Navigation.PushAsync(new CreateMultipleChoiceQuiz(question, false));
        } else { // If the question is a fill in the blank question
            await Navigation.PushAsync(new CreateFillBlank(question, false));
        }
    }

    public async void QuizTitleChanged(object senter, EventArgs e) {
        await MauiProgram.BusinessLogic.EditQuizTitle(NewQuizTitle.Text);
    }

    public async void AddQuestionClicked(object sender, EventArgs e) {
        Console.WriteLine("************Question Clicked!!!!!");
        var popup = new CreateNewQuizPopup();

        // Make a new question with this current quiz Id
        Question question = new Question();
        question.QuestionNo =   MauiProgram.BusinessLogic.EditQuizManager.Questions.Count();
        question.QuizId = MauiProgram.BusinessLogic.EditQuizManager.Quiz.Id;

        popup.QuestionTypeSelected += async (questionType) => {   // get questionType when clicked
            if (questionType == "MultipleChoice") {
                question.QuestionType = QuestionType.MultipleChoice;
                await Task.Delay(100); // delay time
                await Navigation.PushAsync(new CreateMultipleChoiceQuiz(question, true));

            } else if (questionType == "FillInBlank") {
                question.QuestionType = QuestionType.FillBlank;
                await Task.Delay(100); // delay time
                await Navigation.PushAsync(new CreateFillBlank(question, true));
            }
        };

        // Show the popup
        await this.ShowPopupAsync(popup);
    }
} 