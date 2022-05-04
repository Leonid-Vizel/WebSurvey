using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSurvey.Models.Surveyll
{
    public class SurveyUserAnswer
    {
        [Key]
        public int Id { get; set; }
        public int ResultId { get; set; }
        [MaxLength(1000, ErrorMessage = "Значение слишком большое. Максимальная длинна - 1000 символов")]
        public string? TextAnswer { get; set; }
        [Required(ErrorMessage = "Значение должно быть указано")]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Неверный тип значения")]
        public int? IntAnswer { get; set; }
        [Required(ErrorMessage = "Значение должно быть указано")]
        [RegularExpression(@"[-+]?[0-9]*\.?[0-9]*", ErrorMessage = "Неверный тип значения")]
        public double? DoubleAnswer { get; set; }
        [NotMapped]
        public string[]? CheckAnswers { get; set; }
        public byte[] CheckAnswerData { get; set; }
    }
}
