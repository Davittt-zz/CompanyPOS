using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CompanyPOS.Models
{
	public interface IRestWebservice<T>
	{
		HttpResponseMessage Get(string token);
		HttpResponseMessage Get(string token, int id);
		HttpResponseMessage Post([FromBody] T Item, string token);
		HttpResponseMessage Put(int id, [FromBody]T Item, string token);
		HttpResponseMessage Delete(int id, string token);
	}
}