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


        #region Create
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
            if (signInManager.IsSignedIn(User))
            {
                if (ModelState.IsValid)
                {
                    if (model.IsPassworded && model.Password == null)
                    {
                        ModelState.AddModelError("Password", "Укажите пароль");
                        return View(model);
                    }
                    model.CreatedTime = DateTime.Now;
                    model.AuthorId = userManager.GetUserId(User);
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
            else
            {
                ModelState.AddModelError("Options", "Авторизуйтесь для создания голосований");
                return View(model);
            }
        }
        #endregion

        #region Status
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
        #endregion

        #region Take
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
        #endregion

        #region Select
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
        #endregion 

        #region Close
        public IActionResult Close(int Id)
        {
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(x => x.Id == Id);
                if (foundVoting != null)
                {
                    if (!foundVoting.IsClosed)
                    {
                        if (foundVoting.AuthorId.Equals(userManager.GetUserId(User)))
                        {
                            return View(Id);
                        }
                        else
                        {
                            return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
                        }
                    }
                    else
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
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
        [ActionName("Close")]
        public async Task<IActionResult> ClosePost(int Id)
        {
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(x => x.Id == Id);
                if (foundVoting != null)
                {
                    if (!foundVoting.IsClosed)
                    {
                        if (foundVoting.AuthorId.Equals(userManager.GetUserId(User)))
                        {
                            foundVoting.IsClosed = true;
                            db.Votings.Update(foundVoting);
                            await db.SaveChangesAsync();
                            return RedirectToAction("MyVotings");
                        }
                        else
                        {
                            return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
                        }
                    }
                    else
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
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
        #endregion 

        #region Open
        public IActionResult Open(int Id)
        {
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(x => x.Id == Id);
                if (foundVoting != null)
                {
                    if (foundVoting.IsClosed)
                    {
                        if (foundVoting.AuthorId.Equals(userManager.GetUserId(User)))
                        {
                            return View(Id);
                        }
                        else
                        {
                            return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
                        }
                    }
                    else
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
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
        [ActionName("Open")]
        public async Task<IActionResult> OpenPost(int Id)
        {
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(x => x.Id == Id);
                if (foundVoting != null)
                {
                    if (foundVoting.IsClosed)
                    {
                        if (foundVoting.AuthorId.Equals(userManager.GetUserId(User)))
                        {
                            foundVoting.IsClosed = false;
                            db.Votings.Update(foundVoting);
                            await db.SaveChangesAsync();
                            return RedirectToAction("MyVotings");
                        }
                        else
                        {
                            return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
                        }
                    }
                    else
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
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
        #endregion 

        #region Delete
        public IActionResult Delete(int Id)
        {
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(x => x.Id == Id);
                if (foundVoting != null)
                {
                    if (foundVoting.AuthorId.Equals(userManager.GetUserId(User)))
                    {
                        return View(Id);
                    }
                    else
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
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
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int Id)
        {
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(x => x.Id == Id);
                if (foundVoting != null)
                {
                    if (foundVoting.AuthorId.Equals(userManager.GetUserId(User)))
                    {
                        db.VotingOptions.RemoveRange(db.VotingOptions.Where(x => x.VotingId == Id));
                        db.VotingResults.RemoveRange(db.VotingResults.Where(x => x.VotingId == Id));
                        db.Votings.RemoveRange(foundVoting);
                        await db.SaveChangesAsync();
                        return RedirectToAction("MyVotings");
                    }
                    else
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
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
        #endregion 

        #region Edit
        public IActionResult Edit(int Id)
        {
            if (signInManager.IsSignedIn(User))
            {
                Voting? foundVoting = db.Votings.FirstOrDefault(x => x.Id == Id);
                if (foundVoting != null)
                {
                    if (foundVoting.AuthorId.Equals(userManager.GetUserId(User)))
                    {
                        return View(foundVoting);
                    }
                    else
                    {
                        return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
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
        public async Task<IActionResult> Edit(Voting editedVoting)
        {
            if (ModelState.IsValid)
            {
                if (signInManager.IsSignedIn(User))
                {
                    Voting? foundVoting = db.Votings.FirstOrDefault(x => x.Id == editedVoting.Id);
                    if (foundVoting != null)
                    {
                        if (foundVoting.AuthorId.Equals(userManager.GetUserId(User)))
                        {
                            if (editedVoting.IsPassworded && editedVoting.Password == null)
                            {
                                ModelState.AddModelError("Password", "Укажите пароль");
                                return View(editedVoting);
                            }
                            foundVoting.Name = editedVoting.Name;
                            foundVoting.Description = editedVoting.Description;
                            foundVoting.IsPassworded = editedVoting.IsPassworded;
                            if (!foundVoting.IsPassworded)
                            {
                                foundVoting.Password = null;
                            }
                            else
                            {
                                foundVoting.Password = editedVoting.Password;
                            }
                            db.Votings.Update(foundVoting);
                            await db.SaveChangesAsync();
                            return RedirectToAction("MyVotings");
                        }
                        else
                        {
                            return RedirectToAction(controllerName: "Error", actionName: "VotingNotFound");
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
            else
            {
                return View(editedVoting);
            }
        }
        #endregion 

        public IActionResult MyVotings()
        {
            if (signInManager.IsSignedIn(User))
            {
                List<Voting> userVotings = db.Votings.Where(x => x.AuthorId.Equals(userManager.GetUserId(User))).ToList();
                List<VotingStatistics> userVoteAndStats = new List<VotingStatistics>();
                foreach (Voting voting in userVotings)
                {
                    userVoteAndStats.Add(new VotingStatistics(voting, db.VotingResults.Count(x => x.VotingId == voting.Id), db.VotingOptions.Count(x => x.VotingId == voting.Id)));
                }
                return View(userVoteAndStats);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "NeedToSignIn");
            }
        }

        public IActionResult List()
        {
            if (signInManager.IsSignedIn(User))
            {
                IEnumerable<Voting> publicVotings = db.Votings.Where(x => !x.IsPassworded && !x.IsClosed);
                return View(publicVotings);
            }
            else
            {
                return RedirectToAction(controllerName: "Error", actionName: "NeedToSignIn");
            }
        }
    }
}
