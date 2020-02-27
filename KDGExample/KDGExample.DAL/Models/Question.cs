using System.Collections.Generic;

namespace KDGExample.DAL.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionContent { get; set; }
        public ICollection<PossibleAnswer> PossibleAnswers { get; set; }
    }
}