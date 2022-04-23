using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Survey
{
    public class SurveyAnswer
    {
        [MaxLength(1000,ErrorMessage = "Значение слишком большое. Максимальная длинна - 1000 символов")]
        public string? TextAnswer { get; set; }
        [Required(ErrorMessage = "Значение должно быть указано")]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Неверный тип значения")]
        public int? IntAnswer { get; set; }
        [Required(ErrorMessage = "Значение должно быть указано")]
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]*", ErrorMessage = "Неверный тип значения")]
        public double? DoubleAnswer { get; set; }
        public string[]? CheckAnswers { get; set; }
    }
}