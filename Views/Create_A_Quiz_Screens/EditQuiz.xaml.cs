namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;

public partial class EditQuiz : ContentPage {
    public ICommand QuestionClickedCommand { get; set; }
    public bool IsNewQuiz { get; set; }
    public double ScreenWidth { get; set; }
    private readonly QuizManager? manager;
    public EditQuiz() {
        InitializeComponent();
        manager = MauiProgram.BusinessLogic.QuizManager;
        ScreenWidth = DeviceDisplay.MainDisplayInfo.Width;
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
        if (manager?.Quiz == null) {
            return;
        }

        question.QuestionNo = manager.Questions.Count + 1;
        question.QuizId = manager.Quiz.Id;

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
        if (manager == null) {
            return;
        }
        bool deleteQuestion = await DisplayAlert("Are you sure you want to delete this quiz?", manager.Quiz?.Title , "Yes", "No");
        
        if (deleteQuestion) {
            if (manager.Active) {
                await DisplayAlert("Could not delete the following quiz because it is still active", manager.Quiz?.Title, "Ok");
            } else {
                if (manager.Quiz != null) {
                    await MauiProgram.BusinessLogic.DeleteQuiz(manager.Quiz.Id);
                }
                await Navigation.PopAsync();
            }
        }
    }

    public async void OnActivateQuiz(object sender, EventArgs e) {
        // Prepare the quiz
        await MauiProgram.BusinessLogic.PrepareActiveQuiz();

        if (manager?.ActiveQuiz?.AccessCode == null) {
            return;
        }

        // Display the access code
        string accessCode = manager.ActiveQuiz.AccessCode;

        // Use this to start the quiz
        bool startQuiz = await DisplayAlert("Access Code:", accessCode, "Start Quiz", "Cancel");

        if (startQuiz) {
            await MauiProgram.BusinessLogic.ActivateActiveQuiz();
            // Display the page to start the quiz
            // Make the current question an active question
            ActiveQuestion activeQuestion = new ActiveQuestion(MauiProgram.BusinessLogic.QuizManager.CurrentQuestion, MauiProgram.BusinessLogic.QuizManager.ActiveQuiz.Id);
            if (MauiProgram.BusinessLogic.QuizManager.CurrentQuestion.QuestionType == QuestionType.MultipleChoice) {
                await Navigation.PushAsync(new MultipleChoice(activeQuestion, true, false));
            } else {
                await Navigation.PushAsync(new FillBlank(activeQuestion, true, false));
            }
        } else {
            await MauiProgram.BusinessLogic.DeactivateQuiz();
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