using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voting.Data.Data;

namespace Voting.WebApp.ViewComponents
{
    public class ProgressViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public ProgressViewComponent(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(int questionnaireId)
        {
            /*        
            var totalQuestions = await _dbContext.Questions.CountAsync(q => q.QuestionnaireId == questionnaireId);
            var totalAnswers = await _dbContext.Questions.CountAsync(q => q.QuestionnaireId == questionnaireId && q.Answers.Any());
            var percent = (int)(totalAnswers / (decimal)totalQuestions*100);
            return View(new ProgressViewModel(percent)); 
            */

            string userId = userManager.GetUserId(UserClaimsPrincipal);

            int answers = await context.Answers
                .Where(a => a.QuestionnaireId == questionnaireId
                            && a.UserId == userId
                ).CountAsync();

            int totalQuestions = await context.Questions
                .Where(q => q.QuestionnaireId == questionnaireId)
                .CountAsync();

            ProgressViewModel vm = new ProgressViewModel
            {
                Id = questionnaireId,
                PercentComplete = (int)((decimal)answers / totalQuestions * 100)
            };

            return View(vm);
        }
    }

    public class ProgressViewModel
    {
        public int Id { get; set; }
        public int PercentComplete { get; set; }
    }
}