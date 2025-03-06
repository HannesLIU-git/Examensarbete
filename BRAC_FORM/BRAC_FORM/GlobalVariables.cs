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

        public static double[] InitialPoint1 { get; set; } = new double[] {0,0,0 };

        public static double barrelEnd { get; set; } = 0;

        public static double[] forkpoint1 { get; set; } = new double[] { 0, 0, 0 };

        public static double[] forkpoint2 { get; set; } = new double[] { 0, 0, 0 };
        
        public static double forkdistance { get; set; } = 0;

        public static string Width { get; set; } = string.Empty;

        public static string BracketPos { get; set; } = string.Empty;

        public static string PipeDiameter { get; set; } = string.Empty;
    }
}
