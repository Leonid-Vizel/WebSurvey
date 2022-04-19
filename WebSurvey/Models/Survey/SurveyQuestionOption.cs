using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Survey
{
    public class SurveyQuestionOption
    {
        [Key]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        [Required(ErrorMessage = "Укажите опцию или удалите её")]
        public string Text { get; set; }
    }
}
