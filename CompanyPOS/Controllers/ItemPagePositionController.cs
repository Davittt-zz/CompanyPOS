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
    public class ItemPagePositionController : ApiController
    {
		// GET: api/ItemPagePosition
        public HttpResponseMessage GetAll(string token)
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
                        var data = database.ItemPagePositions.ToList().Where(x => (x.StoreID == session.StoreID));
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
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "Item not found");
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

        // GET: api/ItemPagePosition
        // GET: api/ItemPagePosition
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/ItemPagePosition/5
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
                        //Validate storeID and ItemPagePositionID
                        var data = database.ItemPagePositions.ToList().FirstOrDefault(x => (x.ID == id));
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
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "ItemPagePosition not found");
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

        // POST: api/ItemPagePosition
        //CREATE
        public HttpResponseMessage Post([FromBody]ItemPagePosition ItemPagePosition, string token)
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

                        var currentItemPagePosition = database.ItemPagePositions.ToList().FirstOrDefault(x => (x.MenuID == ItemPagePosition.MenuID) && (x.hPos == ItemPagePosition.hPos) && (x.vPos == ItemPagePosition.vPos) && (x.StoreID == session.StoreID));
                        if (currentItemPagePosition != null)
                        {
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "There is a ItemPagePosition in this position");
                            return message;
                        }
                        else
                        {
                            ItemPagePosition.StoreID = session.StoreID;

                            var currentItem = database.Items.ToList().FirstOrDefault(x => x.ID == ItemPagePosition.ItemID);
                            if (currentItem != null)
                            {
								var currentMenu = database.Menues.ToList().FirstOrDefault(x => x.ID == ItemPagePosition.MenuID);

                                if (currentMenu != null)
                                {
                                    database.ItemPagePositions.Add(ItemPagePosition);
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
                                    var message = Request.CreateResponse(HttpStatusCode.OK, "Menu not found");
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

        // PUT: api/ItemPagePosition/5
        //UPDATE
        public HttpResponseMessage Put(int id, [FromBody]ItemPagePosition ItemPagePosition, string token)
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

                        var currentItemPagePosition = database.ItemPagePositions.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (currentItemPagePosition != null)
                        {
                            currentItemPagePosition.hPos = ItemPagePosition.hPos;
                            currentItemPagePosition.vPos = ItemPagePosition.vPos;
                            //currentItemPagePosition.ItemID = ItemPagePosition.ItemID;
                            currentItemPagePosition.Page = ItemPagePosition.Page;

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
                            var message = Request.CreateResponse(HttpStatusCode.OK, "ItemPagePosition Not found");
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

        // DELETE: api/ItemPagePosition/5
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

                        var ItemPagePosition = database.ItemPagePositions.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (ItemPagePosition == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                    "ItemPagePosition with Id = " + id.ToString() + " not found to delete");
                        }
                        else
                        {

                            database.ItemPagePositions.Remove(ItemPagePosition);
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
