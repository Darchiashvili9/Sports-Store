using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Can_Use_Repository()
        {
            //arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId=1,Name="P1"},
                new Product{ProductId=2,Name="P2"}
            }).AsQueryable<Product>());

            HomeController controller = new HomeController(mock.Object);

            //act
            ProductsListViewModel result = controller.Index(null)?.ViewData.Model as ProductsListViewModel ?? new();

            //assert
            Product[] prodArry = result.Products.ToArray();
            Assert.True(prodArry.Length == 2);
            Assert.Equal("P1", prodArry[0].Name);
            Assert.Equal("P2", prodArry[1].Name);
        }

        [Fact]
        public void Can_Paginate()
        {
            //arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId=1,Name="P1"},
                new Product{ProductId=2,Name="P2"},
                new Product{ProductId=3,Name="P3"},
                new Product{ProductId=4,Name="P4"},
                new Product{ProductId=5,Name="P5"}
            }).AsQueryable<Product>);

            HomeController controller = new HomeController(mock.Object);
            controller.pageSize = 3;

            //act
            ProductsListViewModel result = controller.Index(null, 2)?.ViewData.Model as ProductsListViewModel ?? new();

            //assert
            Product[] prodArry = result.Products.ToArray();
            Assert.True(prodArry.Length == 2);
            Assert.Equal("P4", prodArry[0].Name);
            Assert.Equal("P5", prodArry[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //arragnge
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId=1,Name="P1"},
                new Product{ProductId=2,Name="P2"},
                new Product{ProductId=3,Name="P3"},
                new Product{ProductId=4,Name="P4"},
                new Product{ProductId=5,Name="P5"}
            }).AsQueryable<Product>);

            //arrange
            HomeController controller = new HomeController(mock.Object) { pageSize = 3 };

            //act
            ProductsListViewModel result = controller.Index(null, 2)?.ViewData.Model as ProductsListViewModel ?? new();

            //assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }

        [Fact]
        public void Can_Filter_Products()
        {
            //arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId=1,Name="P1",Category="Cat1"},
                new Product{ProductId=2,Name="P2",Category="Cat2"},
                new Product{ProductId=3,Name="P3",Category="Cat1"},
                new Product{ProductId=4,Name="P4",Category="Cat2"},
                new Product{ProductId=5,Name="P5",Category="Cat3"}
            }).AsQueryable<Product>);

            //arrange
            HomeController controller = new HomeController(mock.Object);
            controller.pageSize = 3;

            //action
            Product[] result = (controller.Index("Cat2", 1)?.ViewData.Model as ProductsListViewModel ?? new()).Products.ToArray();

            //assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            //arrange
            Mock<IStoreRepository> mock = new Mock<IStoreRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product{ProductId=1,Name="P1",Category="Cat1"},
                new Product{ProductId=2,Name="P2",Category="Cat2"},
                new Product{ProductId=3,Name="P3",Category="Cat1"},
                new Product{ProductId=4,Name="P4",Category="Cat2"},
                new Product{ProductId=5,Name="P5",Category="Cat3"}
            }).AsQueryable<Product>);

            HomeController target = new HomeController(mock.Object);
            target.pageSize = 3;

            Func<ViewResult, ProductsListViewModel?> GetModel = result => result?.ViewData?.Model as ProductsListViewModel;

            //action
            int? res1 = GetModel(target.Index("Cat1"))?.PagingInfo.TotalItems;
            int? res2 = GetModel(target.Index("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = GetModel(target.Index("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = GetModel(target.Index(null))?.PagingInfo.TotalItems;

            //assert
            Assert.Equal(2, res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);
            Assert.Equal(5, resAll);
        }

    }
}
