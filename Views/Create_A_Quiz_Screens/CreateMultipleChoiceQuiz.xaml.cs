namespace QuizzingApp341.Views;
using System;
using System.Runtime.InteropServices;
#if ANDROID
using Android.Print;
#endif
using Microsoft.Maui.Controls;
using QuizzingApp341.Models;

public partial class CreateMultipleChoiceQuiz : ContentPage {
    static int questionNumber = 0;
    public Question? MultipleChoiceQuestionToChange { get; set; } // This is for editing a current question only if there is one to edit
    public bool? QuestionPresent { get; set; } = false;
    public bool? NoQuestionPresent { get; set; } = false;
    public bool? AnswerPresent { get; set; } = false;
    public string? Answers { get; set; }
    public string? OptionA {get; set;}
    public string? OptionB {get; set;}
    public string? OptionC {get; set;}
    public string? OptionD {get; set;}

    public int? CorrectOption {get; set;}

    public string? QuestionText { get; set; }
    public CreateMultipleChoiceQuiz(Question? question) {
        MultipleChoiceQuestionToChange = question;
        Console.WriteLine(question.QuestionText);

        // If there is a question present to edit
        if (MultipleChoiceQuestionToChange != null) {
            try {
                NoQuestionPresent = false;
                QuestionPresent = true;
                QuestionText = question?.QuestionText;

                if (question?.acceptableAnswers != null) { // If there are any answers
                    Answers = string.Join(", ", question.acceptableAnswers);
                    AnswerPresent = true;
                }

                // Set all the options with the question's current answer options
                OptionA = question?.MultipleChoiceOptions?[0];
                OptionB = question?.MultipleChoiceOptions?[1];
                OptionC = question?.MultipleChoiceOptions?[2];
                OptionD = question?.MultipleChoiceOptions?[3];
                CorrectOption = int.Parse(question.MultipleChoiceAnswers[0]);
            } catch (Exception e) {
                Console.WriteLine("******************************");
                Console.WriteLine("Error: " + e.Message);
            }

        } else {
            NoQuestionPresent = true;
        }

        InitializeComponent();
        BindingContext = this;
    }

    private int getCorrectAnswerNumber(string correctAnswer) {
        correctAnswer = correctAnswer.Substring(correctAnswer.Length - 1);
        switch (correctAnswer) {
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
