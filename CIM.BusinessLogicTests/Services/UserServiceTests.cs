using Microsoft.VisualStudio.TestTools.UnitTesting;
using CIM.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using CIM.DAL.Interfaces;
using CIM.Model;
using FluentAssertions;
using CIM.BusinessLogic.Interfaces;

namespace CIM.BusinessLogic.Services.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void HashPasswordTest()
        {
            var cipherService = new Mock<ICipherService>().Object;
            var userRep = new Mock<IUserRepository>().Object;
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var service = new UserService(cipherService, userRep, unitOfWork);
            var registerUserModel = new RegisterUserModel
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