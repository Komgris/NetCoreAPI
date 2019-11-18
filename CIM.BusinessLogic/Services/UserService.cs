using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

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
            var salt = "SomeRandomSalt";
            var dbModel = new Users
            {
                CreatedAt = DateTime.Now,
                CreatedBy = CurrentUserId,
                IsActive = true,
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                HashedPassword = HashPassword(model, salt),
                Salt = salt,
                Email = model.Email,
            };
            _userRepository.Add(dbModel);
            _unitOfWork.Commit();
        }

        public string HashPassword(RegisterUserModel model, string salt)
        {
            return "hashpassword";
        }
    }
}
