namespace WebSurvey.Models.ViewModel
{
    public class SurveyStatistics : Database.Survey
    {
        public SurveyStatistics(Database.Survey input, int takenCount)
        {
            Id = input.Id;
            Name = input.Name;
            Description = input.Description;
            IsAnonimous = input.IsAnonimous;
            IsOneOff = input.IsOneOff;
            IsPassworded = input.IsPassworded;
            IsClosed = input.IsClosed;
            TakenCount = takenCount;
        }

        public SurveyStatistics() { }

        public int TakenCount { get; set; }
    }
}
