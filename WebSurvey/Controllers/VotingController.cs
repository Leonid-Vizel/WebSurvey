using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebSurvey.Data;
using WebSurvey.Models.Voting;

namespace WebSurvey.Controllers
{
    public class VotingController : Controller
    {
        private SignInManager<IdentityUser> signInManager;
        private UserManager<IdentityUser> userManager;
        private ApplicationDbContext db;
        public VotingController(ApplicationDbContext db, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Status(int Id)
        {
            Id = 2;
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(v => v.Id == Id);
                if (foundVoting == null)
                {
                    return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
                }
                else
                {
                    if (foundVoting.IsClosed)
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "VotingClosed");
                    }
                    else
                    {
                        if (db.VotingOptions.Count(x => x.VotingId == foundVoting.Id) > 1)
                        {
                            return View(new VotingStatistics(foundVoting, db.VotingResults.Count(x => x.VotingId == foundVoting.Id)));
                        }
                        else
                        {
                            return RedirectToAction(controllerName: "Error", actionName: "CorruptVoting");
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "NeedToSignIn");
            }
        }
    }
}
