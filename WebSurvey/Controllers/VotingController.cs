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
        public async Task<IActionResult> Create(VotingCreateModel model)
        {
            if (ModelState.IsValid)
            {
                //if (model.Options == null || model.Options.Count <= 1)
                //{
                //    ModelState.AddModelError("Options", "У опроса должены быть как минимум 2 опции");
                //    return View(model);
                //}
                await db.Votings.AddAsync(model);
                await db.SaveChangesAsync();
                foreach (VotingOption option in model.Options)
                {
                    option.VotingId = model.Id;
                    await db.VotingOptions.AddAsync(option);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Status", new { Id = model.Id });
            }
            else
            {
                return View(model);
            }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Status(VotingStatistics model)
        {
            Voting? foundVoting = db.Votings.FirstOrDefault(s => s.Id == model.Id);
            if (foundVoting != null)
            {
                if (db.VotingResults.Count(x => x.VotingId == foundVoting.Id && x.UserId.Equals(userManager.GetUserId(User))) > 0)
                {
                    ModelState.AddModelError("Password", "Вы уже приняли участие в этом голосовании");
                    VotingStatistics newStatistics = new VotingStatistics(foundVoting, db.VotingResults.Count(x => x.VotingId == foundVoting.Id));
                    newStatistics.Password = model.Password;
                    return View(newStatistics);
                }
                if (foundVoting.Password == null)
                {
                    return RedirectToAction("Take", new { VotingId = model.Id });
                }
                else
                {
                    if (foundVoting.Password.Equals(model.Password))
                    {
                        return RedirectToAction("Take", new { VotingId = model.Id, Password = model.Password });
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Неверный пароль");
                        VotingStatistics newStatistics = new VotingStatistics(foundVoting, db.VotingResults.Count(x => x.VotingId == foundVoting.Id));
                        newStatistics.Password = model.Password;
                        return View(newStatistics);
                    }
                }
            }
            else
            {
                return RedirectToAction("VotingNotFound", "Error");
            }
        }

        public IActionResult Take(int VotingId, string Password)
        {
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(x => x.Id == VotingId);
                if (foundVoting != null)
                {
                    if (db.VotingResults.Count(x => x.VotingId == foundVoting.Id && x.UserId.Equals(userManager.GetUserId(User))) == 0)
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
                        return RedirectToAction(controllerName: "Error", actionName: "VotingUsed");
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
                if (db.VotingResults.Count(x => x.VotingId == result.VotingId && x.UserId.Equals(userManager.GetUserId(User))) == 0)
                {
                    result.CreatedDate = DateTime.Now;
                    result.UserId = userManager.GetUserId(User);
                    await db.VotingResults.AddAsync(result);
                    await db.SaveChangesAsync();
                    return RedirectToAction(controllerName: "Home", actionName: "Index");
                }
                else
                {
                    return RedirectToAction(controllerName: "Error", actionName: "VotingUsed");
                }
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
