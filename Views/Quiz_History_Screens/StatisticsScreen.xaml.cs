
/* 
Description: This instantiates the statistics screen and draws the box plot
Name: Pachia
*/
namespace QuizzingApp341.Views;
using Microsoft.Maui.Graphics;
using System;

public partial class StatisticsScreen : ContentPage {
    public StatisticsScreen() {
        InitializeComponent();
    }

    // Event handler for button click
    private void ViewAllParticipantsClicked(object sender, EventArgs e) {
        // Get the participants and show the participants screen
        Navigation.PushAsync(new QuizParticipants());
    }
}

// This is a canvas that draws the box plot
public class Canvas : IDrawable {
    Statistics scoreStats = new Statistics();
    public void Draw(ICanvas canvas, RectF dirtyRect) {
        canvas.StrokeColor = Colors.DarkBlue;
        canvas.StrokeSize = 2;


        // Draw outline of canvas
        canvas.DrawRoundedRectangle(0, 0, dirtyRect.Width, dirtyRect.Height, 10);
        canvas.StrokeColor = Colors.DarkBlue;

        // Middle line
        canvas.DrawLine((float)(scoreStats.getMin() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2,
            (float)(scoreStats.getMax() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2);

        // draw Min
        canvas.DrawLine((float)(scoreStats.getMin() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 10, (float)(scoreStats.getMin() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 + 10);


        // draw Lower Quartile and upper quartile
        canvas.FillColor = Color.FromRgba("#A5ACE1FF");
        canvas.FillRoundedRectangle((float)(scoreStats.getLowerQuartile() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 25, (float)((scoreStats.getUpperQuartile() - scoreStats.getLowerQuartile()) / 100.0 * dirtyRect.Width), 50, 10);
        canvas.DrawRoundedRectangle((float)(scoreStats.getLowerQuartile() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 25, (float)((scoreStats.getUpperQuartile() - scoreStats.getLowerQuartile()) / 100.0 * dirtyRect.Width), 50, 10);

        // draw Median line
        canvas.DrawLine((float)(scoreStats.getMedian() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 25, (float)(scoreStats.getMedian() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 + 25);

        // draw Max line
        canvas.DrawLine((float)(scoreStats.getMax() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 10, (float)(scoreStats.getMax() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 + 10);
    }
}

