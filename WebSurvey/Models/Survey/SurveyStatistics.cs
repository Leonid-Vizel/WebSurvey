namespace WebSurvey.Models.Survey
{
    public class SurveyStatistics : Survey
    {
        public SurveyStatistics(Survey input, int takenCount, int questionsCount = 0)
        {
            Id = input.Id;
            Name = input.Name;
            Description = input.Description;
            IsAnonimous = input.IsAnonimous;
            CreatedDate = input.CreatedDate;
            IsOneOff = input.IsOneOff;
            IsPassworded = input.IsPassworded;
            IsClosed = input.IsClosed;
            TakenCount = takenCount;
            QuestionsCount = questionsCount;
        }

        public SurveyStatistics() { }

        public int TakenCount { get; set; }
        public int QuestionsCount { get; set; }
    }
}
