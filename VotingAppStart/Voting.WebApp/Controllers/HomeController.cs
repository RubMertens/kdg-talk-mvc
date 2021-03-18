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
        private readonly ICommentRepository commentRepository;

        public HomeController(ILogger<HomeController> logger,
            UserManager<IdentityUser> userManager,
            ICommentRepository commentRepository
        )
        {
            _logger = logger;
            this.userManager = userManager;
            this.commentRepository = commentRepository;
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
}