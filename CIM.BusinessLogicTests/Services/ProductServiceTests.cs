using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using Moq;
using CIM.DAL.Interfaces;
using CIM.BusinessLogic.Services;

namespace CIM.BusinessLogicTests.Services
{
    public class ProductServiceTests
    {
        [Fact]
        public void PageTest()
        {
            var unitOfWork = new Mock<IUnitOfWorkCIM>().Object;
            var productRepository = new Mock<IProductRepository>().Object;
            var productService = new ProductService(unitOfWork, productRepository);
            var result = productService.Paging(1, 5);
            result.Should().NotBeNull();
        }
    }
}
