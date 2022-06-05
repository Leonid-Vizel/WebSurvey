using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models
{
    public class SearchModel
    {
        [Required(ErrorMessage = "Заполните для поиска")]
        [RegularExpression(@"^[-+]?\d+$", ErrorMessage = "Неверный номер")]
        [Range(0, int.MaxValue, ErrorMessage = "Неверный номер")]
        public int Search { get; set; }

        [ValidateNever]
        [MaxLength(100, ErrorMessage = "Слишком длинный пароль")]
        public string? Password { get; set; }
        public bool ShowPasswordInput { get; set; }
        public EntityType Type { get; set; }
        [Required(ErrorMessage = "Заполните для поиска")]
        [Range(0, int.MaxValue, ErrorMessage = "Неверный номер")]
        public int SearchVoting { get; set; }
        [Required(ErrorMessage = "Заполните для поиска")]
        [Range(0, int.MaxValue, ErrorMessage = "Неверный номер")]
        public int SearchSurvey { get; set; }
    }

    public enum EntityType
    {
        Survey,
        Voting
    }
}
