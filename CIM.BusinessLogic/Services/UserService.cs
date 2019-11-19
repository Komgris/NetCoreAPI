using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace CIM.BusinessLogic.Services
{
    public class UserService : BaseService, IUserService
    {
        private IUserRepository _userRepository;
        private IUnitOfWorkCIM _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
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
            AuthModel result = null;
            var dbModel = _userRepository.Where(x=>x.UserName == username)
                .Select(
                    x=> new
                    {
                        FullName = x.UserProfiles.Select(x=>x.FirstName).FirstOrDefault() + " " + x.UserProfiles.Select(x => x.LastName).FirstOrDefault(),
                        Id = x.Id,
                        HashedPassword = x.HashedPassword,
                    }
                )
                .First();
            if (IsPasswordValid(dbModel.HashedPassword, password))
            {
                result = new AuthModel
                {
                    FullName = dbModel.FullName,
                    UserId = dbModel.Id,
                };
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
    }
}
