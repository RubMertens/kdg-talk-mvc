using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Voting.Data.Data;

namespace Voting.WebApp.ViewComponents;

public class ProgressViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ProgressViewComponent(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        this._context = context;
        this._userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(int questionnaireId)
    {
        var userId = _userManager.GetUserId(UserClaimsPrincipal);

        var answers = await _context.Answers
            .Where(a => a.Question.QuestionnaireId == questionnaireId
                        && a.UserId == userId
            ).CountAsync();

        var totalQuestions = await _context.Questions
            .Where(q => q.QuestionnaireId == questionnaireId)
            .CountAsync();

        var percent = (int)((decimal)answers / totalQuestions * 100);
        var vm = new ProgressViewModel(questionnaireId, percent);

        return View(vm);
    }
}

public record ProgressViewModel(int Id, int PercentComplete);