using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Test
{
    public class Test
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Укажите название тест")]
        [MaxLength(150, ErrorMessage = "Слишком длинное название теста")]
        [MinLength(0)]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите описание теста")]
        [MaxLength(500, ErrorMessage = "Слишком длинное описание теста")]
        [MinLength(0)]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [ValidateNever]
        public string AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsClosed { get; set; }
        [Display(Name = "Защитить паролем")]
        public bool IsPassworded { get; set; }
        [DataType(DataType.Password)]
        [MaxLength(15, ErrorMessage = "Слишком длинный пароль")]
        [MinLength(5, ErrorMessage = "Слишком короткий пароль")]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }
    }
}
