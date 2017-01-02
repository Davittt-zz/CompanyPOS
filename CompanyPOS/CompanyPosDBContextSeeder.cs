using CompanyPOS.Models;
using DATA;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CompanyPOS
{
     public class CompanyPosDBContextSeeder : DropCreateDatabaseIfModelChanges<CompanyPosDBContext>
    //public class CompanyPosDBContextSeeder : DropCreateDatabaseAlways<CompanyPosDBContext>
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
                                           , Visible = true
                                           , Price = 1
                                            }
                                       }
                                   }
                               }
                         }
                     }, Users = new List<User> {
                         new User(){
                             Name = "Owner X" , Username = "ownerX" , Password = "test", Type = "Owner" , UserLevel = "admin" 
                         }
                     }
                  }, 
                }
            };

            context.Companies.Add(company);

            base.Seed(context);
        }
    }
}