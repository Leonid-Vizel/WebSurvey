using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models
{
    public class Survey
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
