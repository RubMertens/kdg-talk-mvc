using System.Collections;
using System.Collections.Generic;

namespace KDGExample.DAL.Models
{
    public class Questionnaire
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<Question> Questions { get; set; }
        public decimal PercentCompleted { get; set; }
    }
}