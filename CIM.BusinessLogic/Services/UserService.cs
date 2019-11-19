using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace CIM.BusinessLogic.Services
{
    public class UserService : BaseService, IUserService
    {
        private ICipherService _cipherService;
        private IUserRepository _userRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public UserService(
            ICipherService cipherService,
            IUserRepository userRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _cipherService = cipherService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        public void Register(RegisterUserModel model)
        {
            var dbModel = new Users
            {
                CreatedAt = DateTime.Now,
                CreatedBy = CurrentUserId,
                IsActive = true,
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                HashedPassword = HashPassword(model),
                Email = model.Email,
                UserGroupId = model.UserGroup_Id,
            };
            dbModel.UserProfiles.Add(new UserProfiles
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Image = model.Image,
            });
            _userRepository.Add(dbModel);
            _unitOfWork.Commit();
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

        public AuthModel Auth(string username, string password)
        {   
            AuthModel result = new AuthModel();
            var dbModel = _userRepository.Where(x=>x.UserName == username)
                .Select(
                    x=> new
                    {
                        FullName = x.UserProfiles.Select(x=>x.FirstName).FirstOrDefault() + " " + x.UserProfiles.Select(x => x.LastName).FirstOrDefault(),
                        Id = x.Id,
                        HashedPassword = x.HashedPassword,
                        Group = x.UserGroup.UserGroupLocal.Where(x=>x.LanguageId == CurrentLanguage)
                        .Select(x=>x.Name).FirstOrDefault(),
                        Apps = x.UserGroup.UserGroupsApps.Where(x=>x.App.IsActive)
                        .Select(app => new AppModel
                        {
                            Name = app.App.Name,
                            Url = app.App.Url
                        })
                    }
                )
                .FirstOrDefault();

            if (dbModel != null && IsPasswordValid(dbModel.HashedPassword, password))
            {
                result.FullName = dbModel.FullName;
                result.UserId = dbModel.Id;
                result.IsSuccess = true;
                result.Group = dbModel.Group;
                result.Apps = dbModel.Apps.ToList();
                
            }
            return result;
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

        public bool ValidateToken(string token)
        {
            var decryptedData = _cipherService.Decrypt(token);
            var userAppToken = JsonConvert.DeserializeObject<UserAppTokens>(decryptedData);
            var user = _userRepository.Where(x => x.Id == userAppToken.UserId)
                .Select(x => x.UserAppTokens).FirstOrDefault();
            var dbData = _cipherService.Decrypt(user.Token);
            var tokenData = JsonConvert.DeserializeObject<UserAppTokens>(dbData);

            return user != null && tokenData.UserId == userAppToken.UserId;
        }
    }
}
