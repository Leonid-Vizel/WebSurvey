namespace WebSurvey.Models.ViewModel
{
    public class Survey : Database.Survey
    {
        public Survey(Database.Survey input, Database.SurveyQuestion[] questions)
        {
            Id = input.Id;
            Name = input.Name;
            Description = input.Description;
            AuthorId = input.AuthorId;
            CreatedDate = input.CreatedDate;
            IsAnonimous = input.IsAnonimous;
            IsOneOff = input.IsOneOff;
            IsPassworded = input.IsPassworded;
            IsClosed = input.IsClosed;
            Password = input.Password;
            Questions = questions;
        }

        public Database.SurveyQuestion[] Questions { get; set; }
    }
}
