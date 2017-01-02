using CompanyPOS.Models;
using DATA;
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
    public class ItemAttributeController : ApiController
    {
        // GET: api/ItemAttribute
        // GET: api/ItemAttribute
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ItemAttribute/5
        public HttpResponseMessage Get(string token, int id)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        //Validate storeID
                        var Items = database.Items.FirstOrDefault(x => x.StoreID == session.StoreID);

                        // ItemAttributeID
                        var data = Items.ItemAttributes.ToList().FirstOrDefault(x => (x.ID == id));
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
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "ItemAttribute not found");
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

        // POST: api/ItemAttribute
        //CREATE
        public HttpResponseMessage Post([FromBody]ItemAttribute ItemAttribute, string token, int ItemID)
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

                        //Validate storeID
                        var Item = database.Items.FirstOrDefault(x => x.StoreID == session.StoreID && x.ID == ItemID);
                        var currentItemAttribute = Item.ItemAttributes.ToList().FirstOrDefault(x => (x.Name == ItemAttribute.Name));

                        if (currentItemAttribute != null)
                        {
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "There is an ItemAttribute with this name");
                            return message;
                        }
                        else
                        {
                            Item.ItemAttributes.Add(ItemAttribute);

                            //database.ItemAttributes.Add(ItemAttribute);
                            //SAVE ACTIVITY

                            //database.UserActivities.Add(new UserActivity()
                            //{
                            //    StoreID = session.StoreID
                            //    ,
                            //    UserID = session.UserID
                            //    ,
                            //    Activity = "CREATE ItemPos"
                            //});
                            database.SaveChanges();

                            var message = Request.CreateResponse(HttpStatusCode.Created, "Create Success");
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

        // PUT: api/ItemAttribute/5
        //UPDATE
        public HttpResponseMessage Put(int id, [FromBody]ItemAttribute ItemAttribute, string token)
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

                        //Validate storeID
                        var Item = database.Items.FirstOrDefault(x => x.StoreID == session.StoreID && x.ID == id);

                        var currentItemAttribute = Item.ItemAttributes.ToList().FirstOrDefault(x => x.ID == id);

                        if (currentItemAttribute != null)
                        {
                            currentItemAttribute.Name = ItemAttribute.Name;
                            currentItemAttribute.Price = ItemAttribute.Price;
                            // currentItemAttribute.ItemID = ItemAttribute.ItemID;
                            currentItemAttribute.Value = ItemAttribute.Value;
                            currentItemAttribute.Visible = ItemAttribute.Visible;

                            //SAVE ACTIVITY
                            //database.UserActivities.Add(new UserActivity()
                            //{
                            //    StoreID = session.StoreID
                            //    ,
                            //    UserID = session.UserID
                            //    ,
                            //    Activity = "CREATE ItPagePos"
                            //});

                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
                            return message;
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.OK, "ItemAttribute Not found");
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

        // DELETE: api/ItemAttribute/5
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

                        //Validate storeID
                        var Item = database.Items.FirstOrDefault(x => x.StoreID == session.StoreID && x.ID == id);

                        var ItemAttribute = Item.ItemAttributes.ToList().FirstOrDefault(x => x.ID == id);

                        if (ItemAttribute == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                    "ItemAttribute with Id = " + id.ToString() + " not found to delete");
                        }
                        else
                        {
                            database.ItemAttributes.Remove(ItemAttribute);
                            //SAVE ACTIVITY
                            //database.UserActivities.Add(new UserActivity()
                            //{
                            //    StoreID = session.StoreID
                            //    ,
                            //    UserID = session.UserID
                            //    ,
                            //    Activity = "DELETE ItPagPos"
                            //});

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
