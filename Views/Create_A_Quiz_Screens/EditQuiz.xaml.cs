namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
public partial class EditQuiz : ContentPage {
    public string QuizTitle { get; set; }
    public ObservableCollection<Question> Questions { get; set; }
    public ICommand QuestionClickedCommand { get; set; }

    private Quiz? currentQuiz;
    public EditQuiz(QuizManager quizManager) {
        InitializeComponent();
        QuizTitle = (quizManager.Quiz?.Title != null) ? quizManager.Quiz.Title : "";
        Questions = quizManager.Questions;
        QuestionClickedCommand = new Command<Question>(QuestionClicked);
        currentQuiz = quizManager.Quiz;
        BindingContext = this;
    }

    private async void QuestionClicked(Question question) {
        // If the question is a multiple choice question
        if (question.QuestionType == 0) {
            await Navigation.PushAsync(new CreateMultipleChoiceQuiz(question));
        } else { // If the question is a fill in the blank question
            await Navigation.PushAsync(new CreateFillBlank(question));
        }
    }

    public async void AddQuestionClicked(object sender, EventArgs e) {
        var popup = new CreateNewQuizPopup();

            // Make a new question with this current quiz Id
            Question question = new Question();
            question.QuizId = currentQuiz.Id;
            popup.QuestionTypeSelected += async (questionType) => {   // get questionType when clicked
                if (questionType == "MultipleChoice") {
                    question.QuestionType = QuestionType.MultipleChoice;
                    await Task.Delay(100); // delay time
                    await Navigation.PushAsync(new CreateMultipleChoiceQuiz(question)); // Null here because you are creating a quiz

                } else if (questionType == "FillInBlank") {
                    question.QuestionType = QuestionType.FillBlank;
                    await Task.Delay(100); // delay time
                    await Navigation.PushAsync(new CreateFillBlank(question)); // Passing in null here because you're creating a new question
                }
            };

            // Show the popup
            await this.ShowPopupAsync(popup);
    }
}