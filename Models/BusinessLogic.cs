using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzingApp341.Models;

public class BusinessLogic : IBusinessLogic
{
    public ObservableCollection<MultipleChoiceQuestion> MultipleChoiceQuestions
    {
        get { return GetAllMultipleChoiceQuestions(); }

    }

    public ObservableCollection<FillBlankQuestion> FillBlankQuestions
    {
        get { return GetAllFillBlankQuestions(); }

    }

    public ObservableCollection<MultipleChoiceQuestion> GetAllMultipleChoiceQuestions()
    {
        ObservableCollection<MultipleChoiceQuestion> questions = new ObservableCollection<MultipleChoiceQuestion>();
        ArrayList options = new ArrayList();
        options.Add("1");
        options.Add("3");
        options.Add("10");
        options.Add("30");
        questions.Add(new MultipleChoiceQuestion("How many CS students does it take to screw in a lightbulb ?", 3, -1, options));
        return questions;
    }

    public ObservableCollection<FillBlankQuestion> GetAllFillBlankQuestions()
    {
        ObservableCollection<FillBlankQuestion> questions = new ObservableCollection<FillBlankQuestion>();
        questions.Add(new FillBlankQuestion("How many CS students does it take to screw in a lightbulb ?", "30", ""));
        return questions;
    }
}
