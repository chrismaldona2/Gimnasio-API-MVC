using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Utils
    {
        public static string ObtenerOperador(string prefijo)
        {
            string trimedPrefijo = prefijo.Trim();
            if (trimedPrefijo.StartsWith(">=") || trimedPrefijo.StartsWith("<="))
            {
                return trimedPrefijo.Substring(0, 2); 
            }

            if (trimedPrefijo.StartsWith(">") || trimedPrefijo.StartsWith("<") || trimedPrefijo.StartsWith("="))
            {
                return trimedPrefijo.Substring(0, 1); 
            }

            return "=";
        }


    }
}
