using AutoMapper;
using DaraSurvey.Core;
using DaraSurvey.Entities;
using DaraSurvey.Extentions;
using DaraSurvey.Interfaces;
using DaraSurvey.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DaraSurvey.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IRoleService _roleService;
        private readonly DB _db;

        public UserService(IMapper mapper, UserManager<User> userManager, IRoleService roleService, DB db)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleService = roleService;
            _db = db;
        }

        // --------------------

        public IEnumerable<User> GetAll(UserOrderedFilter model) => Filter(model).GetOrderedQuery(model);

        // --------------------

        public long Count(UserFilter model) => Filter(model).Count();

        // --------------------

        public User GetById(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(o => o.Id == userId);
            if (user == null)
                throw new ServiceException(HttpStatusCode.ExpectationFailed, ServiceExceptionCode.RequestNotFound);

            return user;
        }

        // --------------------

        public User GetByUserName(string userName)
        {
            var user = _userManager.Users.SingleOrDefault(o => o.UserName == userName);
            if (user == null)
                throw new ServiceException(HttpStatusCode.ExpectationFailed, ServiceExceptionCode.RequestNotFound);
            return user;
        }

        // --------------------

        public async Task<User> CreateAsync(UserUpdateModel model)
        {
            var user = _mapper.Map<User>(model);
            user.Created = DateTime.UtcNow;

            var profile = user.Profile;
            profile.Created = DateTime.UtcNow;

            user.Profile = null;

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                throw new ServiceException(HttpStatusCode.ExpectationFailed, ServiceExceptionCode.CreateUserFailed, result.GetErrorMessage());

            profile.Id = user.Id;
            _db.Set<Entities.Profile>().Add(profile);
            _db.SaveChanges();

            user.Profile = profile;

            return user;
        }

        // --------------------

        public async Task<User> UpdateAsync(string id, UserUpdateModel model)
        {
            var user = _userManager.Users.SingleOrDefault(o => o.Id == id);
            if (user == null)
                throw new ServiceException(HttpStatusCode.NotFound, ServiceExceptionCode.RequestNotFound);

            user = _mapper.Map<User>(model);
            user.Id = id;
            user.Updated = DateTime.UtcNow;

            await UpdateAsync(user);

            if (!string.IsNullOrEmpty(model.Password))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetPassResult = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);
                if (!resetPassResult.Succeeded)
                    throw new ServiceException(HttpStatusCode.ExpectationFailed, ServiceExceptionCode.UpdateUserPasswordFailed);
            }

            return user;
        }

        // --------------------

        public async Task UpdateAsync(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new ServiceException(HttpStatusCode.ExpectationFailed, ServiceExceptionCode.UpdateUserFailed, result.GetErrorMessage());
        }

        // --------------------

        public async Task DeleteAsync(string id)
        {
            var user = GetById(id);

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new ServiceException(HttpStatusCode.ExpectationFailed, ServiceExceptionCode.DeleteUserFailed);
        }

        // --------------------

        public async Task<IEnumerable<string>> GetRolesAsync(string id)
        {
            return await _roleService.GetUserRolesAsync(id);
        }

        // --------------------

        public async Task AddRolesToUserAsync(string id, IEnumerable<string> roles)
        {
            var userE = GetById(id);

            // Remove previous roles
            var previousRoles = await GetRolesAsync(id);
            if (previousRoles.Any())
                await _roleService.RemoveRolesFromUserByUserIdAsync(id, previousRoles);

            // Add new roles
            if (roles != null && roles.Any())
                await _roleService.AddToRolesAsync(id, roles);
        }

        // *********************************************************************************** //
        //                                  Helper Methods                                     //
        // *********************************************************************************** //

        private IQueryable<User> Filter(UserFilter model)
        {
            var q = _userManager.Users
                .Include(o => o.Profile)
                .AsQueryable();

            if (!string.IsNullOrEmpty(model.Id))
                q = q.Where(o => o.Id == model.Id);

            if (!string.IsNullOrEmpty(model.Username))
                q = q.Where(o => o.UserName.Contains(model.Username));

            if (!string.IsNullOrEmpty(model.FirstName))
                q = q.Where(o => o.FirstName.Contains(model.FirstName));

            if (!string.IsNullOrEmpty(model.LastName))
                q = q.Where(o => o.LastName.Contains(model.LastName));

            return q;
        }

        // ------------------------

        //    public async Task<OperationResult> UpdateProfileAsync(string id, userProfileIncoming dto)
        //    {
        //        var user = _userManager.Users.Include(o => o.Profile).FirstOrDefault(o => o.Id == id);
        //        if (user == null)
        //            throw new ServiceException(HttpStatusCode.ExpectationFailed);

        //        _mapper.Update(dto.User, user);
        //        _mapper.Update(dto.Profile, user.Profile);
        //        _db.Entry(user.Profile).State = EntityState.Modified;
        //        _db.SaveChanges();

        //        var userResult = await _userManager.UpdateAsync(user);
        //        if (!userResult.Succeeded) return OperationResult.Failed(userResult);

        //        var userProfileOutgoing = new userProfileOutgoing();
        //        userProfileOutgoing.User = _mapper.Create<User, userOutgoing>(user);
        //        userProfileOutgoing.Profile = _mapper.Create<Profile, profileOutgoing>(user.Profile);

        //        return OperationResult.Succceeded(userProfileOutgoing);
        //    }
    }
}
