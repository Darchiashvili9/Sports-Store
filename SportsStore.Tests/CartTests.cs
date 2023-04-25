using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
    public class CartTests
    {
        [Fact]
        public void Can_Add_New_Lines()
        {
            //arrange
            Product p1 = new Product() { ProductId = 1, Name = "P1" };
            Product p2 = new Product() { ProductId = 2, Name = "P2" };

            //arrange - create new cart
            Cart target = new Cart();

            //act
            target.AddItem(p1, 1);
            target.AddItem(p2, 2);
            CartLine[] results = target.Lines.ToArray();

            //assert
            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Product);
            Assert.Equal(p2, results[1].Product);
        }

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //arrange
            Product p1 = new Product() { ProductId = 1, Name = "P1" };
            Product p2 = new Product() { ProductId = 2, Name = "P2" };
            Cart target = new Cart();

            //act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = (target.Lines ?? new()).OrderBy(c => c.Product.ProductId).ToArray();

            //assert
            Assert.Equal(2, results.Length);
            Assert.Equal(11, results[0].Quantity);
            Assert.Equal(1, results[1].Quantity);
        }

        [Fact]
        public void Can_Remove_Line()
        {
            Product p1 = new Product() { ProductId = 1, Name = "P1" };
            Product p2 = new Product() { ProductId = 2, Name = "P2" };
            Product p3 = new Product() { ProductId = 3, Name = "P3" };
            Cart target = new Cart();

            //arrange
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            //act
            target.RemoveLine(p2);

            //assert
            Assert.Empty(target.Lines.Where(c => c.Product == p2));
            Assert.Equal(2, target.Lines.Count);
        }

        [Fact]
        public void Calculate_Cart_Total()
        {
            Product p1 = new Product() { ProductId = 1, Name = "P1", Price = 100m };
            Product p2 = new Product() { ProductId = 2, Name = "P2", Price = 50m };
            Cart target = new Cart();

            //act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            //assert
            Assert.Equal(450m, result);
        }

        [Fact]
        public void Can_Clear_Contents()
        {
            Product p1 = new Product() { ProductId = 1, Name = "P1", Price = 100m };
            Product p2 = new Product() { ProductId = 2, Name = "P2", Price = 50m };
            Cart target = new Cart();

            //arrange
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            //act
            target.Clear();

            //assert
            Assert.Empty(target.Lines);
        }
    }
}
