using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Survey
{
    public class SurveyDbQuestion
    {
        [Key]
        public int Id { get; set; }
        public int SurveyId { get; set; }
        [Required(ErrorMessage = "Укажите вопрос или удалите его")]
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        [Required]
        public QuestionType Type { get; set; }
    }

    public enum QuestionType
    {
        Text,
        Radio,
        Check,
        Integer,
        Double
    }
}
