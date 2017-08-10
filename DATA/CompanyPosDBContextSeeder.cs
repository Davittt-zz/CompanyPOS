using DATA;
using DATA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CompanyPOS
{
    public class CompanyPosDBContextSeeder : DropCreateDatabaseIfModelChanges<CompanyPosDBContext>
   // public class CompanyPosDBContextSeeder : DropCreateDatabaseAlways<CompanyPosDBContext>
    {
        protected override void Seed(CompanyPosDBContext context)
        {
            Company company = new Company()
            {
                Name = "Company A",
                Stores = new List<Store>(){
                 new Store() { 
                     Name= "Store 1"
                     , Categories = new List<Category>(){
                         new Category(){
                             Name = "Category 1"
                             , Value = "Value 1"
                             , Items = new List<Item>() {
                                   new Item(){
                                       Name = "Item 1"
                                       , UnitPrice = 1000
                                       , Description = "Sample Item"
                                       , ItemAttributes = new List<ItemAttribute>(){
                                            new ItemAttribute(){
                                           Name = "Color"
                                           , Value = "Red"
                                           , Amount = 0
                                           , Price = 1
                                            }
                                       }
                                   }
                               }
                         }
                     }, Users = new List<User> {
                         new User(){
                             LastName = "Owner X" , Username = "ownerX" , Password = "test", Type = "Owner" , UserLevel = "admin" 
                         }
                     }
                  },  
                }
            };

            context.Companies.Add(company);
            context.SaveChanges();

            //Create Session
            Session session = new Session()
            {
                Created = DateTime.Now
                ,
                LastUpdate = DateTime.Now
                ,
                StoreID = context.Stores.First().ID
                ,
                UserID = context.Stores.First().Users.First().ID
                ,
                TokenID = "11554895561"
            };

            context.Sessions.Add(session);
            context.SaveChanges();
            Shift shift = new Shift()
            {
                Status = "OPEN"
                ,
                TimeEnd = DateTime.Now.AddYears(1)
                ,
                TimeStart = DateTime.Now.AddYears(-1)
                ,
                StoreID = context.Stores.First().ID
            };

            context.Shifts.Add(shift);
            context.SaveChanges();
            Sale sale = new Sale()
            {
                Title = "New Sale"
                ,
                Date = DateTime.Now
                ,
                DiscountAmount = 10
                ,
                DiscountRate = 0
                ,
                StoreID = context.Stores.First().ID
            };

            context.Sales.Add(sale);
            context.SaveChanges();
            ItemPurchase itemPurchase = new ItemPurchase()
            {
                Discount = "0"
                ,
                ItemID = context.Items.First().ID
                ,
                Quantity = 1
                ,
                StoreID = context.Stores.First().ID
                ,
                TotalPrice = "100"
                ,
                SaleID = context.Sales.First().ID
            };

            context.ItemPurchases.Add(itemPurchase);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}