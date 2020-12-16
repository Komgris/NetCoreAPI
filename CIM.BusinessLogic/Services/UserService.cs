using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class UserService : BaseService, IUserService
    {
        private IUserAppTokenRepository _userAppTokenRepository;
        private ICipherService _cipherService;
        private IUserRepository _userRepository;
        private IEmployeesRepository _employeesRepository;
        private IUnitOfWorkCIM _unitOfWork;
        private IResponseCacheService _responseCacheService;
        private IConfiguration _configuration;
        private IUserGroupAppRepository _userGroupAppRepository;
        public UserService(
            IUserAppTokenRepository userAppTokenRepository,
            ICipherService cipherService,
            IUserRepository userRepository,
            IEmployeesRepository employeesRepository,
            IUnitOfWorkCIM unitOfWork,
            IResponseCacheService responseCacheService,
            IConfiguration configuration,
            IUserGroupAppRepository userGroupAppRepository
            )
        {
            _userAppTokenRepository = userAppTokenRepository;
            _cipherService = cipherService;
            _userRepository = userRepository;
            _employeesRepository = employeesRepository;
            _unitOfWork = unitOfWork;
            _responseCacheService = responseCacheService;
            _configuration = configuration;
            _userGroupAppRepository = userGroupAppRepository;
        }
        public async Task<string> Create(AdminUsersModel model)
        {
            var dbModel = new Users
            {
                CreatedAt = DateTime.Now,
                CreatedBy = CurrentUser.UserId,
                IsActive = true,
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName.ToLower().Trim(),
                HashedPassword = HashPassword(model.Password),
                Email = model.Email,
                Name = model.Name,
                UserGroupId = 0
            };

            _userRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
            return "";
        }

        public string HashPassword(UserModel model)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(model.Password, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        public string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        public async Task<AuthModel> Auth(string username, string password)
        {
            AuthModel result = new AuthModel();
            var dbModel = await _userRepository.Where(x => x.UserName.ToLower() == username.ToLower() && x.IsActive == true)
                .Select(x => new
                {
                    UserName = x.UserName,
                    Email = x.Email,
                    Name = x.Name,
                    HashedPassword = x.HashedPassword,
                })
                .FirstOrDefaultAsync(x => x.UserName == username);

            if (dbModel != null && IsPasswordValid(dbModel.HashedPassword, password))
            {
                result.FullName = dbModel.Name;
                result.UserId = dbModel.UserName;
                result.IsSuccess = true;
                result.Email = dbModel.Email;
                //result.Token = await CreateToken(dbModel.Id);
                //result.Group = dbModel.;
                //result.Apps = dbModel.Apps.ToList();

                //await _responseCacheService.SetAsyncExpire($"{Constans.RedisKey.TOKEN}-{result.Token}", result, _configuration.GetValue<int>("TokenExpire(Min)"));
            }

            //var dbModel = new Users
            //{
            //    CreatedAt = DateTime.Now,
            //    CreatedBy = CurrentUser.UserId,
            //    IsActive = true,
            //    Id = Guid.NewGuid().ToString(),
            //    UserName = model.UserName.ToLower().Trim(),
            //    HashedPassword = HashPassword(model),
            //    Email = model.Email,
            //    UserGroupId = model.UserGroupId
            //};

            //dbModel = await _userRepository.Where(x => x.UserName.ToLower() == username.ToLower() && x.IsActive == true)
            ////    .Select(
            ////        x => new
            //        {
            //            //UserName = x.UserName,
            //            //FullName = x.Name.Select(x => x.FirstName).FirstOrDefault() + " " + x.Name.Select(x => x.LastName).FirstOrDefault(),
            //            //Id = x.Id,
            //            //HashedPassword = x.HashedPassword,
            //            //Group = x.UserGroup.Name,
            //            //Apps = x.UserGroup.UserGroupsApps.Where(x => x.App.IsActive)
            //            //.Select(app => new AppModel
            //            //{
            //            //    Id = app.App.Id,
            //            //    Name = app.App.Name,
            //            //    Url = app.App.Url
            //            //})
            //        }
            //    )
            //    .FirstOrDefaultAsync(x => x.UserName == username);

            //if (dbModel != null && IsPasswordValid(dbModel.HashedPassword, password))
            //{
            //    result.FullName = dbModel.FullName;
            //    result.UserId = dbModel.Id;
            //    result.IsSuccess = true;
            //    result.Token = await CreateToken(dbModel.Id);
            //    result.Group = dbModel.Group;
            //    result.Apps = dbModel.Apps.ToList();

            //    await _responseCacheService.SetAsyncExpire($"{Constans.RedisKey.TOKEN}-{result.Token}", result, _configuration.GetValue<int>("TokenExpire(Min)"));
            //}

            return result;
        }

        public async Task<string> CreateToken(string userId)
        {

            var existingToken = _userAppTokenRepository.Where(x => x.UserId == userId).ToList();
            foreach (var item in existingToken)
            {
                _userAppTokenRepository.Delete(item);
            }

            var userAppToken = new UserAppTokens
            {
                UserId = userId,
                ExpiredAt = DateTime.Now.AddMinutes(_configuration.GetValue<int>("TokenExpire(Min)")),
            };
            var dataString = JsonConvert.SerializeObject(userAppToken);
            userAppToken.Token = _cipherService.Encrypt(dataString);

            _userAppTokenRepository.Add(userAppToken);
            await _unitOfWork.CommitAsync();
            return userAppToken.Token;
        }

        public bool IsPasswordValid(string savedPasswordHash, string password)
        {
            var isValid = true;

            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    isValid = false;

            return isValid;
        }

        public async Task<ProcessReponseModel<object>> VerifyTokenWithApp(string token, int appId)
        {
            var output = new ProcessReponseModel<object>();
            var authModel = await _responseCacheService.GetAsTypeAsync<AuthModel>($"{Constans.RedisKey.TOKEN}-{token}");
            if (authModel != null)
            {
                var userGroupApp = authModel.Apps.Where(x => x.Id == appId);
                if (userGroupApp != null)
                {
                    await _responseCacheService.SetAsyncExpire($"{Constans.RedisKey.TOKEN}-{token}", authModel
                        , _configuration.GetValue<int>("TokenExpire(Min)"));
                    output.IsSuccess = true;
                }
                else
                {
                    output.Message = "Unauthorized";
                }
            }
            else
            {
                output.Message = "Unauthorized";
            }

            return output;
        }

        public async Task<ProcessReponseModel<object>> VerifyToken(string token)
        {
            var output = new ProcessReponseModel<object>();

            var authModel = await _responseCacheService.GetAsTypeAsync<AuthModel>($"{Constans.RedisKey.TOKEN}-{token}");
            if (authModel != null)
            {
                await _responseCacheService.SetAsyncExpire($"{Constans.RedisKey.TOKEN}-{token}", authModel
                    , _configuration.GetValue<int>("TokenExpire(Min)"));

                output.IsSuccess = true;
            }
            else
            {
                output.Message = "Unauthorized";
            }

            return output;
        }

        public async Task Logout(string token)
        {
            await _responseCacheService.RemoveAsync($"{Constans.RedisKey.TOKEN}-{token}");
        }

        public async Task<bool> GetCurrentUserModel(string token, int appId)
        {
            var authModel = await _responseCacheService.GetAsTypeAsync<AuthModel>($"{Constans.RedisKey.TOKEN}-{token}");
            if (authModel != null)
            {
                var userGroupApp = authModel.Apps.Where(x => x.Id == appId);
                if (userGroupApp != null)
                {
                    CurrentUserId = authModel.UserId;
                    IsVerifyTokenPass = true;
                    
                    await _responseCacheService.SetAsyncExpire($"{Constans.RedisKey.TOKEN}-{token}", authModel
                        , _configuration.GetValue<int>("TokenExpire(Min)"));                    
                }
                else
                {
                    CurrentUserId = authModel.UserId;
                    IsVerifyTokenPass = false;
                }
            }
            else
            {
                CurrentUserId = _configuration.GetValue<string>("DefaultUserId");
                IsVerifyTokenPass = false;
            }

            return IsVerifyTokenPass;
        }

        public async Task<PagingModel<UserModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            var output = await _userRepository.ListAsPaging("sp_ListUser", new Dictionary<string, object>()
                {
                    {"@keyword", keyword},
                    {"@howmany", howMany},
                    {"@page", page},
                    {"@is_active", isActive}
                }, page, howMany);
            return output;
        }

        public async Task<UserModel> Get(string id)
        {
            var db_model = await _userRepository.Where(x => x.Id == id)
                .Select(x => new UserModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    OldPassword = x.HashedPassword,
                    Email = x.Email,
                    UserGroupId = x.UserGroupId,
                    //DefaultLanguageId = x.DefaultLanguageId,
                    IsSuspend = x.IsSuspend,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDelete,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    UpdatedAt = x.UpdatedAt,
                    UpdatedBy = x.UpdatedBy
                }).FirstOrDefaultAsync();

            var employeedb_model = _employeesRepository.Where(x => x.UserId == id).FirstOrDefault();
            if (employeedb_model != null)
            {
                db_model.EmployeeId = employeedb_model.Id;
                db_model.EmployeeNo = employeedb_model.EmNo;
                db_model.OldEmployeeNo = employeedb_model.EmNo;
            }
            return db_model;
        }

        public async Task Update(UserModel model)
        {
            var db_model = MapperHelper.AsModel(model, new Users());
            db_model.HashedPassword = model.OldPassword;
            if (!string.IsNullOrEmpty(model.Password))
            {
                if (!IsPasswordValid(model.OldPassword, model.Password))
                    db_model.HashedPassword = HashPassword(model);
            }
            db_model.UpdatedAt = DateTime.Now;
            db_model.UpdatedBy = CurrentUser.UserId;
            _userRepository.Edit(db_model);

            if (model.EmployeeNo != model.OldEmployeeNo)
            {
                var oldEmployeedb_model = _employeesRepository.Where(x => x.EmNo == model.OldEmployeeNo).FirstOrDefault();
                if (oldEmployeedb_model != null)
                {
                    oldEmployeedb_model.UserId = null;
                    oldEmployeedb_model.UpdatedAt = DateTime.Now;
                    oldEmployeedb_model.UpdatedBy = CurrentUser.UserId;
                    _employeesRepository.Edit(oldEmployeedb_model);
                }

                var employeedb_model = _employeesRepository.Where(x => x.EmNo == model.EmployeeNo).FirstOrDefault();
                if (employeedb_model != null)
                {
                    employeedb_model.UserId = model.Id;
                    employeedb_model.UpdatedAt = DateTime.Now;
                    employeedb_model.UpdatedBy = CurrentUser.UserId;
                    _employeesRepository.Edit(employeedb_model);
                }
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<UserModel> GetFromUserName(string userName)
        {
            return await _userRepository.Where(x => x.UserName == userName.ToLower().Trim())
                .Select(x => new UserModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    IsActive = x.IsActive,
                    IsDelete = x.IsDelete
                }).FirstOrDefaultAsync();
        }
    }
}
