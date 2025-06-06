﻿using NXOpen;
using NXOpen.CAE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BRAC_FORM
{
    public partial class Form3: Form
    {
        string originalTextbox6 = "";
        string originalTextbox7 = "";
        string originalTextbox1 = "";
        public Form3()
        {
            InitializeComponent();
            button6.Enabled = false;
            button1.Visible = false;
            button5.Visible = false;
            button5.Enabled = false;
            button7.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e) ///////////////////////////////// PREVIOUS
        {
            Form2 form2 = new Form2(); // Create an instance of Form1
            form2.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button4_Click(object sender, EventArgs e) ///////////////////////////////// NEXT
        {
           
          
        }

        private void button3_Click(object sender, EventArgs e) ///////////////////////////////// INSERT BRACKET
        {
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            GlobalVariables.bracketCounter++;

            string D_width = GlobalVariables.PipeDiameter;
            GlobalVariables.Width = textBox6.Text;
            string Width = GlobalVariables.Width;
            string XPos = textBox7.Text;
            string YPos = "10";

            int IntPoint = (int)GlobalVariables.barrelEnd;

            GlobalVariables.BracketPos = textBox1.Text;
            int bracketPos = int.Parse(GlobalVariables.BracketPos);

            double Pos = IntPoint + bracketPos;

            int Forkint = (int)GlobalVariables.forkdistance + 1;
            string Gaffel_W = Forkint.ToString();

            int bracketWidth = int.Parse(Width);

            int Frontint = (int)GlobalVariables.forkpoint1[1]; // FrontPos
            int FrontDif = Math.Abs(IntPoint - Frontint);
            int FrontPOS = bracketPos - bracketWidth / 2 - FrontDif + 9;


            string Gaffel_L = FrontPOS.ToString();


            Point3d position = new Point3d(0.0, Pos, 0.0);
            string partsFolderPath = GlobalVariables.FilePath;
            Class_Add_item addItem = new Class_Add_item();

            addItem.AddPartToAssembly("Lower_bracet_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos + "," + Gaffel_W + "," + Gaffel_L, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Upper_brac_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Pin_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("M6_Skruv_SAAB.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Las_skruv_SAAB.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("lax_fot_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Sat_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);


            addItem.updateAll();
            addItem.HideDatumsAndSketches();

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket added at {position}.");


            button6.Enabled = true;
            button5.Enabled = true;
            button7.Enabled = true;
            button3.Enabled = false;

            originalTextbox6 = textBox6.Text;
            originalTextbox7 = textBox7.Text;
            originalTextbox1 = textBox1.Text;


        }

        private void button1_Click(object sender, EventArgs e) ////////////////////////// CREATE FORKPOINTS
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

        private void button5_Click(object sender, EventArgs e) /////////////////////// ADD FORK
        {
            //////////////// UNSUPRESS FORK
        
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            NXOpen.Part workPart = theSession.Parts.Work;//////////////// SELECT PART IN TREE
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"COMPONENT Lower_bracet_SAAB_{GlobalVariables.bracketCounter} 1"));
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
        }

        private void button6_Click(object sender, EventArgs e) ////////////////////// DELETE
        {
            Class_Add_item addItem = new Class_Add_item();

            addItem.DeleteBracket("Lower_bracet_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Upper_brac_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Pin_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("M6_Skruv_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Las_skruv_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("lax_fot_SAAB", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Sat_SAAB", GlobalVariables.bracketCounter);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket was removed from assembly");

            button6.Enabled = false;
            button3.Enabled = true;
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button1.Visible = true;
                button5.Visible = true; // Show the button
            }
            else
            {
                button1.Visible = false; // Hide the button again (optional)
                button5.Visible = false; // Hide the button again (optional)
            }
        }

        private void button7_Click(object sender, EventArgs e) /////////////////////////// UPDATE
        {
            if (textBox6.Text != originalTextbox6 & textBox1.Text == originalTextbox1 || textBox7.Text != originalTextbox7 & textBox1.Text == originalTextbox1)
            {
                //////////////// SELECT PART IN TREE

                Session theSession = Session.GetSession();
                Part assemblyPart = (Part)theSession.Parts.Work;

                NXOpen.Part workPart = theSession.Parts.Work;
                NXOpen.Part displayPart = theSession.Parts.Display;
                NXOpen.Session.UndoMarkId markId1;
                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

                NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"COMPONENT Lower_bracet_SAAB_{GlobalVariables.bracketCounter} 1"));
                NXOpen.PartLoadStatus partLoadStatus1;
                theSession.Parts.SetWorkComponent(component1, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus1);

                workPart = theSession.Parts.Work;
                partLoadStatus1.Dispose();
                theSession.SetUndoMarkName(markId1, "Make Work Part");

                /////////////////// UPDATE EXPRESSION

                NXOpen.Session.UndoMarkId markId2;
                markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Edit Expression");

                NXOpen.Expression expression1 = ((NXOpen.Expression)workPart.Expressions.FindObject("Width"));
                NXOpen.Unit unit1 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("MilliMeter"));
                workPart.Expressions.EditWithUnits(expression1, unit1, textBox6.Text);

                int nErrs1;
                nErrs1 = theSession.UpdateManager.DoUpdate(markId2);

                /////////////////// SELECT TOP ASSEMBLY

                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

                NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component1 = null;
                NXOpen.PartLoadStatus partLoadStatusx;
                theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component1, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatusx);

                workPart = theSession.Parts.Work;
                partLoadStatusx.Dispose();
                theSession.SetUndoMarkName(markId1, "Make Work Part");

            

                //////////////// SELECT PART IN TREE

                NXOpen.Session.UndoMarkId markId3;
                markId3 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

                NXOpen.Assemblies.Component component2 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"COMPONENT lax_fot_SAAB_{GlobalVariables.bracketCounter} 1"));
                NXOpen.PartLoadStatus partLoadStatus2;
                theSession.Parts.SetWorkComponent(component2, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

                workPart = theSession.Parts.Work;
                partLoadStatus2.Dispose();
                theSession.SetUndoMarkName(markId3, "Make Work Part");

                /////////////////// UPDATE EXPRESSION

                NXOpen.Session.UndoMarkId markId4;
                markId4 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Edit Expression");

                NXOpen.Expression expression2 = ((NXOpen.Expression)workPart.Expressions.FindObject("X_pos"));
                NXOpen.Unit unit2 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("MilliMeter"));
                workPart.Expressions.EditWithUnits(expression2, unit2, textBox7.Text);

                int nErrs2;
                nErrs2 = theSession.UpdateManager.DoUpdate(markId4);

                /////////////////// UPDATE EXPRESSION

                NXOpen.Session.UndoMarkId markId5;
                markId5 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Edit Expression");

                NXOpen.Expression expression3 = ((NXOpen.Expression)workPart.Expressions.FindObject("Width"));
                NXOpen.Unit unit3 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("MilliMeter"));
                workPart.Expressions.EditWithUnits(expression3, unit3, textBox6.Text);

                int nErrs3;
                nErrs3 = theSession.UpdateManager.DoUpdate(markId5);

                /////////////////// SELECT TOP ASSEMBLY

                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

                NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
                NXOpen.PartLoadStatus partLoadStatus;
                theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus);

                workPart = theSession.Parts.Work;
                partLoadStatus.Dispose();
                theSession.SetUndoMarkName(markId1, "Make Work Part");

                //////////////// SELECT PART IN TREE
                NXOpen.Session.UndoMarkId markId6;
                markId6 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

                NXOpen.Assemblies.Component component3 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject($"COMPONENT Sat_SAAB_{GlobalVariables.bracketCounter} 1"));
                NXOpen.PartLoadStatus partLoadStatus3;
                theSession.Parts.SetWorkComponent(component3, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus3);

                workPart = theSession.Parts.Work;
                partLoadStatus3.Dispose();
                theSession.SetUndoMarkName(markId6, "Make Work Part");

                /////////////////// UPDATE EXPRESSION

                NXOpen.Session.UndoMarkId markId7;
                markId7 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Edit Expression");

                NXOpen.Expression expression4 = ((NXOpen.Expression)workPart.Expressions.FindObject("X_pos"));
                NXOpen.Unit unit4 = ((NXOpen.Unit)workPart.UnitCollection.FindObject("MilliMeter"));
                workPart.Expressions.EditWithUnits(expression4, unit4, textBox7.Text);

                int nErrs4;
                nErrs4 = theSession.UpdateManager.DoUpdate(markId7);

                /////////////////// SELECT TOP ASSEMBLY

                markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

                NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component2 = null;
                NXOpen.PartLoadStatus partLoadStatus8;
                theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component2, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus8);

                workPart = theSession.Parts.Work;
                partLoadStatus8.Dispose();
                theSession.SetUndoMarkName(markId1, "Make Work Part");

                originalTextbox6 = textBox6.Text;

                originalTextbox7 = textBox7.Text;



                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Parameters were updated.");
            }
            if (textBox1.Text != originalTextbox1)
            {
                Class_Add_item addItem = new Class_Add_item();

                addItem.DeleteBracket("Lower_bracet_SAAB", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("Upper_brac_SAAB", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("Pin_SAAB", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("M6_Skruv_SAAB", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("Las_skruv_SAAB", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("lax_fot_SAAB", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("Sat_SAAB", GlobalVariables.bracketCounter);

                Session theSession = Session.GetSession();
                Part assemblyPart = (Part)theSession.Parts.Work;

                GlobalVariables.bracketCounter++;

                string D_width = GlobalVariables.PipeDiameter;
                GlobalVariables.Width = textBox6.Text;
                string Width = GlobalVariables.Width;
                string XPos = textBox7.Text;
                string YPos = "10";

                int IntPoint = (int)GlobalVariables.barrelEnd;

                GlobalVariables.BracketPos = textBox1.Text;
                int bracketPos = int.Parse(GlobalVariables.BracketPos);

                double Pos = IntPoint + bracketPos;

                int Forkint = (int)GlobalVariables.forkdistance + 1;
                string Gaffel_W = Forkint.ToString();

                int bracketWidth = int.Parse(Width);

                int Frontint = (int)GlobalVariables.forkpoint1[1]; // FrontPos
                int FrontDif = Math.Abs(IntPoint - Frontint);
                int FrontPOS = bracketPos - bracketWidth / 2 - FrontDif + 9;


                string Gaffel_L = FrontPOS.ToString();


                Point3d position = new Point3d(0.0, Pos, 0.0);
                string partsFolderPath = GlobalVariables.FilePath;
                

                addItem.AddPartToAssembly("Lower_bracet_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos + "," + Gaffel_W + "," + Gaffel_L, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("Upper_brac_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("Pin_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("M6_Skruv_SAAB.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("Las_skruv_SAAB.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("lax_fot_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("Sat_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);


                addItem.updateAll();
                addItem.HideDatumsAndSketches();

                originalTextbox6 = textBox6.Text;
                originalTextbox7 = textBox7.Text;
                originalTextbox1 = textBox1.Text;


                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Parameters were updated.");
            }
            
        }
    }
}
