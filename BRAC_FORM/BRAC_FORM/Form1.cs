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
        public double[] point1;
        public double[] point2;
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

        private void button5_Click(object sender, EventArgs e)
        {       
            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);

            addItem.DeleteBracket("Pipa", bracketCounter);
            addItem.DeleteBracket("LowerBrac", bracketCounter);
            addItem.DeleteBracket("Upper_brac", bracketCounter);
            addItem.DeleteBracket("Gaffel_1", bracketCounter);
            
            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Cubes were removed from assembly");         
        }

        private void button6_Click(object sender, EventArgs e) //CREATE POINT
        {


            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"COMPONENT Pipa_{bracketCounter} 1"));
            NXOpen.PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus1);

            workPart = theSession.Parts.Work; // Pipa_1
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            Point_UI pointUI = new Point_UI();
        
            pointUI.Show();
            int PointCounterValue = Point_UI.GetCounter();
            var points = pointUI.storedPoints;

            List<double[]> pointsList = new List<double[]>();

           // Assign each point's coordinates to the list
            foreach (var kvp in points)
            {
                pointsList.Add(kvp.Value); // Add the coordinates to the list
            }

            point1 = pointsList[0];
            point2 = pointsList[1];

            

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
                    Math.Pow(point2[0] - point1[0], 2) +
                    Math.Pow(point2[1] - point1[1], 2) +
                    Math.Pow(point2[2] - point1[2], 2)
                );
          
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            string D_pipa = textBox5.Text;
            string Width = textBox6.Text;
            string XPos = textBox7.Text;
            string YPos = textBox8.Text;

            int Forkint = (int)forkdistance + 1;
            string Gaffel_W = Forkint.ToString();

            int Frontint = (int)point1[0] - 1;
            string Front_Pos = Frontint.ToString();

            Point3d position = new Point3d(0.0, 0.0, 0.0);

            string partsFolderPath = @"C:\Users\timpe989\source\repos\from-weapon-brac\BRAC_FORM\CAD\";

            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);
            addItem.AddPartToAssembly("Gaffel_1.prt", bracketCounter, D_pipa + "," + Width + "," + XPos + "," + YPos + "," + Gaffel_W + "," + Front_Pos, position, partsFolderPath, assemblyPart);
            addItem.updateAll();
            addItem.HideDatumsAndSketches();
        }
    }
}
