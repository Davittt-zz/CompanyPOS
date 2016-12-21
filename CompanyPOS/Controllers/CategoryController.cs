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
    public class CategoryController : ApiController
    {
        // GET: api/Category
        public HttpResponseMessage Get(string token)
        {
         try
            {
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        //Validate storeID and CategoryID
                        var data = database.Category.ToList().Where(x => (x.StoreID == session.StoreID));
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
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "Category not found");
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

        // GET: api/Category/5
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
                        //Validate storeID and CategoryID
                        var data = database.Category.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
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
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "Category not found");
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

        // POST: api/Category
        //CREATE
        public HttpResponseMessage Post([FromBody]Category Category, string token)
        {
            string errorStatus = " ";
            try
            {
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    SessionController sessionController = new SessionController();

                    errorStatus += " Before Atutentication || ";
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        //Save last  update
                        session.LastUpdate = DateTime.Now;

                        errorStatus += " Before Find similar category || ";
                        var currentCategory = database.Category.ToList().FirstOrDefault(x => x.Name == Category.Name && (x.StoreID == session.StoreID));
                        if (currentCategory != null)
                        {
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "There is a Category with this name");
                            return message;
                        }
                        else
                        {
                            Category.StoreID = session.StoreID;
                            database.Category.Add(Category);
                            //SAVE ACTIVITY
                            database.UserActivity.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,
                                UserID = session.UserID
                                ,
                                Activity = "CREATE Category"
                            });

                            errorStatus += " Before adding in the db || ";
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex + " || " + errorStatus );
            }
        }


        // PUT: api/Category/5
        //UPDATE
        public HttpResponseMessage Put(int id, [FromBody]Category Category, string token)
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

                        var currentCategory = database.Category.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (currentCategory != null)
                        {
                            currentCategory.Name = Category.Name;
                            currentCategory.Value = Category.Value;
                            
                            //SAVE ACTIVITY
                            database.UserActivity.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,
                                UserID = session.UserID
                                ,
                                Activity = "CREATE Category"
                            });

                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
                            return message;
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Category Not found");
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

        // DELETE: api/Category/5
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

                        var Category = database.Category.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (Category == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                    "Category with Id = " + id.ToString() + " not found to delete");
                        }
                        else
                        {

                            database.Category.Remove(Category);
                            //SAVE ACTIVITY
                            database.UserActivity.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,
                                UserID = session.UserID
                                ,
                                Activity = "DELETE Category"
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
