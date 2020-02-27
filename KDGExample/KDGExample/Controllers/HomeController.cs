using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KDGExample.DAL.Context;
using KDGExample.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using KDGExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KDGExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly VotingContext _context;

        public HomeController(VotingContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var firstQuestion = await _context.Questions.FirstOrDefaultAsync();
            ViewBag.FirstQuestionid = firstQuestion.Id;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Vote([FromRoute] int id)
        {
            var question = await _context.Questions
                .Include(q => q.PossibleAnswers)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
                return RedirectToAction("Index");

            var answer = await _context.Answers.FirstOrDefaultAsync(a => a.QuestionId == question.Id);

            var vm = new QuestionViewModel()
            {
                Question = question.QuestionContent,
                QuestionId = question.Id,
                Answers = question.PossibleAnswers.Select(ToViewModel).ToList(),
                Answer = ToViewModel(answer?.PossibleAnswer)
            };

            return View(vm);
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
        public async Task<IActionResult> Vote([FromRoute] int id, [FromForm] int answerId)
        {
            var question = await _context.Questions.Include(q => q.PossibleAnswers)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
                return RedirectToAction("Index");
            var selectedAnswer = question.PossibleAnswers.FirstOrDefault(pa => pa.Id == answerId);
            if (selectedAnswer == null)
                return NotFound();

            var answer = new Answer()
            {
                QuestionId = question.Id,
                PossibleAnswerId = selectedAnswer.Id
            };
            using (var uow = await _context.StartUnitOfWork())
            {
                _context.Answers.Add(answer);
                await uow.CommitAsync();
            }

            return RedirectToAction("Vote", new {id = question.Id + 1});
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


    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public ICollection<AnswerViewModel> Answers { get; set; }
        public bool IsAnswered => Answer != null;
        public AnswerViewModel Answer { get; set; }
    }

    public class AnswerViewModel
    {
        public string Answer { get; set; }
        public int AnswerId { get; set; }
        public string Color { get; set; }
    }
}