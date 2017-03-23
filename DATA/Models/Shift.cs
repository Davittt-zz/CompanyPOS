namespace DATA.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Shift
	{
		public int ID { get; set; }
		public string Status { get; set; }
		public Nullable<System.DateTime> TimeStart { get; set; }
		public Nullable<System.DateTime> TimeEnd { get; set; }

		public double OpeningAmount { get; set; }
		public double ClosingAmount { get; set; }
		public double DifferenceAmount
		{
			get
			{
				return ClosingAmount - OpeningAmount;
			}
		}

		public int StoreID { get; set; }
		[ForeignKey("StoreID")]
		public virtual Store Store { get; set; }

		//public virtual ICollection<Sale> Sales { get; set; }
	}
}
