using System.Threading.Tasks;
using KDGExample.DAL.Models;
using KDGExample.DAL.Repositories.Generic;

namespace KDGExample.DAL.Repositories.Abstraction
{
    public interface IQuestionnaireRepository: IRepository<Questionnaire>
    {
        Task<int> TotalNumberOfQuestions(int id);
    }
}