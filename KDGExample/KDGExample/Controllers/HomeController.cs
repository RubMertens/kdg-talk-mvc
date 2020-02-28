using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using KDGExample.DAL.Models;
using KDGExample.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using KDGExample.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace KDGExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var questionnaires = await unitOfWork.Questionnaires.GetAll();
            var vm = new QuestionnairesViewModel()
            {
                Questionnaires = questionnaires.Select(q => new QuestionnaireViewModel()
                {
                    Id = q.Id,
                    Title = q.Title,
                    PercentCompleted = (int) Math.Round(q.PercentCompleted*100)
                }).ToList()
            };
            return View(vm);
        }
    }

}