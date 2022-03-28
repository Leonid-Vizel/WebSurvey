using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models.Answers
{
    public class Answer
    {
        public string TextAnswer { get; set; }
        public string[] CheckAnswers { get; set; }
    }
}
