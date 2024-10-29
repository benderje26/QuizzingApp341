using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzingApp341.Models;

[Serializable()]
public class FillBlankQuestion : INotifyPropertyChanged
{
    string? problem;
    string? correntAnswer;
    string? givenAnswer;

    public String Problem
    {
        get { return problem; }
        set
        {
            problem = value;
            OnPropertyChanged(nameof(Problem));
        }
    }

    public string CorrentAnswer
    {
        get { return correntAnswer; }
        set
        {
            correntAnswer = value;
            OnPropertyChanged(nameof(CorrentAnswer));
        }
    }

    public string GivenAnswer
    {
        get { return givenAnswer; }
        set
        {
            givenAnswer = value;
            OnPropertyChanged(nameof(GivenAnswer));
        }
    }

    public FillBlankQuestion()
    {
        Problem = "";
        CorrentAnswer = "";
        GivenAnswer = "";
    }

    public FillBlankQuestion(string problem, string correntAnswer, string givenAnswer)
    {
        Problem = problem;
        CorrentAnswer = correntAnswer;
        GivenAnswer = givenAnswer;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
