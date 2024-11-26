namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

public partial class QuizStudio : ContentPage {
    public ICommand EditQuizCommand {get; set;}
    public ObservableCollection<Quiz> CreatedQuizzes {get; set;}
    public QuizStudio() {
        InitializeComponent();
        CreatedQuizzes = MauiProgram.BusinessLogic.UserInfo?.CreatedQuizzes ?? [];
        EditQuizCommand = new Command<Quiz>(EditQuiz);
        BindingContext = this;
    }

    private async void EditQuiz(Quiz quiz) {
        QuizManager quizManager = new QuizManager(quiz);
        var test = await quizManager.GetQuestions();
      
        await Navigation.PushAsync(new EditQuiz(quizManager));
    }
  

    private void OnCreateNewQuizButtonClicked(object sender, EventArgs e) {
        Navigation.PushAsync(new CreateNewQuiz());
    }

}