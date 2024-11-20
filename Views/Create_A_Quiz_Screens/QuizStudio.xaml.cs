namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;

public partial class QuizStudio : ContentPage {
    UserInfo userInfo = MauiProgram.BusinessLogic.UserInfo();
    ObservableCollection<Quiz> CreatedQuizzes {get; set;} = [];
    public QuizStudio() {
        InitializeComponent();
        CreatedQuizzes = userInfo.CreatedQuizzes;
    }


    // Event handler for button click
    private void StudyButtonClickedCreateQuiz(object sender, EventArgs e) {
        // pull out the quiz that got clicked
    }


    private void OnCreateNewQuizButtonClicked(object sender, EventArgs e) {
        Navigation.PushAsync(new CreateNewQuiz());
    }

}