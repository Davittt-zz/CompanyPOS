
namespace DATA.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Sale
	{
		//public Sale()
		//{
		//    this.Invoice = new HashSet<Invoice>();
		//    this.ItemPurchase = new HashSet<ItemPurchase>();
		//}

		public int ID { get; set; }
		public Nullable<System.DateTime> Date { get; set; }

		public string Title { get; set; }
		public Nullable<decimal> DiscountAmount { get; set; }
		public Nullable<decimal> DiscountRate { get; set; }
		public Nullable<decimal> TaxAmunt { get; set; }
		public Nullable<decimal> TaxRate { get; set; }
		public Nullable<decimal> TotalPrice { get; set; }
		public Nullable<decimal> SubtotalPrice { get; set; }

		public string Status { get; set; }

		public string TransactionNumber { get; set; }


		public int UserID { get; set; }
		//  [ForeignKey("UserID")]
		//  public virtual User User { get; set; }

		public int ShiftID { get; set; }
		//   [ForeignKey("ShiftID")]
		//   public virtual Shift Shift { get; set; }

		public int StoreID { get; set; }
		//    [ForeignKey("StoreID")]
		//    public virtual Store Store { get; set; }

		public virtual ICollection<Invoice> Invoices { get; set; }
		public virtual ICollection<ItemPurchase> ItemPurchases { get; set; }
	}
}
