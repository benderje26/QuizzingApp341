using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzingApp341.Models;

public class BusinessLogic : IBusinessLogic
{
    public ObservableCollection<Question> Questions
    {
        get { return GetAllQuestions(); }

    }

    public ObservableCollection<Question> GetAllQuestions()
    {
        ObservableCollection<Question> questions = new ObservableCollection<Question>();
        questions.Add(new Question("How many CS students does it take to screw in a lightbulb ?", "30", "", "1", "3", "10", "30", true));
        return questions;
    }
}
