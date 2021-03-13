namespace Voting.Data.Models
{
    public class PossibleAnswer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string Answer { get; set; }
        public string Color { get; set; }
    }
}