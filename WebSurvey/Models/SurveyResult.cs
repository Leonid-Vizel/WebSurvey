using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models
{
    public class SurveyResult
    {
        [Key]
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public Byte[] Data { get; set; }
    }
}
