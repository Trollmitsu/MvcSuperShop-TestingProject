using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Services;

namespace MvcSuperShop.Tests.Services
{
    [TestClass]
    public class CategoryServiceTests
    {
        private readonly ApplicationDbContext _context;

        private CategoryService _sut;

        public CategoryServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "test")
                .Options;
            _context = new ApplicationDbContext(options);
            _sut = new CategoryService(_context);
        }

        [TestMethod]
        public void TrendingCategory_returns_asked_for_amount_of_Categories()
        {
            //Arrange

            var category1 = new Category() { Id = 4, Name = "Electric", Icon = "Nice" };

            var category2 = new Category() { Id = 5, Name = "Automated", Icon = "Perfekt" };

            var category3 = new Category() { Id = 6, Name = "Hybrid", Icon = "Good" };

            _context.Categories.Add(category1);
            _context.Categories.Add(category2);
            _context.Categories.Add(category3);
            
            _context.SaveChanges();

            //Act
            var result = _sut.GetTrendingCategories(3);

            //Assert
            Assert.AreEqual(3, result.Count());
        }
    }
}
