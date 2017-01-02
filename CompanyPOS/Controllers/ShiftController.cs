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
    public class ShiftController : ApiController
    {
        // GET: api/Shift
        public HttpResponseMessage Get(string token, string period)
        {
            try
            {
                string[] dPart = period.Split('-');
                 DateTime date = new DateTime(Int32.Parse(dPart[0])
                    ,Int32.Parse(dPart[1])
                    ,Int32.Parse(dPart[2])
                    ,Int32.Parse(dPart[3])
                    ,Int32.Parse(dPart[4])
                    ,Int32.Parse(dPart[5]));
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        //Validate storeID and ShiftID
                        var data = database.Shifts.ToList().Where(x => (x.TimeStart <= date) && (x.TimeEnd >= date) && (x.StoreID == session.StoreID));
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
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "Shift not found");
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

        // GET: api/Shift/5
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
                        //Validate storeID and ShiftID
                        var data = database.Shifts.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
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
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "Shift not found");
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

        // POST: api/Shift
        //CREATE
        public HttpResponseMessage Post([FromBody]Shift Shift, string token)
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

                        //var currentShift = database.Shifts.ToList().FirstOrDefault(x => (x.StoreID == session.StoreID));
                        //if (currentShift != null)
                        //{
                        //    database.SaveChanges();
                        //    var message = Request.CreateResponse(HttpStatusCode.OK, "There is a Shift with this name");
                        //    return message;
                        //}
                        //else
                        //{
                            Shift.StoreID = session.StoreID;
                            database.Shifts.Add(Shift);
                            //SAVE ACTIVITY
                            //database.UserActivities.Add(new UserActivity()
                            //{
                            //    StoreID = session.StoreID
                            //    ,
                            //    UserID = session.UserID
                            //    ,
                            //    Activity = "CREATE Shift"
                            //});
                            database.SaveChanges();

                            var message = Request.CreateResponse(HttpStatusCode.Created, "Create Success");
                            return message;
                     //   }
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

        // PUT: api/Shift/5
        //UPDATE
        public HttpResponseMessage Put(int id, [FromBody]Shift Shift, string token)
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

                        var currentShift = database.Shifts.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (currentShift != null)
                        {
                            currentShift.Status    = Shift.Status   ;
                            currentShift.TimeEnd   = Shift.TimeEnd  ;
                            currentShift.TimeStart = Shift.TimeStart;
                            //SAVE ACTIVITY
                            //database.UserActivities.Add(new UserActivity()
                            //{
                            //    StoreID = session.StoreID
                            //    ,
                            //    UserID = session.UserID
                            //    ,
                            //    Activity = "CREATE Shift"
                            //});

                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
                            return message;
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Shift Not found");
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

        // DELETE: api/Shift/5
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

                        var Shift = database.Shifts.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (Shift == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                    "Shift with Id = " + id.ToString() + " not found to delete");
                        }
                        else
                        {

                            database.Shifts.Remove(Shift);
                            //SAVE ACTIVITY
                            //database.UserActivities.Add(new UserActivity()
                            //{
                            //    StoreID = session.StoreID
                            //    ,
                            //    UserID = session.UserID
                            //    ,
                            //    Activity = "DELETE Shift"
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
