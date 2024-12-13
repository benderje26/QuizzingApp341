
/* 
Description: This instantiates the statistics screen and draws the box plot
Name: Pachia
*/
namespace QuizzingApp341.Views;

using CommunityToolkit.Maui.Storage;
using Microsoft.Maui.Graphics;
using System;

public partial class StatisticsScreen : ContentPage {

    public Statistics QuizStatistics { get; set; }
    public Dictionary<string, int> UserStats { get; set; }
    public Boxplot QuizStatsBoxplot { get; set; }
    public StatisticsScreen(Dictionary<string, int> userStats) {
        InitializeComponent();
        UserStats = userStats;
        QuizStatistics = new Statistics(userStats.Values.ToList());
        QuizStatsBoxplot = new Boxplot(QuizStatistics);
        BindingContext = this;
    }

    // Event handler for button click
    private void ViewAllParticipantsClicked(object sender, EventArgs e) {
        // Get the participants and show the participants screen
        var fileSaver = FileSaver.Default; // Initialize the IFileSaver
        Navigation.PushAsync(new QuizParticipants(UserStats, fileSaver));
    }
}

// This is a canvas that draws the box plot
public class Boxplot(Statistics stats) : IDrawable {
    public void Draw(ICanvas canvas, RectF dirtyRect) {
        // Set up the canvas stroke color and size
        canvas.StrokeColor = Colors.DarkBlue;
        canvas.StrokeSize = 2;

        // Draw the outline of the canvas
        canvas.DrawRoundedRectangle(0, 0, dirtyRect.Width, dirtyRect.Height, 10);

        // Calculate the data range (max - min) to determine the scale factor
        float dataRange = (float)(stats.Maximum - stats.Minimum);
        float scaleFactor = dataRange > 0 ? dirtyRect.Width / dataRange : 1;  // Ensure we don't divide by zero

        // Add a small margin to the min and max values to prevent them from being at the very edges
        float margin = 0.02f; // 2% margin on both sides of the data range

        // Normalize the data (scale the values based on the canvas width)
        float minScaled = (float)(stats.Minimum - stats.Minimum) * scaleFactor + margin * dirtyRect.Width;
        float maxScaled = (float)(stats.Maximum - stats.Minimum) * scaleFactor - margin * dirtyRect.Width;
        float lowerQuartileScaled = (float)(stats.LowerQuartile - stats.Minimum) * scaleFactor;
        float upperQuartileScaled = (float)(stats.UpperQuartile - stats.Minimum) * scaleFactor;
        float medianScaled = (float)(stats.Median - stats.Minimum) * scaleFactor;

        // Ensure that quartiles are clamped within the min and max bounds
        lowerQuartileScaled = Math.Max(lowerQuartileScaled, minScaled);
        upperQuartileScaled = Math.Min(upperQuartileScaled, maxScaled);

        // Middle line (line connecting min and max)
        canvas.DrawLine(minScaled, dirtyRect.Height / 2,
            maxScaled, dirtyRect.Height / 2);

        // Draw Min line with slight inward adjustment
        canvas.DrawLine(minScaled, dirtyRect.Height / 2 - 10, minScaled, dirtyRect.Height / 2 + 10);

        // Draw the Lower Quartile and Upper Quartile
        canvas.FillColor = Color.FromRgba("#A5ACE1FF");
        canvas.FillRoundedRectangle(lowerQuartileScaled, dirtyRect.Height / 2 - 25, upperQuartileScaled - lowerQuartileScaled, 50, 10);
        canvas.DrawRoundedRectangle(lowerQuartileScaled, dirtyRect.Height / 2 - 25, upperQuartileScaled - lowerQuartileScaled, 50, 10);

        // Draw Median line
        canvas.DrawLine(medianScaled, dirtyRect.Height / 2 - 25, medianScaled, dirtyRect.Height / 2 + 25);

        // Draw Max line with slight inward adjustment
        canvas.DrawLine(maxScaled, dirtyRect.Height / 2 - 10, maxScaled, dirtyRect.Height / 2 + 10);
    }
}

