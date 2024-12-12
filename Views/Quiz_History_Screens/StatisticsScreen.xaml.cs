
/* 
Description: This instantiates the statistics screen and draws the box plot
Name: Pachia
*/
namespace QuizzingApp341.Views;
using Microsoft.Maui.Graphics;
using System;

public partial class StatisticsScreen : ContentPage {
    public StatisticsScreen(long activeQuizId) {
        InitializeComponent();
    }

    // Event handler for button click
    private void ViewAllParticipantsClicked(object sender, EventArgs e) {
        // Get the participants and show the participants screen
        Navigation.PushAsync(new QuizParticipants());
    }
}

// This is a canvas that draws the box plot
public class Boxplot() : IDrawable {
//public class Boxplot(Statistics stats) : IDrawable {

    //The above line that passed in a Statistics is commented out for the time being because otherwise an error is thrown in the StatisticsScreen.xaml since
    //Boxplot is called but doesn't pass in the parameter. Since I commented out the parameter I had to add the below line declaring stats so no errors get
    //throw in the Draw method, so also delete that line once the parameter issue is fixed.
    Statistics stats = new Statistics();
    public void Draw(ICanvas canvas, RectF dirtyRect) {
        canvas.StrokeColor = Colors.DarkBlue;
        canvas.StrokeSize = 2;


        // Draw outline of canvas
        canvas.DrawRoundedRectangle(0, 0, dirtyRect.Width, dirtyRect.Height, 10);
        canvas.StrokeColor = Colors.DarkBlue;

        // Middle line
        canvas.DrawLine((float)(stats.GetMin() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2,
            (float)(stats.GetMax() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2);

        // draw Min
        canvas.DrawLine((float)(stats.GetMin() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 10, (float)(stats.GetMin() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 + 10);


        // draw Lower Quartile and upper quartile
        canvas.FillColor = Color.FromRgba("#A5ACE1FF");
        canvas.FillRoundedRectangle((float)(stats.GetLowerQuartile() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 25, (float)((stats.GetUpperQuartile() - stats.GetLowerQuartile()) / 100.0 * dirtyRect.Width), 50, 10);
        canvas.DrawRoundedRectangle((float)(stats.GetLowerQuartile() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 25, (float)((stats.GetUpperQuartile() - stats.GetLowerQuartile()) / 100.0 * dirtyRect.Width), 50, 10);

        // draw Median line
        canvas.DrawLine((float)(stats.GetMedian() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 25, (float)(stats.GetMedian() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 + 25);

        // draw Max line
        canvas.DrawLine((float)(stats.GetMax() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 - 10, (float)(stats.GetMax() / 100.0 * dirtyRect.Width), dirtyRect.Height / 2 + 10);
    }
}

