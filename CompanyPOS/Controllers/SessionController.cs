using DATA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyPOS.Controllers
{
    public class SessionController : ApiController
    {

        // POST api/Account/Login
        [Route("Login")]
        public string Get([FromUri] Users user)
        {
            using (CompanyPOSEntities database = new CompanyPOSEntities())
            {
                return database.Users.ToList().Any(x => x.Name == user.Name && x.Password == user.Password).ToString();
            }
        }


        //// GET: api/Session
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/Session/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST: api/Session
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Session/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Session/5
        public void Delete(int id)
        {
        }
    }
}
