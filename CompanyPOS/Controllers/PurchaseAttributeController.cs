using DATA;
using DATA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CompanyPOS.Controllers
{
	public class PurchaseAttributeController : ApiController
    {
		// GET: api/PurchaseAttribute
		public HttpResponseMessage Get()
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					var data = database.PurchaseAttributes.ToList();
					var message = Request.CreateResponse(HttpStatusCode.OK, data);
					return message;
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// GET: api/PurchaseAttribute/5
		public HttpResponseMessage Get(string token, int itemPurchaseID)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);
					if (session != null)
					{
						//Validate storeID and PurchaseAttributeID
						var data = database.PurchaseAttributes.ToList().FirstOrDefault(x => (x.ItemPurchaseID == itemPurchaseID) && (x.StoreID == session.StoreID));
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
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "PurchaseAttribute not found");
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

		// GET: api/PurchaseAttribute/5
		public HttpResponseMessage Get(string token, string id)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();
					Session session = sessionController.Autenticate(token);
					if (session != null)
					{
						//Validate storeID and PurchaseAttributeID
						var data = database.PurchaseAttributes.ToList().FirstOrDefault(x => (x.ID == Int32.Parse(id)) && (x.StoreID == session.StoreID));
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
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "PurchaseAttribute not found");
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

		// POST: api/PurchaseAttribute
		//CREATE
		public HttpResponseMessage Post([FromBody]PurchaseAttribute PurchaseAttribute, string token)
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

						var currentPurchaseAttribute = database.PurchaseAttributes.ToList().FirstOrDefault(x => (x.StoreID == session.StoreID) && (x.ItemPurchaseID == PurchaseAttribute.ItemPurchaseID));
						if (currentPurchaseAttribute != null)
						{
							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "There is a PurchaseAttribute assosiated with the same Item");
							return message;
						}
						else
						{
							if (database.Items.Any(x => x.ID == PurchaseAttribute.ItemPurchaseID))
							{
									PurchaseAttribute.StoreID = session.StoreID;
									database.PurchaseAttributes.Add(PurchaseAttribute);
									//SAVE ACTIVITY
									database.UserActivities.Add(new UserActivity()
									{
										StoreID = session.StoreID
										,
										UserID = session.UserID
										,
										Activity = "CREATE PurchaseAtt",
										Date = DateTime.Now
									});
									database.SaveChanges();

									var message = Request.CreateResponse(HttpStatusCode.Created, "Create Success");
									return message;
							}
							else
							{
								var message = Request.CreateResponse(HttpStatusCode.OK, "Item not found");
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

		// PUT: api/PurchaseAttribute/5
		//UPDATE
		public HttpResponseMessage Put(int id, [FromBody]PurchaseAttribute PurchaseAttribute, string token)
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
						var currentPurchaseAttribute = database.PurchaseAttributes.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));
						if (currentPurchaseAttribute != null)
						{
							currentPurchaseAttribute.Quantity = PurchaseAttribute.Quantity;
							currentPurchaseAttribute.TotalPrice = PurchaseAttribute.TotalPrice;
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "UPDATE PurchaseAttr",
								Date = DateTime.Now
							});
							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
							return message;
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.OK, "PurchaseAttribute Not found");
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

		// PUT: api/PurchaseAttribute/5
		//UPDATE
		public HttpResponseMessage Put(string ItemPurchaseID, [FromBody]PurchaseAttribute PurchaseAttribute, string token)
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
						var currentPurchaseAttribute = database.PurchaseAttributes.ToList().FirstOrDefault(x => x.ItemPurchaseID.ToString() == ItemPurchaseID && (x.StoreID == session.StoreID));
						if (currentPurchaseAttribute != null)
						{
							currentPurchaseAttribute.Quantity = PurchaseAttribute.Quantity;
							currentPurchaseAttribute.TotalPrice = PurchaseAttribute.TotalPrice;
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "UPDATE PurchaseAttr",
								Date = DateTime.Now
							});
							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
							return message;
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.OK, "PurchaseAttribute Not found");
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

		// DELETE: api/PurchaseAttribute/5
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
						var PurchaseAttribute = database.PurchaseAttributes.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));
						if (PurchaseAttribute == null)
						{
							return Request.CreateErrorResponse(HttpStatusCode.NotFound,
									"PurchaseAttribute with Id = " + id.ToString() + " not found to delete");
						}
						else
						{
							database.PurchaseAttributes.Remove(PurchaseAttribute);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "DELETE PurchaseAtt",
								Date = DateTime.Now
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

		// DELETE: api/PurchaseAttribute/5
		//DELETE
		public HttpResponseMessage Delete(string ItemPurchaseID, string token)
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
						var PurchaseAttribute = database.PurchaseAttributes.ToList().FirstOrDefault(x => x.ItemPurchaseID.ToString() == ItemPurchaseID && (x.StoreID == session.StoreID));
						if (PurchaseAttribute == null)
						{
							return Request.CreateErrorResponse(HttpStatusCode.NotFound,
									"PurchaseAttribute with ItemPurchaseID = " + ItemPurchaseID + " not found to delete");
						}
						else
						{
							database.PurchaseAttributes.Remove(PurchaseAttribute);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "DELETE PurchaseAtt",
								Date = DateTime.Now
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