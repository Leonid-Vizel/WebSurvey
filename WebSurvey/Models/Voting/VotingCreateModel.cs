using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Voting
{
    public class VotingCreateModel : Voting
    {
        [Required(ErrorMessage = "У голосования должены быть как минимум 2 опции")]
        [MinLength(2,ErrorMessage = "У голосования должены быть как минимум 2 опции")]
        public List<VotingOption> Options { get; set; }
    }
}
