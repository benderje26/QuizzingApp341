namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;

public partial class EditQuiz : ContentPage {
    public ICommand QuestionClickedCommand { get; set; }
    public EditQuiz(QuizManager quizManager) {
        InitializeComponent();
        MauiProgram.BusinessLogic.QuizManager = quizManager;
        QuestionClickedCommand = new Command<Question>(QuestionClicked);
        BindingContext = MauiProgram.BusinessLogic;
    }

    private async void QuestionClicked(Question question) {
        // If the question is a multiple choice question
        if (question.QuestionType == QuestionType.MultipleChoice) {
            await Navigation.PushAsync(new CreateMultipleChoice(question));
        } else { // If the question is a fill in the blank question
            await Navigation.PushAsync(new CreateFillBlank(question));
        }
    }

    public async void QuizTitleChanged(object senter, EventArgs e) {
        await MauiProgram.BusinessLogic.EditQuizTitle(NewQuizTitle.Text);
    }

    public async void AddQuestionClicked(object sender, EventArgs e) {
        Console.WriteLine("************Question Clicked!!!!!");
        var popup = new CreateNewQuestionPopup();

        // Make a new question with this current quiz Id
        Question question = new Question();
        if (MauiProgram.BusinessLogic.QuizManager?.Quiz == null) {
            return;
        }

        question.QuestionNo = MauiProgram.BusinessLogic.QuizManager.Questions.Count;
        question.QuizId = MauiProgram.BusinessLogic.QuizManager.Quiz.Id;

        popup.QuestionTypeSelected += async (questionType) => {   // get questionType when clicked
            if (questionType == QuestionType.MultipleChoice) {
                question.QuestionType = QuestionType.MultipleChoice;
                await Task.Delay(100); // delay time
                await Navigation.PushAsync(new CreateMultipleChoice(question));

            } else if (questionType == QuestionType.FillBlank) {
                question.QuestionType = QuestionType.FillBlank;
                await Task.Delay(100); // delay time
                await Navigation.PushAsync(new CreateFillBlank(question));
            }
        };

        // Show the popup
        await this.ShowPopupAsync(popup);
    }
} 