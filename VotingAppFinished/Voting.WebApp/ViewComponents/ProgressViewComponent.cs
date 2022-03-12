using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voting.Data.Data;

namespace Voting.WebApp.ViewComponents
{
    public class ProgressViewComponent: ViewComponent
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
            var userId = userManager.GetUserId(UserClaimsPrincipal);

            var answers = await context.Answers
                .Where(a => a.QuestionnaireId == questionnaireId
                            && a.UserId == userId
                ).CountAsync();

            var totalQuestions = await context.Questions
                .Where(q => q.QuestionnaireId == questionnaireId)
                .CountAsync();

            var vm = new ProgressViewModel
            {
                Id = questionnaireId,
                PercentComplete = (int) ((decimal) answers / totalQuestions *100)
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