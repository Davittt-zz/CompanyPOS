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
	public class FakturiController : ApiController, IRestWebservice<Fakturi>
	{
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
						//Validate storeID and FakturiID
						var data = database.Fakturies.ToList().Where(x => (x.StoreID == session.StoreID)).ToList();

						//le agrego la lista de c/u
						data.ForEach(
						x => x.Items = database.FakturiArticles.Where(y => y.FakturiID == x.ID).ToList()
						);

						if ((data != null) && (data.Count() > 0))
						{
							//Save last  update
							session.LastUpdate = DateTime.Now;
							database.SaveChanges();
							return Request.CreateResponse(HttpStatusCode.OK, data);
						}
						else
						{
							return Request.CreateResponse(HttpStatusCode.NotFound, "Fakturi not found");
						}
					}
					else
					{
						return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
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
						//Validate storeID and FakturiID
						var data = database.Fakturies.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
						//le agrego la lista de c/u
						data.Items = database.FakturiArticles.Where(y => y.FakturiID == data.ID).ToList();

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
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "Fakturi not found");
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
						//Validate storeID and FakturiID
						var data = database.Fakturies.ToList().FirstOrDefault(x => (x.AssociatesID == Int32.Parse(AssociatedId)) && (x.StoreID == session.StoreID));
						//le agrego la lista de c/u
						data.Items = database.FakturiArticles.Where(y => y.FakturiID == data.ID).ToList();

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
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "Fakturi not found");
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
						//Validate storeID and FakturiID
						var data = database.Fakturies.ToList().FirstOrDefault(x => (x.InvoiceNumber == InvoiceNumber)
							&& (x.StoreID == session.StoreID));

						//le agrego la lista de c/u
						data.Items = database.FakturiArticles.Where(y => y.FakturiID == data.ID).ToList();

						if (data != null)
						{
							data.Items = database.FakturiArticles.Where(x => x.FakturiID == data.ID).ToList();
							//Save last  update
							session.LastUpdate = DateTime.Now;
							database.SaveChanges();

							var message = Request.CreateResponse(HttpStatusCode.OK, data);
							return message;
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "Fakturi not found");
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
						//Validate storeID and FakturiID
						var data = database.Fakturies.Where(x => (x.Date <= endDate) && (x.Date >= startDate) && (x.StoreID == session.StoreID)).ToList();
						//le agrego la lista de c/u
						data.ForEach(
						x => x.Items = database.FakturiArticles.Where(y => y.FakturiID == x.ID).ToList()
						);

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
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "Fakturi not found");
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

		public HttpResponseMessage Post(Fakturi Item, string token)
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

						var Fakturies = database.Fakturies.Where(x => x.StoreID == session.StoreID);
						if (Fakturies != null)
						{
							Item.Date = DateTime.Now;
							Item.StoreID = session.StoreID;

							if (Item.SystemNumber == null)
								return Request.CreateResponse(HttpStatusCode.OK, "SystemNumber attribute not found (Auto-Generated, Manual)");

							if ((Item.InvoiceNumber == null) && (Item.SystemNumber.ToLower() == "manual"))
								return Request.CreateResponse(HttpStatusCode.OK, "SystemNumber is manual but Item.InvoiceNumber is null");

							if ((Item.InvoiceNumber == null) || Item.SystemNumber.ToLower().Equals(("Auto-Generated").ToLower()))
							{
								var newFakturi = "000000000001";
								if (Fakturies.Count() > 0)
								{
									var lastFakturi = Fakturies.ToList().Max(i => Int64.Parse(i.InvoiceNumber));
									newFakturi = (lastFakturi + 1).ToString().PadLeft(12, '0');
								}
								Item.InvoiceNumber = newFakturi;
							}

							var currentFakturi = database.Fakturies.ToList().FirstOrDefault(x => (x.StoreID == session.StoreID) && (x.InvoiceNumber == Item.InvoiceNumber));
							if (currentFakturi != null)
							{
								return Request.CreateResponse(HttpStatusCode.OK, "There is already a Fakturi with this FakturiNumber");
							}

							database.Fakturies.Add(Item);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "CREATE Fakturi"
								,
								Date = DateTime.Now
							});
							database.SaveChanges();
							return Request.CreateResponse(HttpStatusCode.Created, "Create Success");

						}
						else
						{
							return Request.CreateResponse(HttpStatusCode.MethodNotAllowed, "No asociated Session");
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

		public HttpResponseMessage Put(int id, Fakturi Item, string token)
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

						var currentFakturi = database.Fakturies.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (currentFakturi != null)
						{
							currentFakturi.InvoiceNumber = Item.InvoiceNumber ?? currentFakturi.InvoiceNumber;
							currentFakturi.AssociatesID = Item.AssociatesID;
							currentFakturi.Date = Item.Date;
							currentFakturi.PaymentType = Item.PaymentType ?? currentFakturi.PaymentType;
							currentFakturi.Details = Item.Details ?? currentFakturi.Details;
							currentFakturi.Notes = Item.Notes ?? currentFakturi.Notes;
							currentFakturi.Date = DateTime.Now;
							currentFakturi.Total = Item.Total ?? currentFakturi.Total;
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "CREATE Fakturi"
								,
								Date = DateTime.Now
							});

							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
							return message;
						}
						else
						{
							var message = Request.CreateResponse(HttpStatusCode.OK, "Fakturi Not found");
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

						var Fakturi = database.Fakturies.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (Fakturi == null)
						{
							return Request.CreateErrorResponse(HttpStatusCode.NotFound,
									"Fakturi with Id = " + id.ToString() + " not found to delete");
						}
						else
						{

							database.Fakturies.Remove(Fakturi);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "DELETE Fakturi"
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