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
    public class StoreController : ApiController
    {
		//// GET: api/Store
		//public HttpResponseMessage Get()
		//{
		//	try
		//	{
		//		using (CompanyPosDBContext database = new CompanyPosDBContext())
		//		{
		//			List<Store> storeList = database.Stores.ToList();

		//			if (storeList != null)
		//			{
		//				var message = Request.CreateResponse(HttpStatusCode.OK, storeList);
		//				return message;
		//			}
		//			else
		//			{
		//				var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
		//				return message;
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{
		//		return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
		//	}
		//}

        // GET: api/Store/5
        public HttpResponseMessage GetRead(string token, int id)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        Store store = database.Stores.ToList().FirstOrDefault(x => (x.ID == id));
                        if (store != null)
                        {
                            //Save last  update
                            session.LastUpdate = DateTime.Now;

                            database.SaveChanges();

                            var message = Request.CreateResponse(HttpStatusCode.OK, store);
                            return message;
                        }
                        else
                        {
                            var message = Request.CreateResponse(HttpStatusCode.NoContent, "No asociated Store");
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

        // POST: api/Store
        public HttpResponseMessage PostCreate([FromBody] Store store, string token)
        {
            try
            {
                using (CompanyPosDBContext database = new CompanyPosDBContext())
                {
                    SessionController sessionController = new SessionController();
                    Session session = sessionController.Autenticate(token);

                    if (session != null)
                    {
                        Store currentStore = database.Stores.ToList().FirstOrDefault(x => x.Name == store.Name);
                        if (currentStore != null)
                        {
                            //Save last  update
                            currentStore.CompanyID = database.Stores.FirstOrDefault(x => x.ID == session.StoreID).CompanyID;

                            session.LastUpdate = DateTime.Now;
                            database.SaveChanges();

                            var message = Request.CreateResponse(HttpStatusCode.NotModified, "There is a store with this name");
                            return message;
                        }
                        else
                        {
                            var User = database.Users.FirstOrDefault(x => (x.UserLevel == "admin")
                                && x.ID == session.UserID);
                            if (User != null)
                            {
                                store.CompanyID = database.Stores.FirstOrDefault(x => x.ID == session.StoreID).CompanyID;
                                database.Stores.Add(store);
                                //SAVE ACTIVITY
                               database.UserActivities.Add(new UserActivity()
                               {
                                   StoreID = session.StoreID
                                   ,
                                   UserID = session.UserID
                                   ,
								   Activity = "CREATE STORE",
								   Date = DateTime.Now
                               }
                                   );
                                database.SaveChanges();

                                var message = Request.CreateResponse(HttpStatusCode.Created, "Create Success");
                                return message;
                            }
                            else
                            {
                                var message = Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "The user does not have privileges for this operation");
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
    }
}
