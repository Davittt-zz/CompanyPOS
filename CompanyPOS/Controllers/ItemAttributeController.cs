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
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        //Validate storeID and ItemAttributeID
                        var data = database.ItemAttribute.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
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
        public HttpResponseMessage Post([FromBody]ItemAttribute ItemAttribute, string token)
        {
            try
            {
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        //Save last  update
                        session.LastUpdate = DateTime.Now;

                        var currentItemAttribute = database.ItemAttribute.ToList().FirstOrDefault(x => (x.ItemID == ItemAttribute.ItemID) && (x.Name == ItemAttribute.Name) && (x.StoreID == session.StoreID));
                        if (currentItemAttribute != null)
                        {
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "There is an ItemAttribute with this name");
                            return message;
                        }
                        else
                        {
                            ItemAttribute.StoreID = session.StoreID;
                            database.ItemAttribute.Add(ItemAttribute);
                            //SAVE ACTIVITY
                            database.UserActivity.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,
                                UserID = session.UserID
                                ,
                                Activity = "CREATE ItemPos"
                            });
                            database.SaveChanges();

                            var message = Request.CreateResponse(HttpStatusCode.OK, "Create Success");
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
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        //Save last  update
                        session.LastUpdate = DateTime.Now;

                        var currentItemAttribute = database.ItemAttribute.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (currentItemAttribute != null)
                        {
                            currentItemAttribute.Name = ItemAttribute.Name;
                            currentItemAttribute.Price = ItemAttribute.Price;
                            currentItemAttribute.ItemID = ItemAttribute.ItemID;
                            currentItemAttribute.Value = ItemAttribute.Value;
                            currentItemAttribute.Visible = ItemAttribute.Visible;

                            //SAVE ACTIVITY
                            database.UserActivity.Add(new UserActivity()
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
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        //Save last  update
                        session.LastUpdate = DateTime.Now;

                        var ItemAttribute = database.ItemAttribute.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (ItemAttribute == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                    "ItemAttribute with Id = " + id.ToString() + " not found to delete");
                        }
                        else
                        {

                            database.ItemAttribute.Remove(ItemAttribute);
                            //SAVE ACTIVITY
                            database.UserActivity.Add(new UserActivity()
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
