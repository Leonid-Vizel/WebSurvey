using Microsoft.EntityFrameworkCore;
using WebSurvey.Models;
using WebSurvey.Models.Database;

namespace WebSurvey.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        DbSet<Survey> Surveys { get; set; }
        DbSet<SurveyQuestion> Questions { get; set; }
        DbSet<SurveyQuestionOption> Options { get; set; }
        DbSet<SurveyResult> Results { get; set; }
    }
}
