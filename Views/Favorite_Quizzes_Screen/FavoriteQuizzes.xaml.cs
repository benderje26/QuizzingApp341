using QuizzingApp341.Models;

namespace QuizzingApp341.Views;

public partial class FavoriteQuizzes : ContentPage {
    public FavoriteQuizzes() {
        InitializeComponent();

        BindingContext = MauiProgram.BusinessLogic;
    }

    // Study the favorite quiz that got clicked
    private async void StudyQuiz_Clicked(object sender, EventArgs e) {
        Quiz selectedQuiz = CV.SelectedItem as Quiz;

        // Navigate to the study page here
    }

    // Delete the favorite quiz that got clicked
    private void RemoveFavorite_Clicked(object sender, EventArgs e) {
        // Delete the favorite quiz that got clicked
        //MauiProgram.BusinessLogic.DeleteFavoriteQuiz(1);
        Quiz selectedQuiz = CV.SelectedItem as Quiz;
        MauiProgram.BusinessLogic.DeleteFavoriteQuiz(selectedQuiz.Id);
    }
}