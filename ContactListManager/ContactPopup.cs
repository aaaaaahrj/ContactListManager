using System;
using System.Windows.Forms;

namespace ContactListManager
{
    public partial class ContactPopup : Form
    {
        public Contact Contact { get; set; }

        public ContactPopup()
        {
            InitializeComponent();
        }

        private void ContactPopup_Load(object sender, EventArgs e)
        {
            // If a Contact was provided, populate the fields for editing.
            if (Contact != null)
            {
                txtName.Text = Contact.Name;
                txtEmail.Text = Contact.Email;
                txtPhoneNumber.Text = Contact.PhoneNumber;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Create or update Contact based on user input
            if (Contact == null)
                Contact = new Contact();

            Contact.Name = txtName.Text?.Trim();
            Contact.Email = txtEmail.Text?.Trim();
            Contact.PhoneNumber = txtPhoneNumber.Text?.Trim();

            DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}