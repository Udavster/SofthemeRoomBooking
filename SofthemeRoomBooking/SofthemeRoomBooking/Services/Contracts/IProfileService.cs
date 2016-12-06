using SofthemeRoomBooking.Models;
using System.Collections.Generic;
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

        List<ApplicationUser> GetAllUsers();

        List<ProfileUserViewModel> GetAllProfileUserViewModels();

        Task<bool> ChangePasswordAsync(ChangePasswordViewModel model);

        Task<bool> Edit(EditUserViewModel model);
        
        Task<bool> Delete(string userId);
    }
}
