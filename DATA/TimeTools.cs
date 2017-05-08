using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA
{
	public class TimeTools
	{
		public static DateTime GetDate(string value)
		{
			try
			{
				var inputDate = string.Join("/", value.Split(new Char[] { '-' }).Take(3).Reverse());
				var inputTime = string.Join(":", value.Split(new Char[] { '-' }).Skip(3));
				return Convert.ToDateTime(inputDate + " " + inputTime);
			}
			catch (Exception)
			{
				return new DateTime(2000, 1, 1, 0, 0, 0);
			}
		}
	}
}
