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
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CompanyPOS.Controllers
{
	public class FakturiArticleController : ApiController
	//, IRestWebservice<FakturiArticle>
	{
		/*	public HttpResponseMessage Get(string token)
			{
				try
				{
					using (CompanyPosDBContext database = new CompanyPosDBContext())
					{
						SessionController sessionController = new SessionController();
						Session session = sessionController.Autenticate(token);

						if (session != null)
						{
							//Validate storeID and FakturiArticleID
							var data = database.FakturiArticles.ToList().Where(x => (x.StoreID == session.StoreID));

							if ((data != null) && (data.Count()>0) )
							{
								//Save last  update
								session.LastUpdate = DateTime.Now;
								database.SaveChanges();
								return Request.CreateResponse(HttpStatusCode.OK, data);
							}
							else
							{
								var message = Request.CreateResponse(HttpStatusCode.NotFound, "FakturiArticle not found");
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
							//Validate storeID and FakturiArticleID
							var data = database.FakturiArticles.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
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
								var message = Request.CreateResponse(HttpStatusCode.NotFound, "FakturiArticle not found");
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

			public HttpResponseMessage Get(string token, string AssociatedId)
			{
				try
				{
					using (CompanyPosDBContext database = new CompanyPosDBContext())
					{
						SessionController sessionController = new SessionController();
						Session session = sessionController.Autenticate(token);

						if (session != null)
						{
							//Validate storeID and FakturiArticleID
							var data = database.FakturiArticles.ToList().FirstOrDefault(x => (x.AssociatesID == Int32.Parse(AssociatedId)) && (x.StoreID == session.StoreID));
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
								var message = Request.CreateResponse(HttpStatusCode.NotFound, "FakturiArticle not found");
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

			public HttpResponseMessage Get(string token, string InvoiceNumber, bool active = true)
			{
				try
				{
					using (CompanyPosDBContext database = new CompanyPosDBContext())
					{
						SessionController sessionController = new SessionController();
						Session session = sessionController.Autenticate(token);

						if (session != null)
						{
							//Validate storeID and FakturiArticleID
							var data = database.FakturiArticles.ToList().Where(x => (x.InvoiceNumber == InvoiceNumber) && (x.StoreID == session.StoreID));
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
								var message = Request.CreateResponse(HttpStatusCode.NotFound, "FakturiArticle not found");
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

			public HttpResponseMessage Get(string token, string start, string end)
			{
				try
				{
					using (CompanyPosDBContext database = new CompanyPosDBContext())
					{
						SessionController sessionController = new SessionController();
						Session session = sessionController.Autenticate(token);

						string[] startPart = start.Split('-');
						DateTime startDate = new DateTime(Int32.Parse(startPart[0])
						   , Int32.Parse(startPart[1])
						   , Int32.Parse(startPart[2])
						   , Int32.Parse(startPart[3])
						   , Int32.Parse(startPart[4])
						   , Int32.Parse(startPart[5]));
						string[] endPart = end.Split('-');
						DateTime endDate = new DateTime(Int32.Parse(endPart[0])
						   , Int32.Parse(endPart[1])
						   , Int32.Parse(endPart[2])
						   , Int32.Parse(endPart[3])
						   , Int32.Parse(endPart[4])
						   , Int32.Parse(endPart[5]));

						if (session != null)
						{
							//Validate storeID and FakturiArticleID
							var data = database.FakturiArticles.ToList().Where(x => (x.Date <= endDate) && (x.Date >= startDate) && (x.StoreID == session.StoreID));
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
								var message = Request.CreateResponse(HttpStatusCode.NotFound, "FakturiArticle not found");
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
	*/

		public HttpResponseMessage Post(FakturiArticle Item, string token)
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
						var currentFakturiArticle = database.FakturiArticles.ToList().FirstOrDefault(x => (x.FakturiID == Item.FakturiID) && (x.ProductTableID == Item.ProductTableID) && (x.StoreID == session.StoreID));

						if (currentFakturiArticle != null)
						{
							database.SaveChanges();
							return Request.CreateResponse(HttpStatusCode.OK, "This Product is already in the Faktury");
						}
						else
						{
							//Validate storeID and FakturiID
							var data = database.Fakturies.ToList().FirstOrDefault(x => (x.ID == Item.FakturiID)
								&& (x.StoreID == session.StoreID));

							if (data != null)
							{
								Item.StoreID = session.StoreID;
								database.FakturiArticles.Add(Item);
								//SAVE ACTIVITY
								database.UserActivities.Add(new UserActivity()
								{
									StoreID = session.StoreID
									,
									UserID = session.UserID
									,
									Activity = "CREATE FakturiArticle"
									,
									Date = DateTime.Now
								});
								database.SaveChanges();
								return Request.CreateResponse(HttpStatusCode.Created, "Create Success");
							}
							else
							{
								var message = Request.CreateResponse(HttpStatusCode.NotFound, "Fakturi not found");
								return message;
							}
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

		public HttpResponseMessage Put(int id, FakturiArticle Item, string token)
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

						var currentFakturiArticle = database.FakturiArticles.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (currentFakturiArticle != null)
						{
							//Validate storeID and FakturiID
							var data = database.Fakturies.ToList().FirstOrDefault(x => (x.ID == Item.FakturiID)
								&& (x.StoreID == session.StoreID));

							if (data != null)
							{
								var ItemWithProduct = database.ProductTables.ToList().FirstOrDefault(x => (x.ID == Item.ProductTableID) && (x.StoreID == session.StoreID));
								if (ItemWithProduct != null)
								{
									currentFakturiArticle.Unit = Item.Unit ?? currentFakturiArticle.Unit;
									currentFakturiArticle.Item = Item.Item ?? currentFakturiArticle.Item;
									currentFakturiArticle.Qty = Item.Qty;
									currentFakturiArticle.Price = Item.Price ?? currentFakturiArticle.Price;
									currentFakturiArticle.Tax = Item.Tax ?? currentFakturiArticle.Tax;

									//SAVE ACTIVITY
									database.UserActivities.Add(new UserActivity()
									{
										StoreID = session.StoreID
										,
										UserID = session.UserID
										,
										Activity = "CREATE FakturiArticle"
										,
										Date = DateTime.Now
									});

									database.SaveChanges();
									var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
									return message;
								}
								else
								{
									var message = Request.CreateResponse(HttpStatusCode.NotFound, "Product not found");
									return message;
								}
							}
							else
							{
								var message = Request.CreateResponse(HttpStatusCode.NotFound, "Fakturi not found");
								return message;
							}
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.OK, "FakturiArticle Not found");
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

						var FakturiArticle = database.FakturiArticles.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (FakturiArticle == null)
						{
							return Request.CreateErrorResponse(HttpStatusCode.NotFound,
									"FakturiArticle with Id = " + id.ToString() + " not found to delete");
						}
						else
						{

							database.FakturiArticles.Remove(FakturiArticle);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "DELETE FakturiArticle"
								,
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