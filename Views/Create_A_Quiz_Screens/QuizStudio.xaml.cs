namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public partial class QuizStudio : ContentPage {
    public ICommand EditQuizCommand { get; set; }

    public QuizStudio() {
        InitializeComponent();
        EditQuizCommand = new Command<Quiz>(EditQuiz);
        BindingContext = MauiProgram.BusinessLogic;
    }
    private async void EditQuiz(Quiz quiz) {
        QuizManager quizManager = new QuizManager(quiz);
        await quizManager.GetQuestions();
        MauiProgram.BusinessLogic.QuizManager = quizManager;

        await Navigation.PushAsync(new EditQuiz(quizManager));
    }


    private async void OnCreateNewQuizButtonClicked(object sender, EventArgs e) {
        Quiz newQuiz = new Quiz();
        newQuiz.Title = "New Quiz";
        QuizManager quizManager = new QuizManager(newQuiz);
        MauiProgram.BusinessLogic.QuizManager = quizManager;
        await MauiProgram.BusinessLogic.AddQuiz(quizManager.Quiz);
        await Navigation.PushAsync(new EditQuiz(quizManager));
    }

}