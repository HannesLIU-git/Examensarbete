using NXOpen;
using NXOpen.Annotations;
using NXOpen.CAE;
using NXOpen.Drawings;
using NXOpen.UF;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace BRAC_FORM
{
    public partial class Form2_PICATINNY : Form
    {
        public Form2_PICATINNY()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Session theSession = Session.GetSession();
            Part assemblyPart = (Part)theSession.Parts.Work;

            string Width = textBox1.Text;
            string Parallax = textBox2.Text;
            Point3d position = new Point3d(0.0, 0.0, 0.0);
            string D_width = GlobalVariables.PipeDiameter;
            string partsFolderPath = GlobalVariables.FilePath;
            string XPos = textBox3.Text;
            string YPos = textBox4.Text;
            string ParaWidth = textBox1.Text;

            
            Point3d SATpos = new Point3d(0.0, 0.0, 0.0);

            Class_Add_item addItem = new Class_Add_item();
            string selected = comboBox1.SelectedItem?.ToString();
           
            if (string.IsNullOrEmpty(selected))
            {
                MessageBox.Show("Please select a SAT position");
                return;
            }

            if (selected == "Equal")
            {
                D_width = "0";
                SATpos = new Point3d(0.0, 0.0, 0.0);

                


            }
            else if (selected == "Over")
            {
                D_width = "1";
                double X_Double = double.Parse(XPos);
                double W_Double = double.Parse(Width);
                double Y_Double = double.Parse(YPos);

                SATpos = new Point3d(-X_Double - 11.8713, (-W_Double / 2) - Y_Double, 1.6);

                ParaWidth = "20";

            }
            else if (selected == "Under")
            {
                D_width = "2";
                double X_Double = double.Parse(XPos);
                double W_Double = double.Parse(Width);
                double Y_Double = double.Parse(YPos);

                SATpos = new Point3d(X_Double + 7.2950, (-W_Double / 2) - Y_Double, 1.6);

                ParaWidth = "20";
            }


            GlobalVariables.bracketCounter++;
            

            addItem.AddPartToAssembly("Lower_brac_Picatinny.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Upper_brac_Picatinny.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + XPos + "," + YPos, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Skruvar_picatinny.prt", GlobalVariables.bracketCounter, D_width + "," + Width, position, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("Parralax_distans.prt", GlobalVariables.bracketCounter, D_width + "," + ParaWidth + "," + Parallax, SATpos, partsFolderPath, assemblyPart);
            addItem.AddPartToAssembly("SAT_II_Picatinny.prt", GlobalVariables.bracketCounter, D_width + "," + Width + "," + Parallax, SATpos, partsFolderPath, assemblyPart);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, "Picatinny Bracket added.");

            addItem.updateAll();
            addItem.HideDatumsAndSketches();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1_PICATINNY form1 = new Form1_PICATINNY();
            form1.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3_PICATINNY Form3_PICATINNY = new Form3_PICATINNY();
            Form3_PICATINNY.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Class_Add_item addItem = new Class_Add_item();

            addItem.DeleteBracket("Lower_brac_Picatinny", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Upper_brac_Picatinny", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Skruvar_picatinny", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("Parralax_distans", GlobalVariables.bracketCounter);
            addItem.DeleteBracket("SAT_II_Picatinny", GlobalVariables.bracketCounter);
            //addItem.DeleteBracket("Picatinny_rail", GlobalVariables.bracketCounter);

            UI.GetUI().NXMessageBox.Show("Success", NXMessageBox.DialogType.Information, $"Bracket was removed from assembly");
        }

        private void button5_Click(object sender, EventArgs e) ////////////////////// DRAWING
        {

        }


        private void Form2_PICATINNY_Load(object sender, EventArgs e)
        {
        }

        private void button6_Click(object sender, EventArgs e)
        {
           
        
            this.Close(); // stänger formuläret
            Session.GetSession().ApplicationSwitchImmediate("UG_APP_MODELING"); // Ger tillbaka kontrollen
            Environment.Exit(0);

        }
    }
}
