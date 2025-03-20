using NXOpen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRAC_FORM
{
    public partial class Form1_AR15: Form
    {
        public Form1_AR15()
        {
            InitializeComponent();
            button3.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e) //////////////////// ADD BARREL
        {

            NXOpen.Session theSession = NXOpen.Session.GetSession(); /////// SELECT BARREL IN TREE
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"COMPONENT BARREL_m16_{GlobalVariables.pipeCounter} 1"));
            NXOpen.PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus1);

            workPart = theSession.Parts.Work; // Pipa_1
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            CreatePoint1 CreatePoint1 = new CreatePoint1(); /////// RUN CREATEPOUINT1 

            CreatePoint1.Show();
            var points = CreatePoint1.InitialPoints;

            List<double[]> pointsList = new List<double[]>();

            // Assign each point's coordinates to the list
            foreach (var kvp in points)
            {
                pointsList.Add(kvp.Value); // Add the coordinates to the list
            }

            GlobalVariables.InitialPoint1 = pointsList[0];
            //InitialPoint2 = pointsList[1];
            GlobalVariables.barrelEnd = GlobalVariables.InitialPoint1[1];


            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
            NXOpen.PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

            workPart = theSession.Parts.Work; // assembly1
            partLoadStatus2.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");
            button3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e) ///////////////////// PREVIOUS
        {
            Form1 form1 = new Form1(); // Create an instance of Form1
            form1.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button3_Click(object sender, EventArgs e) ///////////////////// NEXT
        {
            Form2_AR15 form2 = new Form2_AR15(); // Create an instance of Form1
            form2.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }
    }
}
