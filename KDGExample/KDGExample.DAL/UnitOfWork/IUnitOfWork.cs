using System;
using KDGExample.DAL.Repositories.Abstraction;

namespace KDGExample.DAL.Repositories
{
    public interface IUnitOfWork: IDisposable
    {
        IQuestionRepository Questions { get; }
        IAnswerRepository Answers { get; }
        IQuestionnaireRepository Questionnaires { get; }
        void Commit();
    }
}