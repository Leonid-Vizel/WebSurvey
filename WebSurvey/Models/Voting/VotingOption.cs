using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Voting
{
    public class VotingOption
    {
        [Key]
        public int Id { get; set; }
        public int VotingId { get; set; }
        [Required(ErrorMessage = "Укажите название опциии или удалите её")]
        public string Text { get; set; }
    }
}
