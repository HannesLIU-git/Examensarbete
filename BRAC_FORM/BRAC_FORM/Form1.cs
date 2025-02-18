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
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hannes hannes = new Hannes(textBox1,textBox2);
            hannes.AddCylinders();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Hannes hannes = new Hannes(textBox1, textBox2);
            hannes.AddCubes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Hannes hannes = new Hannes(textBox1, textBox2);
            hannes.DeleteCubes();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Create the instance of the Class_Add_item class
            Class_Add_item addItem = new Class_Add_item(textBox5, textBox6);

            // Call method to add the three parts to the assembly
            addItem.AddThreeParts();
        }
    }
}
