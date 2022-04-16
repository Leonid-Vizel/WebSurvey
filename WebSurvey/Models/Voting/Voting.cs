using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Voting
{
    public class Voting
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Укажите название голосования")]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Укажите описание голосования")]
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }
        public bool IsClosed { get; set; }
        [Display(Name = "Закрытый")]
        public bool IsPassworded { get; set; }
        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
