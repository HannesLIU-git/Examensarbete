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
    public partial class Form3: Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e) ///////////////////////////////// PREVIOUS
        {
            Form2 form2 = new Form2(); // Create an instance of Form1
            form2.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button4_Click(object sender, EventArgs e) ///////////////////////////////// NEXT
        {
            Form4 form4 = new Form4(); // Create an instance of Form1
            form4.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button3_Click(object sender, EventArgs e) ///////////////////////////////// INSERT BRACKET
        {
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            string D_width = GlobalVariables.PipeDiameter;
            GlobalVariables.Width = textBox6.Text;
            string Width = GlobalVariables.Width;
            string XPos = textBox7.Text;
            string YPos = "10";

            int IntPoint = (int)GlobalVariables.barrelEnd;

            GlobalVariables.BracketPos = textBox1.Text;
            int bracketPos = int.Parse(GlobalVariables.BracketPos);

            double Pos = IntPoint + bracketPos;

            Point3d position = new Point3d(0.0, Pos, 0.0);
            string partsFolderPath = GlobalVariables.FilePath;
            Class_Add_item addItem = new Class_Add_item();

            addItem.AddPartToAssembly("Lower_bracet_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Upper_brac_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Pin_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("M6_Skruv_SAAB.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Las_skruv_SAAB.prt", GlobalVariables.bracketCounter, D_width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("lax_fot_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Sat_SAAB.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket added at {position}.");

            addItem.updateAll();
            addItem.HideDatumsAndSketches();
        }
    }
}
