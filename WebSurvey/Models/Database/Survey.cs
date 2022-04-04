using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Database
{
    public class Survey
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [MaxLength(150, ErrorMessage = "Слишком длинное название опроса")]
        [MinLength(0)]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Это обязательное поле")]
        [MaxLength(500, ErrorMessage = "Слишком длинное описание опроса")]
        [MinLength(0)]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        public string AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsClosed { get; set; }
        [Display(Name = "Закрытый")]
        public bool IsPassworded { get; set; } //Закрытый/Публичный
        [DataType(DataType.Password)]
        [MaxLength(15, ErrorMessage = "Слишком длинный пароль")]
        [MinLength(5, ErrorMessage = "Слишком короткий пароль")]
        [Display(Name = "Пароль")]
        public string? Password { get; set; }
        [Display(Name = "Анонимный")]
        public bool IsAnonimous { get; set; } //Анонимный/Стандартный
        [Display(Name = "Однозаровый")]
        public bool IsOneOff { get; set; } //Однозаровый/Многоразовый
    }
}
