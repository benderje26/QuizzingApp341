namespace QuizzingApp341.Views;

public partial class TestQuiz : ContentPage
{
    public TestQuiz(string quizId)
    {
        InitializeComponent();
        LoadQuizData(quizId);
    }

    private void LoadQuizData(string quizId)
    {
        // Example default data
        if (quizId == "12345")
        {
            quizTitleLabel.Text = "Sample Quiz";
            quizDescriptionLabel.Text = "This is a sample quiz description.";
        }
    }
}