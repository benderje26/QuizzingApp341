namespace QuizzingApp341.Views {
    public class MultipleChoiceQuestion {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public int CorrectAnswer { get; set; }
        public int QuestionNumber { get; set; }

        public MultipleChoiceQuestion(int questionNumber, bool isActive, string text, List<string> options, int correctAnswer) {
            QuestionNumber = questionNumber;
            Text = text;
            Options = options;
            CorrectAnswer = correctAnswer;
        }
    }

}