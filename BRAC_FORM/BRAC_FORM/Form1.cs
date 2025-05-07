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
            textBox2.Text = GlobalVariables.FilePath;
            comboBox1.SelectedItem = GlobalVariables.BracketType;

        }


      

        private void button4_Click(object sender, EventArgs e) /////////////////////////// NEXT
        {
            if (browser_pressed == false)
            {
                GlobalVariables.FilePath = $@"{textBox2.Text}";
            }
            GlobalVariables.FilePathUI = GlobalVariables.FilePath.Replace("CAD", "Point_UI.dlx");


            GlobalVariables.BracketType = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(GlobalVariables.BracketType))
            {
                MessageBox.Show("Please select a form to open.");
                return;
            }

            if (GlobalVariables.BracketType == "Clamp")
            {
                Form1_CLAMP form1_CLAMP = new Form1_CLAMP();
                form1_CLAMP.Show(); // or formA.ShowDialog();
                this.Hide();
            }
            else if (GlobalVariables.BracketType == "Picatinny")
            {

                Form1_PICATINNY form1_PICATTINY = new Form1_PICATINNY(); // Create an instance of Form2
                form1_PICATTINY.Show(); // Show Form2
                this.Hide();  // Hide Form1
            }
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
                folderDialog.Description = "Choose the map with the CAD parts.";
                folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderDialog.SelectedPath;
                    GlobalVariables.FilePath = selectedPath;
                    MessageBox.Show("Map Chosen: " + selectedPath);
                    textBox2.Text = selectedPath;
                }
            }
            browser_pressed = true;
            
        }
    }
}
