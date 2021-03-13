using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Voting.Data;
using Voting.Data.Data;
using Voting.Data.Models;
using Voting.Data.Repositories;
using Voting.WebApp.Models;

namespace Voting.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IAnswerRepository answerRepository;
        private readonly IQuestionnaireRepository questionnaireRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IUnitOfWork unitOfWork;

        public HomeController(ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            IAnswerRepository answerRepository,
            IQuestionnaireRepository questionnaireRepository,
            IQuestionRepository questionRepository,
            ICommentRepository commentRepository,
            IUnitOfWork unitOfWork
        )
        {
            _logger = logger;
            this.userManager = userManager;
            this.answerRepository = answerRepository;
            this.questionnaireRepository = questionnaireRepository;
            this.questionRepository = questionRepository;
            this.commentRepository = commentRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            // var questionnaires = await context.Questionnaires.Include(q => q.Questions)
            //     .ToListAsync();
            var questionnaires = await questionnaireRepository.All();

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
            // var question = await context.Questions.Include(q => q.PossibleAnswers)
            //     .FirstOrDefaultAsync(q => q.Id == id);
            var question = await questionRepository.ById(id);
            var userId = userManager.GetUserId(User);

            if (question == null)
            {
                return RedirectToAction("Index");
            }

            // var answer = await context.Answers
            //     .Include(a => a.Comment)
            //     .FirstOrDefaultAsync(a => a.QuestionId == id && a.UserId == userId);
            var answer = await answerRepository.ByQuestionAndUserId(id, userId);
            // var nextQuestionId = await NextQuestionId(question.QuestionnaireId, question.Id);
            var nextQuestionId = await questionnaireRepository.NextQuestionId(question.QuestionnaireId, id);
            
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
                IsAnswered = answer != null,
                Comment = answer?.Comment?.CommentValue
                
            };
            return View(vm);
        }

        [HttpPost("{questionnaireId}/question/{questionId:int}")]
        public async Task<IActionResult> Vote(int questionnaireId, int questionId, int answerId, string comment)
        {
            var userId = userManager.GetUserId(User);

            Comment c = null;
            if (!string.IsNullOrWhiteSpace(comment))
            {
                c = new Comment()
                {
                    CommentValue = comment,
                    UserId = userId
                };
                await commentRepository.AddAsync(c);
            }
            
            var answer = new Answer()
            {
                QuestionId = questionId,
                QuestionnaireId = questionnaireId,
                PossibleAnswerId = answerId,
                UserId = userId,
                CommentId = c?.Id
            };
            await answerRepository.Add(answer);
            await unitOfWork.CommitAsync();  

            // var nextQuestionId = await NextQuestionId(questionnaireId, questionId);
            var nextQuestionId = await questionnaireRepository.NextQuestionId(questionnaireId, questionId);
            return RedirectToAction("Question", new {id= nextQuestionId ?? -1});
        }
        
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        [HttpGet]
        public async Task<IActionResult> AllComments()
        {
            var comments = await commentRepository.All();
            var vm = new List<CommentViewModel>();

            foreach (var comment in comments)
            {
                var user = await userManager.FindByIdAsync(comment.UserId);
                vm.Add(new CommentViewModel()
                {
                    By = user.UserName,
                    Comment = comment.CommentValue
                });
            }
            return View(vm);
        }
    }
    
    public class CommentViewModel
    {
        public string By { get; set; }
        public string Comment { get; set; }
    }

    public class QuestionViewModel
    {
        public int Id { get; set; }
        public int QuestionnaireId { get; set; }
        public int? NextQuestionId { get; set; }
        public string Question { get; set; }
        public List<PossibleAnswerViewModel> PossibleAnswers { get; set; }
        public bool  IsAnswered { get; set; }
        public string Comment { get; set; }
    }

    public class PossibleAnswerViewModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }

        public bool IsSelectedAnswer { get; set; }
    }
}