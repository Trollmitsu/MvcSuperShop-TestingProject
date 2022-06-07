using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;

namespace MvcSuperShop.Tests.Services
{
    [TestClass]
    public class PricingServiceTests
    {
        private PricingService _sut;

        [TestInitialize]
        public void Initialize()
        {
            _sut = new PricingService();
        }

        [TestMethod]
        public void When_no_agreement_exists_product_baseprice_is_used()
        {
            //Arrange
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel{BasePrice = 399999}
            };

            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>()
            };


            //act
            var products = _sut.CalculatePrices(productList, customerContext);


            //Assert

            Assert.AreEqual(399999, products.First().Price);
        }
    }
}
