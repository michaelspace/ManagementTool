using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class DetailForm : Form
    {
        ManagementEntities db = null;

        public DetailForm(Detail obj)
        {
            InitializeComponent();
            db = new ManagementEntities();
            if (obj == null)
            {
                detailBindingSource.DataSource = new Detail();
                db.Details.Add(detailBindingSource.Current as Detail);
            }
            else
            {
                detailBindingSource.DataSource = obj;
                db.Details.Attach(detailBindingSource.Current as Detail);
            }
        }

        private async void DetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
                {
                    MessageBox.Show("Name and Phone fields cannot be empty.\nEnter missing fields.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Focus();
                    e.Cancel = true;
                    return;
                }
                await db.SaveChangesAsync();
                e.Cancel = false;
            }
            e.Cancel = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }
    }
}
