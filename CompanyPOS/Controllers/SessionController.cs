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
	public class SessionController : ApiController
	{
		//GET
		//api/Session/
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
						var SessionUser = database.Users.FirstOrDefault(x => x.ID == session.UserID);
						if (SessionUser.UserLevel.ToLower() == "admin")
						{
							var sessionList = database.Sessions.ToList();
							var message = Request.CreateResponse(HttpStatusCode.OK, sessionList);
							return message;
						}
						else
						{
							return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "You don't have privileges");
						}
					}
					else
					{
						return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
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

		// POST api/Session/Login
		// [Route("Login")]
		[HttpPost]
		public HttpResponseMessage PostLogin([FromBody] User user, string UUID)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					if (database.Users.ToList().Any(x => x.Username.Trim().Equals(user.Username.Trim()) && x.Active == true))
					{
						if (database.Users.ToList().Any(x => x.Username.Trim().Equals(user.Username.Trim()) && x.Password.Trim().Equals(user.Password.Trim())))
						{
							//Get user's data
							User userEntity = database.Users.ToList().FirstOrDefault(x => x.Username.Trim().Equals(user.Username.Trim()) && x.Password.Trim().Equals(user.Password.Trim()));
													
							Session session = (database.Sessions.ToList().FirstOrDefault(x => x.UserID == userEntity.ID));

							if (session == null)
							{
								session = new Session();
								//Save Session
								session.StoreID = userEntity.StoreID;
								session.TokenID = DateTime.Now.GetHashCode().GetHashCode().ToString() + session.StoreID;
								session.UserID = userEntity.ID;
								session.Created = DateTime.Now;
								session.LastUpdate = session.Created;
								database.Sessions.Add(session);
							}
							else
							{
								session.StoreID = userEntity.StoreID;
								session.TokenID = DateTime.Now.GetHashCode().GetHashCode().ToString() + session.StoreID;
								session.UserID = userEntity.ID;
								session.LastUpdate = DateTime.Now;
								//not add because us an Update
								//database.Sessions.Add(session);
							}

							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
								{
									StoreID = session.StoreID
									 ,	UserID = session.UserID
									 ,	Activity = "LOGIN"
									 ,	Date = DateTime.Now
								}
								);
							database.SaveChanges();

							var message = Request.CreateResponse(HttpStatusCode.Created, session);
							message.Headers.Location = new Uri(Request.RequestUri + "/" + session.ID.ToString());
							return message;
						}
						else
						{
							return Request.CreateResponse(HttpStatusCode.NotFound, "User or password invalid");
						}
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.NotFound, "User invalid or Inactive");
						return message;
					}
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}
		
		[HttpPut]
		// PUT: api/Session/5
		public HttpResponseMessage PutLogout([FromBody] Session Session)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					Session session = database.Sessions.ToList().LastOrDefault(x => x.TokenID.Trim().Equals(Session.TokenID.Trim()));
					if (session != null)
					{
						database.Sessions.Remove(session);

						//SAVE ACTIVITY
						database.UserActivities.Add(new UserActivity()
						{
							StoreID = session.StoreID
						   ,
							UserID = session.UserID
						   ,
							Activity = "LOGOUT",
							Date = DateTime.Now
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
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					Session session = database.Sessions.ToList().FirstOrDefault(x => x.TokenID.Trim().Equals(token.Trim()));
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
