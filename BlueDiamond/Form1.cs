using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BlueDiamond
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        int order = 1;
        Double total = 0;
        int billID;
        Double totalDiscount = 0;
        Double sum = 0;

        public Form1()
        {
            InitializeComponent();
            AutoCompleteProductName();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

            
                if ((string.IsNullOrWhiteSpace(txtDiscount.Text) == true) || (string.IsNullOrWhiteSpace(txtAreaName.Text) == true) || (string.IsNullOrWhiteSpace(txtShopName.Text) == true) || (string.IsNullOrWhiteSpace(txtBillNumber.Text) == true) || (string.IsNullOrWhiteSpace(txtProductName.Text) == true) || (string.IsNullOrWhiteSpace(txtQuantity.Text) == true) || (string.IsNullOrWhiteSpace(txtRate.Text) == true) || (string.IsNullOrWhiteSpace(txtTradePrice.Text) == true))
                {
                    MessageBox.Show("fill out the values please");
                }
                else
                {

                    if (chkScheme.Checked == false)
                        sum = Convert.ToDouble(txtQuantity.Text) * Convert.ToDouble(txtTradePrice.Text);
                    Bill obj = new Bill()
                    {
                        id = order++,
                        productName = txtProductName.Text,
                        Quantity = Convert.ToDouble(txtQuantity.Text),
                        TP = Math.Round(Convert.ToDouble(txtTradePrice.Text), 3),
                        Rate = Math.Round(Convert.ToDouble(txtRate.Text), 3),
                        NP = Math.Round(Convert.ToDouble(txtTradePrice.Text), 3),
                        Discount = Math.Round(Convert.ToDouble(txtDiscount.Text), 2),
                        Scheme = txtScheme.Text,
                        Amount = Math.Round(Convert.ToDouble(sum - ((Convert.ToDouble(txtDiscount.Text) * sum) / 100)), 3)
                    };
                    if (chkScheme.Checked == false)
                    {
                        total += Math.Round(obj.Amount, 3);
                        txtTotal.Text = total.ToString();
                        totalDiscount += Math.Round((Convert.ToDouble(txtDiscount.Text) * sum) / 100, 3);
                    }
                    billID = Convert.ToInt32(txtBillNumber.Text);
                    billBindingSource.Add(obj);
                    billBindingSource.MoveLast();
                    txtDiscount.Text = null;
                    txtProductName.Text = null;
                    txtQuantity.Text = null;
                    txtRate.Text = null;
                    txtTradePrice.Text = null;
                    if (String.IsNullOrWhiteSpace(txtScheme.Text))
                        obj.Scheme = "0";
                    if (chkScheme.Checked == true)
                    {
                        obj.NP = Math.Round(obj.Rate / Convert.ToDouble(txtTotalscheme.Text), 3);
                        obj.Amount = Math.Round((obj.NP * obj.Quantity) - (((obj.NP * obj.Quantity) * obj.Discount) / 100), 3);
                        total += Math.Round(obj.Amount, 3);
                        totalDiscount += Math.Round((((obj.NP * obj.Quantity) * obj.Discount) / 100), 3);
                        sum = Math.Round(Convert.ToDouble(obj.NP) * Convert.ToDouble(obj.Quantity));
                        txtTotal.Text = total.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            using (Print prnt = new Print(billBindingSource.DataSource as List<Bill>, sum=totalDiscount+total, txtAreaName.Text, txtShopName.Text, string.Format("{0}", totalDiscount), billID, DateTime.Now.ToString("MM/dd/yy"), string.Format("RS: {0}", total)))
            {
                prnt.ShowDialog();
            }
            billBindingSource.Clear();
            order = 1;
           total = 0;
           totalDiscount = 0;
           sum = 0;
            txtTotal.Text = null;

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            billBindingSource.DataSource = new List<Bill>();
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                    (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtTradePrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;

            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            if (string.IsNullOrWhiteSpace(txtTradePrice.Text))
            {
                txtRate.Text = null;
            }
        }

        private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
        (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }

        }
        void AutoCompleteProductName()
        {
            if (con != null && con.State == ConnectionState.Open)
                con.Close();
            txtProductName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtProductName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection col = new AutoCompleteStringCollection();
            SqlDataReader reader;
            string query = "select * from Stock";
            SqlCommand cmd = new SqlCommand(query, con);

            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string estimate = Convert.ToString(reader["ProductName"]);
                    col.Add(estimate);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtProductName.AutoCompleteCustomSource = col;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

            Bill obj = billBindingSource.Current as Bill;
            if (obj != null)
            {
                if (obj.Scheme != "0")
                {
                    totalDiscount = Convert.ToDouble(totalDiscount - (Math.Round((((obj.NP * obj.Quantity) * obj.Discount) / 100), 3)));
                    total = total - (Math.Round(obj.Amount, 3));
                    txtTotal.Text = total.ToString();
                    billBindingSource.RemoveCurrent();
                    order = order - 1;
                }
                else
                {
                    total = total - Math.Round(obj.Amount, 3);
                    txtTotal.Text = total.ToString();
                    totalDiscount = totalDiscount - Convert.ToDouble(((obj.NP * obj.Quantity) * obj.Discount) / 100);
                    billBindingSource.RemoveCurrent();
                    order = order - 1;
                }

            }
        }

        private void txtBillNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                   (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;

            }
        }

        private void txtTradePrice_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtTradePrice.Text))
            {
                double number;
                if (double.TryParse(txtTradePrice.Text, out number))
                    txtRate.Text = (number * 12).ToString();
            }
        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtRate.Text))
            {
                double number;
                if (double.TryParse(txtRate.Text, out number))
                    txtTradePrice.Text = (number / 12).ToString();
            }
        }

    }
}