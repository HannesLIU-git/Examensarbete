using NXOpen;
using NXOpen.Annotations;
using NXOpen.Drawings;
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
    public partial class Form2_AR15: Form
    {
        string originalTextbox1 = "";
        string originalTextbox2 = "";
        string originalTextbox3 = "";
        string originalTextbox4 = "";
        public Form2_AR15()
        {
            InitializeComponent();
            button4.Enabled = false;
            button6.Enabled = false;
            
        }

        private void button1_Click(object sender, EventArgs e) /////////////////// PREVIOUS
        {
            Form1_CLAMP form1_CLAMP = new Form1_CLAMP(); // Create an instance of Form1
            form1_CLAMP.Show(); // Show Form1
            this.Hide();  // Hide Form2
        }

        private void button2_Click(object sender, EventArgs e) /////////////////// NEXT
        {
            //Form3_AR15 Form3_AR15 = new Form3_AR15(); // Create an instance of Form1
            //Form3_AR15.Show(); // Show Form1
            //this.Hide();  // Hide Form2
        }

        private void button3_Click(object sender, EventArgs e) /////////////////// ADD BRACKET
        {
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            GlobalVariables.bracketCounter++;

            string D_width = GlobalVariables.PipeDiameter;
            GlobalVariables.Width = textBox1.Text;
            string Width = GlobalVariables.Width;
            string XPos = "10";
            string YPos = "10";

            int IntPoint = (int)GlobalVariables.barrelEnd;

            GlobalVariables.BracketPos = textBox2.Text;
            int bracketPos = int.Parse(GlobalVariables.BracketPos);

            double Pos = IntPoint + bracketPos;

            int Forkint = (int)GlobalVariables.forkdistance + 1;
            //string Gaffel_W = Forkint.ToString();

            int bracketWidth = int.Parse(Width);

            int Frontint = (int)GlobalVariables.forkpoint1[1]; // FrontPos
            int FrontDif = Math.Abs(IntPoint - Frontint);
            int FrontPOS = bracketPos - bracketWidth / 2 - FrontDif + 9;


            //string Gaffel_L = FrontPOS.ToString();
            string Gaffel_L = textBox3.Text;
            string Gaffel_W = textBox4.Text;


            Point3d position = new Point3d(0.0, Pos, 0.0);
            string partsFolderPath = GlobalVariables.FilePath;
            Class_Add_item addItem = new Class_Add_item();

            string selected = comboBox1.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selected))
            {
                MessageBox.Show("Please select bracket direction");
                return;
            }

            if (selected == "Over")
            {
                XPos = "0.01";

            }
            else if (selected == "Right")
            {
                XPos = "270";
            }
            else if (selected == "Under")
            {
                XPos = "180";
            }
            else if (selected == "Left")
            {
                XPos = "90";
            }
            addItem.AddPartToAssembly("Locking_brack.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Locking_Pin.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Lower_brac_new_m16.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("M6_35_NEW_BRAC.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("RPD_PIN.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("SAT_FUNK.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Upper_NEW_clamp.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos + "," + Gaffel_W + "," + Gaffel_L, position, partsFolderPath, assemblyPart);


            addItem.updateAll();
            addItem.HideDatumsAndSketches();

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket added at {position}.");

            button4.Enabled = true;
            button3.Enabled = false;
            button6.Enabled = true;
          
        }

        private void button4_Click(object sender, EventArgs e) ////////////////////// DELETE
        {
            Class_Add_item addItem = new Class_Add_item();

            addItem.DeleteBracket("Locking_brack", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Locking_Pin", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Lower_brac_new_m16", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("M6_35_NEW_BRAC", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("RPD_PIN", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("SAT_FUNK", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Upper_NEW_clamp", GlobalVariables.bracketCounter);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket was removed from assembly");

            button4.Enabled = false;
            button3.Enabled = true;
        }

        

       

        private void button6_Click(object sender, EventArgs e) ///////////////////////// UPDATE
        {
            if (textBox1.Text != originalTextbox1 || textBox2.Text != originalTextbox2 || textBox3.Text != originalTextbox3 || textBox3.Text != originalTextbox3)
            {
                Class_Add_item addItem = new Class_Add_item();

                addItem.DeleteBracket("Locking_brack", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("Locking_Pin", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("Lower_brac_new_m16", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("M6_35_NEW_BRAC", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("RPD_PIN", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("SAT_FUNK", GlobalVariables.bracketCounter);
                addItem.DeleteBracket("Upper_NEW_clamp", GlobalVariables.bracketCounter);

                Session theSession = Session.GetSession();
                Part assemblyPart = (Part)theSession.Parts.Work;

                GlobalVariables.bracketCounter++;

                string D_width = GlobalVariables.PipeDiameter;
                GlobalVariables.Width = textBox1.Text;
                string Width = GlobalVariables.Width;
                string XPos = "10";
                string YPos = "10";

                int IntPoint = (int)GlobalVariables.barrelEnd;

                GlobalVariables.BracketPos = textBox2.Text;
                int bracketPos = int.Parse(GlobalVariables.BracketPos);

                double Pos = IntPoint + bracketPos;

                int Forkint = (int)GlobalVariables.forkdistance + 1;
                //string Gaffel_W = Forkint.ToString();

                int bracketWidth = int.Parse(Width);

                int Frontint = (int)GlobalVariables.forkpoint1[1]; // FrontPos
                int FrontDif = Math.Abs(IntPoint - Frontint);
                int FrontPOS = bracketPos - bracketWidth / 2 - FrontDif + 9;


                //string Gaffel_L = FrontPOS.ToString();
                string Gaffel_L = textBox3.Text;
                string Gaffel_W = textBox4.Text;


                Point3d position = new Point3d(0.0, Pos, 0.0);
                string partsFolderPath = GlobalVariables.FilePath;

                string selected = comboBox1.SelectedItem?.ToString();

                if (string.IsNullOrEmpty(selected))
                {
                    MessageBox.Show("Please select bracket direction");
                    return;
                }

                if (selected == "Over")
                {
                    XPos = "0.01";

                }
                else if (selected == "Right")
                {
                    XPos = "270";
                }
                else if (selected == "Under")
                {
                    XPos = "180";
                }
                else if (selected == "Left")
                {
                    XPos = "90";
                }
                addItem.AddPartToAssembly("Locking_brack.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("Locking_Pin.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("Lower_brac_new_m16.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("M6_35_NEW_BRAC.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("RPD_PIN.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("SAT_FUNK.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos, position, partsFolderPath, assemblyPart);
                addItem.AddPartToAssembly("Upper_NEW_clamp.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos + "," + Gaffel_W + "," + Gaffel_L, position, partsFolderPath, assemblyPart);

                addItem.updateAll();
                addItem.HideDatumsAndSketches();

                UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket updated");

                button6.Enabled = true;
            }

        }
       

        private void Form2_AR15_Load(object sender, EventArgs e)
        {

        }
    }
}
