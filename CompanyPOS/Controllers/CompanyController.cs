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
	public class CompanyController : ApiController
	{
		// GET: api/Company/
		public HttpResponseMessage GetAll()
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					//Validate storeID and CompanyID
					var data = database.Companies.ToList();
					if (data != null)
					{
						var message = Request.CreateResponse(HttpStatusCode.OK, data);
						return message;
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.NotFound, "Companies not found");
						return message;
					}
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// GET: api/Company/5
		public HttpResponseMessage GetCompany(int id)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{

					//Validate storeID and CompanyID
					var data = database.Companies.ToList().FirstOrDefault(x => (x.Id == id));
					if (data != null)
					{
						var message = Request.CreateResponse(HttpStatusCode.OK, data);
						return message;
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.NotFound, "Company not found");
						return message;
					}
				}
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
			}
		}

		// POST: api/Company
		//CREATE
		public HttpResponseMessage Post([FromBody]Company Company)
		{
			string errorStatus = " ";
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					SessionController sessionController = new SessionController();

					errorStatus += " Before Find similar Company || ";
					var currentCompany = database.Companies.ToList().FirstOrDefault(x => x.Name == Company.Name);
					if (currentCompany != null)
					{
						database.SaveChanges();
						var message = Request.CreateResponse(HttpStatusCode.OK, "There is a Company with this name");
						return message;
					}
					else
					{
						database.Companies.Add(Company);

						errorStatus += " Before adding in the db || ";
						database.SaveChanges();

						var message = Request.CreateResponse(HttpStatusCode.Created, "Create Success");
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
				return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex + " || " + errorStatus);
			}
		}

		// PUT: api/Company/5
		//UPDATE
		public HttpResponseMessage Put(int id, [FromBody]Company Company)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{

					var currentCompany = database.Companies.ToList().FirstOrDefault(x => x.Id == id);

					if (currentCompany != null)
					{
						currentCompany.Name = Company.Name;
						currentCompany.Address = Company.Address;
						currentCompany.Email = Company.Email;
						currentCompany.Phone = Company.Phone;
						currentCompany.BankAccount = Company.BankAccount;
						currentCompany.Bulstat_Eik = Company.Bulstat_Eik;

						database.SaveChanges();
						var message = Request.CreateResponse(HttpStatusCode.OK, "Update Success");
						return message;
					}
					else
					{
						var message = Request.CreateResponse(HttpStatusCode.OK, "Company Not found");
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

		// DELETE: api/Company/5
		//DELETE
		public HttpResponseMessage Delete(int id)
		{
			try
			{
				using (CompanyPosDBContext database = new CompanyPosDBContext())
				{
					var Company = database.Companies.ToList().FirstOrDefault(x => x.Id == id);
					if (Company == null)
					{
						return Request.CreateErrorResponse(HttpStatusCode.NotFound,
								"Company with Id = " + id.ToString() + " not found to delete");
					}
					else
					{
						database.Companies.Remove(Company);
						database.SaveChanges();
						var message = Request.CreateResponse(HttpStatusCode.OK, "Delete Success");
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
