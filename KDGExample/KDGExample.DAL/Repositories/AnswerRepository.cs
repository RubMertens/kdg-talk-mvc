using System.Threading.Tasks;
using KDGExample.DAL.Context;
using KDGExample.DAL.Models;
using KDGExample.DAL.Repositories.Abstraction;
using KDGExample.DAL.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace KDGExample.DAL.Repositories
{
    public class AnswerRepository: Repository<Answer>,  IAnswerRepository
    {
        public AnswerRepository(DbContext context) : base(context)
        {
        }

        public Task<Answer> GetByQuestionId(int questionId)
        {
            return
                VotingContext
                    .Answers
                    .FirstOrDefaultAsync(a => a.QuestionId == questionId);
        }
        
        private VotingContext VotingContext => Context as VotingContext;
    }
}