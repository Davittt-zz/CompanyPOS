
namespace DATA.Models
{

	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;

	public class MenuPage
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public Nullable<int> Page { get; set; }

		public int StoreID { get; set; }

		public int MenuID { get; set; }

		//public virtual ICollection<ItemPagePosition> ItemPagePositions { get; set; }
	}
}
