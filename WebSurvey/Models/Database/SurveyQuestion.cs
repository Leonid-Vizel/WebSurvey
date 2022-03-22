using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Database
{
    public class SurveyQuestion
    {
        [Key]
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string Name { get; set; }
        public QuestionType Type { get; set; }
    }

    public enum QuestionType
    {
        Text,
        Radio,
        Check
    }
}
