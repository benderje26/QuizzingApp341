using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizzingApp341.Models;

public interface IBusinessLogic
{
    ObservableCollection<Question> GetAllQuestions();
}
