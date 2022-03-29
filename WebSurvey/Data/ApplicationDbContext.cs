using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebSurvey.Models;
using WebSurvey.Models.Database;

namespace WebSurvey.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyQuestion> Questions { get; set; }
        public DbSet<SurveyQuestionOption> Options { get; set; }
        public DbSet<SurveyResult> Results { get; set; }
    }
}
