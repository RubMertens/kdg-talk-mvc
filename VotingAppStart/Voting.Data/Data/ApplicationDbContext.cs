using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Voting.Data.Models;

namespace Voting.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<PossibleAnswer> PossibleAnswers { get; set; }
        
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Questionnaire>()
                .HasData(new Questionnaire()
                    {
                        Title = "Silly Questions",
                        Id = 1
                    },
                    new Questionnaire()
                    {
                        Title = "Monty Python references",
                        Id = 2
                    }
                );
            

            builder.Entity<Question>()
                .HasData(new Question()
                    {
                        Id = 1,
                        QuestionValue = "Is Cereal soup?",
                        QuestionnaireId = 1
                    },
                    new Question()
                    {
                        Id = 2,
                        QuestionnaireId = 1,
                        QuestionValue = "Does toilet paper go over or under the roll?"
                    },
                    new Question()
                    {
                        Id = 3,
                        QuestionnaireId = 2,
                        QuestionValue = "What is your favourite colour?"
                    },
                    new Question()
                    {
                        Id = 4,
                        QuestionnaireId = 2,
                        QuestionValue = "What should you bring the knights who say Ni!"
                    }
        );

        builder.Entity<PossibleAnswer>()
                .HasData(new PossibleAnswer()
                    {
                        Id = 1,
                        Answer = "Totally yes",
                        Color = "green",
                        QuestionId = 1
                    },
                    new PossibleAnswer()
                    {
                        Id = 2,
                        Answer = "Hell no, cereal is just breakfast",
                        Color = "red",
                        QuestionId = 1
                    },
                    new PossibleAnswer()
                    {
                        Id = 3,
                        Answer = "Over! Under is simply barbaric!",
                        Color = "yellow",
                        QuestionId = 2
                    },
                    new PossibleAnswer()
                    {
                        Id = 4,
                        Answer = "Under! Over is for spawn of Satan!",
                        Color = "yellow",
                        QuestionId = 2
                    },
                    new PossibleAnswer()
                    {
                        Id = 5,
                        Answer = "Blue",
                        QuestionId = 3
                    },
                    new PossibleAnswer()
                    {
                        Id = 6,
                        Answer = "Yellow",
                        QuestionId = 3
                    },
                    new PossibleAnswer()
                    {
                        QuestionId = 4,
                        Id = 7,
                        Answer = "A shrubbery!"
                    },
                    new PossibleAnswer()
                    {
                        QuestionId = 4,
                        Id = 8,
                        Answer = "A holy grail?"
                    }
                    );
            base.OnModelCreating(builder);
        }
    }
}