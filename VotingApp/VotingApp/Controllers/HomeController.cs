using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using VotingApp.DAL;
using VotingApp.DAL.Repositories;

namespace VotingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuestionnaireRepository questionnaireRepository;

        public HomeController(ILogger<HomeController> logger, IQuestionnaireRepository questionnaireRepository)
        {
            _logger = logger;
            this.questionnaireRepository = questionnaireRepository;
        }

        public async Task<IActionResult> Index()
        {
            await questionnaireRepository.GetAsync(1);
            return View();
        }
    }
}