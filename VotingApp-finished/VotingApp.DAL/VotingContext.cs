using Microsoft.EntityFrameworkCore;
using VotingApp.DAL.Models;

namespace VotingApp.DAL
{
    public class VotingContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<PossibleAnswer> PossibleAnswers { get; set; }
        public DbSet<Progress> Progresses { get; set; }    

        public VotingContext(DbContextOptions<VotingContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .HasData(new Question()
                    {
                        Id = 1,
                        QuestionValue = "Is Cereal soup?",
                    },
                    new Question()
                    {
                        Id = 2,
                        QuestionValue = "Does toilet paper go over or under the roll?"
                    }
                );
            modelBuilder.Entity<PossibleAnswer>()
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
                    });
        }
    }
}