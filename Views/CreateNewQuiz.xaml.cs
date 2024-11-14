using CommunityToolkit.Maui.Views;

namespace QuizzingApp341.Views {
    public partial class CreateNewQuiz : ContentPage {
        public CreateNewQuiz() {
            InitializeComponent();
        }

        private async void OnQuestionTypeButtonClicked(object sender, EventArgs e) {
            var popup = new CreateNewQuizPopup();

            popup.QuestionTypeSelected += async (questionType) => {   // get questionType when clicked
                if (questionType == "MultipleChoice") {

                    await Task.Delay(100); // delay time
                    await Navigation.PushAsync(new CreateMultipleChoiceQuiz());

                } else if (questionType == "FillInBlank") {

                    await Task.Delay(100); // delay time
                    await Navigation.PushAsync(new CreateFillBlank());
                }
            };

            // Show the popup
            await this.ShowPopupAsync(popup);
        }

        private async void OnSaveNewQuizClicked(object sender, EventArgs e) {
            var newPage = new MyQuiz();

            //completely reset the navigation stack
            await Navigation.PopToRootAsync();
            await Navigation.PushAsync(new NavigationPage(newPage));
        }
    }
}