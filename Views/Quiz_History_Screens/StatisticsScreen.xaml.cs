
/* 
Description: This instantiates the statistics screen and draws the box plot
Name: Pachia
*/
namespace QuizzingApp341.Views;
using Microsoft.Maui.Graphics;
using System;

public partial class StatisticsScreen : ContentPage {

    public Statistics QuizStatistics { get; set; }
    public Dictionary<string, int> UserStats { get; set; }
    public Boxplot QuizStatsBoxplot { get; set; }
    public int TotalQuestions { get; set; }
    public StatisticsScreen(Dictionary<string, int> userStats, int totalQuestions) {
        InitializeComponent();
        UserStats = userStats;
        TotalQuestions = totalQuestions;
        QuizStatistics = new Statistics(userStats.Values.ToList());
        QuizStatsBoxplot = new Boxplot(QuizStatistics, totalQuestions);
        BindingContext = this;
    }

    // Event handler for button click
    private void ViewAllParticipantsClicked(object sender, EventArgs e) {
        // Get the participants and show the participants screen
        Navigation.PushAsync(new QuizParticipants(UserStats));
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

