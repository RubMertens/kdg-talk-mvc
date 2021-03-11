using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voting.Data.Data;
using Voting.Data.Models;

namespace Voting.WebApp.ViewComponents
{
    public class UserStatsViewComponent: ViewComponent
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ApplicationDbContext context;

        public UserStatsViewComponent(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        private List<string> colorStack = new List<string>()
        {
            "red","green","blue","purple","gray"
        };

        private int LoopIndex(int currentIndex)
        {
            return currentIndex + 1 % (colorStack.Count - 1);
        }
        

        public async Task<IViewComponentResult> InvokeAsync(int questionId)
        {
            var q = await context.Questions
                .Include(q => q.PossibleAnswers)
                .FirstOrDefaultAsync(q => q.Id == questionId);
            var answers = await context.Answers
                .Where(a => a.QuestionId == questionId)
                .ToListAsync();

            var totalCount = answers.Count;

            var countPerAnswer = answers
                .GroupBy(a => a.PossibleAnswerId)
                .Select(g => new {g.Key, Count = g.Count()})
                ;
            var i = 0;
            var vm = new StatsModel()
            {
                AnsweredCounts = countPerAnswer
                    .Select(cpa =>  new AnsweredCount()
                    {
                        AnswerId = cpa.Key,
                        Answer = q.PossibleAnswers.First(pa => pa.Id == cpa.Key).Answer,
                        Percent =(int) ((decimal) cpa.Count / totalCount * 100),
                        BackgroundColor = colorStack[LoopIndex(i++)]
                    }).ToList()
            };

            return View(vm);
        }
    }

    public class StatsModel
    {
        public List<AnsweredCount> AnsweredCounts { get; set; }
    }

    public class AnsweredCount
    {
        public int AnswerId { get; set; }
        public int Percent { get; set; }
        public string Answer { get; set; }
        public string BackgroundColor { get; set; }
    }
}