using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace QuizzingApp341.Views {
    public partial class CreateNewQuiz : ContentPage {
        public ObservableCollection<Object> Questions { get; set; } = new ObservableCollection<Object>();
        public CreateNewQuiz() {
            InitializeComponent();

            BindingContext = this;
        }

        private async void OnQuestionTypeButtonClicked(object sender, EventArgs e) {
            var popup = new CreateNewQuizPopup();

            popup.QuestionTypeSelected += async (questionType) => {   // get questionType when clicked
                if (questionType == "MultipleChoice") {

                    await Task.Delay(100); // delay time
                    await Navigation.PushAsync(new CreateMultipleChoiceQuiz(null)); // Null here because you are creating a quiz

                } else if (questionType == "FillInBlank") {

                    await Task.Delay(100); // delay time
                    await Navigation.PushAsync(new CreateFillBlank(null)); // Passing in null here because you're creating a new question
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