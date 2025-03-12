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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BRAC_FORM
{
    public partial class Form2_PICATINNY: Form
    {
        public Form2_PICATINNY()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) ////////////////// INSERT PICATINNY BRACKET
        {

            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            string Width = textBox1.Text;

            string Parallax = textBox2.Text;

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

        private void button3_Click(object sender, EventArgs e) ////////////////// PREVIOUS
        {
            Form1_PICATINNY form1 = new Form1_PICATINNY();
            form1.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button4_Click(object sender, EventArgs e) ///////////////// 1st PAGE
        {
            Form1 form1 = new Form1(); // Create an instance of Form1
            form1.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button2_Click(object sender, EventArgs e) ///////////////// DELETE
        {

            Class_Add_item addItem = new Class_Add_item();

            addItem.DeleteBracket("Lower_brac_Picatinny", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Upper_brac_Picatinny", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Skruvar_picatinny", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Parralax_distans", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Picatinny_SAT", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Picatinny_rail", GlobalVariables.bracketCounter);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket was removed from assembly");
        }
    }
}
