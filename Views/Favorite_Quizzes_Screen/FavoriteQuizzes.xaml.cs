namespace QuizzingApp341.Views;

public partial class FavoriteQuizzes : ContentPage {
    public FavoriteQuizzes() {
        InitializeComponent();

        BindingContext = MauiProgram.BusinessLogic;
    }


    // Event handler for button click
    private void RemoveFavorite_Clicked(object sender, EventArgs e) {
        // pull out the quize that got clicked
    }
    private void StudyQuiz_Clicked(object sender, EventArgs e) {
        // pull out the quize that got clicked
    }
}