using System.Runtime.Serialization.Formatters.Binary;
using WebSurvey.Models.Answers;

namespace WebSurvey.Models.ViewModel
{
    public class SurveyResult : Database.SurveyResult
    {
        public SurveyResult(Database.SurveyResult input, string name, List<SurveyQuestion> questions)
        {
            Questions = questions;
            Id = input.Id;
            SurveyId = input.SurveyId;
            Name = name;
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(Data, 0, Data.Length);
                stream.Position = 0;

                Results = formatter.Deserialize(stream) as List<Answer>;
            }
            if (Results == null)
            {
                Results = new List<Answer>();
            }
        }

        public SurveyResult()
        {

        }

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

        public SurveyResult(Survey survey, List<SurveyQuestion> questions)
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

        public string Name { get; set; }
        public List<SurveyQuestion> Questions { get; set; }
        public List<Answer> Results { get; set; }
    }
}
