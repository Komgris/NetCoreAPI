using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
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
        private IUnitOfWorkCIM _unitOfWork;

        public UserService(
            IUserAppTokenRepository userAppTokenRepository,
            ICipherService cipherService,
            IUserRepository userRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _userAppTokenRepository = userAppTokenRepository;
            _cipherService = cipherService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public async void Register(RegisterUserModel model)
        {
            var dbModel = new Users
            {
                CreatedAt = DateTime.Now,
                CreatedBy = CurrentUser.UserId,
                IsActive = true,
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                HashedPassword = HashPassword(model),
                Email = model.Email,
                UserGroupId = model.UserGroupId,
                DefaultLanguageId = model.LanguageId
            };
            dbModel.UserProfiles.Add(new UserProfiles
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Image = model.Image,
            });
            _userRepository.Add(dbModel);
            await _unitOfWork.CommitAsync();
        }

        public string HashPassword(RegisterUserModel model)
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

        public async Task<AuthModel> Auth(string username, string password)
        {   
            AuthModel result = new AuthModel();
            var dbModel = await _userRepository.Where(x => x.UserName == username)
                .Select(
                    x=> new
                    {
                        UserName = x.UserName,
                        FullName = x.UserProfiles.Select(x=>x.FirstName).FirstOrDefault() + " " + x.UserProfiles.Select(x => x.LastName).FirstOrDefault(),
                        Id = x.Id,
                        HashedPassword = x.HashedPassword,
                        Group = x.UserGroup.Name,
                        Apps = x.UserGroup.UserGroupsApps.Where(x=>x.App.IsActive)
                        .Select(app => new AppModel
                        {
                            Name = app.App.Name,
                            Url = app.App.Url
                        })
                    }
                )
                .FirstOrDefaultAsync(x => x.UserName == username);

            if (dbModel != null && IsPasswordValid(dbModel.HashedPassword, password))
            {

                result.FullName = dbModel.FullName;
                result.UserId = dbModel.Id;
                result.IsSuccess = true;
                result.Token = await CreateToken(dbModel.Id);
                result.Group = dbModel.Group;
                result.Apps = dbModel.Apps.ToList();
                
            }
            return result;
        }

        public async Task<string> CreateToken(string userId)
        {

            var existingToken = _userAppTokenRepository.Where(x => x.UserId == userId).ToList();
            foreach(var item in existingToken)
            {
                _userAppTokenRepository.Delete(item);
            }

            var userAppToken = new UserAppTokens
            {
                UserId = userId,
                ExpiredAt = DateTime.Now.AddYears(1),
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

        public CurrentUserModel GetCurrentUserModel(string token)
        {
            var decryptedData = _cipherService.Decrypt(token);
            var userAppToken = JsonConvert.DeserializeObject<UserAppTokens>(decryptedData);
            var user = _userRepository.Where(x => x.Id == userAppToken.UserId)
                .Select(x => new {
                    Token = x.UserAppTokens.Token,
                    DefaultLanguageId = x.DefaultLanguageId,
                }).FirstOrDefault();
            var dbData = _cipherService.Decrypt(user.Token);
            var tokenData = JsonConvert.DeserializeObject<UserAppTokens>(dbData);
            var currentUserModel = new CurrentUserModel
            {
                IsValid = user != null && tokenData.UserId == userAppToken.UserId,
                UserId = tokenData.UserId,
                LanguageId = user.DefaultLanguageId
            };
            return currentUserModel;
        }
    }
}
