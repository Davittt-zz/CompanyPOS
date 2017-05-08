namespace DATA.Models
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations.Schema;

	public class TimeTable
	{
		public int ID { get; set; }

		public DateTime Date { get; set; }
		public DateTime TimeIn { get; set; }
		public DateTime TimeOut { get; set; }

		public int UserID { get; set; }
		public int StoreID { get; set; }
		public int CompanyID { get; set; }

		[NotMapped]
		public string _date
		{
			get { return Date.ToString(); }
			set
			{
				Date = TimeTools.GetDate(value);
			}
		}

		[NotMapped]
		public string _timeIn
		{
			get { return TimeIn.ToString(); }
			set
			{
				TimeIn = TimeTools.GetDate(value);
			}
		}

		[NotMapped]
		public string _timeOut
		{
			get { return TimeOut.ToString(); }
			set
			{
				TimeOut = TimeTools.GetDate(value);
			}
		}
	}
}
