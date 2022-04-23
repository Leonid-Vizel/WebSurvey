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
        [Required(ErrorMessage = "Выберите один из вариантов")]
        public string Answer { get; set; }

        public Voting voting;
        public List<QuestionOption> options;

        public VotingResult() { }

        public VotingResult(Voting voting, List<QuestionOption> options)
        {
            this.voting = voting;
            this.options = options;
        }
    }
}
