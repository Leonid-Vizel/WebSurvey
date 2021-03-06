using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Survey
{
    public class CreateSurveyModel : Survey
    {
        [Required(ErrorMessage = "У опроса должен быть как минимум 1 вопрос")]
        [MinLength(1, ErrorMessage = "У опроса должен быть как минимум 1 вопрос")]
        public SurveyQuestion[] Questions { get; set; }
    }
}
