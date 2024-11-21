namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

public partial class QuizStudio : ContentPage {
    public ObservableCollection<Quiz> CreatedQuizzes {get; set;} = new ObservableCollection<Quiz>();
    public ICommand EditQuizCommand {get; set;}
    public QuizStudio() {
        InitializeComponent();
        CreatedQuizzes = MauiProgram.BusinessLogic.UserInfo.CreatedQuizzes;
        EditQuizCommand = new Command<Quiz>(EditQuiz);

        foreach (Quiz quiz in CreatedQuizzes) {
            Console.WriteLine(quiz.Title);
        }

        BindingContext = this;
    }

    private async void EditQuiz(Quiz quiz) {
        await Navigation.PushAsync(new EditQuiz(quiz));
    }
  

    private void OnCreateNewQuizButtonClicked(object sender, EventArgs e) {
        Navigation.PushAsync(new CreateNewQuiz());
    }

}