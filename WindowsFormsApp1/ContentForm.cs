using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ContentForm : Form
    {
        ManagementEntities db = null;

        public ContentForm()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ContentForm_Load(object sender, EventArgs e)
        {
            db = new ManagementEntities();
            detailBindingSource.DataSource = db.Details.ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (DetailForm df = new DetailForm(null))
            {
                if (df.ShowDialog() == DialogResult.OK)
                {
                    detailBindingSource.DataSource = db.Details.ToList();

                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (detailBindingSource.Current == null)
            {
                return;
            }
            using (DetailForm df = new DetailForm(detailBindingSource.Current as Detail))
            {
                if (df.ShowDialog() == DialogResult.OK)
                {
                    detailBindingSource.DataSource = db.Details.ToList();

                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (detailBindingSource.Current != null)
            {
                if (MessageBox.Show("Confirm to delete this record.", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    db.Details.Remove(detailBindingSource.Current as Detail);
                    detailBindingSource.RemoveCurrent();
                    db.SaveChanges();
                }
            }
        }

        private void btnXML_Click(object sender, EventArgs e)
        {
            DataTable dt = getDataTableFromDataGridView(dataGridView);
            DataSet ds = new DataSet() { DataSetName = "Employees" };
            ds.Tables.Add(dt);

            SaveFileDialog sfd = new SaveFileDialog() { Filter = "XML|*.xml" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ds.WriteXml(sfd.FileName);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
        private DataTable getDataTableFromDataGridView(DataGridView dgv)
        {
            var dt = new DataTable() { TableName = "Details" };
            string[] columns = { "Id", "Name", "Surname", "Email", "PhoneNumber", "Position", "Address" };
            int tmp = 0;

            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible)
                {
                    dt.Columns.Add();
                    dt.Columns[tmp].ColumnName = tmp < columns.Length ? columns[tmp++] : "Unset Column";
                }
            }

            object[] cells = new object[dgv.Columns.Count];

            foreach (DataGridViewRow row in dgv.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    cells[i] = row.Cells[i].Value.ToString().Trim();
                }
                dt.Rows.Add(cells);
            }

            return dt;
        }
    }
}
