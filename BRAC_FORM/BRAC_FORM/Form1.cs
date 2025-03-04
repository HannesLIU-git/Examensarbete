using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NXOpen;

namespace BRAC_FORM
{
    public partial class Form1: Form
    {
        private int bracketCounter = 0;

        public double[] InitialPoint1;
        //public double[] InitialPoint2;
        //public double barrelCentre;
        public double barrelEnd;

        public double[] forkpoint1;
        public double[] forkpoint2;
        public double forkdistance;
        


        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bracketCounter++;
            // Create the instance of the Class_Add_item class
            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);

            // Call method to add the three parts to the assembly
            addItem.AddThreeParts();

            addItem.updateAll();

            addItem.HideDatumsAndSketches();

        }

        private void button5_Click(object sender, EventArgs e) ////////////////////////////////// Delete Everything
        {       
            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);

            //addItem.DeleteBracket("Pipa", bracketCounter);
            //addItem.DeleteBracket("LowerBrac", bracketCounter);
            //addItem.DeleteBracket("Upper_brac", bracketCounter);
            //addItem.DeleteBracket("Gaffel_1", bracketCounter);
            addItem.DeleteBracket("Lower_bracet_SAAB", bracketCounter);
            addItem.DeleteBracket("Upper_brac_SAAB", bracketCounter);
            addItem.DeleteBracket("Pin_SAAB", bracketCounter);
            addItem.DeleteBracket("M6_Skruv_SAAB", bracketCounter);
            addItem.DeleteBracket("Las_skruv_SAAB", bracketCounter);
            addItem.DeleteBracket("lax_fot_SAAB", bracketCounter);
            addItem.DeleteBracket("Sat_SAAB", bracketCounter);
            addItem.DeleteBracket("Gaffel_SAAB_v2", bracketCounter);
            addItem.DeleteBracket("Pipa_SAAB", bracketCounter);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket was removed from assembly");         
        }

        private void button6_Click(object sender, EventArgs e) ///////////////////////////////CREATE FORK POINT
        {


            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"COMPONENT Pipa_SAAB_{bracketCounter} 1"));
            NXOpen.PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus1);

            workPart = theSession.Parts.Work; // Pipa_1
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            Point_UI pointUI = new Point_UI();
        
            pointUI.Show();
            //int PointCounterValue = Point_UI.GetCounter();
            var points = pointUI.storedPoints;

            List<double[]> pointsList = new List<double[]>();

           // Assign each point's coordinates to the list
            foreach (var kvp in points)
            {
                pointsList.Add(kvp.Value); // Add the coordinates to the list
            }

            forkpoint1 = pointsList[0];
            forkpoint2 = pointsList[1];

            

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
            NXOpen.PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

            workPart = theSession.Parts.Work; // assembly1
            partLoadStatus2.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            CalculateDistance();
        }
        private void CalculateDistance()
        {
                forkdistance = Math.Sqrt(
                    Math.Pow(forkpoint2[0] - forkpoint1[0], 2) +
                    Math.Pow(forkpoint2[1] - forkpoint1[1], 2) +
                    Math.Pow(forkpoint2[2] - forkpoint1[2], 2)
                );
          
        }


        private void button7_Click(object sender, EventArgs e) ///////////////////////////Insert Barrel
        {
            GlobalVariables.FilePath = $@"{textBox2.Text}";
            GlobalVariables.FilePathUI = GlobalVariables.FilePath.Replace("CAD", "Point_UI.dlx");

            bracketCounter++;
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            string D_width = textBox5.Text;

            Point3d position = new Point3d(0.0, 0.0, 0.0);

            //string partsFolderPath = @"C:\Users\timpe989\source\repos\from-weapon-brac\BRAC_FORM\CAD\";
            string partsFolderPath = GlobalVariables.FilePath;

            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);

            addItem.AddPartToAssembly("Pipa_SAAB.prt", bracketCounter, D_width, position, partsFolderPath, assemblyPart);
            
            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Barrel added at origin.");

            addItem.updateAll();
            addItem.HideDatumsAndSketches();
        }

        private void button2_Click(object sender, EventArgs e) //////////////////////////////// Create Initial point
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"COMPONENT Pipa_SAAB_{bracketCounter} 1"));
            NXOpen.PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus1);

            workPart = theSession.Parts.Work; // Pipa_1
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            CreatePoint1 CreatePoint1 = new CreatePoint1();

            CreatePoint1.Show();
            //int PointCounterValue = Point_UI.GetCounter();
            var points = CreatePoint1.InitialPoints;

            List<double[]> pointsList = new List<double[]>();

            // Assign each point's coordinates to the list
            foreach (var kvp in points)
            {
                pointsList.Add(kvp.Value); // Add the coordinates to the list
            }

            InitialPoint1 = pointsList[0];
            //InitialPoint2 = pointsList[1];
            barrelEnd = InitialPoint1[1];


            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
            NXOpen.PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

            workPart = theSession.Parts.Work; // assembly1
            partLoadStatus2.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) /////////////////////////////////////// Insert Bracket
        {
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            string D_width = textBox5.Text;
            string Width = textBox6.Text;
            string XPos = textBox7.Text;
            string YPos = textBox8.Text;

            int IntPoint = (int)barrelEnd;

            string bracketPosString = textBox1.Text;
            int bracketPos = int.Parse(bracketPosString);

            double Pos = IntPoint + bracketPos;

            Point3d position = new Point3d(0.0, Pos, 0.0);

            string partsFolderPath = GlobalVariables.FilePath;

            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);

            addItem.AddPartToAssembly("Lower_bracet_SAAB.prt", bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Upper_brac_SAAB.prt", bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Pin_SAAB.prt", bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("M6_Skruv_SAAB.prt", bracketCounter, D_width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Las_skruv_SAAB.prt", bracketCounter, D_width , position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("lax_fot_SAAB.prt", bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Sat_SAAB.prt", bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket added at {position}.");

            addItem.updateAll();
            addItem.HideDatumsAndSketches();
        }

        private void button1_Click_1(object sender, EventArgs e) /////////////////////////INSERT FORK
        {
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            string D_width = textBox5.Text;
            string Width = textBox6.Text;
            string XPos = textBox7.Text;
            string YPos = textBox8.Text;
            int intwidth = int.Parse(Width);


            int IntPoint = (int)barrelEnd;  //End of Barrel

            string bracketPosString = textBox1.Text; // Bracket Pos
            int bracketPos = int.Parse(bracketPosString);

            double Pos = IntPoint + bracketPos;

            Point3d position = new Point3d(0.0, Pos, 0.0);

            int Forkint = (int)forkdistance + 1;
            string Gaffel_W = Forkint.ToString();

            int bracketWidth = int.Parse(Width);

            int Frontint = (int)forkpoint1[1]; // FrontPos
            int FrontDif = Math.Abs(IntPoint - Frontint);
            int FrontPOS = bracketPos - bracketWidth/2 - FrontDif +9 ;


            string Gaffel_L = FrontPOS.ToString();


            string partsFolderPath = GlobalVariables.FilePath;

            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);
            addItem.AddPartToAssembly("Gaffel_SAAB_v2.prt", bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos + "," + Gaffel_W + "," + Gaffel_L, position, partsFolderPath, assemblyPart);
            addItem.updateAll();
            addItem.HideDatumsAndSketches();
        }

    
    }
}
