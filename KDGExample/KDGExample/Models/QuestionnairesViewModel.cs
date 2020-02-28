using System.Collections.Generic;

namespace KDGExample.Models
{
    public class QuestionnairesViewModel
    {
        public IList<QuestionnaireViewModel> Questionnaires { get; set; } 
    }

    public class QuestionnaireViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}