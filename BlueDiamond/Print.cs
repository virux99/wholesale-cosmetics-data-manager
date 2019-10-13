using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlueDiamond
{
    public partial class Print : Form
    {
        List<Bill> _list;
        string _date;
        string _total;
        int _BillID;
        string _totalDiscount;
        string _shopName;
        double _sum;
        string _areaName;
        public Print(List<Bill> datasource,double sum, string areaName, string shopName, string totalDiscount, int BillID, string date, string total)
        {
            InitializeComponent();
            _list = datasource;
            _date = date;
            _total = total;
            _BillID = BillID;
            _totalDiscount = totalDiscount;
            _shopName = shopName;
            _sum = sum;
            _areaName = areaName;
        }

        private void Print_Load(object sender, EventArgs e)
        {
            BillBindingSource.DataSource = _list;
            Microsoft.Reporting.WinForms.ReportParameter[] para = new Microsoft.Reporting.WinForms.ReportParameter[]
            {
                   new Microsoft.Reporting.WinForms.ReportParameter("pTotal",_total),
                   new Microsoft.Reporting.WinForms.ReportParameter("pSum",_sum.ToString()),
                   new Microsoft.Reporting.WinForms.ReportParameter("pTotalDiscount",_totalDiscount),
                   new Microsoft.Reporting.WinForms.ReportParameter("pShopName",_shopName),
                   new Microsoft.Reporting.WinForms.ReportParameter("pAreaName",_areaName),
                   new Microsoft.Reporting.WinForms.ReportParameter("pBillID",_BillID.ToString()),
                   new Microsoft.Reporting.WinForms.ReportParameter("pDate",_date)
           };
            this.reportViewer1.LocalReport.SetParameters(para);
            this.reportViewer1.RefreshReport();
            exportToPDF();

        }

       

        private void exportToPDF()

        {
            string path = "D:\\"+ DateTime.Now.ToString("dd_HH_mm_ss")+".pdf"; // case sensitive

           
            System.IO.FileInfo fi = new System.IO.FileInfo(path);

            if (fi.Exists) fi.Delete();

            Warning[] warnings;

            string[] streamids;

            string mimeType, encoding, filenameExtension;

            byte[] bytes = reportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension,

            out streamids, out warnings);

            System.IO.FileStream fs = System.IO.File.Create(path);

            fs.Write(bytes, 0, bytes.Length);

            fs.Close();

        }
    }
}
