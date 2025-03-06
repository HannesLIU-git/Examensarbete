using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRAC_FORM
{
    public static class GlobalVariables
    {
        public static string FilePath { get; set; } = string.Empty;
        public static string FilePathUI { get; set; } = string.Empty;

        public static int bracketCounter { get; set; } = 0;

        public static double[] InitialPoint1 { get; set; } = [];
    }
}
