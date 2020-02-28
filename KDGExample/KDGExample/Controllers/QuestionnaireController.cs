using System.Linq;
using System.Threading.Tasks;
using KDGExample.DAL.Models;
using KDGExample.DAL.Repositories;
using KDGExample.DAL.Repositories.Generic;
using KDGExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace KDGExample.Controllers
{
    [Route("Questionnaire")]
    public class QuestionnaireController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public QuestionnaireController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [Route("{id:int}")]
        public async Task<IActionResult> Index([FromRoute] int id)
        {
            var questionnaire = await unitOfWork.Questionnaires.Get(id);
            var firstQuestion = await unitOfWork.Questions.FirstQuestionOfQuestionnaire(id);
            ViewBag.FirstQuestionid = firstQuestion.Id;
            return View(new QuestionnaireViewModel()
            {
                Id = questionnaire.Id,
                Title = questionnaire.Title
            });
        }

        [HttpGet("{questionnaireId:int}/question/{questionId:int}")]
        public async Task<IActionResult> Question(int questionnaireId, int questionId)
        {
            var question = await unitOfWork.Questions.GetWithPossibleAnswers(questionId);
            if (question == null)
                return RedirectToAction("Index", new {id = questionnaireId});

            var answer = await unitOfWork.Answers.GetByQuestionId(question.Id);

            var nextQuestion = await unitOfWork.Questions.GetNextQuestionId(questionId, questionnaireId);
            var questionnaire = await unitOfWork.Questionnaires.Get(questionnaireId);

            var vm = new QuestionViewModel()
            {
                Question = question.QuestionContent,
                QuestionId = question.Id,
                Answers = question.PossibleAnswers.Select(ToViewModel).ToList(),
                Answer = ToViewModel(answer?.PossibleAnswer),
                HasNextQuestion = nextQuestion.HasValue,
                NextQuestionId = nextQuestion ?? -1,
                QuestionnaireId = questionnaireId,
                Progress = ToViewModel(questionnaire.PercentCompleted)
            };
            return View(vm);
        }

        private ProgressViewModel ToViewModel(decimal percentComplete)
        {
            return new ProgressViewModel()
            {
                PercentComplete = (int) (percentComplete * 100)
            };
        }

        private AnswerViewModel ToViewModel(PossibleAnswer pa)
        {
            if (pa == null) return null;
            return new AnswerViewModel()
            {
                Answer = pa.Answer,
                Color = ToBootStrapColor(pa.Color),
                AnswerId = pa.Id
            };
        }

        [HttpPost]
        [Route("vote/{id:int}")]
        public async Task<IActionResult> Vote([FromRoute] int id, [FromForm] int answerId)
        {
            var question = await unitOfWork.Questions.GetWithPossibleAnswers(id);
            if (question == null)
                return RedirectToAction("Index", "Home");
            var selectedAnswer = question.PossibleAnswers.FirstOrDefault(pa => pa.Id == answerId);

            if (selectedAnswer == null)
                return NotFound();

            var answer = new Answer()
            {
                QuestionId = question.Id,
                PossibleAnswerId = selectedAnswer.Id,
                QuestionnaireId = question.QuestionnaireId
                
            };
            unitOfWork.Answers.Add(answer);
            var questionnaire= await unitOfWork.Questionnaires.Get(question.QuestionnaireId);
            var answerCount = await unitOfWork.Answers.CountByQuestionnaire(question.QuestionnaireId) + 1;
            var questionCount = await unitOfWork.Questions.CountByQuestionnaire(question.QuestionnaireId);

            questionnaire.PercentCompleted = answerCount / (decimal) questionCount;
            unitOfWork.Commit(); 
            
            var nextQuestionId = await unitOfWork.Questions.GetNextQuestionId(id, question.QuestionnaireId);
            if (nextQuestionId == null)
                return RedirectToAction("Index", "Home");
            return RedirectToAction("Question",
                new
                {
                    questionnaireId = question.QuestionnaireId, 
                    questionId = nextQuestionId,
                });
        }

        private string ToBootStrapColor(string color)
        {
            return color switch
            {
                "green" => "success",
                "red" => "danger",
                "yellow" => "warning",
                _ => ""
            };
        }
    }
}