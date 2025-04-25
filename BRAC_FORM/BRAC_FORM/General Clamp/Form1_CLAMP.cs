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
        string ScannedFilePath = string.Empty;
        string ScannedFileName = string.Empty;
        public Form1_CLAMP()
        {
            InitializeComponent();
            button2.Enabled = true;
            label1.Visible = false;
            label3.Visible = false;
            textBox1.Visible = false;
            button5.Visible = false;
            textBox2.Visible = false;

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Hide all buttons first
          
            
            // Show the button corresponding to the selected option
            switch (comboBox1.SelectedItem.ToString())
            {
                case "General":
                    label1.Visible = true;
                    label3.Visible = true;
                    textBox1.Visible = true;
                    button5.Visible = false;
                    textBox2.Visible = false;
                    break;
                case "M4A1":
                    label1.Visible = true;
                    label3.Visible = true;
                    textBox1.Visible = true;
                    button5.Visible = false;
                    textBox2.Visible = false;
                    break;
                case "AR15":
                    label1.Visible = true;
                    label3.Visible = true;
                    textBox1.Visible = true;
                    button5.Visible = false;
                    textBox2.Visible = false;
                    break;
                case "From File Browser":
                    button5.Visible = true;
                    textBox2.Visible = true;
                    label1.Visible = false;
                    textBox1.Visible = false;
                    label3.Visible = false;
                    break;

            }
        }

        private void button1_Click(object sender, EventArgs e) //////////////////// ADD BARREL
        {

            


            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            GlobalVariables.PipeDiameter = textBox1.Text;

            string D_width = GlobalVariables.PipeDiameter;

            Point3d position = new Point3d(0.0, 0.0, 0.0);

            string selected = comboBox1.SelectedItem?.ToString();


            if (string.IsNullOrEmpty(selected))
            {
                MessageBox.Show("Please select a type of barrel.");
                return;
            }

            if (selected == "General")
            {

                GlobalVariables.pipeCounter++;

                string partsFolderPath = GlobalVariables.FilePath;

                Class_Add_item addItem = new Class_Add_item();

                addItem.AddPartToAssembly("Pipa_SAAB.prt", GlobalVariables.pipeCounter, D_width, position, partsFolderPath, assemblyPart);

                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Barrel added at origin.");

                addItem.updateAll();
                addItem.HideDatumsAndSketches();
                button2.Enabled = true;
            }
            else if (selected == "AR15")
            {
                GlobalVariables.AR15Counter++;

                //GlobalVariables.FilePath = GlobalVariables.FilePath + "\\Nya_CAD";

                string partsFolderPath = GlobalVariables.FilePath;

                Class_Add_item addItem = new Class_Add_item();

                addItem.AddPartToAssembly("BARREL_m16.prt", GlobalVariables.AR15Counter, D_width, position, partsFolderPath, assemblyPart);

                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Barrel added at origin.");

                addItem.updateAll();
                addItem.HideDatumsAndSketches();

                //GlobalVariables.FilePath = GlobalVariables.FilePath.Replace("\\Nya_CAD", "");

                button2.Enabled = true;
                button4.Enabled = true;
            }
            else if (selected == "M4A1")
            {
                GlobalVariables.M4A1Counter++;

                //GlobalVariables.FilePath = GlobalVariables.FilePath + "\\Nya_CAD";

                string partsFolderPath = GlobalVariables.FilePath;

                Class_Add_item addItem = new Class_Add_item();

                addItem.AddPartToAssembly("M4A1_barrel.prt", GlobalVariables.M4A1Counter, D_width, position, partsFolderPath, assemblyPart);

                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Barrel added at origin.");

                //GlobalVariables.FilePath = GlobalVariables.FilePath.Replace("\\Nya_CAD", "");

                addItem.updateAll();
                addItem.HideDatumsAndSketches();

                button2.Enabled = true;
            }
            else if (selected == "From File Browser")
            {
                string partsFolderPath = ScannedFilePath;

                Class_Add_item addItem = new Class_Add_item();

                addItem.AddScannedPartToAssembly(ScannedFileName,ScannedFilePath,position, assemblyPart);

                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Barrel added at origin.");
            }
            

        }

        private void button3_Click(object sender, EventArgs e) ///////////////// PREVIOUS
        {
            Form1 form1 = new Form1(); // Create an instance of Form1
            form1.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button2_Click(object sender, EventArgs e) ///////////////// NEXT
        {

            string selected = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selected))
            {
                MessageBox.Show("Please select a form to open.");
                return;
            }

            if (selected == "General")
            {
                Form2 form2 = new Form2();
                form2.Show(); // or formA.ShowDialog();
                this.Hide();
            }
            else if (selected == "AR15")
            {

                Form2_AR15 form1_AR15 = new Form2_AR15(); // Create an instance of Form2
                form1_AR15.Show(); // Show Form2
                this.Hide();  // Hide Form1
            }
            else if (selected == "M4A1")
            {

                Form2_M4A1 form1_M4A1 = new Form2_M4A1(); // Create an instance of Form2
                form1_M4A1.Show(); // Show Form2
                this.Hide();  // Hide Form1
            }
            else if (selected == "From File Browser")
            {

                _3DSCAN1 _3DSCAN1 = new _3DSCAN1(); // Create an instance of Form2
                _3DSCAN1.Show(); // Show Form2
                this.Hide();  // Hide Form1
            }

        }

        private void button4_Click(object sender, EventArgs e) //////////////////// DELETE
        {
            Class_Add_item addItem = new Class_Add_item();

            

            string selected = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selected))
            {
                MessageBox.Show("Please select a Barrel to delete.");
                return;
            }

            if (selected == "General")
            {
                addItem.DeleteBracket("Pipa_SAAB", GlobalVariables.pipeCounter);
            }
            else if (selected == "AR15")
            {
                addItem.DeleteBracket("BARREL_m16", GlobalVariables.AR15Counter); 
            }
            else if (selected == "M4A1")
            {
                addItem.DeleteBracket("M4A1_barrel", GlobalVariables.M4A1Counter);
            }
            else if (selected == "From File Browser")
            {
                addItem.DeleteScannedBracket(ScannedFileName);
            }
            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Barrel deleted.");

            
        }

        private void button5_Click(object sender, EventArgs e) ////////////////////////// FILE BROWSER
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                     ScannedFilePath = openFileDialog.FileName;
                     ScannedFileName = openFileDialog.SafeFileName;

                    // Do something with the file path, like show it in a textbox
                    textBox2.Text = ScannedFilePath;
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(textBox1.Text, out double value))
            {
                double min = 16.0;
                double max = 40.0;

                if (value >= min && value <= max)
                {
                    textBox1.BackColor = Color.White; // valid input
                }
                else
                {
                    textBox1.BackColor = Color.LightCoral; // number out of range
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    textBox1.BackColor = Color.White; // neutral when empty
                }
                else
                {
                    textBox1.BackColor = Color.LightCoral; // not a number
                }
            }
        }
    }
}
