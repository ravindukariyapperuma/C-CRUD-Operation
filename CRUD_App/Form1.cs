using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CRUD_App
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        void Clear()
        {
            txtFirstname.Text = txtLastname.Text = txtCity.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            populateDataGridView();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstname.Text.Trim();
            model.Lastname = txtLastname.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();
            using (CrudDBEntities db = new CrudDBEntities())
            {
                if (model.CustomerID == 0)//Insert
                    db.Customers.Add(model);
                else //Update
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            Clear();
            populateDataGridView();
            MessageBox.Show("Submitted Successfully");
        }

        void populateDataGridView()
        {
            dataCustomer.AutoGenerateColumns = false;
            using (CrudDBEntities db = new CrudDBEntities())
            {
                dataCustomer.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dataCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataCustomer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dataCustomer.CurrentRow.Cells["CustomerID"].Value);
                using (CrudDBEntities db = new CrudDBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtFirstname.Text = model.FirstName;
                    txtLastname.Text = model.Lastname;
                    txtCity.Text = model.City;
                    txtAddress.Text = model.Address;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you Sure to Delete this Record ?","CRUD Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (CrudDBEntities db = new CrudDBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Customers.Attach(model);
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    populateDataGridView();
                    Clear();
                    MessageBox.Show("Deleted Successfully");
                }
            }
        }
    }
}
