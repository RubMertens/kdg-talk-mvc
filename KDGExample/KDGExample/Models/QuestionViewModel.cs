using System.Collections.Generic;

namespace KDGExample.Models
{
    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string Question { get; set; }
        public ICollection<AnswerViewModel> Answers { get; set; }
        public bool IsAnswered => Answer != null;
        public AnswerViewModel Answer { get; set; }
        public bool HasNextQuestion { get; set; }
        public int NextQuestionId { get; set; }
        public int QuestionnaireId { get; set; }
        public ProgressViewModel Progress { get; set; }
    }
}