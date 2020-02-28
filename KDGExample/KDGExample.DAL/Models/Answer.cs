namespace KDGExample.DAL.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public PossibleAnswer  PossibleAnswer { get; set; }
        public int PossibleAnswerId { get; set; }
        public int QuestionnaireId { get; set; }
    }
}