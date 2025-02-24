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
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hannes hannes = new Hannes(textBox1,textBox2);
            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);
            hannes.AddCylinders();
            addItem.updateAll();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hannes hannes = new Hannes(textBox1, textBox2);
            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);
            hannes.AddCubes();
            addItem.updateAll();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hannes hannes = new Hannes(textBox1, textBox2);
            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);
            hannes.DeleteCubes();
            addItem.updateAll();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bracketCounter++;
            // Create the instance of the Class_Add_item class
            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);

            // Call method to add the three parts to the assembly
            addItem.AddThreeParts();

            //addItem.updateStructure();

            addItem.updateAll();

            addItem.HideDatumsAndSketches();

            
         

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            
            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6, textBox7, textBox8, bracketCounter);

            addItem.DeleteBracket("Pipa", bracketCounter);
            addItem.DeleteBracket("LowerBrac", bracketCounter);
            addItem.DeleteBracket("Upper_brac", bracketCounter);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NXOpen.Session theSession = NXOpen.Session.GetSession();
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;
            NXOpen.Session.UndoMarkId markId1;
            markId1 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component component1 = ((NXOpen.Assemblies.Component)workPart.ComponentAssembly.RootComponent.FindObject("COMPONENT Pipa_1 1"));
            NXOpen.PartLoadStatus partLoadStatus1;
            theSession.Parts.SetWorkComponent(component1, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus1);

            workPart = theSession.Parts.Work; // Pipa_1
            partLoadStatus1.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

            Point_UI pointUI = new Point_UI();
            pointUI.Main();

            NXOpen.Session.UndoMarkId markId2;
            markId2 = theSession.SetUndoMark(NXOpen.Session.MarkVisibility.Visible, "Make Work Part");

            NXOpen.Assemblies.Component nullNXOpen_Assemblies_Component = null;
            NXOpen.PartLoadStatus partLoadStatus2;
            theSession.Parts.SetWorkComponent(nullNXOpen_Assemblies_Component, NXOpen.PartCollection.RefsetOption.Entire, NXOpen.PartCollection.WorkComponentOption.Visible, out partLoadStatus2);

            workPart = theSession.Parts.Work; // assembly1
            partLoadStatus2.Dispose();
            theSession.SetUndoMarkName(markId1, "Make Work Part");

        }
    }
}
