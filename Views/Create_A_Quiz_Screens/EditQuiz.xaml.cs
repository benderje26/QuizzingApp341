namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;

public partial class EditQuiz : ContentPage {
    public ICommand QuestionClickedCommand { get; set; }
    public bool IsNewQuiz { get; set; }
    public double ScreenWidth { get; set; }
    public EditQuiz(QuizManager quizManager) {
        InitializeComponent();
        ScreenWidth = DeviceDisplay.MainDisplayInfo.Width; ;
        QuestionClickedCommand = new Command<Question>(QuestionClicked);
        BindingContext = MauiProgram.BusinessLogic;
    }

    private async void QuestionClicked(Question question) {
        // If the question is a multiple choice question
        if (question.QuestionType == QuestionType.MultipleChoice) {
            await Navigation.PushAsync(new CreateMultipleChoice(question, false));
        } else { // If the question is a fill in the blank question
            await Navigation.PushAsync(new CreateFillBlank(question, false));
        }
    }

    public async void QuizTitleChanged(object sender, EventArgs e) {
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

        question.QuestionNo = MauiProgram.BusinessLogic.QuizManager.Questions.Count + 1;
        question.QuizId = MauiProgram.BusinessLogic.QuizManager.Quiz.Id;

        popup.QuestionTypeSelected += async (questionType) => {   // get questionType when clicked
            if (questionType == QuestionType.MultipleChoice) {
                question.QuestionType = QuestionType.MultipleChoice;
                await Task.Delay(100); // delay time
                await Navigation.PushAsync(new CreateMultipleChoice(question, true));

            } else if (questionType == QuestionType.FillBlank) {
                question.QuestionType = QuestionType.FillBlank;
                await Task.Delay(100); // delay time
                await Navigation.PushAsync(new CreateFillBlank(question, true));
            }
        };

        // Show the popup
        await this.ShowPopupAsync(popup);
    }

    public async void OnDeleteQuiz(object sender, EventArgs e) {
        bool deleteQuestion = await DisplayAlert("Are you sure you want to delete this quiz?", MauiProgram.BusinessLogic.QuizManager.Quiz.Title, "Yes", "No");

        if (deleteQuestion) {
            if (MauiProgram.BusinessLogic.QuizManager.Active) {
                await DisplayAlert("Could not delete the following quiz because it is still active", MauiProgram.BusinessLogic.QuizManager.Quiz.Title, "Ok");
            } else {
                await MauiProgram.BusinessLogic.DeleteQuiz(MauiProgram.BusinessLogic.QuizManager.Quiz.Id);
                await Navigation.PopAsync();
            }
        }
    }

    public async void OnActivateQuiz(object sender, EventArgs e) {
        // Activate the quiz
        await MauiProgram.BusinessLogic.ActivateQuiz();

        // Display the access code
        string accessCode = MauiProgram.BusinessLogic.QuizManager.ActiveQuiz.AccessCode;

        // Use this to start the quiz
        bool startQuiz = await DisplayAlert("Access Code:", accessCode, "Start Quiz", "Cancel");

        // If start quiz ---> start Quiz
        if (startQuiz) {
            // Display the page to start the quiz
        }
    }

    public async void OnPublicToggled(object sender, ToggledEventArgs e) {
        try {
            // Optionally: Check the new toggle state (e.Value)
            bool newState = e.Value;

            // Perform the async operation without blocking the UI
            await MauiProgram.BusinessLogic.ChangeQuizVisibility(newState);

            // Log or handle the result if needed
            Console.WriteLine($"Quiz visibility changed to: {newState}");
        } catch (Exception ex) {
            // Handle any errors gracefully
            Console.WriteLine($"Error changing quiz visibility: {ex.Message}");
        }
    }

}