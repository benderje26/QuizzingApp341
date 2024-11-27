namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;
public partial class EditQuiz : ContentPage {
    public string QuizTitle {get; set;}
    public ObservableCollection<Question> Questions {get; set;}

    public EditQuiz(QuizManager quizManager) {
        InitializeComponent();
        QuizTitle = (quizManager.Quiz?.Title != null) ? quizManager.Quiz.Title: "";
        Questions = quizManager.Questions;
        BindingContext = this;
    }
}