namespace QuizzingApp341.Views;
using QuizzingApp341.Models;
using System.Collections.ObjectModel;
public partial class EditQuiz {
    public Quiz Quiz {get; set;}

    public EditQuiz(Quiz quiz) {
        Quiz = quiz;
        BindingContext = this;
        InitializeComponent();
    }


}