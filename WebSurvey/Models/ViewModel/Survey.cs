namespace WebSurvey.Models.ViewModel
{
    public class Survey : Database.Survey
    {
        public Survey(Database.Survey input, Database.SurveyQuestion[] questions)
        {
            Id = input.Id;
            Name = input.Name;
            Description = input.Description;
            IsAnonimous = input.IsAnonimous;
            IsOneOff = input.IsOneOff;
            IsPassworded = input.IsPassworded;
            Password = input.Password;
            Questions = questions;
        }

        public Database.SurveyQuestion[] Questions { get; set; }
    }
}
