using System.Runtime.Serialization.Formatters.Binary;

namespace WebSurvey.Models.ViewModel
{
    public class SurveyResult : Database.SurveyResult
    {
        public SurveyResult(Database.SurveyResult input)
        {
            Id = input.Id;
            SurveyId = input.SurveyId;
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(Data, 0, Data.Length);
                stream.Position = 0;

                Results = formatter.Deserialize(stream) as Dictionary<SurveyQuestion, object>;
            }
            if (Results == null)
            {
                Results = new Dictionary<SurveyQuestion, object>();
            }
        }

        public SurveyResult(int surveyId, SurveyQuestion[] questions)
        {
            SurveyId = surveyId;
            Results = new Dictionary<SurveyQuestion, object?>();
            foreach (SurveyQuestion question in questions)
            {
                Results.Add(question, null);
            }
        }

        public Dictionary<SurveyQuestion, object?> Results { get; set; }
    }
}
