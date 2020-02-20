using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordManager
{
    public partial class MainForm : Form
        // This is the initial form the user sees upon login and presents them with options to add, change, view, or delete stored credentials.
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form AddCredForm = new AddCredForm();
            AddCredForm.Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Form RetCredForm = new RetCredForm();
            RetCredForm.Show();
        }

        private void ChangeCredButton_Click(object sender, EventArgs e)
        {
            Form DeleteCredForm = new DeleteCredForm();
            DeleteCredForm.Show();
        }
    }
}
