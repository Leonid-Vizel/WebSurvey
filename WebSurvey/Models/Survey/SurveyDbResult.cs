using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Survey
{
    public class SurveyDbResult
    {
        [Key]
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public DateTime DateTaken { get; set; }
        public string? UserId { get; set; }
        public byte[] Data { get; set; }
    }
}
