namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Windows.Input;

public partial class QuizStudio : ContentPage {
    public ICommand EditQuizCommand { get; set; }

    public QuizStudio() {
        InitializeComponent();
        EditQuizCommand = new Command<Quiz>(EditQuiz);
        BindingContext = MauiProgram.BusinessLogic;
    }
    private async void EditQuiz(Quiz quiz) {
        QuizManager quizManager = new(quiz);
        await quizManager.GetQuestions();
        MauiProgram.BusinessLogic.QuizManager = quizManager;
        await Navigation.PushAsync(new EditQuiz());
    }


    private async void OnCreateNewQuizButtonClicked(object sender, EventArgs e) {
        Quiz newQuiz = new() {
            Title = "New Quiz"
        };
        QuizManager quizManager = new(newQuiz);
        MauiProgram.BusinessLogic.QuizManager = quizManager;
        await MauiProgram.BusinessLogic.AddQuiz(newQuiz);
        await Navigation.PushAsync(new EditQuiz());
    }

}