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
    public class UserController : ApiController
    {
        public HttpResponseMessage Getall()
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    var ListUsers = database.Users.ToList();
                    var message = Request.CreateResponse(HttpStatusCode.OK, ListUsers);
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage GetStoreUsers(string token)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    var SessionUser = database.Users.FirstOrDefault(x => x.ID == session.UserID);

                    if (session != null)
                    {
                        if (SessionUser.UserLevel.ToLower() == "admin")
                        {
                            //Validate storeID and UserID
                            List<User> userList = database.Users.Where(x => (x.StoreID == session.StoreID)).ToList();
                            if (userList != null)
                            {
                                //Save last  update
                                session.LastUpdate = DateTime.Now;
                                database.SaveChanges();

                                var message = Request.CreateResponse(HttpStatusCode.OK, userList);
                                return message;
                            }
                            else
                            {
                                var message = Request.CreateResponse(HttpStatusCode.NotFound, "Users not found");
                                return message;
                            }
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "You don't have privileges");
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

        // GET: api/Store/5
        //READ
        //It should have permissions
        public HttpResponseMessage GetCompanyUsers(string token, int companyID)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);


                    var SessionUser = database.Users.FirstOrDefault(x => x.ID == session.UserID);

                    if (session != null)
                    {
                        if (SessionUser.UserLevel.ToLower() == "admin")
                        {
                            //Validate storeID and UserID
                            List<User> userList = database.Users.Where(x => (x.CompanyID == companyID)).ToList();
                            if (userList != null)
                            {
                                //Save last  update
                                session.LastUpdate = DateTime.Now;
                                database.SaveChanges();

                                var message = Request.CreateResponse(HttpStatusCode.OK, userList);
                                return message;
                            }
                            else
                            {
                                var message = Request.CreateResponse(HttpStatusCode.NotFound, "Users not found");
                                return message;
                            }
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "You don't have privileges");
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

        // GET: api/Store/5
        //READ
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
                        var SessionUser = database.Users.FirstOrDefault(x => x.ID == session.UserID);

                        //Validate storeID and UserID
                        User user = database.Users.ToList().FirstOrDefault(x => (x.ID == id)
                            && ((x.StoreID == session.StoreID) || ((SessionUser.Type.ToLower() == "owner") && SessionUser.CompanyID == x.CompanyID))
                            );
                        if (user != null)
                        {
                            //Save last  update
                            session.LastUpdate = DateTime.Now;
                            database.SaveChanges();

                            var message = Request.CreateResponse(HttpStatusCode.OK, user);
                            return message;
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "User not found");
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

        // POST: api/User
        //CREATE
        public HttpResponseMessage Post([FromBody]User user, string token, int? storeID)
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

                        var currentUser = database.Users.ToList().FirstOrDefault(x => x.Username == user.Username && (x.StoreID == session.StoreID));
                        if (currentUser != null)
                        {
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "There is a user with this name");
                            return message;
                        }
                        else
                        {
                            //Save last  update
                            var currentCompanyID = database.Stores.FirstOrDefault(x => x.ID == session.ID).CompanyID;
                            user.CompanyID = currentCompanyID;

                            if (storeID == null)
                            {
                                user.StoreID = session.StoreID;
                            }
                            else
                            {
                                var newStore = database.Companies.FirstOrDefault(x => x.Id == user.CompanyID).Stores.First(y => y.ID == storeID);
                                if (newStore != null)
                                {
                                    user.StoreID = newStore.ID;
                                }
                                else
                                {
                                    return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "Wrong StoreID");
                                }
                            }

                            database.Users.Add(user);
                            //SAVE ACTIVITY
                            //database.UserActivities.Add(new UserActivity()
                            //{
                            //    StoreID = session.StoreID
                            //    ,
                            //    UserID = session.UserID
                            //    ,
                            //    Activity = "CREATE USER"
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

        // PUT: api/User/5
        //UPDATE
        public HttpResponseMessage Put(int id, [FromBody]User user, string token)
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

                        var currentUser = database.Users.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (currentUser != null)
                        {
                            currentUser.Name = user.Name;
                            //currentUser.Type = user.Type;
                            //currentUser.TypeID = user.TypeID;
                            // currentUser.Password = user.Password;
                            // currentUser.StoreID = user.StoreID;
                            //  currentUser.UserLevel = user.UserLevel;
                            currentUser.Username = user.Username;
                            currentUser.Email = user.Email;

                            //SAVE ACTIVITY
                            //database.UserActivities.Add(new UserActivity()
                            //{
                            //    StoreID = session.StoreID
                            //    ,
                            //    UserID = session.UserID
                            //    ,
                            //    Activity = "CREATE USER"
                            //});

                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
                            return message;
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.OK, "User Not found");
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

        // DELETE: api/User/5
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

                        var user = database.Users.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (user == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                    "User with Id = " + id.ToString() + " not found to delete");
                        }
                        else
                        {

                            database.Users.Remove(user);
                            //SAVE ACTIVITY
                            //database.UserActivities.Add(new UserActivity()
                            //{
                            //    StoreID = session.StoreID
                            //    ,
                            //    UserID = session.UserID
                            //    ,
                            //    Activity = "DELETE USER"
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
