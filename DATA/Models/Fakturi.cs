using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATA.Models
{
	[Table("Fakturies")]
	public class Fakturi
	{
		public int ID					{ get; set; }
		public string	InvoiceNumber	{ get; set; }
		public int		AssociatesID	{ get; set; }
		public DateTime Date			{ get; set; }
		public string	PaymentType		{ get; set; }
		public string	Details			{ get; set; }
		public string	Unit			{ get; set; }

		public string	Item			{ get; set; }
		public int		Qty				{ get; set; }

		public string	Price			{ get; set; }
		public string	Tax				{ get; set; }
		public string	Notes			{ get; set; }

		public int StoreID { get; set; }
		[ForeignKey("StoreID")]
		public virtual Store Store { get; set; }
	}
}
