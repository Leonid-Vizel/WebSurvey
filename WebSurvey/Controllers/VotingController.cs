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

        public IActionResult Create()
        {
            if (signInManager.IsSignedIn(User))
            {
                return View();
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "NeedToSignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VotingCreateModel model)
        {
            throw new NotImplementedException();
        }

        public IActionResult Status(int Id)
        {
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(v => v.Id == Id);
                if (foundVoting != null)
                {
                    if (!foundVoting.IsClosed)
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
                    else
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "VotingClosed");
                    }
                }
                else
                {
                    return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "NeedToSignIn");
            }
        }

        public IActionResult Take(int VotingId, string Password)
        {
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(x => x.Id == VotingId);
                if (foundVoting != null)
                {
                    if (!foundVoting.IsClosed)
                    {
                        if (!foundVoting.IsPassworded || foundVoting.Password.Equals(Password))
                        {
                            IEnumerable<VotingOption> foundOptions = db.VotingOptions.Where(x => x.VotingId == foundVoting.Id);
                            if (foundOptions.Count() > 1)
                            {
                                return View(new VotingResult(foundVoting, foundOptions.ToList()));
                            }
                            else
                            {
                                return RedirectToAction(controllerName: "Error", actionName: "CorruptVoting");
                            }
                        }
                        else
                        {
                            return RedirectToAction(controllerName: "Error", actionName: "WrongPassword");
                        }
                    }
                    else
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "VotingClosed");
                    }
                }
                else
                {
                    return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
                }
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "NeedToSignIn");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Take(VotingResult result)
        {
            if (signInManager.IsSignedIn(User))
            {
                result.CreatedDate = DateTime.Now;
                result.UserId = userManager.GetUserId(User);
                await db.VotingResults.AddAsync(result);
                await db.SaveChangesAsync();
                return RedirectToAction(controllerName: "Home", actionName: "Index");
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "NeedToSignIn");
            }
        }

        public IActionResult Select()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Select(VotingSearchModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Votings.Any(x => x.Id == model.Search))
                {
                    return RedirectToAction("Status", new { Id = model.Search });
                }
                else
                {
                    ModelState.AddModelError("Search", "Голосование не найдено");
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}
