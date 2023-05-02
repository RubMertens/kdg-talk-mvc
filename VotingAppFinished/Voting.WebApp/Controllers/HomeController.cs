using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Logging;
using Voting.Data;
using Voting.Data.Data;
using Voting.Data.Models;
using Voting.Data.Repositories;
using Voting.WebApp.Models;

namespace Voting.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IQuestionnaireRepository _questionnaireRepository;
    private readonly IQuestionRepository _questionRepository;
    private readonly IAnswerRepository _answerRepository;

    public HomeController(ILogger<HomeController> logger,
        UserManager<IdentityUser> userManager,
        IQuestionnaireRepository questionnaireRepository,
        IQuestionRepository questionRepository,
        IAnswerRepository answerRepository
    )
    {
        _logger = logger;
        this._userManager = userManager;
        _questionnaireRepository = questionnaireRepository;
        _questionRepository = questionRepository;
        _answerRepository = answerRepository;
    }

    public async Task<IActionResult> Index()
    {
        var questionnaires = await _questionnaireRepository.All();

        return View(new QuestionnairesViewModel(
            questionnaires.Select(q => new QuestionnaireViewModel(
                Id: q.Id,
                Title: q.Title,
                FirstQuestionId: q.Questions.First().Id
            )).ToList()
        ));
    }

    [HttpGet("question/{id:int}")]
    public async Task<IActionResult> Question(int id)
    {
        //part one
        var question = await _questionRepository.ById(id);
        //part two
        var userId = _userManager.GetUserId(User);

        if (question == null)
        {
            return RedirectToAction("Index");
        }

        var answer = await _answerRepository.ByQuestionAndUserId(id, userId);
        //part three
        var nextQuestionId = await _questionnaireRepository.NextQuestionId(id);

        var vm = new QuestionViewModel(
            Id: question.Id,
            Question: question.QuestionValue,
            QuestionnaireId: question.QuestionnaireId,
            NextQuestionId: nextQuestionId ?? -1,
            IsAnswered: answer != null,
            Answers: question.PossibleAnswers.Select(pa => new AnswerViewModel(
                pa.Id,
                pa.Answer,
                IsSelectedAnswer: pa.Id == answer?.PossibleAnswerId
            )).ToList()
        );
        return View(vm);
    }

    [HttpPost("Vote/{questionId:int}")]
    public async Task<IActionResult> Vote(int questionId, int answerId)
    {
        var userId = _userManager.GetUserId(User);
        var answer = new Answer()
        {
            QuestionId = questionId,
            PossibleAnswerId = answerId,
            UserId = userId,
        };
        await _answerRepository.Add(answer);
        
        var nextQuestionId = await _questionnaireRepository.NextQuestionId(questionId);
        return RedirectToAction("Question", new {id= nextQuestionId ?? -1});
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public record QuestionnairesViewModel(
    IList<QuestionnaireViewModel> Questionnaires
);

public record QuestionnaireViewModel(
    int Id,
    string Title,
    int FirstQuestionId
);

public record QuestionViewModel(
    int Id,
    string Question,
    int QuestionnaireId,
    int NextQuestionId,
    bool IsAnswered,
    IList<AnswerViewModel> Answers
);

public record AnswerViewModel(
    int Id,
    string Answer,
    bool IsSelectedAnswer);