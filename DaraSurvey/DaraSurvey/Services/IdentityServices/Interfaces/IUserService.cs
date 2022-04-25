using DaraSurvey.Entities;
using DaraSurvey.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaraSurvey.Interfaces
{
    public interface IUserService
    {
        Task AddRolesToUserAsync(string id, IEnumerable<string> roles);

        IEnumerable<User> GetAll(UserOrderedFilter model);
        long Count(UserFilter model);

        Task<User> CreateAsync(UserUpdateModel model);

        User GetById(string userId);

        User GetByUserName(string userName);

        Task<IEnumerable<string>> GetRolesAsync(string id);

        Task<User> UpdateAsync(string id, UserUpdateModel model);
        Task UpdateAsync(User user);

        Task DeleteAsync(string id);
    }
}