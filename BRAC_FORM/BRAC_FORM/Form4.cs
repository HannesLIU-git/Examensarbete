using NXOpen;
using NXOpen.Validate;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NXOpen.UF.UFPart;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static NXOpen.Selection;

namespace BRAC_FORM
{
    public partial class Form4: Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e) ////////////////////////// CREATE FORK POINTS
        {

            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"COMPONENT Pipa_SAAB_{GlobalVariables.bracketCounter} 1"));
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

            GlobalVariables.forkpoint1 = pointsList[0];
            GlobalVariables.forkpoint2 = pointsList[1];



            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
            NXOpen.PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

            workPart = theSession.Parts.Work; // assembly1
            partLoadStatus2.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            GlobalVariables.forkdistance = Math.Sqrt(
                Math.Pow(GlobalVariables.forkpoint2[0] - GlobalVariables.forkpoint1[0], 2) +
                Math.Pow(GlobalVariables.forkpoint2[1] - GlobalVariables.forkpoint1[1], 2) +
                Math.Pow(GlobalVariables.forkpoint2[2] - GlobalVariables.forkpoint1[2], 2)
            );

        }


        private void button1_Click(object sender, EventArgs e) ///////////////////////// ADD FORK
        {
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            string D_width = GlobalVariables.PipeDiameter;
            string Width = GlobalVariables.Width;
            string XPos = "0";
            string YPos = "0";
            int intwidth = int.Parse(Width);

            int IntPoint = (int)GlobalVariables.barrelEnd;  //End of Barrel

            string bracketPosString = GlobalVariables.BracketPos; // Bracket Pos
            int bracketPos = int.Parse(bracketPosString);

            int Forkint = (int)GlobalVariables.forkdistance + 1;
            string Gaffel_W = Forkint.ToString();

            int bracketWidth = int.Parse(Width);

            int Frontint = (int)GlobalVariables.forkpoint1[1]; // FrontPos
            int FrontDif = Math.Abs(IntPoint - Frontint);
            int FrontPOS = bracketPos - bracketWidth / 2 - FrontDif + 9;


            string Gaffel_L = FrontPOS.ToString();


            string dimensions = D_width + "," + Width + "," + XPos + "," + YPos + "," + Gaffel_W + "," + Gaffel_L;

            string partFilePath = $"C:\\Users\\timpe989\\source\\repos\\from-weapon-brac\\BRAC_FORM\\CAD\\Lower_bracet_SAAB_{GlobalVariables.bracketCounter}.prt";

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, partFilePath);

            PartLoadStatus loadStatus;
            Part part = (Part)theSession.Parts.OpenBase(partFilePath, out loadStatus); // Open part
            loadStatus.Dispose(); // Dispose loadStatus once it's used

            Class_Add_item addItem = new Class_Add_item();
            addItem.UpdatePartExpression(part, dimensions, assemblyPart);


            //////////////////////////////////////////// Unsuppress

            NXOpen.Part workPart = theSession.Parts.Work;//////////////// SELECT PART IN TREE
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"Lower_bracet_SAAB_{GlobalVariables.bracketCounter} 1"));
            NXOpen.PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus1);

            workPart = theSession.Parts.Work; // FORKTEST
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

 
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Unsuppress Feature"); ///////////////////////// UNSUPRESS

            NXOpen.Features.Feature[] features1 = new NXOpen.Features.Feature[1];
            NXOpen.Features.FeatureGroup featureGroup1 = ((NXOpen.Features.FeatureGroup)workPart.Features.FindObject("FEATURE_SET(20)"));
            features1[0] = featureGroup1;
            NXOpen.Features.Feature[] errorFeatures1;
            errorFeatures1 = workPart.Features.UnsuppressFeatures(features1);

            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part"); /////////////////// SELECT TOP ASSEMBLY

            NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
            NXOpen.PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

            workPart = theSession.Parts.Work; // assembly1
            partLoadStatus2.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            //int IntPoint = (int)GlobalVariables.barrelEnd;  //End of Barrel

            //string bracketPosString = GlobalVariables.BracketPos; // Bracket Pos
            //int bracketPos = int.Parse(bracketPosString);

            //double Pos = IntPoint + bracketPos;

            //Point3d position = new Point3d(0.0, Pos, 0.0);

            //int Forkint = (int)GlobalVariables.forkdistance + 1;
            //string Gaffel_W = Forkint.ToString();

            //int bracketWidth = int.Parse(Width);

            //int Frontint = (int)GlobalVariables.forkpoint1[1]; // FrontPos
            //int FrontDif = Math.Abs(IntPoint - Frontint);
            //int FrontPOS = bracketPos - bracketWidth / 2 - FrontDif + 9;


            //string Gaffel_L = FrontPOS.ToString();


            //string partsFolderPath = GlobalVariables.FilePath;

            //Class_Add_item addItem = new Class_Add_item();
            //addItem.AddPartToAssembly("Gaffel_SAAB_v2.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos + "," + Gaffel_W + "," + Gaffel_L, position, partsFolderPath, assemblyPart);
            //addItem.updateAll();
            //addItem.HideDatumsAndSketches();




        }
        private void button2_Click(object sender, EventArgs e) ///////////////////////// PREVIOUS
        {
            Form3 form3 = new Form3(); // Create an instance of Form1
            form3.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button3_Click(object sender, EventArgs e) ////////////////////////// DELETE
        {
            Class_Add_item addItem = new Class_Add_item();

            addItem.DeleteBracket("Lower_bracet_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Upper_brac_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Pin_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("M6_Skruv_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Las_skruv_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("lax_fot_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Sat_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Gaffel_SAAB_v2", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Pipa_SAAB", GlobalVariables.bracketCounter);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket was removed from assembly");
        }

        private void button4_Click(object sender, EventArgs e) ///////////////////////// 1st Page
        {
            Form1 form1 = new Form1(); // Create an instance of Form1
            form1.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }
    }
}
