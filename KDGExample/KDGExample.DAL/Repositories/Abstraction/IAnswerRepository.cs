using System.Threading.Tasks;
using KDGExample.DAL.Models;
using KDGExample.DAL.Repositories.Generic;

namespace KDGExample.DAL.Repositories.Abstraction
{
    public interface IAnswerRepository: IRepository<Answer>
    {
        Task<Answer> GetByQuestionId(int questionId);
        Task<int> CountByQuestionnaire(int questionnaireId);
    }
}