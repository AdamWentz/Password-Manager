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

namespace PasswordManager
{
    public partial class DeleteCredForm : Form
        //Allows the user to delete an account and the corresponding credentials from the database

    {
        public DeleteCredForm()
        {
            InitializeComponent();
            FillCombo();
        }
        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeleteAcctButton_Click(object sender, EventArgs e)
        {
            string SelectedAccount = CredentialDDL.Text;

            //Checks if the user made a selection. Dispalys a message box if not
            if (SelectedAccount == "Select an account. . .")
            {
                string message = "Please select an account.";
                string title = "No Account Selected";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);
            }

            //Confirms the delete action with the user, then runs SQL query to delete the row containing the account selected in the dropdown list
            else
            {
                string message = "Are you sure you want to delete this account?";
                string title = "Confirm Delete";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult ConfirmDelete =  MessageBox.Show(message, title, buttons, MessageBoxIcon.Question);

                if (ConfirmDelete == DialogResult.Yes)
                {
                    using (SqlConnection conn = new SqlConnection("Data Source = ADAM-LAPTOP,1433\\MSSQLSERVER; Initial Catalog = Passwords;User Id = PWDBUser; Password = PWDBUser;" +
                        "Trusted_Connection = True; MultipleActiveResultSets = True;"))
                    {
                        conn.Open();

                        SqlCommand DeleteAccount = new SqlCommand("DELETE FROM Passwords WHERE Account = '" + SelectedAccount + "'", conn);
                        DeleteAccount.ExecuteNonQuery();

                        string DeletedBoxMessage = "The selected account has been deleted.";
                        string DeletedBoxTitle = "Account Deleted!";
                        MessageBoxButtons DeletedBoxButtons = MessageBoxButtons.OK;
                        MessageBox.Show(DeletedBoxMessage, DeletedBoxTitle, DeletedBoxButtons, MessageBoxIcon.Information);

                        CredentialDDL.ResetText();

                        FillCombo();
                    }
                }
            }
        }

        private void CredentialDDL_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Populates the dropdown list
        void FillCombo()
        {
            using (SqlConnection conn = new SqlConnection("Data Source = ADAM-LAPTOP,1433\\MSSQLSERVER; Initial Catalog = Passwords;User Id = PWDBUser; Password = PWDBUser;" +
                "Trusted_Connection = True; MultipleActiveResultSets = True;"))
            {
                conn.Open();

                SqlCommand RetrieveAcct = new SqlCommand("SELECT * FROM Passwords", conn);
                using (SqlDataReader Accts = RetrieveAcct.ExecuteReader())
                {
                    while (Accts.Read())
                    {
                        string ColumnItem = Accts["Account"] as string;
                        CredentialDDL.Items.Add(ColumnItem);
                    }
                }
            }
        }
    }
}
