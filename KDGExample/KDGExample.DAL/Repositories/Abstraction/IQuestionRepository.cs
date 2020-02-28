using System.Threading.Tasks;
using KDGExample.DAL.Models;
using KDGExample.DAL.Repositories.Generic;

namespace KDGExample.DAL.Repositories.Abstraction
{
    public interface IQuestionRepository: IRepository<Question>
    {
        Task<Question> FirstQuestionOfQuestionnaire(int questionnaireId);
        Task<Question> GetWithPossibleAnswers(int id);
        Task<int?> GetNextQuestionId(int previousQuestionId, int questionnaireId);
        Task<int> CountByQuestionnaire(int questionnaireId);
    }
}