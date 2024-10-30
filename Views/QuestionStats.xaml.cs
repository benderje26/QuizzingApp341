namespace QuizzingApp341.Views;
using Microsoft.Maui.Controls.PlatformConfiguration;
using QuizzingApp341.Models;
using System.Collections.Generic;

/*
 * Name: Peter Skogman
 */
public partial class QuestionStats : ContentPage {
    //private int currentQuestionIndex = 0;
    //private Quiz currentQuiz;


    public QuestionStats(Quiz quiz) {
        InitializeComponent();
       // currentQuiz = quiz;
    }

    //private async void OnNextQuestionButtonClicked(object sender, EventArgs e) {
    //    currentQuestionIndex++;
    //    // Check if there are more questions in the quiz
    //    if (currentQuestionIndex < currentQuiz.Questions.Count) {
    //        string question = currentQuiz.Questions[currentQuestionIndex];


    //        bool isMultipleChoice = CheckIfMultipleChoice(question);

    //        if (isMultipleChoice) {
    //            // Navigate to the multiple choice screen
    //            await Navigation.PushAsync(new MultipleChoice());
    //        } else {
    //            // Navigate to the fill-in-the-blank screen
    //            await Navigation.PushAsync(new FillBlank());
    //        }


    //    } else {
    //        // No more questions, display statistics page
    //        await Navigation.PushAsync(new Statistics());
    //    }
    }

//    //private bool CheckIfMultipleChoice(string question) {
//    //    return true;
//    //}
//}
