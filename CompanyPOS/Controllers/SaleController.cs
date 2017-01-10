﻿using DATA;
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
    public class SaleController : ApiController
    {
        // GET: api/Sale
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
                        //Validate storeID and SaleID
                        var data = database.Sales.ToList().Where(x => (x.StoreID == session.StoreID)).ToList();
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
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "Sale not found");
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

        // GET: api/Sale/5
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
                        //Validate storeID and SaleID
                        var data = database.Sales.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
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
                            var message = Request.CreateResponse(HttpStatusCode.NotFound, "Sale not found");
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

        // POST: api/Sale
        //CREATE
        public HttpResponseMessage Post([FromBody]Sale Sale, string token)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        //Save last update
                        session.LastUpdate = DateTime.Now;

                        var currentSale = database.Sales.ToList().FirstOrDefault(x => (x.Title == Sale.Title) && (x.StoreID == session.StoreID));
                        if (currentSale != null)
                        {
                            database.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.OK, "There is a Sale with this name");
                            return message;
                        }
                        else
                        {
                            var shiftToFind = database.Shifts.FirstOrDefault(x => x.ID == Sale.ShiftID && session.StoreID == x.StoreID);
                            if ((shiftToFind != null) && (shiftToFind.Status == "OPEN"))
                            {
                                Sale.StoreID = session.StoreID;
                                Sale.Date = DateTime.Now;
                                database.Sales.Add(Sale);
                                //SAVE ACTIVITY
                                database.UserActivities.Add(new UserActivity()
                                {
                                    StoreID = session.StoreID
                                    ,
                                    UserID = session.UserID
                                    ,
                                    Activity = "CREATE Sale"
                                });
                                database.SaveChanges();

                                var message = Request.CreateResponse(HttpStatusCode.Created, "Create Success");
                                return message;
                            }
                            else
                            {
                                var message = Request.CreateResponse(HttpStatusCode.OK, "Shift not found or not OPEN");
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

        // PUT: api/Sale/5
        //UPDATE
        public HttpResponseMessage Put(int id, [FromBody]Sale Sale, string token)
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

                        var currentSale = database.Sales.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (currentSale != null)
                        {
                            var shiftToFind = database.Shifts.FirstOrDefault(x => x.ID == Sale.ShiftID && session.StoreID == x.StoreID);
                            if ((shiftToFind != null) && (shiftToFind.Status == "OPEN"))
                            {
                                currentSale.Title = Sale.Title;
                                currentSale.DiscountAmount = Sale.DiscountAmount;
                                currentSale.DiscountRate = Sale.DiscountRate;
                                currentSale.TaxAmunt = Sale.TaxAmunt;
                                currentSale.TaxRate = Sale.TaxRate;
                                currentSale.TotalPrice = Sale.TotalPrice;
                                currentSale.SubtotalPrice = Sale.SubtotalPrice;
                                currentSale.Status = Sale.Status;
                                currentSale.UserID = Sale.UserID;
                                currentSale.ShiftID = Sale.ShiftID;

                                //SAVE ACTIVITY
                                database.UserActivities.Add(new UserActivity()
                                {
                                    StoreID = session.StoreID
                                    , UserID = session.UserID
                                    , Activity = "CREATE Sale"
                                });

                                database.SaveChanges();
                                var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
                                return message;
                            }
                            else
                            {
                                var message = Request.CreateResponse(HttpStatusCode.OK, "Shift not found or not OPEN");
                                return message;
                            }
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.OK, "Sale Not found");
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

        // DELETE: api/Sale/5
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

                        var Sale = database.Sales.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

                        if (Sale == null)
                        {
                            return Request.CreateErrorResponse(HttpStatusCode.NotFound,
                                    "Sale with Id = " + id.ToString() + " not found to delete");
                        }
                        else
                        {

                            database.Sales.Remove(Sale);
                            //SAVE ACTIVITY
                            database.UserActivities.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,
                                UserID = session.UserID
                                ,
                                Activity = "DELETE Sale"
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
