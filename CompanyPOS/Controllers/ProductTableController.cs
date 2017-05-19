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
	public class ProductTableController : ApiController
	{
		// GET: api/Products 
		public IQueryable<ProductTable> GetProductTable()
		{
			using (CompanyPosDBContext database = new CompanyPosDBContext())
			{
				return database.ProductTables;
			}
		}

		// GET: api/ProductTable
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
						//Validate storeID and ProductTableID
						var data = database.ProductTables.ToList().Where(x => (x.StoreID == session.StoreID));

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
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "ProductTable not found");
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

		// GET: api/ProductTable/5
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
						//Validate storeID and ProductTableID
						var data = database.ProductTables.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
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
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "ProductTable not found");
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

		// POST: api/ProductTable
		//CREATE
		public HttpResponseMessage Post([FromBody]ProductTable ProductTable, string token)
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
						var currentProductTable = database.ProductTables.ToList().FirstOrDefault(x => x.Name.ToLower().Trim() == ProductTable.Name.ToLower().Trim() && (x.StoreID == session.StoreID));
						if (currentProductTable != null)
						{
							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "There is a ProductTable with this name");
							return message;
						}
						else
						{
							ProductTable.StoreID = session.StoreID;
							ProductTable.LastUpdated = DateTime.Now;
							database.ProductTables.Add(ProductTable);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								, UserID = session.UserID
								, Activity = "CREATE ProductTable"
								, Date = DateTime.Now
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

		// PUT: api/ProductTable/5
		//UPDATE
		public HttpResponseMessage Put(int id, [FromBody]ProductTable ProductTable, string token)
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

						var currentProductTable = database.ProductTables.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (currentProductTable != null)
						{
							currentProductTable.Name = ProductTable.Name;
							currentProductTable.InStock = ProductTable.InStock;
							currentProductTable.LastUpdated = DateTime.Now;

							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,UserID = session.UserID
								,Activity = "CREATE ProductTable"
								,Date = DateTime.Now
							});

							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
							return message;
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.OK, "ProductTable Not found");
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

		// DELETE: api/ProductTable/5
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

						var ProductTable = database.ProductTables.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (ProductTable == null)
						{
							return Request.CreateErrorResponse(HttpStatusCode.NotFound,
									"ProductTable with Id = " + id.ToString() + " not found to delete");
						}
						else
						{
							database.ProductTables.Remove(ProductTable);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,UserID = session.UserID
								,Activity = "DELETE ProductTable"
								,Date = DateTime.Now
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
