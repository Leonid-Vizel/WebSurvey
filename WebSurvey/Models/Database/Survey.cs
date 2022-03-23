using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Database
{
    public class Survey
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsPassworded { get; set; } //Закрытый/Публичный
        public string? Password { get; set; }
        [Required]
        public bool IsAnonimous { get; set; } //Анонимный/Стандартный
        [Required]
        public bool IsOneOff { get; set; } //Однозаровый/Многоразовый
    }
}
