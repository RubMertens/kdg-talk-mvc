using System.Collections.Generic;

namespace Voting.Data.Models
{
    public class Questionnaire
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}