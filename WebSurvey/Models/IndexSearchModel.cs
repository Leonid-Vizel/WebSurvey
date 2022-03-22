using System.ComponentModel.DataAnnotations;

namespace WebSurvey.Models
{
    public class IndexSearchModel
    {
        [Required(ErrorMessage = "Заполните для поиска")]
        [Range(0,int.MaxValue, ErrorMessage = "Неверный номер")]
        public int Search { get; set; }
    }
}
