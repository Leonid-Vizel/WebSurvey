using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebSurvey.Models;
using WebSurvey.Models.Database;
using WebSurvey.Models.Voting;

namespace WebSurvey.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        #region Surveys
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<SurveyQuestionOption> SurveyQuestionOptions { get; set; }
        public DbSet<SurveyResult> SurveyResults { get; set; }
        #endregion
        #region Votings
        public DbSet<Voting> Votings { get; set; }
        public DbSet<VotingOption> VotingOptions { get; set; }
        public DbSet<VotingResult> VotingResults { get; set; }
        #endregion
    }
}
