using CompanyPOS.Models;
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
	public class ProductAmountController : ApiController, IRestWebservice<ProductAmount>
	{
		// GET: api/ProductAmount
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
						//Validate storeID
						var ItemsAttributeList = database.ProductAmounts.Where(x => x.StoreID == session.StoreID).ToList();
						var message = Request.CreateResponse(HttpStatusCode.OK, ItemsAttributeList);
						return message;
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

		// GET: api/ProductAmount/5
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
						//Validate storeID
						var ProductAmount = database.ProductAmounts.FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));

						if (ProductAmount != null)
						{
							//Save last  update
							session.LastUpdate = DateTime.Now;
							database.SaveChanges();

							var message = Request.CreateResponse(HttpStatusCode.OK, ProductAmount);
							return message;
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "ProductAmount not found");
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

		// POST: api/ProductAmount (CREATE)
		public HttpResponseMessage Post([FromBody]ProductAmount ProductAmount, string token)
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

						//Validate storeID
						var Item = database.Items.FirstOrDefault(x => x.StoreID == session.StoreID && x.ID == ProductAmount.ItemID);

						if (Item != null)
						{
							var Product = database.ProductTables.FirstOrDefault(x => x.StoreID == session.StoreID && x.ID == ProductAmount.ProductID);

							if (Product != null)
							{
								//var currentProductAmount = Item.ID;

								//if ((currentProductAmount != null) && (currentProductAmount.ToList().Exists(x => (x.Name == ProductAmount.Name))))
								//{
								//	var message = Request.CreateResponse(HttpStatusCode.OK, "There is an ProductAmount with this name");
								//	return message;
								//}
								//else
								//{
								ProductAmount.StoreID = session.StoreID;
								database.ProductAmounts.Add(ProductAmount);
								//SAVE ACTIVITY
								database.UserActivities.Add(new UserActivity()
								{
									StoreID = session.StoreID
									,
									UserID = session.UserID
									,
									Activity = "CREATE ProductAmount",
									Date = DateTime.Now
								});

								database.SaveChanges();
								var message = Request.CreateResponse(HttpStatusCode.Created, "Create Success");
								return message;
								//}
							}
							else
							{
								var message = Request.CreateResponse(HttpStatusCode.OK, "Product not found");
								return message;
							}
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.OK, "Item not found");
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

		// PUT: api/ProductAmount/5
		//UPDATE
		public HttpResponseMessage Put(int Id, [FromBody]ProductAmount ProductAmount, string token)
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

						//Validate storeID
						var Item = database.Items.FirstOrDefault(x => x.StoreID == session.StoreID && x.ID == ProductAmount.ItemID);

						if (Item != null)
						{
							var Product = database.ProductTables.FirstOrDefault(x => x.StoreID == session.StoreID && x.ID == ProductAmount.ProductID);

							if (Product != null)
							{

								var currentProductAmount = database.ProductAmounts.ToList().FirstOrDefault(x => x.ID == Id && (x.StoreID == session.StoreID));

								if (currentProductAmount != null)
								{
									currentProductAmount.Name = ProductAmount.Name;
									currentProductAmount.Amount = ProductAmount.Amount ?? currentProductAmount.Amount;
									currentProductAmount.Unit = ProductAmount.Unit ?? currentProductAmount.Unit;
									
									//SAVE ACTIVITY
									database.UserActivities.Add(new UserActivity()
									{
										StoreID = session.StoreID
										,
										UserID = session.UserID
										,
										Activity = "CREATE ProductAmount",
										Date = DateTime.Now
									});

									database.SaveChanges();
									return Request.CreateResponse(HttpStatusCode.OK, "Update Success");
								}
								else
								{
									return Request.CreateResponse(HttpStatusCode.OK, "ProductAmount Item not found");
								}
							}
							else
							{
								return Request.CreateResponse(HttpStatusCode.OK, "Product not found");
							}
						}
						else
						{
							return Request.CreateResponse(HttpStatusCode.OK, "Item not found");
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

		// DELETE: api/ProductAmount/5
		//DELETE
		public HttpResponseMessage Delete(int Id, string token)
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

						var ProductAmount = database.ProductAmounts.ToList().FirstOrDefault(x => x.ID == Id && (x.StoreID == session.StoreID));

						if (ProductAmount == null)
						{
							return Request.CreateErrorResponse(HttpStatusCode.NotFound,
									"ProductAmount with Id = " + Id.ToString() + " not found to delete");
						}
						else
						{
							database.ProductAmounts.Remove(ProductAmount);
							//SAVE ACTIVITY

							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "DELETE ItPagPos",
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
