using System;
using System.Collections.Generic;
using System.Text;
using OfficeOpenXml;

namespace CIM.BusinessLogic.Utility
{
    public static class ConvertExtension
    {
        public static DateTime? _cellval2dtnull(this ExcelRange val)
        {
			try
			{
				return Convert.ToDateTime(val.Value);
			}
			catch (Exception)
			{

				return null;
			}
        }

		public static DateTime _cellval2dt(this ExcelRange val)
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

		public static int _cellval2int(this ExcelRange val)
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

		public static string _cellval2str(this ExcelRange val)
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
