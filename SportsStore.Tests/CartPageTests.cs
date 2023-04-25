using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using SportsStore.Models;
using SportsStore.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
    public class CartPageTests
    {
        [Fact]
        public void Can_Load_Cart()
        {
            //arrange
            Product p1 = new Product() { ProductId = 1, Name = "P1" };
            Product p2 = new Product() { ProductId = 2, Name = "P2" };
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] { p1, p2 }).AsQueryable<Product>());

            Cart testCart = new Cart();
            testCart.AddItem(p1, 2);
            testCart.AddItem(p2, 1);

            //action
            CartModel cartModel = new CartModel(mock.Object, testCart);
            cartModel.OnGet("myUrl");

            //assert
            Assert.Equal(2, cartModel.Cart?.Lines.Count());
            Assert.Equal("myUrl", cartModel.ReturnUrl);
        }

        [Fact]
        public void Can_Update_Cart()
        {
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] { new Product { ProductId = 1, Name = "P1" } }).AsQueryable<Product>());

            Cart testCart = new Cart();

            //action 
            CartModel cartModel = new CartModel(mock.Object, testCart);           
            cartModel.OnPost(1, "myUrl");

            //assert
            Assert.Single(testCart.Lines);
            Assert.Equal("P1", testCart.Lines.First().Product.Name);
            Assert.Equal(1, testCart.Lines.First().Quantity);
        }
    }
}
