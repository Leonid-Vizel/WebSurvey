using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Voting
{
    public class Voting
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Укажите название голосования")]
        [Display(Name = "Название")]
        [MaxLength(150, ErrorMessage = "Слишком длинное название голосования")]
        [MinLength(0)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите описание голосования")]
        [Display(Name = "Описание")]
        [MaxLength(500, ErrorMessage = "Слишком длинное описание голосования")]
        [MinLength(0)]
        public string Description { get; set; }
        [ValidateNever]
        public string AuthorId { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool IsClosed { get; set; }
        [Display(Name = "Защитить паролем")]
        public bool IsPassworded { get; set; }
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
