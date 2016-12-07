using System.Collections.Generic;
using System.Linq;

namespace SofthemeRoomBooking.Models.UserViewModels
{
    public class PageableUsersViewModel
    {
        private const int ItemsOnPageDefault = 20;

        public IEnumerable<ProfileUserViewModel> List { get; set; }

        public int PageNumber { get; set; }

        public int CountPage { get; set; }

        public int ItemsOnPage { get; set; }

        public int ItemsCount { get; set; }

        public PageableUsersViewModel(IQueryable<ApplicationUser> queryableSet, int pageNumber, int itemsOnPage = 0)
        {
            ItemsCount = queryableSet.Count();

            if (itemsOnPage == 0 || itemsOnPage > ItemsCount)
            {
                itemsOnPage = ItemsOnPageDefault;
            }

            ItemsOnPage = itemsOnPage;
            PageNumber = pageNumber;
            CountPage = ItemsCount % itemsOnPage == 0 ? ItemsCount / itemsOnPage : ItemsCount / itemsOnPage + 1;

            List = queryableSet.Select(model => model)
                               .OrderBy(model => model.Email)
                               .Skip((PageNumber - 1) * ItemsOnPage)
                               .Take(itemsOnPage)
                               .AsEnumerable()
                               .Select(model => new ProfileUserViewModel
                               {
                                   Id = model.Id,
                                   UserName = $"{model.Name} {model.Surname}",
                                   Email = model.Email,
                                   AdminRole = model.Roles.Count != 0
                               });
        }
    }
}