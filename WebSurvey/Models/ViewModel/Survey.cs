namespace WebSurvey.Models.ViewModel
{
    public class Survey : Database.Survey
    {
        public Survey(Database.Survey input, Database.SurveyQuestion[] questions)
        {
            Id = input.Id;
            Name = input.Name;
            Description = input.Description;
            this.questions = questions;
        }

        Database.SurveyQuestion[] questions { get; set; }
    }
}
