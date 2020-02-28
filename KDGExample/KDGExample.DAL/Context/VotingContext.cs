using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using KDGExample.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace KDGExample.DAL.Context
{

    public class VotingContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<PossibleAnswer> PossibleAnswers { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }

        public VotingContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Questionnaire>().HasData(
                new Questionnaire()
                {
                    Id = 1,
                    Title = "Helemaal relevante gemeente vragen",
                }
            );
            modelBuilder.Entity<Question>().HasData(
                new Question()
                {
                    Id = 1,
                    QuestionContent = "LEZ uitbreiden of niet? ",
                    QuestionnaireId = 1
                },
                new Question()
                {
                    Id = 2,
                    QuestionContent = "Een tunnel of een brug? wat zal het worden Antwerpen?",
                    QuestionnaireId = 1
                }
            );
            modelBuilder.Entity<PossibleAnswer>().HasData(
                new PossibleAnswer()
                {
                    Answer = "Yes, totally!",
                    Color = "green",
                    Id = 1,
                    QuestionId = 1
                },
                new PossibleAnswer()
                {
                    Answer = "nope, no way!",
                    Color = "red",
                    Id = 2,
                    QuestionId = 1
                },
                new PossibleAnswer()
                {
                    Answer = "Een brug graag!",
                    Color = "yellow",
                    Id = 3,
                    QuestionId = 2
                },
                new PossibleAnswer()
                {
                    Answer = "Een tunnel please!",
                    Color = "yellow",
                    Id = 4,
                    QuestionId = 2
                }
            );
            
        }
    }
}