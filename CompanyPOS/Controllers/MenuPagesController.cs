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
	public class MenuPageController : ApiController
	{
		// GET: api/Menu
		public HttpResponseMessage Get(string token)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);

					if (session != null)
					{
						//Validate storeID and MenuID
						var data = database.MenuPages.ToList().Where(x => (x.StoreID == session.StoreID));
						if ((data != null) && (data.Count() > 0))
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

		// GET: api/Menu/5
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
						//Validate storeID and MenuID
						var data = database.MenuPages.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
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
		// CREATE
		public HttpResponseMessage Post([FromBody]MenuPage menuPage, string token)
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

						var currentMenu = database.MenuPages.ToList().FirstOrDefault(x => x.Name == menuPage.Name && (x.StoreID == session.StoreID));
						if (currentMenu != null)
						{
							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "There is a menu page with this name");
							return message;
						}
						else
						{

							var menuList = database.MenuPages.ToList().FindAll(x => x.StoreID == session.StoreID);

							if ((menuList == null) || (menuList.Count == 0))
							{
								menuPage.Page = 1;

								var menu = database.Menues.ToList().FirstOrDefault(x => x.StoreID == session.StoreID);

								if (menu == null)
								{
									return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Menu not found");
								}
								else
								{
									menuPage.MenuID = menu.ID;
								}
							}
							else
							{
								var maximum = menuList.Max(obj => obj.Page);
								menuPage.Page = menuList.Find(obj => obj.Page == maximum).Page + 1;
								menuPage.MenuID = menuList.First().MenuID;
							}

							menuPage.StoreID = session.StoreID;
							database.MenuPages.Add(menuPage);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "CREATE MENU"
							});
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

		// PUT: api/Menu/5
		// UPDATE
		public HttpResponseMessage Put(int id, [FromBody]MenuPage menuPage, string token)
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

						var currentMenu = database.MenuPages.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (currentMenu != null)
						{
							//currentMenu.Page = menuPage.Page;
							currentMenu.Description = menuPage.Description;
							currentMenu.Name = menuPage.Name;
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "CREATE MENU"
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
		// DELETE
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

						var menu = database.MenuPages.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (menu == null)
						{
							return Request.CreateErrorResponse(HttpStatusCode.NotFound,
									"Menu with Id = " + id.ToString() + " not found to delete");
						}
						else
						{
							database.MenuPages.Remove(menu);

							var menuePages = from menuPage in database.MenuPages
											 where ((menuPage.Page > menu.Page) && (menuPage.StoreID == session.StoreID))
											 select menuPage;

							foreach (var item in menuePages)
								item.Page = item.Page - 1;

							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
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
