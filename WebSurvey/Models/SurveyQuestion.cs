using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models
{
    public class SurveyQuestion
    {
        [Key]
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public QuestionType type { get; set; }
    }

    public enum QuestionType
    {
        Text,
        Radio,
        Check
    }
}
