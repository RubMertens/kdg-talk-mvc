using System.Collections.Generic;

namespace VotingApp.DAL.Models
{
    public class Question 
    {
        public int Id { get; set; }
        public string QuestionValue { get; set; }
        public ICollection<PossibleAnswer> PossibleAnswers { get; set; }
        public int QuestionnaireId { get; set; }
        public Questionnaire Questionnaire { get; set; }
    }
}