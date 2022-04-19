using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Survey
{
    public class CreateSurveyModel : Survey
    {
        public CreateSurveyModel() { /*Пустой конструктор*/ }

        [Required(ErrorMessage = "У опроса должен быть как минимум 1 вопрос")]
        [MinLength(2, ErrorMessage = "У опроса должен быть как минимум 1 вопрос")]
        public SurveyQuestion[] Questions { get; set; }
    }
}
