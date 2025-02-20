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
    }
}
