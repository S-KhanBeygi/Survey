using DaraSurvey.Entities;
using DaraSurvey.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DaraSurvey.Interfaces
{
    public interface IAccountService
    {
        Task ChangePasswordAsync(string userId, ChangePasswordReq model);
        Task ForgotPasswordAsync(ForgotPasswordReq model, HttpRequest request);
        Task<LoginRes> LoginAsync(LoginReq model);
        Task<RegisterRes> RegisterAsync(RegisterReq model);
        Task LogoutAsync();
        Task ResetPasswordAsync(ResetPasswordVmIn model);
        public User GetProfile(string id);
    }
}