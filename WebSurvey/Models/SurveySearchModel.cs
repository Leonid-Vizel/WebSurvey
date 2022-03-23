using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models
{
    public class SurveySearchModel
    {
        [Required(ErrorMessage = "Заполните для поиска")]
        [Range(0,int.MaxValue, ErrorMessage = "Неверный номер")]
        public int Search { get; set; }
        [MaxLength(100,ErrorMessage = "Слишком длинный пароль")]
        public string? Password { get; set; }
        public bool ShowPasswordInput { get; set; }
    }
}
