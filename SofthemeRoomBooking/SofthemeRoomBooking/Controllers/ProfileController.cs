using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SofthemeRoomBooking.Converters;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Models.UserViewModels;

namespace SofthemeRoomBooking.Controllers
{
    public class ProfileController : Controller
    {
        private readonly SignInManager<ApplicationUser, string> _signInManager;
        private readonly ApplicationUserManager _userManager;

        public ProfileController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public ActionResult Index(string userId)
        {
            var user = _userManager.FindById(userId);

            if (user != null)
            {
                var model = user.ToProfileUserViewModel(_userManager);
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Edit(string userId)
        {
            var user = _userManager.FindById(userId);
            if (user != null)
            {
                ViewBag.IsAdmin = _userManager.GetRoles(User.Identity.GetUserId()).Count != 0;

                var model = user.ToEditUserViewModel(_userManager);
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                user = model.ToApplicationUser(user);

                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    //_dbContext.SaveChanges();
                    return RedirectToAction("Index", "Profile", new { userId = user.Id });
                }

                ModelState.AddModelError("", "Что-то пошло не так");
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Delete()
        {
            //redirect to confirmation popup
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    if (userId == User.Identity.GetUserId())
                    {
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        //to Users
                        //return RedirectToAction("", "");
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "Что-то пошло не так");
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Что-то пошло не так");

                return RedirectToAction("Index", "Profile");
            }
            ModelState.AddModelError("", "Что-то пошло не так");

            return View(model);
        }
    }
}