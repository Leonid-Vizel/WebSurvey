using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Voting
{
    public class VotingResult
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int VotingId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public string Answer { get; set; }

        public List<VotingOption> options;

        public VotingResult() { }

        public VotingResult(List<VotingOption> options)
        {
            this.options = options;
        }
    }
}
