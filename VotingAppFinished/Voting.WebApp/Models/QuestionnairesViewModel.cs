using System.Collections.Generic;

namespace Voting.WebApp.Models
{
    public class QuestionnairesViewModel
    {
        public List<QuestionnaireViewModel> Questionnaires { get; set; }
    }

    public class QuestionnaireViewModel
    {
        public int Id { get; set; }
        public int FirstQuestionId { get; set; }
        public string Title { get; set; }
    }
}