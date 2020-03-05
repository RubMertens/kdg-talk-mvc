using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VotingApp.DAL.Models;
using VotingApp.DAL.Repositories;
using VotingApp.DAL.UnitOfWOrk;

namespace VotingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IQuestionRepository questionRepository;
        private readonly IAnswerRepository answerRepository;
        private readonly IProgressRepository progressRepository;

        public HomeController(IUnitOfWork unitOfWork
        )
        {
            this.unitOfWork = unitOfWork;
            this.questionRepository = unitOfWork.QuestionRepository;
            this.answerRepository = unitOfWork.AnswerRepository;
            this.progressRepository = unitOfWork.ProgressRepository;
        }

        public async Task<IActionResult> Index()
        {
            var firstQuestion = await questionRepository.First();
            ViewBag.FirstQuestionId = firstQuestion.Id;
            return View();
        }

        [Route("question/{id:int}")]
        public async Task<IActionResult> Question(int id)
        {
            var question = await questionRepository.WithPossibleAnswsers(id);
            if (question == null)
            {
                return RedirectToAction("Index");
            }

            var answer = await answerRepository.ForQuestion(id);
            
            var progress = await progressRepository.Get();
            
            var vm = new QuestionModel()
            {
                Id = question.Id,
                Question = question.QuestionValue,
                PossibleAnswers = question.PossibleAnswers.Select(pa => new AnswerModel()
                {
                    Answer = pa.Answer,
                    Id = pa.Id
                }).ToList(),
                SelectedAnswer = answer?.PossibleAnswer?.Answer,
                ProgressModel =  new ProgressModel()
                {
                    PercentComplete =  (int) (progress.ProgressPercent *100)
                }
            };

            return View(vm);
        }

        [Route("vote/{id:int}")]
        public async Task<IActionResult> Vote(int id, [FromForm] int answerId)
        {
            var question = await questionRepository.WithPossibleAnswsers(id);

            var selectedAnswer = question.PossibleAnswers.FirstOrDefault(pa => pa.Id == answerId);

            if (selectedAnswer == null)
            {
                return NotFound();
            }

            var answer = new Answer()
            {
                QuestionId = id,
                PossibleAnswerId = answerId
            };

            answerRepository.Add(answer);
            await UpdateProgress();
            
            unitOfWork.Commit();

            return RedirectToAction("Question", new {id = id + 1});
        }

        private async Task UpdateProgress()
        {
            var questionCount = await questionRepository.Count();
            var answerCount = await answerRepository.Count()+1;
            var p = await progressRepository.Get();
            p.ProgressPercent = (decimal) answerCount / questionCount;
            await progressRepository.Set(p);
        }
    }

    public class QuestionModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public IList<AnswerModel> PossibleAnswers { get; set; }
        public string SelectedAnswer { get; set; }
        public bool IsAnswered => !string.IsNullOrWhiteSpace(SelectedAnswer);
        public ProgressModel ProgressModel { get; set; }
    }

    public class ProgressModel
    {
        public int PercentComplete { get; set; }
    }

    public class AnswerModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }
    }
}