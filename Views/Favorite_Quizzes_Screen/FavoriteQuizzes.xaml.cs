using QuizzingApp341.Models;
using Microsoft.Maui.Controls;

namespace QuizzingApp341.Views;

public partial class FavoriteQuizzes : ContentPage {
    public FavoriteQuizzes() {
        InitializeComponent();

        BindingContext = MauiProgram.BusinessLogic;
    }

    // Study the favorite quiz that got clicked
    private async void StudyQuiz_Clicked(object sender, EventArgs e) {
        Quiz selectedQuiz = CV.SelectedItem as Quiz;
        

        //Current code that starts a quiz by searching the id, but isn't working so once someone fixes the start quiz
        //by searching I will update this code to reflect it 

        //if (await MauiProgram.BusinessLogic.GetQuiz(selectedQuiz.Id) is Quiz quiz) {
        //    MauiProgram.BusinessLogic.SetQuiz(quiz);
        //    bool multipleChoice = MauiProgram.BusinessLogic.CurrentQuestion?.Type == Models.QuestionType.MultipleChoice;
        //    if (multipleChoice) {
        //        await Navigation.PushModalAsync(new MultipleChoice());
        //    } else {
        //        await Navigation.PushModalAsync(new FillBlank());
        //    }
        //} else {
        //    await DisplayAlert("Invalid Quiz ID", "Error has occured with starting the quiz, "OK");
        // }
        // TODO
    }

    // Delete the favorite quiz that got clicked
    private void RemoveFavorite_Clicked(object sender, EventArgs e) {
        // Delete the favorite quiz that got clicked
        //MauiProgram.BusinessLogic.DeleteFavoriteQuiz(1);
        Quiz selectedQuiz = CV.SelectedItem as Quiz;
        MauiProgram.BusinessLogic.DeleteFavoriteQuiz(selectedQuiz.Id);
    }
}