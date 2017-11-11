
namespace DATA.Models
{
    using Newtonsoft.Json;
    using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Sale
	{
        public Sale()
        {
            this.Invoices = new HashSet<Invoice>();
            this.ItemPurchases = new HashSet<ItemPurchase>();
        }

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
        [JsonIgnore]
        public virtual ICollection<Invoice> Invoices { get; set; }
        [JsonIgnore]
        public virtual ICollection<ItemPurchase> ItemPurchases { get; set; }
        public decimal? TotalPaid { get; set; }
        public int? PaymentsNumber { get; set; }
    }
}
