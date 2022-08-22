using System;
using Microsoft.EntityFrameworkCore;
using Nortwind_EF_HW.Models;

namespace Nortwind_EF_HW;

internal class Program
{
    static void Main(string[] args)
    {

        var db = new NorthwindContext();

        //Ex1
        var regionList = db.Regions.ToList();

        //Ex2
        foreach (var singleRegion in regionList)
        {
            Console.WriteLine(singleRegion.RegionDescription);

        }
        Console.WriteLine("************************");

        //Ex3
        var teritories = db.Territories.ToList();
        teritories.ForEach(t => Console.WriteLine(t.TerritoryDescription));
        Console.WriteLine("************************");

        //Ex4
        //a

        // C# Syntax
        var regionWithTeritores = db.Regions.Join(db.Territories,
            region => region.RegionId,
            teritory => teritory.RegionId,
            (region, teritory) => new
            {
                regionName = region.RegionDescription,
                teritoryName = teritory.TerritoryDescription
            }
            ).ToList();

        #region region With Teritores Query Syntax
        //var regionWithTeritoresQuerySyntax = (from region in db.Regions
        //                            join teritory in db.Territories
        //                            on region.RegionId equals teritory.RegionId
        //                            select new
        //                            {
        //                                regionName = region.RegionDescription,
        //                                teritoryName = teritory.TerritoryDescription
        //                            }).ToList();
        #endregion
        var x = regionWithTeritores.GroupBy(x => x.regionName).ToDictionary(group =>
                                                               group.Key, group => group.ToList());

        foreach (var item in x)
        {
            Console.WriteLine(item.Key);
            for (int i = 0; i < item.Value.Count; i++)
            {
                Console.WriteLine($"-{item.Value[i].teritoryName}");

            }


        }
        Console.WriteLine("*******************************");
        //b

        var regionWithIncludeMethod = db.Regions.Include("Territories").ToList();
        foreach (var item1 in regionWithIncludeMethod)
        {
            Console.WriteLine(item1.RegionDescription);
            item1.Territories.ToList().ForEach(t => Console.WriteLine($"-{t.TerritoryDescription}"));
        }
        Console.WriteLine("********************");

        //Ex6
        DateTime aDate = DateTime.Now;
        var orders = db.Orders.ToList();

        //Creating new Order
        var newOrder = new Order
        {
            CustomerId = "FRANK",
            EmployeeId = 6,
            OrderDate = aDate,
            ShipAddress = "7 Piccadilly Road.",
            ShipCity = "New York",
            ShipCountry = "New York",
        };

        //Adding the new order to the Database
        db.Orders.Add(newOrder);
        db.SaveChanges();

        //Creating new Order Details to this new Order
        var OrderDetails1 = new OrderDetail { ProductId = 11, UnitPrice = 95, Quantity = 3, OrderId = newOrder.OrderId };
        var OrderDetails2 = new OrderDetail { ProductId = 56, UnitPrice = 47, Quantity = 6, OrderId = newOrder.OrderId };
        var OrderDetails3 = new OrderDetail { ProductId = 74, UnitPrice = 120, Quantity = 5, OrderId = newOrder.OrderId };

        //Adding new Order Details to the Database
        db.OrderDetails.Add(OrderDetails1);
        db.OrderDetails.Add(OrderDetails2);
        db.OrderDetails.Add(OrderDetails3);
        db.SaveChanges();


        //Ex7
        // C# Syntax
        var printProductNameAndEmploee = db.Orders.Join(db.OrderDetails,
            orders => orders.OrderId,
            orderDetails => orderDetails.OrderId,
            (orders, orderDetails) => new
            {
                orders.OrderId,
                orders.EmployeeId,
                orderDetails.ProductId

            }).Join(db.Products,
            orderJoinOrderDetails => orderJoinOrderDetails.ProductId,
            products => products.ProductId,
            (orderJoinOrderDetails, products) => new
            {
                products.ProductName,
                orderJoinOrderDetails.EmployeeId
            }).Join(db.Employees,
            emploeeJoined => emploeeJoined.EmployeeId,
            emloees => emloees.EmployeeId,
            (emploeeJoined, emloees) => new
            {
                emloees.FirstName,
                emloees.LastName,
                emploeeJoined.ProductName
            }).ToList();

        printProductNameAndEmploee.ForEach(x =>
        {
            Console.WriteLine(x.ProductName);
            Console.WriteLine(x.FirstName + ' ' + x.LastName);
            Console.WriteLine("___");
        });



        //Query Syntax
        //var printProductNameAndEmploeeQuerySyntax = from order in db.Orders
        //                                            join orderDetails in db.OrderDetails
        //                                            on order.OrderId equals orderDetails.OrderId
        //                                            join products in db.Products
        //                                            on orderDetails.ProductId equals products.ProductId
        //                                            join Employees in db.Employees
        //                                            on order.EmployeeId equals Employees.EmployeeId
        //                                            select new
        //                                            {
        //                                                productName = products.ProductName,
        //                                                emploeeName = Employees.FirstName + ' ' + Employees.LastName
        //                                            };



        //foreach (var item in printProductNameAndEmploeeQuerySyntax)
        //{
        //    Console.WriteLine(item.productName);
        //    Console.WriteLine(item.emploeeName);
        //    Console.WriteLine("___");
        //}


        //Ex8

        var changedEmploeeId = db.Orders.Single(x => x.OrderId == 11079);
        changedEmploeeId.EmployeeId = 5;
        db.SaveChanges();

        var deletedOrderDetails = db.OrderDetails.Single(x => x.OrderId == 11079 && x.ProductId ==56);
        db.OrderDetails.Remove(deletedOrderDetails);
        db.SaveChanges();
    }




}
