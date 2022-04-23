using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebSurvey.Models.Survey
{
    public class SurveyQuestion : SurveyDbQuestion
    {
        public SurveyQuestion(SurveyDbQuestion input, QuestionOption[] options)
        {
            Id = input.Id;
            SurveyId = input.SurveyId;
            Name = input.Name;
            Type = input.Type;
            Options = options;
        }

        public SurveyQuestion() { }
        [ValidateNever]
        public QuestionOption[] Options { get; set; }
    }
}
