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
    public partial class Form1_CLAMP: Form
    {
        public Form1_CLAMP()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) //////////////////// ADD BARREL
        {

            GlobalVariables.pipeCounter++;
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            GlobalVariables.PipeDiameter = textBox1.Text;

            string D_width = GlobalVariables.PipeDiameter;

            Point3d position = new Point3d(0.0, 0.0, 0.0);

            string partsFolderPath = GlobalVariables.FilePath;

            Class_Add_item addItem = new Class_Add_item();

            addItem.AddPartToAssembly("Pipa_SAAB.prt", GlobalVariables.pipeCounter, D_width, position, partsFolderPath, assemblyPart);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Barrel added at origin.");

            addItem.updateAll();
            addItem.HideDatumsAndSketches();
        }

        private void button3_Click(object sender, EventArgs e) ///////////////// PREVIOUS
        {
            Form1 form1 = new Form1(); // Create an instance of Form1
            form1.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button2_Click(object sender, EventArgs e) ///////////////// NEXT
        {
            Form2 form2 = new Form2(); // Create an instance of Form1
            form2.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }
    }
}
