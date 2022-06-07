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

        [TestMethod]
        public void When_agreement_exists_product_price_is_reduced_by_10_percent()
        {
            //Arrange
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel{BasePrice = 10000}
            };

            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>
                { 
                    new Agreement
                     {
                          AgreementRows = new List<AgreementRow>
                         {
                    
                              new AgreementRow
                              {
                                  PercentageDiscount = 10.0m
                              }
    
                          }

                     }

                }
            };

            


            //act
            var products = _sut.CalculatePrices(productList, customerContext);



            //Assert

            Assert.AreEqual(9000, products.First().Price);
        }

        [TestMethod]
        public void When_CategoryMatch_has_Van_product_price_is_reduced_by_5_percent()
        {
            //Arrange
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel{BasePrice = 10000}
            };

            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>
                {
                    new Agreement
                    {
                        AgreementRows = new List<AgreementRow>
                        {

                            new AgreementRow
                            {

                                CategoryMatch = "Van",
                                PercentageDiscount = 5.0m
                                
                            }

                        }

                    }

                }
            };




            //act
            var products = _sut.CalculatePrices(productList, customerContext);



            //Assert

            Assert.AreEqual(9500, products.First().Price);
        }
    }
}
