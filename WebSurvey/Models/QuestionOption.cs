using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models
{
    public class QuestionOption
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        [Required(ErrorMessage = "Укажите название опциии или удалите её")]
        public string Text { get; set; }
    }
}
