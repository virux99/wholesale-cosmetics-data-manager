using System;
using System.Windows.Forms;

namespace BlueDiamond
{
    public partial class MainScreen : Form
    {
        public MainScreen()
        {
           
            InitializeComponent();
        }

        private void btnMakeBill_Click(object sender, EventArgs e)
        {
            Form1 frm = new Form1();
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stock_List stock = new Stock_List();
            stock.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}