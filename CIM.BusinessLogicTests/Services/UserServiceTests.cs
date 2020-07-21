using Microsoft.VisualStudio.TestTools.UnitTesting;
using CIM.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using Moq;
using CIM.DAL.Interfaces;
using CIM.Model;
using CIM.BusinessLogic.Interfaces;

namespace CIM.BusinessLogic.Services.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void HashPasswordTest()
        {
            var userAppTokenRepository = new Mock<IUserAppTokenRepository>().Object;
            var cipherService = new Mock<ICipherService>().Object;
            var userRep = new Mock<IUserRepository>().Object;
            var employee = new Mock<IEmployeesRepository>().Object;
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var service = new UserService(
                userAppTokenRepository,
                cipherService, 
                userRep,
                employee,
                unitOfWork);
            var registerUserModel = new UserModel
            {
                Password = "password"
            };
            var hashedPassword = service.HashPassword(registerUserModel);
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(registerUserModel.Password);

            var validatedPasswordResult = service.IsPasswordValid(hashedPassword, registerUserModel.Password);
            validatedPasswordResult.Should().BeTrue();

            var invalidatedPasswordResult = service.IsPasswordValid(hashedPassword, "WrongPassword");
            invalidatedPasswordResult.Should().BeFalse();

        }
    }
}