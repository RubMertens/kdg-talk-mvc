
namespace KDGExample.DAL.Models
{
    public class PossibleAnswer
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public Question  Question { get; set; }
        public int QuestionId { get; set; }
        public string Color { get; set; }
    }
}