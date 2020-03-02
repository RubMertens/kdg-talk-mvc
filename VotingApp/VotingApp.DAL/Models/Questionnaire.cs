using System.Collections.Generic;

namespace VotingApp.DAL.Models
{
    public class Questionnaire
    {
        public int Id { get; set; }
        public ICollection<Question> Questions { get; set; }
        public decimal PercentComplete { get; set; }
        public string Title { get; set; }
    }
}