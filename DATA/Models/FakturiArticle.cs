using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Models
{
	public class FakturiArticle
	{
        public int ID { get; set; }
		
		public int ProductTableID { get; set; }
		public int FakturiID { get; set; }

		public string Unit { get; set; }
		public string Item { get; set; }
		public Nullable<decimal> Qty { get; set; }
		public string Price { get; set; }
		public string Tax { get; set; }

        public int StoreID { get; set; }
	}
}
