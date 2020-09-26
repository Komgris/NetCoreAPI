using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Utility
{
    public static class HelperUtility
    {
        public static int[] StringToArray(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new int[] { };
            return text.Split(',').Select(x => Convert.ToInt32(x)).ToArray();
        }

        public static string[] StringToStringArray(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new string[] { };
            return text.Split(',').Select(x => x).ToArray();
        }

        public static void Logging(string file, string mng)
        {
            var path = @"C:\log\" + file;
            File.AppendAllTextAsync(path, $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")} >> {mng} \r\n",CancellationToken.None);
        }
    }
}
