using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models
{
	public class PurchaseAttribute
	{
		public int ID { get; set; }
		public int Quantity { get; set; }
		public string TotalPrice { get; set; }
		public int ItemPurchaseID { get; set; }
		public int StoreID { get; set; }
	}
}
