namespace WebSurvey.Models.Voting
{
    public class VotingStatistics : Voting
    {
        public static string[] Colors;
        public int TakenCount { get; private set; }
        public int OptionCount { get; private set; }
        public Dictionary<string, double>? ResultPercents { get; private set; }
        public Dictionary<string, int>? ResultCounts { get; private set; }
        public VotingStatistics() { }

        static VotingStatistics()
        {
            Colors = new string[]
            {
                "#153c3b",
                "#4e9d97",
                "#e1aeab",
                "#698644",
                "#5e6464",
                "#cc883e",
                "#f4c88c",
                "#a5c6b1",
                "#f2c947",
                "#abd7eb",
                "#2c9a8e",
                "#01463f",
                "#47485d",
                "#6B5B5B",
                "#8F755A",
                "#C5977D",
                "#E3B780",
                "#D7C3B7",
                "#7d3865",
                "#c1a7b0",
                "#f8b703",
                "#949217",
                "#0fa2a9",
                "#843d3b",
                "#bd787d",
                "#e0acbe",
                "#ded1dc",
                "#8f8383"
            };
        }

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

        public VotingStatistics(Voting info, IEnumerable<VotingResult> results, IEnumerable<QuestionOption> options)
        {
            Id = info.Id;
            Name = info.Name;
            Description = info.Description;
            CreatedTime = info.CreatedTime;
            IsClosed = info.IsClosed;
            IsPassworded = info.IsPassworded;
            Password = info.Password;
            TakenCount = results.Count();
            ResultPercents = new Dictionary<string, double>();
            ResultCounts = new Dictionary<string, int>();
            if (results.Count() > 0)
            {
                foreach (QuestionOption option in options)
                {
                    ResultPercents.Add(option.Text, (double)results.Count(x => x.Answer.Equals(option.Text)) / results.Count() * 100);
                    ResultCounts.Add(option.Text, results.Count(x => x.Answer.Equals(option.Text)));
                }
            }
            else
            {
                foreach (QuestionOption option in options)
                {
                    ResultPercents.Add(option.Text, 0);
                    ResultCounts.Add(option.Text, 0);
                }
            }
        }
    }
}
