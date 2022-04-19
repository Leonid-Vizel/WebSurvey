namespace WebSurvey.Models.Survey
{
    public class SurveyQuestion : SurveyDbQuestion
    {
        public SurveyQuestion(SurveyDbQuestion input, SurveyQuestionOption[] options)
        {
            Id = input.Id;
            SurveyId = input.SurveyId;
            Name = input.Name;
            Type = input.Type;
            Options = options;
        }

        public SurveyQuestion() { }

        public SurveyQuestionOption[] Options { get; set; }
    }
}
