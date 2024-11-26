namespace QuizzingApp341.Views;
using System;
#if ANDROID
using Android.Print;
#endif
using Microsoft.Maui.Controls;
using QuizzingApp341.Models;

public partial class CreateMultipleChoiceQuiz : ContentPage {
    static int questionNumber = 0;
    public CreateMultipleChoiceQuiz() {
        InitializeComponent();
    }

    private int getCorrectQuestion(string correctAnswer) {
        correctAnswer = correctAnswer.Substring(correctAnswer.Length - 1);
        switch(correctAnswer) {
            case "A":
                return 1;
            case "B":
                return 2;
            case "C":
                return 3;
            case "D":
                return 4;

        }
        return 0;
    }

    private void OnSaveClicked(object sender, EventArgs e) {
        // Retrieve data from user input
        //check for null, if null - replace with empty string
        string question = questionMultipleChoice.Text != null ? questionMultipleChoice.Text.Trim() : string.Empty;
        string optionA = optionAEntry.Text != null ? optionAEntry.Text.Trim() : string.Empty;
        string optionB = optionBEntry.Text != null ? optionBEntry.Text.Trim() : string.Empty;
        string optionC = optionCEntry.Text != null ? optionCEntry.Text.Trim() : string.Empty;
        string optionD = optionDEntry.Text != null ? optionDEntry.Text.Trim() : string.Empty;
        // string correctOption = correctAnswerFromUser.SelectedItem != null ? correctAnswerFromUser.SelectedItem.ToString().Trim() : string.Empty;
        // int correctAnswer = getCorrectQuestion(correctOption);
        // List<String> options = [optionA, optionB, optionC, optionD];

        // // Make a multiple choice question
        // MultipleChoiceQuestion thisQuestion = new MultipleChoiceQuestion(questionNumber++, true, question, options, correctAnswer);

        // Check for required fields not empty
        if (string.IsNullOrEmpty(question)) {
            DisplayAlert("Error", "Please enter a question.", "OK");
            return;
        }

        if (string.IsNullOrEmpty(optionA) || string.IsNullOrEmpty(optionB) ||
            string.IsNullOrEmpty(optionC) || string.IsNullOrEmpty(optionD)) {
            DisplayAlert("Error", "Please provide all options.", "OK");
            return;
        }

        // if (string.IsNullOrEmpty(correctOption)) {
        //     DisplayAlert("Error", "Please select the correct answer.", "OK");
        //     return;
        // }

        Console.Write("before saving question");
        // Send the question object using MessagingCenter
        // MessagingCenter.Send(this, "AddQuestion", thisQuestion);

        // Navigate back to the CreateNewQuiz page
        Navigation.PopAsync();  
    }


   
}
