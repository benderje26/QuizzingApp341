using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzingApp341.Models;

[Serializable()]
public class MultipleChoiceQuestion : INotifyPropertyChanged
{
    string? problem;
    int? correntAnswer;
    int? givenAnswer;
    ArrayList options;

    public String Problem
    {
        get { return problem; }
        set
        {
            problem = value;
            OnPropertyChanged(nameof(Problem));
        }
    }

    public int CorrentAnswer
    {
        get { return correntAnswer.Value; }
        set
        {
            correntAnswer = value;
            OnPropertyChanged(nameof(CorrentAnswer));
        }
    }

    public int GivenAnswer
    {
        get { return givenAnswer.Value; }
        set
        {
            givenAnswer = value;
            OnPropertyChanged(nameof(GivenAnswer));
        }
    }

    public ArrayList Options
    {
        get { return options; }
        set
        {
            options = value;
            OnPropertyChanged(nameof(Options));
        }
    }

    public MultipleChoiceQuestion()
    {
        Problem = "";
        CorrentAnswer = -1;
        GivenAnswer = -1;
        Options = new ArrayList();
    }

    public MultipleChoiceQuestion(string problem, int correntAnswer, int givenAnswer, ArrayList options)
    {
        Problem = problem;
        CorrentAnswer = correntAnswer;
        GivenAnswer = givenAnswer;
        Options = options;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
