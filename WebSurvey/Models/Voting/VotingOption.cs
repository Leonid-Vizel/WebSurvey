using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Voting
{
    public class VotingOption
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int VotingId { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
