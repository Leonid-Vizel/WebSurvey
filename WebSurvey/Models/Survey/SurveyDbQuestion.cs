using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Survey
{
    public class SurveyDbQuestion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int SurveyId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsRequired { get; set; }
        [Required]
        public QuestionType Type { get; set; }
    }

    public enum QuestionType
    {
        Text,
        Radio,
        Check
    }
}
