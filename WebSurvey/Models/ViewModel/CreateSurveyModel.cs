namespace WebSurvey.Models.ViewModel
{
    public class CreateSurveyModel : Database.Survey
    {
        public CreateSurveyModel() { /*Пустой конструктор*/ }

        public SurveyQuestion[] Questions { get; set; }
    }
}
