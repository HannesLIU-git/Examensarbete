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

        public static string BracketType {get; set;} = string.Empty;

        public static string BarrelType { get; set; } = string.Empty;

        public static string FilePathUI { get; set; } = string.Empty;

        public static int bracketCounter { get; set; } = 0;

        public static int pipeCounter { get; set; } = 0;

        public static int M4A1Counter { get; set; } = 0;

        public static int AR15Counter { get; set; } = 0;

        public static double[] InitialPoint1 { get; set; } = new double[] {0,0,0 };

        public static double barrelEnd { get; set; } = 0;

        public static double[] forkpoint1 { get; set; } = new double[] { 0, 0, 0 };

        public static double[] forkpoint2 { get; set; } = new double[] { 0, 0, 0 };
        
        public static double forkdistance { get; set; } = 0;

        public static string Width { get; set; } = "40";

        public static string BracketPos { get; set; } = "100";

        public static string PipeDiameter { get; set; } = string.Empty;

        public static string ScannedFileName { get; set; } = string.Empty;

        public static string ScannedFilePath { get; set; } = string.Empty;

        public static int PicaCounter { get; set; } = 0;

        public static string Diameter { get; set; } = "20";

        public static string ForkLength { get; set; } = "40";

        public static string ForkWidth { get; set; } = "10";

        public static string BracketSide { get; set; } = "--Select Side--";

        public static string SelectedComponent { get; set; } = "--Optional--";

        public static bool Thermal { get; set; } = false;

        public static bool BracketAdded { get; set; } = false;


    }

}
