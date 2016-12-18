using SofthemeRoomBooking.Models;
using System.Linq;
using System.Threading.Tasks;
using SofthemeRoomBooking.Models.UserViewModels;

namespace SofthemeRoomBooking.Services.Contracts
{
    public interface IProfileService
    {
        bool IsAdmin(string userId);

        ApplicationUser GetUserById(string userId);

        EditUserViewModel GetEditUserViewModelById(string userId);

        LayoutUserViewModel GetLayoutUserViewModelById(string userId);

        ProfileUserViewModel GetProfileUserViewModelById(string userId);

        IQueryable<ApplicationUser> GetAllUsers();

        IQueryable<ApplicationUser> GetUsersByNameOrEmail(string searchString);

        PageableUsersViewModel GetAllUsersByPage(int page, int itemsOnPage);

        PageableUsersViewModel GetUsersByNameOrEmailByPage(string searchPattern, int page, int itemsOnPage);

        Task<bool> ChangePasswordAsync(ChangePasswordViewModel model);

        Task<bool> Edit(EditUserViewModel model);
        
        Task<bool> Delete(string userId);

        IQueryable<string> GetAllAdminsEmails();
    }
}
