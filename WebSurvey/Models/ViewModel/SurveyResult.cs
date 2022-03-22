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

                results = formatter.Deserialize(stream) as Dictionary<SurveyQuestion, object>;
            }
            if (results == null)
            {
                results = new Dictionary<SurveyQuestion, object>();
            }
        }
        Dictionary<SurveyQuestion, object> results { get; set; }
    }
}
