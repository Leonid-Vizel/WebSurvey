using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Voting
{
    public class Voting
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedTime { get; set; }
        public bool IsClosed { get; set; }
        public bool IsPassworded { get; set; }
        public string? Password { get; set; }
    }
}
