using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Database
{
    public class SurveyResult
    {
        [Key]
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public DateTime DateTaken { get; set; }
        public string? UserId { get; set; }
        public byte[] Data { get; set; }
    }
}
