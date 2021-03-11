using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Voting.Data.Data;
using Voting.Data.Models;
using Voting.WebApp.Models;

namespace Voting.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            this.context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var questionnaires = await context.Questionnaires.Include(q => q.Questions)
                .ToListAsync();

            var vm = new QuestionnairesViewModel()
            {
                Questionnaires = questionnaires.Select(q =>
                {
                    return new QuestionnaireViewModel()
                    {
                        Id = q.Id,
                        Title = q.Title,
                        FirstQuestionId = q.Questions.OrderBy(q => q.Id).First().Id
                    };
                }).ToList()
            };
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet("question/{id:int}")]
        public async Task<IActionResult> Question(int id)
        {
            var question = await context.Questions.Include(q => q.PossibleAnswers)
                .FirstOrDefaultAsync(q => q.Id == id);
            var userId = userManager.GetUserId(User);

            if (question == null)
            {
                return RedirectToAction("Index");
            }

            var answer = await context.Answers
                .FirstOrDefaultAsync(a => a.QuestionId == id && a.UserId == userId);

            var nextQuestionId = await NextQuestionId(question.QuestionnaireId, question.Id);

            var vm = new QuestionViewModel()
            {
                Question = question.QuestionValue,
                Id = question.Id,
                QuestionnaireId = question.QuestionnaireId,
                NextQuestionId = nextQuestionId,
                PossibleAnswers = question.PossibleAnswers.Select(pa => new PossibleAnswerViewModel()
                {
                    Id = pa.Id,
                    Answer = pa.Answer,
                    IsSelectedAnswer = answer?.PossibleAnswerId == pa.Id
                }).ToList(),
                IsAnswered = answer != null
            };
            return View(vm);
        }

        private async Task<int?> NextQuestionId(int questionnaireId, int currentQuestionId)
        {
            return (await context.Questionnaires.Include(q => q.Questions)
                    .FirstOrDefaultAsync(q => q.Id == questionnaireId))
                .Questions.FirstOrDefault(q => q.Id > currentQuestionId)?.Id;
        }
        

        [HttpPost("{questionnaireId}/question/{questionId:int}")]
        public async Task<IActionResult> Vote(int questionnaireId, int questionId, int answerId)
        {
            var userId = userManager.GetUserId(User);
            var answer = new Answer()
            {
                QuestionId = questionId,
                QuestionnaireId = questionnaireId,
                PossibleAnswerId = answerId,
                UserId = userId
            };
            await context.Answers.AddAsync(answer);
            await context.SaveChangesAsync();

            var nextQuestionId = await NextQuestionId(questionnaireId, questionId);
            return RedirectToAction("Question", new {id= nextQuestionId ?? -1});
        }
        
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }

    public class QuestionViewModel
    {
        public int Id { get; set; }
        public int QuestionnaireId { get; set; }
        public int? NextQuestionId { get; set; }
        public string Question { get; set; }
        public List<PossibleAnswerViewModel> PossibleAnswers { get; set; }
        public bool  IsAnswered { get; set; }
    }

    public class PossibleAnswerViewModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }

        public bool IsSelectedAnswer { get; set; }
    }
}