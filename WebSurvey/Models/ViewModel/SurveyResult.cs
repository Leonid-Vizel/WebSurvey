using System.Text.Json;
using WebSurvey.Models.Answers;

namespace WebSurvey.Models.ViewModel
{
    public class SurveyResult : Database.SurveyResult
    {
        public SurveyResult(Database.SurveyResult input, string name, List<SurveyQuestion> questions)
        {
            Questions = questions;
            Id = input.Id;
            UserId = input.UserId;
            DateTaken = input.DateTaken;
            SurveyId = input.SurveyId;
            Name = name;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(input.Data, 0, input.Data.Length);
                stream.Position = 0;
                Results = JsonSerializer.Deserialize(stream, typeof(List<Answer>)) as List<Answer>;
            }
            if (Results == null)
            {
                Results = new List<Answer>();
            }
        }

        public SurveyResult() { }

        public SurveyResult(Database.Survey survey, List<SurveyQuestion> questions)
        {
            SurveyId = survey.Id;
            Name = survey.Name;
            Results = new List<Answer>();
            Questions = questions;
            for (int i = 0; i < questions.Count; i++)
            {
                Results.Add(new Answer());
            }
        }

        public Database.SurveyResult ToDbClass()
        {
            Database.SurveyResult dbResult = new Database.SurveyResult();
            dbResult.SurveyId = SurveyId;
            using (MemoryStream memStream = new MemoryStream())
            {
                JsonSerializer.Serialize(memStream, Results);
                dbResult.Data = memStream.ToArray();
            }
            return dbResult;
        }

        public string Name { get; set; }
        public List<SurveyQuestion> Questions { get; set; }
        public List<Answer> Results { get; set; }
    }
}
