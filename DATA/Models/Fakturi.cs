using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATA.Models
{
	[Table("Fakturies")]
	public class Fakturi
	{
		public int ID { get; set; }

		public string InvoiceNumber { get; set; }
		public int AssociatesID { get; set; }
		public DateTime Date { get; set; }
		public string PaymentType { get; set; }
		public string Details { get; set; }

		[NotMapped]
		public List<FakturiArticle> Items { get; set; }

		public string Notes { get; set; }
		public string Total { get; set; }

		public int StoreID { get; set; }
		[ForeignKey("StoreID")]
		public virtual Store Store { get; set; }
	}
}