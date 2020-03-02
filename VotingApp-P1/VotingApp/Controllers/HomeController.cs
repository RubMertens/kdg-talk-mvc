using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VotingApp.DAL;
using VotingApp.DAL.Repositories;

namespace VotingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuestionnaireRepository questionnaireRepository;

        public HomeController(IQuestionnaireRepository questionnaireRepository)
        {
            this.questionnaireRepository = questionnaireRepository;
        }

        public async Task<IActionResult> Index()
        {
            var questionnaires = await questionnaireRepository.GetAll();

            return View(
                questionnaires.Select(q => new QuestionnaireViewModel()
                {
                    Id = q.Id,
                    Title = q.Title
                })
            );
        }
    }
    
    public class QuestionnaireViewModel
    {
        public string Title { get; set; }
        public int Id { get; set; }
    }
}