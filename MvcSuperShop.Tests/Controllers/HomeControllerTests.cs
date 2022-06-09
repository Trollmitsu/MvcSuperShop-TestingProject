using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcSuperShop.Controllers;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;
using MvcSuperShop.ViewModels;

namespace MvcSuperShop.Tests.Controllers;

[TestClass]
public class HomeControllerTests
{
    private HomeController _sut;
    private Mock<ICategoryService> _categoryServiceMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IProductService> _productServiceMock;
    private ApplicationDbContext _context;
    


    [TestInitialize]
    public void Initialize()
    {
        _categoryServiceMock = new Mock<ICategoryService>();
        _productServiceMock = new Mock<IProductService>();
        _mapperMock = new Mock<IMapper>();
        

        var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(contextOptions);
        _context.Database.EnsureCreated();

        _sut = new HomeController(_categoryServiceMock.Object,
            _productServiceMock.Object,
            _mapperMock.Object,
            _context);
    }

    

    [TestMethod]
    public void Index_should_show_3_trending_categories()
    {
        //arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, "Danish@hotmail.com")
        }, "testAuthentication"));

        _sut.ControllerContext = new ControllerContext();
        _sut.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = user
        };

        _categoryServiceMock.Setup(e => e.GetTrendingCategories(3))
            .Returns(new List<Category>
            {
                new Category(),
                new Category(),
                new Category()
            });

        _mapperMock.Setup(m => m.Map<List<CategoryViewModel>>(It.IsAny<List<Category>>())).Returns(
            new List<CategoryViewModel>
            {
                new CategoryViewModel(),
                new CategoryViewModel(),
                new CategoryViewModel()
            });

        //act
        var result = _sut.Index() as ViewResult;
        var model = result.Model as HomeIndexViewModel;

        //assert
        Assert.AreEqual(3, model.TrendingCategories.Count);
    }

    [TestMethod]
    public void Index_should_show_10_products()
    {
        //arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, "Danish@hotmail.com")
        }, "testAuthentication"));

        _sut.ControllerContext = new ControllerContext();
        _sut.ControllerContext.HttpContext = new DefaultHttpContext
        {
            User = user
        };

        _productServiceMock.Setup(e => e.GetNewProducts(10, new CurrentCustomerContext()))
            .Returns(new List<ProductServiceModel>
            {
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel()
            });


        _mapperMock.Setup(m => m.Map<IEnumerable<ProductBoxViewModel>>(It.IsAny<IEnumerable<ProductServiceModel>>())).Returns(
            new List<ProductBoxViewModel>
            {
                new ProductBoxViewModel(),
                new ProductBoxViewModel(),
                new ProductBoxViewModel(),
                new ProductBoxViewModel(),
                new ProductBoxViewModel(),
                new ProductBoxViewModel(),
                new ProductBoxViewModel(),
                new ProductBoxViewModel(),
                new ProductBoxViewModel(),
                new ProductBoxViewModel()
            });

        //act
        var result = _sut.Index() as ViewResult;
        var model = result.Model as HomeIndexViewModel;

        //assert
        Assert.AreEqual(10, model.NewProducts.Count);
    }
}
