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

namespace ContactListManager
{
    public partial class MainForm : Form
    {
        // Public property that stores all contacts
        public BindingList<Contact> Contacts { get; } = new BindingList<Contact>();

        public MainForm()
        {
            InitializeComponent();

            // Wire button click handlers
            this.btnAddContact.Click += this.btnAddContact_Click;
            this.btnEditContact.Click += this.btnEditContact_Click;
            this.btnSaveToCSV.Click += this.btnSaveToCSV_Click;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadContactsFromCSV();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadContactsFromCSV()
        {
            const string fileName = "contacts.csv";

            if (!File.Exists(fileName))
            {
                File.WriteAllText(fileName, "Name,Email,PhoneNumber");
            }

            Contacts.Clear();

            string[] lines = File.ReadAllLines(fileName);
            foreach (string line in lines.Skip(1)) // Skip header
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                // Simple CSV split — assumes no embedded commas in values.
                var parts = line.Split(',');
                var contact = new Contact
                {
                    Name = parts.Length > 0 ? parts[0] : string.Empty,
                    Email = parts.Length > 1 ? parts[1] : string.Empty,
                    PhoneNumber = parts.Length > 2 ? parts[2] : string.Empty
                };

                Contacts.Add(contact);
            }

            UpdateContactGrid();
        }

        private void UpdateContactGrid()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = Contacts;
        }

        private void SaveContactsToCSV()
        {
            const string fileName = "contacts.csv";

            var lines = new List<string> { "Name,Email,PhoneNumber" };
            foreach (var contact in Contacts)
            {
                lines.Add($"{EscapeForCsv(contact.Name)},{EscapeForCsv(contact.Email)},{EscapeForCsv(contact.PhoneNumber)}");
            }

            File.WriteAllLines(fileName, lines, Encoding.UTF8);
            MessageBox.Show("Contacts saved successfully!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static string EscapeForCsv(string value)
        {
            if (value == null)
                return string.Empty;

            // If value contains comma or quote or newline, wrap in quotes and escape inner quotes.
            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r"))
            {
                return $"\"{value.Replace("\"", "\"\"")}\"";
            }

            return value;
        }

        private void btnAddContact_Click(object sender, EventArgs e)
        {
            var popup = new ContactPopup();
            if (popup.ShowDialog() == DialogResult.OK)
            {
                Contacts.Add(popup.Contact);
                UpdateContactGrid();
            }
        }

        private void btnEditContact_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a contact to edit.", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selected = dataGridView1.CurrentRow.DataBoundItem as Contact;
            if (selected == null)
                return;

            // Create a copy for editing so cancel doesn't change the original
            var editable = new Contact
            {
                Name = selected.Name,
                Email = selected.Email,
                PhoneNumber = selected.PhoneNumber
            };

            var popup = new ContactPopup
            {
                Contact = editable
            };

            if (popup.ShowDialog() == DialogResult.OK)
            {
                // Apply changes back to selected contact
                selected.Name = popup.Contact.Name;
                selected.Email = popup.Contact.Email;
                selected.PhoneNumber = popup.Contact.PhoneNumber;
                UpdateContactGrid();
            }
        }

        private void btnSaveToCSV_Click(object sender, EventArgs e)
        {
            SaveContactsToCSV();
        }

        private void btnEditContact_Click_1(object sender, EventArgs e)
        {

        }
    }
}
