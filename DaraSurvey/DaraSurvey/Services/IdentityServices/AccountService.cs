using AutoMapper;
using DaraSurvey.BaseClasses;
using DaraSurvey.Core;
using DaraSurvey.Entities;
using DaraSurvey.Interfaces;
using DaraSurvey.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Profile = DaraSurvey.Entities.Profile;

namespace DaraSurvey.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly IRoleService _roleService;
        private readonly UserManager<User> _userManager;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public AccountService(
            IUserService userService,
            SignInManager<User> signInManager,
            IRoleService roleService,
            UserManager<User> userManager,
            IOptionsSnapshot<AppSettings> appSettings,
            IMapper mapper)

        {
            _userService = userService;
            _signInManager = signInManager;
            _roleService = roleService;
            _userManager = userManager;
            _appSettings = appSettings.Value;
            _mapper = mapper;
        }

        // --------------------

        public async Task<LoginRes> LoginAsync(LoginReq model)
        {
            var user = _userService.GetByUserName(model.username);

            await DoLoginChecking(user);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.password, false);

            if (!signInResult.Succeeded)
                throw new ServiceException(HttpStatusCode.Unauthorized, ServiceExceptionCode.LoginFailed);

            return await getLoginResult(user);
        }

        // ----------------------

        public async Task<RegisterRes> RegisterAsync(RegisterReq model)
        {
            var userE = _mapper.Map<User>(model.User);
            var profileE = _mapper.Map<Profile>(model.Profile);
            userE.Profile = profileE ?? new Profile();
            userE.Created = DateTime.UtcNow;
            userE.PhoneNumberConfirmed = true;
            userE.EmailConfirmed = true;

            var createUserResult = await _userManager.CreateAsync(userE, model.Password);

            if (createUserResult.Succeeded)
            {
                await _signInManager.SignInAsync(userE, isPersistent: false);

                return new RegisterRes
                {
                    Token = GetAccessToken(userE, new string[0]),
                    Id = userE.Id
                };
            }

            if (createUserResult.Errors.First().Code == "DuplicateUserName")
                throw new ServiceException(HttpStatusCode.BadRequest, ServiceExceptionCode.DuplicateUserName);

            return null;
        }

        // ----------------------

        public async Task LogoutAsync()
        {
            try { await _signInManager.SignOutAsync(); }
            catch { }
        }

        // ----------------------------

        public async Task ForgotPasswordAsync(ForgotPasswordReq model, HttpRequest request)
        {
            var user = await _userManager.FindByEmailAsync(model.email);
            if (user == null)
                throw new ServiceException(HttpStatusCode.NotFound, ServiceExceptionCode.RequestNotFound);

            await SendForgotPasswordMailAsync(user, request);
        }

        // ----------------------------

        public async Task ResetPasswordAsync(ResetPasswordVmIn model)
        {
            var user = await _userManager.FindByIdAsync(model.userId);
            if (user == null)
                throw new ServiceException(HttpStatusCode.NotFound, ServiceExceptionCode.RequestNotFound);

            var result = await _userManager.ResetPasswordAsync(user, model.token, model.newPassword);
            if (!result.Succeeded)
                throw new ServiceException(HttpStatusCode.Unauthorized, ServiceExceptionCode.InvalidToken);
        }

        // ----------------------------

        public async Task ChangePasswordAsync(string userId, ChangePasswordReq model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new ServiceException(HttpStatusCode.NotFound, ServiceExceptionCode.RequestNotFound);

            /* Try change password */
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            /*  Current passwort not mached with old password */
            if (result.Errors.Any(o => o.Code == "PasswordMismatch"))
                throw new ServiceException(HttpStatusCode.Unauthorized, ServiceExceptionCode.InvalidPassword);

            if (!result.Succeeded)
                throw new ServiceException(HttpStatusCode.ExpectationFailed, ServiceExceptionCode.OperationFailed);
        }

        // *********************************************************************** //
        //                            Helper Methods                               // 
        // *********************************************************************** //

        private async Task DoLoginChecking(User user)
        {
            // requires two factor authentication
            if (await _userManager.GetTwoFactorEnabledAsync(user))
                throw new ServiceException(HttpStatusCode.Unauthorized, ServiceExceptionCode.RequiresTwoFactor);

            /* is not allowed */
            if (!await _signInManager.CanSignInAsync(user))
            {
                if (_userManager.Options.SignIn.RequireConfirmedPhoneNumber && !await _userManager.IsPhoneNumberConfirmedAsync(user))
                    throw new ServiceException(HttpStatusCode.Unauthorized, ServiceExceptionCode.PhoneNumberNotVerified);

                if (_userManager.Options.SignIn.RequireConfirmedEmail && !await _userManager.IsEmailConfirmedAsync(user))
                    throw new ServiceException(HttpStatusCode.Unauthorized, ServiceExceptionCode.EmailNotVerified);
            }

            /* User account locked out */
            if ((_userManager.SupportsUserLockout && await _userManager.IsLockedOutAsync(user)))
                throw new ServiceException(HttpStatusCode.Unauthorized, ServiceExceptionCode.IsLockedOut);
        }

        // --------------------

        private async Task<LoginRes> getLoginResult(User user)
        {
            // reset lockout counts, if necessary
            if (_userManager.SupportsUserLockout)
                await _userManager.ResetAccessFailedCountAsync(user);

            var userRoles = await _roleService.GetUserRolesAsync(user.Id);

            return new LoginRes
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = userRoles,
                CountryCode = user.CountryCode,
                PhoneNumber = user.PhoneNumber,
                Token = GetAccessToken(user, userRoles),
                TokenTtl = _appSettings.IdentitySettings.Jwt.AccessTokenExpireMins,
            };
        }

        // --------------------

        private string GetAccessToken(User user, IEnumerable<string> roles)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.IdentitySettings.Jwt.SecurityKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            claims.Add(new Claim(IdentityClaimTypes.Name, user.UserName));

            claims.Add(new Claim(IdentityClaimTypes.NameIdentifier, user.Id));

            foreach (var role in roles)
                claims.Add(new Claim(IdentityClaimTypes.Role, role));

            var tokenOption = new JwtSecurityToken(
                issuer: _appSettings.Hosts.Api,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_appSettings.IdentitySettings.Jwt.AccessTokenExpireMins),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenOption);
        }

        // ----------------------

        private async Task SendForgotPasswordMailAsync(User user, HttpRequest request)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            /* Encoding values for send to query string */
            var enUserId = WebUtility.UrlEncode(user.Id);
            var enToken = WebUtility.UrlEncode(token);
            /*------------------------------------------ */

            var callBackUrl = string.Concat("_hosts.Web", $"/reset-password?userId={enUserId}&token={enToken}");

            // SendEmail
        }

        // ----------------------

        public User GetProfile(string id)
        {
            var user = _userManager.Users.Include(o => o.Profile).FirstOrDefault(o => o.Id == id);
            if (user == null)
                throw new ServiceException(HttpStatusCode.ExpectationFailed, ServiceExceptionCode.RequestNotFound);

            return user;
        }
    }
}
