using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Models.UserViewModels;
using SofthemeRoomBooking.Services.Contracts;

namespace SofthemeRoomBooking.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public ActionResult Index(string userId)
        {
            var model = _profileService.GetProfileUserViewModelById(userId);

            if (model != null)
            {
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Users()
        {
            var isAdmin = _profileService.IsAdmin(User.Identity.GetUserId());

            if (!isAdmin)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UsersTable(int page = 1, string searchString = null)
        {
            var isAdmin = _profileService.IsAdmin(User.Identity.GetUserId());

            if (isAdmin)
            {
                var itemsOnPage = 20;

                var model = !string.IsNullOrWhiteSpace(searchString)
                    ? _profileService.GetUsersByNameOrEmailByPage(searchString, page, itemsOnPage)
                    : _profileService.GetAllUsersByPage(page, itemsOnPage);

                return PartialView(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Edit(string userId)
        {
            var model = _profileService.GetEditUserViewModelById(userId);

            if (model != null)
            {
                ViewBag.AdminRole = _profileService.IsAdmin(User.Identity.GetUserId());

                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            var result = await _profileService.Edit(model);

            if (!result)
            {
                ModelState.AddModelError("", "Что-то пошло не так");
                return View(model);
            }

            return RedirectToAction("Index", "Profile", new { userId = model.Id });
        }


        [HttpGet]
        public ActionResult Delete(string userId)
        {
            var user = _profileService.GetUserById(userId);

            if (user != null)
            {
                var model = new ConfirmationViewModel
                {
                    Question = "Вы уверены, что хотите удалить этот аккаунт?",
                    Message = "",
                    Action = "Delete",
                    Controller = "Profile",
                    DataId = userId
                };

                return PartialView("_PopupConfirmationPartial", model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmation(string id)
        {
            var result = await _profileService.Delete(id);

            if (result)
            {
                return id == User.Identity.GetUserId() ? RedirectToAction("Login", "Account") : RedirectToAction("Users");
            }

            ModelState.AddModelError("", "Что-то пошло не так");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ChangePassword(string userId)
        {
            var user = _profileService.GetUserById(userId);

            if (user != null)
            {
                return PartialView();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            var result = await _profileService.ChangePasswordAsync(model);

            if (result)
            {
                ViewBag.PasswordSuccessfulyChanged = true;

                return PartialView();
            }

            ModelState.AddModelError("", "Неверный старый пароль");

            return PartialView(model);
        }
    }
}