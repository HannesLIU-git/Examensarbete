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
            hannes.AddMultipleCubesAndSetExpression();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
