using DATA;
using DATA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyPOS.Controllers
{
    public class ItemPurchaseController : ApiController
    {
        public List<ItemPurchase> ReadAll(int StoreID, int SaleID)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    var ItemPurchaseList = database.ItemPurchases.Where(x => x.StoreID == StoreID && x.SaleID == SaleID).ToList();
                    return ItemPurchaseList;
                }
            }
            catch (Exception ex)
            {
                return null;
                //  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public void Create(int StoreID, int ItemID, int SaleID, int Quantity, string Discount, string TotalPrice)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    database.ItemPurchases.Add(new ItemPurchase()
                    {
                        // StoreID = StoreID
                        // ,
                        ItemID = ItemID
                        ,
                        SaleID = SaleID
                        ,
                        Quantity = Quantity
                        ,
                        Discount = Discount
                        ,
                        TotalPrice = TotalPrice
                    });

                    database.SaveChanges();

                    //    var message = Request.CreateResponse(HttpStatusCode.OK, data);
                    //    return message;
                    //}
                    //else
                    //{
                    //    var message = Request.CreateResponse(HttpStatusCode.NotFound, "ItemPurchase not found");
                    //    return message;
                    //}
                }
            }
            catch (Exception ex)
            {
                //  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public void Delete(int ItemPurchaseID, int StoreID)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    var ItemPurchase = database.ItemPurchases.FirstOrDefault(x => x.ID == ItemPurchaseID && x.StoreID == StoreID);

                    if (ItemPurchase != null)
                    {
                        database.ItemPurchases.Remove(ItemPurchase);
                        database.SaveChanges();

                    }
                    //    var message = Request.CreateResponse(HttpStatusCode.OK, data);
                    //    return message;
                    //}
                    //else
                    //{
                    //    var message = Request.CreateResponse(HttpStatusCode.NotFound, "ItemPurchase not found");
                    //    return message;
                    //}
                }
            }
            catch (Exception ex)
            {
                //  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public void Update(int ItemPurchaseID, int StoreID, int Quantity, string Discount, string TotalPrice)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    var ItemPurchase = database.ItemPurchases.FirstOrDefault(x => x.ID == ItemPurchaseID && x.StoreID == StoreID);

                    if (ItemPurchase != null)
                    {
                        ItemPurchase.Quantity = Quantity;
                        ItemPurchase.Discount = Discount;
                        ItemPurchase.TotalPrice = TotalPrice;
                        //Update
                        database.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                //  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/ItemPurchase
        public HttpResponseMessage Get()
        {
         try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    var data = database.ItemPurchases.ToList();
                    var message = Request.CreateResponse(HttpStatusCode.OK, data);
                    return message;
                }
            }
            catch (Exception ex)
            {
                 return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/ItemPurchase/5
        public HttpResponseMessage Get(string token, int saleId)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);
                    if (session != null)
                    {
                        //Validate storeID and ItemPurchaseID
                        var data = database.ItemPurchases.ToList().FirstOrDefault(x => (x.SaleID == saleId) && (x.StoreID == session.StoreID));
                        if (data != null)
                        {
                            //Save last  update
                            session.LastUpdate = DateTime.Now;
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, data);
                            return message;
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "ItemPurchase not found");
                            return message;
                        }
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // GET: api/ItemPurchase/5
        public HttpResponseMessage Get(string token, string id)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);
                    if (session != null)
                    {
                        //Validate storeID and ItemPurchaseID
                        var data = database.ItemPurchases.ToList().FirstOrDefault(x => (x.ID == Int32.Parse(id)) && (x.StoreID == session.StoreID));
                        if (data != null)
                        {
                            //Save last  update
                            session.LastUpdate = DateTime.Now;
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, data);
                            return message;
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "ItemPurchase not found");
                            return message;
                        }
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // POST: api/ItemPurchase
        //CREATE
        public HttpResponseMessage Post([FromBody]ItemPurchase ItemPurchase, string token)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);
                    if (session != null)
                    {
                        //Save last  update
                        session.LastUpdate = DateTime.Now;

                        var currentItemPurchase = database.ItemPurchases.ToList().FirstOrDefault(x => (x.StoreID == session.StoreID) && (x.ItemID == ItemPurchase.SaleID) && (x.ItemID == ItemPurchase.ItemID));
                        if (currentItemPurchase != null)
                        {
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "There is a ItemPurchase assosiated with the same Sale and Item");
                            return message;
                        }
                        else
                        {
                           if (database.Items.Any(x => x.ID == ItemPurchase.ItemID))
                            {
                                if (database.Sales.Any(x => x.ID == ItemPurchase.SaleID))
                                {
                                    ItemPurchase.StoreID = session.StoreID;
                                    database.ItemPurchases.Add(ItemPurchase);
                                    //SAVE ACTIVITY
                                    database.UserActivities.Add(new UserActivity()
                                    {
                                        StoreID = session.StoreID
                                        ,
                                        UserID = session.UserID
                                        ,
                                        Activity = "CREATE ItemPos"
                                    });
                                    database.SaveChanges();

                                    var message = Request.CreateResponse(HttpStatusCode.Created, "Create Success");
                                    return message;
                                }
                                else
                                {
                                    var message = Request.CreateResponse(HttpStatusCode.OK, "Sale not found");
                                    return message;
                                }
                            }
                            else
                            {
                                var message = Request.CreateResponse(HttpStatusCode.OK, "Item not found");
                                return message;
                            }
                        }
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
                        return message;
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // PUT: api/ItemPurchase/5
        //UPDATE
        public HttpResponseMessage Put(int id, [FromBody]ItemPurchase ItemPurchase, string token)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        //Save last  update
                        session.LastUpdate = DateTime.Now;
                        var currentItemPurchase = database.ItemPurchases.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));
                        if (currentItemPurchase != null)
                        {
                            currentItemPurchase.Quantity = ItemPurchase.Quantity;
                            currentItemPurchase.TotalPrice = ItemPurchase.TotalPrice;

                            //SAVE ACTIVITY
                            database.UserActivities.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,
                                UserID = session.UserID
                                ,
                                Activity = "CREATE ItPagePos"
                            });
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
                            return message;
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.OK, "ItemPurchase Not found");
                            return message;
                        }
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
                        return message;
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE: api/ItemPurchase/5
        //DELETE
        public HttpResponseMessage Delete(int id, string token)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);
                    if (session != null)
                    {
                        //Save last  update
                        session.LastUpdate = DateTime.Now;
                        var ItemPurchase = database.ItemPurchases.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));
                        if (ItemPurchase == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                    "ItemPurchase with Id = " + id.ToString() + " not found to delete");
                        }
                        else
                        {
                            database.ItemPurchases.Remove(ItemPurchase);
                            //SAVE ACTIVITY
                            database.UserActivities.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,
                                UserID = session.UserID
                                ,
                                Activity = "DELETE ItPagPos"
                            });
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Delete Success");
                            return message;
                        }
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
                        return message;
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, dbEx);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }
    }
}
