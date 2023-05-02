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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}