using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzingApp341.Models;

[Serializable()]
public class Question : INotifyPropertyChanged
{
    String problem;
    String correntAnswer;
    String givenAnswer;
    String optionOne;
    String optionTwo;
    String optionThree;
    String optionFour;
    bool isMultipleChoice;

    public String Problem
    {
        get { return problem; }
        set
        {
            problem = value;
            OnPropertyChanged(nameof(Problem));
        }
    }

    public String CorrentAnswer
    {
        get { return correntAnswer; }
        set
        {
            correntAnswer = value;
            OnPropertyChanged(nameof(CorrentAnswer));
        }
    }

    public String GivenAnswer
    {
        get { return givenAnswer; }
        set
        {
            givenAnswer = value;
            OnPropertyChanged(nameof(GivenAnswer));
        }
    }
    public String OptionOne
    {
        get { return optionOne; }
        set
        {
            optionOne = value;
            OnPropertyChanged(nameof(OptionOne));
        }
    }

    public String OptionTwo
    {
        get { return optionTwo; }
        set
        {
            optionTwo = value;
            OnPropertyChanged(nameof(OptionTwo));
        }
    }

    public String OptionThree
    {
        get { return optionThree; }
        set
        {
            optionThree = value;
            OnPropertyChanged(nameof(OptionThree));
        }
    }

    public String OptionFour
    {
        get { return optionFour; }
        set
        {
            optionFour = value;
            OnPropertyChanged(nameof(OptionFour));
        }
    }

    public bool IsMultipleChoice
    {
        get { return isMultipleChoice; }
        set
        {
            isMultipleChoice = value;
            OnPropertyChanged(nameof(IsMultipleChoice));
        }
    }

    public Question()
    {
        Problem = "";
        CorrentAnswer = "";
        GivenAnswer = "";
        OptionOne = "";
        OptionTwo = "";
        OptionThree = "";
        OptionFour = "";
        IsMultipleChoice = false;
    }

    public Question(String problem, String correntAnswer, String givenAnswer, String optionOne, String optionTwo, String optionThree, String optionFour, bool isMultipleChoice)
    {
        Problem = problem;
        CorrentAnswer = correntAnswer;
        GivenAnswer = givenAnswer;
        OptionOne = optionOne;
        OptionTwo = optionTwo;
        OptionThree = optionThree;
        OptionFour = optionFour;
        IsMultipleChoice = isMultipleChoice;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
