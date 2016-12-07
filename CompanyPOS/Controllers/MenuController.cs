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
    public class MenuController : ApiController
    {
        // GET: api/Menu
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET: api/Menu/5
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
                        //Validate storeID and MenuID
                        var data = database.Menu.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
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
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "Menu not found");
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

        // POST: api/Menu
        //CREATE
        public HttpResponseMessage Post([FromBody]Menu menu, string token)
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

                        var currentMenu = database.Menu.ToList().FirstOrDefault(x => x.Name == menu.Name && (x.StoreID == session.StoreID));
                        if (currentMenu != null)
                        {
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "There is a menu with this name");
                            return message;
                        }
                        else
                        {
                            menu.StoreID = session.StoreID;
                            database.Menu.Add(menu);
                            //SAVE ACTIVITY
                            database.UserActivity.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,
                                UserID = session.UserID
                                ,
                                Activity = "CREATE MENU"
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


        // PUT: api/Menu/5
        //UPDATE
        public HttpResponseMessage Put(int id, [FromBody]Menu menu, string token)
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

                        var currentMenu = database.Menu.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (currentMenu != null)
                        {
                            currentMenu.Name = menu.Name;
                            currentMenu.Page = menu.Page;
                           // currentMenu.StoreID = menu.StoreID;
                            currentMenu.Description = menu.Description;

                            //SAVE ACTIVITY
                            database.UserActivity.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,UserID = session.UserID
                                ,Activity = "CREATE MENU"
                            });

                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
                            return message;
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Menu Not found");
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

        // DELETE: api/Menu/5
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

                        var menu = database.Menu.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (menu == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                    "Menu with Id = " + id.ToString() + " not found to delete");
                        }
                        else
                        {

                            database.Menu.Remove(menu);
                            //SAVE ACTIVITY
                            database.UserActivity.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,
                                UserID = session.UserID
                                ,
                                Activity = "DELETE MENU"
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
