using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PasswordManager
{
    static class Program
    {
        /// <summary>
        /// Basic password management program. Interacts with a local SQL Server to store username, password, account name, website, and notes information.
        /// The user can add, change, or delete accounts and all corresponding information.
        /// For the intial login screen, username and password are encrypted using SHA256 before being sent to the SQL server.
        /// For the various account details, Account name, website, and notes are stored as plain text, but Username and Password info is encrypted using AES symmetrical encryption.
        /// </summary>
        [STAThread]
        static void Main()
        {
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }

    }
}
