namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
public partial class EditQuiz : ContentPage {
    public string QuizTitle {get; set;}
    public ObservableCollection<Question> Questions {get; set;}
    public ICommand QuestionClickedCommand {get; set;}

    public EditQuiz(QuizManager quizManager) {
        InitializeComponent();
        QuizTitle = (quizManager.Quiz?.Title != null) ? quizManager.Quiz.Title: "";
        Questions = quizManager.Questions;
        QuestionClickedCommand = new Command<long>(QuestionClicked);
        BindingContext = this;
    }

    private async void QuestionClicked(long questionId) {
        
    }
}