namespace WebSurvey.Models.Survey
{
    public class SurveyAnswer
    {
        public string TextAnswer { get; set; }
        public int IntAnswer { get; set; }
        public double DoubleAnswer { get; set; }
        public string[] CheckAnswers { get; set; }
    }
}