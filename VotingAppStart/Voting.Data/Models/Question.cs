using System.Collections.Generic;

namespace Voting.Data.Models
{
    public class Question 
    {
        public int Id { get; set; }
        public string QuestionValue { get; set; }
        public ICollection<PossibleAnswer> PossibleAnswers { get; set; }
        public int QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }
        
        public ICollection<Answer> Answers { get; set; }
    }
}