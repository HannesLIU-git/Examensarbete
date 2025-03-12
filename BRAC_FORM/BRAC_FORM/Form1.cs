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
        public Form1()
        {
            InitializeComponent();
        }

    

        private void button5_Click(object sender, EventArgs e) ////////////////////////////////// Delete Everything
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


        private void button7_Click(object sender, EventArgs e) ///////////////////////////Insert Barrel
        {
            GlobalVariables.FilePath = $@"{textBox2.Text}";
            GlobalVariables.FilePathUI = GlobalVariables.FilePath.Replace("CAD", "Point_UI.dlx");

            GlobalVariables.bracketCounter++;
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            GlobalVariables.PipeDiameter = textBox5.Text;

            string D_width = GlobalVariables.PipeDiameter;

            Point3d position = new Point3d(0.0, 0.0, 0.0);

            string partsFolderPath = GlobalVariables.FilePath;

            Class_Add_item addItem = new Class_Add_item();

            addItem.AddPartToAssembly("Pipa_SAAB.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
            
            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Barrel added at origin.");

            addItem.updateAll();
            addItem.HideDatumsAndSketches();
        }

        private void button4_Click(object sender, EventArgs e) /////////////////////////// NEXT
        {
            Form2 form2 = new Form2(); // Create an instance of Form2
            form2.Show(); // Show Form2
            this.Hide();  // Hide Form1
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

        private void button1_Click(object sender, EventArgs e) ///////////////////// INSERT RAIL
        {
            GlobalVariables.FilePath = $@"{textBox2.Text}";

            GlobalVariables.bracketCounter++;
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            Point3d position = new Point3d(0.0, 0.0, 0.0);

            string D_width = GlobalVariables.PipeDiameter;

            string partsFolderPath = GlobalVariables.FilePath;

            Class_Add_item addItem = new Class_Add_item();

            addItem.AddPartToAssembly("Picatinny_rail.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Rail Added to Assembly.");

            addItem.updateAll();
            addItem.HideDatumsAndSketches();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            string Width = textBox1.Text;

            string Parallax = textBox3.Text;

            Point3d position = new Point3d(0.0, 0.0, 0.0);

            string D_width = GlobalVariables.PipeDiameter;

            string partsFolderPath = GlobalVariables.FilePath;

            Class_Add_item addItem = new Class_Add_item();

            addItem.AddPartToAssembly("Lower_brac_Picatinny.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Upper_brac_Picatinny.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Skruvar_picatinny.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Parralax_distans.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + Parallax, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Picatinny_SAT.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + Parallax, position, partsFolderPath, assemblyPart);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Picatinny Bracket added.");

            addItem.updateAll();
            addItem.HideDatumsAndSketches();

        }
    }
}
