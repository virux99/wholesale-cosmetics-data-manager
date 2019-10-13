using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlueDiamond
{
    public partial class Stock_List : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        //ID variable used in Updating and Deleting Record
        int ID = 0;
        public Stock_List()
        {
            InitializeComponent();
            DisplayData();

          
            if (con != null && con.State == ConnectionState.Open)
                con.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (con != null && con.State == ConnectionState.Open)
                con.Close();

            try
            {
                cmd = new SqlCommand("insert into Stock(ProductName) values(@ProductName)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@ProductName", txtName.Text);
               

                cmd.ExecuteNonQuery();
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
                MessageBox.Show("Product Inserted Successfully");
                DisplayData();
                ClearData();
                
            }
            catch (Exception ex)
            {
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
                MessageBox.Show(ex.Message);
            }

        }


        //Display Data in DataGridView
        private void DisplayData()
        {
            if (con != null && con.State == ConnectionState.Open)
                con.Close();
            try
            {
                con.Open();
                DataTable dt = new DataTable();
                adapt = new SqlDataAdapter("select * from Stock", con);
                adapt.Fill(dt);
                dataGridView1.DataSource = dt;
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }
            catch (Exception ex)
            {
                if (con != null && con.State == ConnectionState.Closed)
                    MessageBox.Show(ex.Message);
            }
        }
        //Clear Data 
        private void ClearData()
        {

            txtName.Text = "";
            txtID.Text = "";
                ID = 0;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (con != null && con.State == ConnectionState.Open)
                con.Close();
            ID = Convert.ToInt32(txtID.Text);
            if (ID != 0)
            {

                cmd = new SqlCommand("delete from Stock where Id=@id", con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", ID);
                cmd.ExecuteNonQuery();
                if (con != null && con.State == ConnectionState.Closed)
                    MessageBox.Show("Record Deleted Successfully!");
                DisplayData();
                ClearData();
                MessageBox.Show("Record deleted");
            }
            else
            {
                MessageBox.Show("Please Select Record to Delete");
                if (con != null && con.State == ConnectionState.Open)
                    con.Close();
            }

        }
    }
}
