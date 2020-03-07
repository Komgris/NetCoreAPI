using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using Moq;
using CIM.DAL.Interfaces;
using CIM.BusinessLogic.Services;
using CIM.Model;

namespace CIM.BusinessLogicTests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public void createTest()
        {
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var productRepository = new Mock<IProductRepository>().Object;
            var service = new ProductService(productRepository,unitOfWork);

            List<ProductModel> pomodel = new List<ProductModel>()
            {
                new ProductModel(){ Code="1111", Description="Bill"},
                new ProductModel(){ Code="2222", Description="Steve"},
                new ProductModel(){ Code="3333", Description="Ram"},
                new ProductModel(){ Code="4444", Description="Moin"}
            };

            service.Create(pomodel);
        }
    }
}
