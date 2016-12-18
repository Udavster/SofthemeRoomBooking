using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SofthemeRoomBooking.Converters;
using SofthemeRoomBooking.Models;
using SofthemeRoomBooking.Models.UserViewModels;
using SofthemeRoomBooking.Services.Contracts;

namespace SofthemeRoomBooking.Services.Implementation
{
    public class ProfileService : IProfileService, IDisposable
    {
        private SignInManager<ApplicationUser, string> _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _dbContext;

        private bool _disposed;

        public ProfileService(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationDbContext dbContext)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public bool IsAdmin(string userId)
        {         
            if (!string.IsNullOrEmpty(userId))
            {
                var roles = _userManager.GetRoles(userId);

                return roles.Count != 0;
            }

            return false;
        }

        public ApplicationUser GetUserById(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return _userManager.FindById(userId);
            }

            return null;
        }

        public EditUserViewModel GetEditUserViewModelById(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = GetUserById(userId);
                if (user != null)
                {
                    return user.ToEditUserViewModel(_userManager);
                }
            }

            return null;
        }

        public LayoutUserViewModel GetLayoutUserViewModelById(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = GetUserById(userId);
                if (user != null)
                {
                    return user.ToLayoutUserViewModel();
                }
            }

            return null;
        }

        public ProfileUserViewModel GetProfileUserViewModelById(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = GetUserById(userId);
                if (user != null)
                {
                    return user.ToProfileUserViewModel(_userManager);
                }
            }

            return null;
        }

        public IQueryable<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users;
        }

        public IQueryable<string> GetAllAdminsEmails()
        {
            var role = _dbContext.Roles.SingleOrDefault(m => m.Name == "Admin");
            var usersInRoleEmails = _dbContext.Users.Where(m => m.Roles.Any(r => r.RoleId == role.Id)).Select(x => x.Email);
            return usersInRoleEmails;
        }

        public IQueryable<ApplicationUser> GetUsersByNameOrEmail(string searchString)
        {
            var searchPattern = searchString.ToLowerInvariant()
                                            .Trim()
                                            .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var regex = string.Format(CultureInfo.InvariantCulture, "({0})", string.Join("|", searchPattern));

            IQueryable<ApplicationUser> users = null;
            foreach (var pattern in searchPattern)
            {
                var request = _userManager.Users.Where(model => model.Name.Contains(pattern) ||
                                                                model.Surname.Contains(pattern) ||
                                                                model.Email.Contains(pattern))
                                                .Select(model => model);

                if (users == null)
                {
                    users = request;
                }
                else
                {
                    users = users.Union(request);
                }
            }

            return users;
        }

        public PageableUsersViewModel GetAllUsersByPage(int page, int itemsOnPage)
        {
            return new PageableUsersViewModel(GetAllUsers(), page, itemsOnPage);
        }

        public PageableUsersViewModel GetUsersByNameOrEmailByPage(string searchPattern, int page, int itemsOnPage)
        {
            return new PageableUsersViewModel(GetUsersByNameOrEmail(searchPattern), page, itemsOnPage);
        }

        public async Task<bool> Edit(EditUserViewModel model)
        {
            if (model == null)
            {
                return false;
            }

            var user = GetUserById(model.Id);
            if (user != null)
            {
                user = model.ToApplicationUser(user);

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var isAdmin = IsAdmin(user.Id);

                    if (!isAdmin && model.AdminRole)
                    {
                        await _userManager.AddToRoleAsync(user.Id, "Admin");
                    }

                    if (isAdmin && !model.AdminRole)
                    {
                        await _userManager.RemoveFromRoleAsync(user.Id, "Admin");
                    }

                    return true;
                }
            }
            return false;
        }

        public async Task<bool> Delete(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = GetUserById(userId);
                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return true;
                    }
                }
            }          
            return false;
        }

        public async Task<bool> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            if (model == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(HttpContext.Current.User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(HttpContext.Current.User.Identity.GetUserId());
                if (user != null)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    return true;
                }
            }

            return false;
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_userManager != null)
                    {
                        _userManager.Dispose();
                        _userManager = null;
                    }

                    if (_signInManager != null)
                    {
                        _signInManager.Dispose();
                        _signInManager = null;
                    }
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}