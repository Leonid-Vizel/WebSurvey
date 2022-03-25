namespace WebSurvey.Models.ViewModel
{
    public class SurveyQuestion : Database.SurveyQuestion
    {
        public SurveyQuestion(Database.SurveyQuestion input, SurveyQuestionOption[] options)
        {
            Id = input.Id;
            SurveyId = input.SurveyId;
            Name = input.Name;
            Type = input.Type;
            Options = options;
        }

        public SurveyQuestionOption[] Options { get; set; }
    }
}
