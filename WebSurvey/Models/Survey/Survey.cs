using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Survey
{
    public class Survey
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Укажите название опроса")]
        [MaxLength(150, ErrorMessage = "Слишком длинное название опроса")]
        [MinLength(0)]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите описание опроса")]
        [MaxLength(500, ErrorMessage = "Слишком длинное описание опроса")]
        [MinLength(0)]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [ValidateNever]
        public string AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsClosed { get; set; }
        [Display(Name = "Закрытый")]
        public bool IsPassworded { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(15, ErrorMessage = "Слишком длинный пароль")]
        [MinLength(5, ErrorMessage = "Слишком короткий пароль")]
        [Display(Name = "Защитить паролем")]
        public string? Password { get; set; }
        [Display(Name = "Анонимный")]
        public bool IsAnonimous { get; set; }
        [Display(Name = "Однозаровый")]
        public bool IsOneOff { get; set; }
    }
}
