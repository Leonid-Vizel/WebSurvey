using System.Text.Json;

namespace WebSurvey.Models.Survey
{
    public class SurveyResult : SurveyDbResult
    {
        public SurveyResult(SurveyDbResult input, string name, List<SurveyQuestion> questions)
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
                Results = JsonSerializer.Deserialize(stream, typeof(List<SurveyAnswer>)) as List<SurveyAnswer>;
            }
            if (Results == null)
            {
                Results = new List<SurveyAnswer>();
            }
        }

        public SurveyResult() { }

        public SurveyResult(Survey survey, List<SurveyQuestion> questions)
        {
            SurveyId = survey.Id;
            Name = survey.Name;
            Results = new List<SurveyAnswer>();
            Questions = questions;
            for (int i = 0; i < questions.Count; i++)
            {
                Results.Add(new SurveyAnswer());
            }
        }

        public SurveyDbResult ToDbClass()
        {
            SurveyDbResult dbResult = new SurveyDbResult();
            dbResult.SurveyId = SurveyId;
            dbResult.UserId = UserId;
            dbResult.DateTaken = DateTaken;

            using (MemoryStream memStream = new MemoryStream())
            {
                JsonSerializer.Serialize(memStream, Results);
                dbResult.Data = memStream.ToArray();
            }
            return dbResult;
        }

        public string Name { get; set; }
        public List<SurveyQuestion> Questions { get; set; }
        public List<SurveyAnswer> Results { get; set; }
    }
}
