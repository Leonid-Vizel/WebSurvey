namespace WebSurvey.Models.Voting
{
    public class VotingStatistics : Voting
    {
        public int TakenCount { get; private set; }
        public int OptionCount { get; private set; }
        public VotingStatistics() { }

        public VotingStatistics(Voting info, int takenCount)
        {
            Id = info.Id;
            Name = info.Name;
            Description = info.Description;
            CreatedTime = info.CreatedTime;
            IsClosed = info.IsClosed;
            IsPassworded = info.IsPassworded;
            Password = info.Password;
            TakenCount = takenCount;
        }

        public VotingStatistics(Voting info, int takenCount, int optionCount)
        {
            Id = info.Id;
            Name = info.Name;
            Description = info.Description;
            CreatedTime = info.CreatedTime;
            IsClosed = info.IsClosed;
            IsPassworded = info.IsPassworded;
            Password = info.Password;
            TakenCount = takenCount;
            OptionCount = optionCount;
        }
    }
}
