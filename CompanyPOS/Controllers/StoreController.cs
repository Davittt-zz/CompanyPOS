using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyPOS.Controllers
{
    public class StoreController : ApiController
    {
        // GET: api/Store
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}


        // GET: api/Store/5
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
                        Store store = database.Store.ToList().FirstOrDefault(x => x.ID == session.StoreID);
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
                        var message = Request.CreateResponse(HttpStatusCode.NoContent, "No asociated Session");
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
        public void Post([FromBody]string value)
        {

        }

        // PUT: api/Store/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Store/5
        public void Delete(int id)
        {
        }
    }
}
