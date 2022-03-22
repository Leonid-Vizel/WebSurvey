namespace WebSurvey.Models.ViewModel
{
    public class SurveyQuestion : Database.SurveyQuestion
    {
        public SurveyQuestion(Database.SurveyQuestion input, Database.SurveyQuestionOption[] options)
        {
            Id = input.Id;
            SurveyId = input.SurveyId;
            Name = input.Name;
            Type = input.Type;
            this.options = options;
        }

        Database.SurveyQuestionOption[] options { get; set; }
    }
}
