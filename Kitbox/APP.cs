using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class APP : Form
    {
        public APP()
        {
            InitializeComponent();
        }

        private void button21_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(textBox9.Text))
            {
                panel3.Visible = false;
            }
            else
            {

                panel3.Visible = true;
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            
        }

        private void button18_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {
            panel5.Visible = true;
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {              
                
        }

        private void button2_Click(object sender, EventArgs e)
        {    
                panel2.Visible = true;
                
           
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void label33_Click(object sender, EventArgs e)
        {
            label33.Text= "Kitbox Fabric" + "\n" + "Avenue du Chocolat 50" + "\n" + "1400 Nivelles" + "\n" + "Tél: 02 99 99 99 99" + "  " + "TVA: BE08XXXXXXXX" + "\n" + "www.kitboxfabric.com";

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void button25_Click(object sender, EventArgs e)
        {
            panel4.Visible = true;
            //panel3.Visible = false;
        }

        private void button27_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
            panel3.Visible = true;
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {

        }

        private void button28_Click(object sender, EventArgs e)
        {
            panel5.Visible = false;
        }

        private void Connexion_Click(object sender, EventArgs e)
        {

        }
    }
}
