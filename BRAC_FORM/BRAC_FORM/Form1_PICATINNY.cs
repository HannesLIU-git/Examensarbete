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
    public partial class Form1_PICATINNY: Form
    {
        public Form1_PICATINNY()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) ////////////// INSERT RAIL
        {

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

        private void button2_Click(object sender, EventArgs e) ////////////// PREVIOUS
        {
            Form1 form1 = new Form1();
            form1.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button3_Click(object sender, EventArgs e) ///////////// NEXT
        {
            Form2_PICATINNY form1 = new Form2_PICATINNY();
            form1.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }
    }
}
