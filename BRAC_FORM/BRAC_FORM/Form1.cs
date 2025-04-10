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
        bool browser_pressed = false;
        public Form1()
        {
            InitializeComponent();
        }


      

        private void button4_Click(object sender, EventArgs e) /////////////////////////// NEXT
        {
            if (browser_pressed == false)
            {
                GlobalVariables.FilePath = $@"{textBox2.Text}";
            }
            GlobalVariables.FilePathUI = GlobalVariables.FilePath.Replace("CAD", "Point_UI.dlx");


            string selected = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selected))
            {
                MessageBox.Show("Please select a form to open.");
                return;
            }

            if (selected == "Clamp")
            {
                Form1_CLAMP form1_CLAMP = new Form1_CLAMP();
                form1_CLAMP.Show(); // or formA.ShowDialog();
                this.Hide();
            }
            else if (selected == "Picatinny")
            {

                Form1_PICATINNY form1_PICATTINY = new Form1_PICATINNY(); // Create an instance of Form2
                form1_PICATTINY.Show(); // Show Form2
                this.Hide();  // Hide Form1
            }
        }

        private void button8_Click(object sender, EventArgs e) ///////////////////////////// SUPRESS
        {

            NXOpen.Session theSession = NXOpen.Session.GetSession(); //////////////// SELECT PART IN TREE
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT FORKTEST 1"));
            NXOpen.PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus1);

            workPart = theSession.Parts.Work; // FORKTEST
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Suppress Feature"); /////////////////// SUPRESS FEATURE

            NXOpen.Features.Feature[] features1 = new NXOpen.Features.Feature[1];
            NXOpen.Features.Extrude extrude1 = ((NXOpen.Features.Extrude)workPart.Features.FindObject("EXTRUDE(3)"));
            features1[0] = extrude1;
            workPart.Features.SuppressFeatures(features1);

            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part"); /////////////////// SELECT TOP ASSEMBLY

            NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
            NXOpen.PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

            workPart = theSession.Parts.Work; // assembly1
            partLoadStatus2.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public static int GetUnloadOption(string dummy) 
        { return (int)NXOpen.Session.LibraryUnloadOption.Immediately; }

        private void button1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Välj en mapp";
                folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    GlobalVariables.FilePath = selectedPath;
                    MessageBox.Show("Vald mapp: " + selectedPath);
                }
            }
            browser_pressed = true;
        }
    }
}
