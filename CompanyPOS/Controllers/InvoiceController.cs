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
using System.Web.Http.Description;

namespace CompanyPOS.Controllers
{
	public class InvoiceController : ApiController
	{
		public List<Invoice> ReadAll(int StoreID, int SaleID)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					//var InvoiceList = database.Invoices.Where(x => x.Sale.StoreID == StoreID && x.SaleID == SaleID).ToList();
					var InvoiceList = database.Invoices.Where(x => x.SaleID == SaleID).ToList();

					return InvoiceList;
				}
			}
			catch (Exception ex)
			{
				return null;
				//  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		public void Create(int StoreID, int SaleID, DateTime Date, string PaymentMethod, string TotalPrice)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					database.Invoices.Add(new Invoice()
					{
						//StoreID = StoreID
						//,
						SaleID = SaleID
						,
						Date = Date
						,
						PaymentMethod = PaymentMethod
					});


					database.SaveChanges();


					//    var message = Request.CreateResponse(HttpStatusCode.OK, data);
					//    return message;
					//}
					//else
					//{
					//    var message = Request.CreateResponse(HttpStatusCode.NotFound, "Invoice not found");
					//    return message;
					//}
				}
			}
			catch (Exception ex)
			{
				//  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}

		}

		public void Delete(int InvoiceID, int StoreID)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					//var Invoice = database.Invoices.FirstOrDefault(x => x.ID == InvoiceID && x.Sale.StoreID == StoreID);
					var Invoice = database.Invoices.FirstOrDefault(x => x.ID == InvoiceID);

					if (Invoice != null)
					{
						database.Invoices.Remove(Invoice);
						database.SaveChanges();
					}

					//    var message = Request.CreateResponse(HttpStatusCode.OK, data);
					//    return message;
					//}
					//else
					//{
					//    var message = Request.CreateResponse(HttpStatusCode.NotFound, "Invoice not found");
					//    return message;
					//}
				}
			}
			catch (Exception ex)
			{
				//  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		public void Update(int InvoiceID, int StoreID, DateTime Date, string PaymentMethod, string TotalPrice)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					//var Invoice = database.Invoices.FirstOrDefault(x => x.ID == InvoiceID && x.Sale.StoreID == StoreID);
					var Invoice = database.Invoices.FirstOrDefault(x => x.ID == InvoiceID);


					if (Invoice != null)
					{
						Invoice.Date = Date;
						Invoice.PaymentMethod = PaymentMethod;
						//Update
						database.SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				//  return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// GET: api/Invoice
		//public IEnumerable<string> Get()
		//{
		//    return new string[] { "value1", "value2" };
		//}

		//  GET: api/Invoice/5
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
						//Validate storeID and InvoiceID
						var data = database.Invoices.ToList().FirstOrDefault(x => (x.ID == id) && (x.StoreID == session.StoreID));
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
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "Invoice not found");
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

		//  GET: api/Invoice/
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
						//Validate storeID and InvoiceID
						var data = database.Invoices.ToList();
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
							var message = Request.CreateResponse(HttpStatusCode.NotFound, "Invoice not found");
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

		// POST: api/Invoice
		//CREATE
		public HttpResponseMessage Post(int saleId, [FromBody]Invoice Invoice, string token)
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

						var currentInvoice = database.Invoices.ToList().FirstOrDefault(x => (x.SaleID == saleId) && (x.StoreID == session.StoreID));
						if (currentInvoice != null)
						{
							database.SaveChanges();
							var message = Request.CreateResponse(HttpStatusCode.OK, "There is a Invoice with this name");
							return message;
						}
						else
						{
							if (!(database.Invoices.Any(x => x.SaleID == saleId && x.StoreID == session.StoreID)))
							{
								if ((database.Sales.Any(x => x.ID == saleId)))
								{
									if (Invoice.Date == null)
									{
										return Request.CreateResponse(HttpStatusCode.OK, "Date not found." + Invoice.errorMessage);
									}

									Invoice.StoreID = session.StoreID;
									Invoice.SaleID = saleId;

									database.Invoices.Add(Invoice);
									//SAVE ACTIVITY
									database.UserActivities.Add(new UserActivity()
									{
										StoreID = session.StoreID
										,
										UserID = session.UserID
										,
										Activity = "CREATE Invoice",
										Date = DateTime.Now
									});
									database.SaveChanges();

									return Request.CreateResponse(HttpStatusCode.Created, "Create Success");
								}
								else
								{
									return Request.CreateResponse(HttpStatusCode.OK, "Sale not found.");
								}
							}
							else
							{
								return Request.CreateResponse(HttpStatusCode.OK, "Sale has already an Invoice");
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
			catch (System.FormatException ex)
			{
				var message = Request.CreateResponse(HttpStatusCode.BadRequest, @"Bad Datetime format, it must be (YYYY-DD-MM-hh-mm-ss)");
				return message;
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// PUT: api/Invoice/5
		//UPDATE
		public HttpResponseMessage Put(int id, [FromBody]Invoice Invoice, string token)
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

						var currentInvoice = database.Invoices.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (currentInvoice != null)
						{
							currentInvoice.PaymentMethod = Invoice.PaymentMethod;

							if (Invoice.Date == null)
							{
								return Request.CreateResponse(HttpStatusCode.OK, "Date not found.");
							}

							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "UPDATE Invoice",
								Date = DateTime.Now
							});

							database.SaveChanges();
							return Request.CreateResponse(HttpStatusCode.OK, "Update Success");

						}
						else
						{
							return Request.CreateResponse(HttpStatusCode.OK, "Invoice Not found");
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

		// DELETE: api/Invoice/5
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

						var Invoice = database.Invoices.ToList().FirstOrDefault(x => x.ID == id && (x.StoreID == session.StoreID));

						if (Invoice == null)
						{
							return Request.CreateErrorResponse(HttpStatusCode.NotFound,
									"Invoice with Id = " + id.ToString() + " not found to delete");
						}
						else
						{

							database.Invoices.Remove(Invoice);
							//SAVE ACTIVITY
							database.UserActivities.Add(new UserActivity()
							{
								StoreID = session.StoreID
								,
								UserID = session.UserID
								,
								Activity = "DELETE Invoice",
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

		// GET: api/Products/5 
		[ResponseType(typeof(Invoice))]
		public IHttpActionResult GetInvoice(int id)
		{
			Invoice invoice = new Invoice()
			{
				ID = 1234567,
				Date = DateTime.Now
			};
			if (invoice == null)
			{
				return NotFound();
			}

			return Ok(invoice);
		}
	}
}
