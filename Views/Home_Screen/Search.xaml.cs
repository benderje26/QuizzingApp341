using System.Collections.ObjectModel;

namespace QuizzingApp341.Views;

public partial class Search : ContentPage {
    public ObservableCollection<QuizSearch> Quizzes { get; set; }

    public Search() {
        InitializeComponent();
        Quizzes = new ObservableCollection<QuizSearch>
        {
            new QuizSearch ( "Anatomy Quiz 1",  "Created by user1" ),
            new QuizSearch (  "Anatomy Quiz 2",  "Created by user2" )
        };

        BindingContext = this;
    }

    // Command for study button
    public Command<QuizSearch> StudyCommand => new Command<QuizSearch>(async (quiz) => {
        // Handle study button logic here
        await DisplayAlert("Study", $"Start studying {quiz.Title}?", "OK");
    });
}

// Quiz class
public class QuizSearch(string title, string creator) {
    public string? Title { get; set; } = title;
    public string? Creator { get; set; } = creator;
}
