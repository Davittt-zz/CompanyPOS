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
        // POST api/Session/Login
        // [Route("Login")]
        [HttpPost]
        public HttpResponseMessage PostLogin([FromBody] Users user)
        {
            string flow = "Before coneect to database || ";
            try{
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    flow += "Before search for users"; 
                    if (database.Users.ToList().Any(x => x.Username.Trim().Equals(user.Username.Trim()) && x.Password.Trim().Equals(user.Password.Trim())))
                    {
                        //Get user's data
                        Users userEntity = database.Users.ToList().FirstOrDefault(x => x.Username.Trim().Equals(user.Username.Trim()) && x.Password.Trim().Equals(user.Password.Trim()));

                        Session session = (database.Session.ToList().FirstOrDefault(x => x.UserID == userEntity.ID));

                        if (session == null)
                        {
                            session = new Session();
                            //Save SessionC:\Users\admin\Google Drive\Proyectos\Freelancer.com\CompanyPOS\projects\CompanyPOS\CompanyPOS\Controllers\SessionController.cs
                            session.StoreID = userEntity.StoreID;
                            session.TokenID = DateTime.Now.GetHashCode().GetHashCode().ToString() + session.StoreID;
                            session.UserID = userEntity.ID;
                            session.Created = DateTime.Now;
                            session.LastUpdate = session.Created;
                            database.Session.Add(session);
                        }
                        else
                        {
                            session.StoreID = userEntity.StoreID;
                            session.TokenID = DateTime.Now.GetHashCode().GetHashCode().ToString() + session.StoreID;
                            session.UserID = userEntity.ID;
                            session.LastUpdate = DateTime.Now;
                            //not add because us an Update
                            //   database.Session.Add(session);
                        }

                        //SAVE ACTIVITY
                        database.UserActivity.Add(new UserActivity()
                            {
                                StoreID = session.StoreID
                                ,
                                UserID = session.UserID
                                ,
                                Activity = "LOGIN"
                            }
                            );
                        database.SaveChanges();

                        var message = Request.CreateResponse(HttpStatusCode.Created, session);
                        message.Headers.Location = new Uri(Request.RequestUri + "/" + session.ID.ToString());
                        return message;
                    }
                    else
                    {
                        var message = Request.CreateResponse(HttpStatusCode.NotFound, "User or password invalid");
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex + " "+flow);
            }
        }

        [HttpPut]
        // PUT: api/Session/5
        public HttpResponseMessage PutLogout([FromBody] Session Session)
        {
            try
            {
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    Session session = database.Session.ToList().LastOrDefault(x => x.TokenID.Trim().Equals(Session.TokenID.Trim()));
                    if (session != null)
                    {
                        database.Session.Remove(session);

                        //SAVE ACTIVITY
                        database.UserActivity.Add(new UserActivity()
                        {
                            StoreID = session.StoreID
                            ,
                            UserID = session.UserID
                            ,
                            Activity = "LOGOUT"
                        }
                            );

                        database.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Logout Succesfully");
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "Nothing to Delete");
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public Session Autenticate(string token)
        {
            try
            {
                using (CompanyPOSEntities database = new CompanyPOSEntities())
                {
                    Session session = database.Session.ToList().FirstOrDefault(x => x.TokenID.Trim().Equals(token.Trim()));
                    if (session != null)
                    {
                        return session;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET: api/Session
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
        //  public void Post([FromBody]string value)
        //  {
        //  }
        //
        // PUT: api/Session/5
        //   public void Put(int id, [FromBody]string value)
        //   {
        //   }

        // DELETE: api/Session/5
        public void Delete(int id)
        {
        }
    }
}
