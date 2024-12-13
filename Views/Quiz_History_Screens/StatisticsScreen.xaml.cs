
/* 
Description: This instantiates the statistics screen and draws the box plot
Name: Pachia
*/
namespace QuizzingApp341.Views;

using CommunityToolkit.Maui.Storage;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Maui.Graphics;
using QuizzingApp341.Models;
using System;
using System.Collections.ObjectModel;

public partial class StatisticsScreen : ContentPage {

    public Statistics QuizStatistics { get; set; }
    public Dictionary<string, int> UserStats { get; set; }
    public Boxplot QuizStatsBoxplot { get; set; }
    public int TotalQuestions { get; set; }

    public Quiz Quiz { get; set; }

    public DateTime? Date { get; set; }

    public bool IsStudying { get; set; }

    public bool ShowBoxPlot { get; set; }

    public double UserScore { get; set; }

    public ObservableCollection<Question> Questions { get; set; }

    public List<Answer>? Answers { get; set; } = [];

    public bool ShowQuizResponse { get; set; }

    public StatisticsScreen(Dictionary<string, int> userStats, int totalQuestions, Quiz quiz, List<Response> responses, ObservableCollection<Question> questions, DateTime? date) {
        InitializeComponent();
        Quiz = quiz;
        Date = date;
        Questions = questions;
        UserStats = userStats;
        TotalQuestions = totalQuestions;
        IsStudying = userStats.Count() == 1;
        List<Response> userResponse = responses.Where(r => r.UserId == MauiProgram.BusinessLogic.UserInfo?.Id).ToList();
        ShowQuizResponse = !userResponse.IsNullOrEmpty();
        ShowBoxPlot = !IsStudying;
        if (IsStudying && userStats.Count != 0) {
            UserScore = userStats[userStats.Keys.ToList()[0]];
        }
        EditResponses(userResponse);
        QuizStatistics = new Statistics(userStats.Values.ToList());
        QuizStatsBoxplot = new Boxplot(QuizStatistics, totalQuestions);
        BindingContext = this;
    }

    private void EditResponses(List<Response> responses) {
        for (int i = 0; i < Questions.Count && i < responses.Count; i++) {
            Answer answer = new Answer(responses[i], Questions[i]);
            Answers.Add(answer);
        }
    }

    // Event handler for button click
    private void ViewAllParticipantsClicked(object sender, EventArgs e) {
        // Get the participants and show the participants screen
        var fileSaver = FileSaver.Default; // Initialize the IFileSaver
        Navigation.PushAsync(new QuizParticipants(UserStats, fileSaver));
    }
}

// This is a canvas that draws the box plot
public class Boxplot(Statistics stats, int totalQuestions) : IDrawable {
    public void Draw(ICanvas canvas, RectF dirtyRect) {
        //Convert the raw scores to percents to be shown in the box plot
        float min = ((float)stats.Minimum / totalQuestions) * 100;
        float max = ((float)stats.Maximum / totalQuestions) * 100;
        float median = ((float)stats.Median / totalQuestions) * 100;
        float lowerQuartile = ((float)stats.LowerQuartile / totalQuestions) * 100;
        float upperQuartile = ((float)stats.UpperQuartile / totalQuestions) * 100;

        canvas.StrokeColor = Colors.DarkBlue;
        canvas.StrokeSize = 2;
        // Draw outline of canvas
        canvas.DrawRoundedRectangle(0, 0, dirtyRect.Width, dirtyRect.Height, 10);
        canvas.StrokeColor = Colors.DarkBlue;
        // Middle line
        canvas.DrawLine((float)(min / 100.0 * dirtyRect.Width), dirtyRect.Height / 2,
            (float)(max / 100.0 * dirtyRect.Width), dirtyRect.Height / 2);
        // draw Min
        canvas.DrawLine((float)(min / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 10, (float)(min / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 + 10);
        // draw Lower Quartile and upper quartile
        canvas.FillColor = Color.FromRgba("#A5ACE1FF");
        canvas.FillRoundedRectangle((float)(lowerQuartile / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 25, (float)((upperQuartile - lowerQuartile) / 100.0 * dirtyRect.Width), 50, 10);
        canvas.DrawRoundedRectangle((float)(lowerQuartile / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 25, (float)((upperQuartile - lowerQuartile) / 100.0 * dirtyRect.Width), 50, 10);
        // draw Median line
        canvas.DrawLine((float)(median / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 25, (float)(median / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 + 25);
        // draw Max line
        canvas.DrawLine((float)(max / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 10, (float)(max / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 + 10);
    }
}

public class Answer(Response response, Question question) {
    public string Response {
        get {
            if (Question.QuestionType == QuestionType.MultipleChoice) {
                return GetSelectedAnswersAsString(response.MultipleChoiceResponse ?? [], Question.MultipleChoiceOptions ?? []);
            } else {
                return response.FillBlankResponse ?? string.Empty;
            }
        }
    }
    public Question Question { get; set; } = question;

    public string CorrectAnswer {
        get {
            if (Question.QuestionType == QuestionType.MultipleChoice) {
                return GetSelectedAnswersAsString(Question.MultipleChoiceCorrectAnswers ?? [], Question.MultipleChoiceOptions ?? []);
            } else {
                return string.Join(", ", Question.AcceptableAnswers ?? []);
            }
        }
    }

    private static string GetSelectedAnswersAsString(int[] selectedIndices, string[] options) {
        List<string> correctOptions = [];
        foreach (int testtesttest in selectedIndices) {
            if (testtesttest < options.Length && testtesttest >= 0) {
                correctOptions.Add(options[testtesttest]);
            }
        }
        return string.Join(", ", correctOptions);
    }
}

