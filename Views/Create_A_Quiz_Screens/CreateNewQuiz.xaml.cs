using CommunityToolkit.Maui.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;

namespace QuizzingApp341.Views {
    public partial class CreateNewQuiz : ContentPage {
        public ObservableCollection<Question> Questions { get; set; } = [];
        public CreateNewQuiz() {
            InitializeComponent();

            BindingContext = this;
        }

        private async void OnQuestionTypeButtonClicked(object sender, EventArgs e) {
            var popup = new CreateNewQuestionPopup();

            popup.QuestionTypeSelected += async (questionType) => {   // get questionType when clicked
                if (questionType == QuestionType.MultipleChoice) {
                    await Task.Delay(100); // delay time
                    await Navigation.PushAsync(new CreateMultipleChoice(new Question(), true)); // Null here because you are creating a quiz
                } else if (questionType == QuestionType.FillBlank) {
                    await Task.Delay(100); // delay time
                    await Navigation.PushAsync(new CreateFillBlank(new Question(), true)); // Passing in null here because you're creating a new question
                }
            };

            // Show the popup
            await this.ShowPopupAsync(popup);
        }

        private async void OnSaveNewQuizClicked(object sender, EventArgs e) {
            var newPage = new QuizStudio();

            //completely reset the navigation stack
            await Navigation.PopToRootAsync();
            await Navigation.PushAsync(new NavigationPage(newPage));
        }
    }
}