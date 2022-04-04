namespace WebSurvey.Models.ViewModel
{
    public class CreateSurveyModel : Database.Survey
    {
        public CreateSurveyModel(Database.Survey input, SurveyQuestion[] questions)
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

        public SurveyQuestion[] Questions { get; set; }
    }
}
