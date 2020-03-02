using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using VotingApp.DAL.Models;
using VotingApp.DAL.Repositories;

namespace VotingApp.Controllers
{
    [Route("Voting")]
    public class VotingController : Controller
    {
        private readonly IQuestionRepository questionRepository;
        private readonly IAnswerRepository answerRepository;

        public VotingController(IQuestionRepository questionRepository, IAnswerRepository answerRepository)
        {
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.FirstQuestionId = await questionRepository.GetFirstQuestionForQuestionnaire(id);
            return View();
        }

        private static QuestionViewModel ToViewModel(Question question, Answer answer, int? nextQuestionId)
        {
            return new QuestionViewModel()
            {
                Id = question.Id,
                Question = question.QuestionValue,
                PossibleAnswers = question.PossibleAnswers.Select(a => new AnswerViewModel()
                {
                    Answer = a.Answer,
                    Id = a.Id
                }).ToList(),
                IsAnswered = answer != null,
                SelectedAnswer = answer == null
                    ? null
                    : new AnswerViewModel()
                    {
                        Answer = answer.PossibleAnswer.Answer,
                        Id = answer.Id
                    },
                NextQuestionId = nextQuestionId
            };
        }

        [Route("question/{id:int}")]
        public async Task<IActionResult> Question(int id)
        {
            var question = await questionRepository.GetWithPossibleAnswers(id);
            
            var nextQuestionId = await questionRepository.GetNextQuestionId(question.QuestionnaireId,question.Id);
            var answer = await answerRepository.GetByQuestion(id);

            return View(ToViewModel(question, answer, nextQuestionId));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Vote(int id, [FromForm] int answerId)
        {
            var question = await questionRepository.GetWithPossibleAnswers(id);
            var selectedAnswer = question?.PossibleAnswers?.FirstOrDefault(a => a.Id == answerId);
            if (question == null || selectedAnswer == null)
                return NotFound();

            var answer = new Answer()
            {
                QuestionId = question.Id,
                PossibleAnswerId = selectedAnswer.Id
            };
            answerRepository.Add(answer);
            var nextQuestionId = await questionRepository.GetNextQuestionId(question.QuestionnaireId, question.Id);
            if (nextQuestionId == null)
                return RedirectToAction("Index", "Home");

            return RedirectToAction("Question", new {id = nextQuestionId.Value});
        }
    }

    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public IList<AnswerViewModel> PossibleAnswers { get; set; }

        public bool IsAnswered { get; set; }
        public AnswerViewModel SelectedAnswer { get; set; }
        public int? NextQuestionId { get; set; }
    }

    public class AnswerViewModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }
    }
}