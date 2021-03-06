using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebSurvey.Models;
using WebSurvey.Models.Survey;
using WebSurvey.Models.Voting;

namespace WebSurvey.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        #region Surveys
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyDbQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyDbResult> SurveyResults { get; set; }
        #endregion

        #region Votings
        public DbSet<Voting> Votings { get; set; }
        public DbSet<VotingResult> VotingResults { get; set; }
        #endregion

        public DbSet<QuestionOption> Options { get; set; }
    }
}
