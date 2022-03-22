namespace WebSurvey.Models.ViewModel
{
    public class SurveyStatistics : Database.Survey
    {
        public SurveyStatistics(Database.Survey input, int takenCount)
        {
            Id = input.Id;
            Name = input.Name;
            Description = input.Description;
            TakenCount = takenCount;
        }

        public int TakenCount { get; set; }
    }
}
