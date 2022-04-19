using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Survey
{
    public class SurveyQuestionOption
    {
        [Key]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
    }
}
