using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using OfficeOpenXml;

namespace CIM.BusinessLogic.Utility
{
    public static class ConvertExtension
    {
        public static DateTime? CellValToDateTimeNull (this ExcelRange val)
        {
			try
			{
				DateTime oDate;
				if (DateTime.TryParseExact(val.Text.ToString(), "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out oDate))
				{
					return Convert.ToDateTime(oDate);
				}
				else
				{
					return Convert.ToDateTime(DateTime.Parse(val.Text.ToString()));
				}
			}
			catch (Exception)
			{

				return null;
			}
		}

		public static DateTime CellValToDateTime(this ExcelRange val)
		{
			try
			{
				return Convert.ToDateTime(val.Value);
			}
			catch (Exception)
			{

				return DateTime.MinValue;
			}
		}

		public static int CellValToInt(this ExcelRange val)
		{
			try
			{
				return Convert.ToInt32(val.Value);
			}
			catch
			{
				return 0;
			}
		}

		public static string CellValToString(this ExcelRange val)
		{
			try
			{
				return val.Value.ToString();
			}
			catch
			{
				return "";
			}
		}
	}
}
