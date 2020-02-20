using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace PasswordManager
{
    public partial class LoginForm : Form
        //Login screen for the application
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        //Checks if the user made input and displays a message box if not.
        public void CreateAcctButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserNameTxtBox.Text) || string.IsNullOrWhiteSpace(PasswordTxtBox.Text))
            {
                string Message = "Please provide a user name and password";
                string Title = "Info needed";
                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                MessageBox.Show(Message, Title, Buttons, MessageBoxIcon.Warning);
            }

            //Defines the arguments for the AddCredentials() method, calling the encryption method below to do so
            else
            {
                using (SqlConnection conn = new SqlConnection("Data Source = ADAM-LAPTOP,1433\\MSSQLSERVER; Initial Catalog = Passwords;User Id = PWDBUser; Password = PWDBUser;" +
                "Trusted_Connection = True; MultipleActiveResultSets = True;"))
                {
                    conn.Open();

                    string SavedUserName = CreateHash(UserNameTxtBox.Text);
                    string SavedPassword = CreateHash(PasswordTxtBox.Text);
                    
                    AddCredentials(conn, SavedUserName, SavedPassword);

                    string Message = "Account Created!";
                    string Title = "Yay!";
                    MessageBoxButtons Buttons = MessageBoxButtons.OK;
                    MessageBox.Show(Message, Title, Buttons, MessageBoxIcon.Information);

                    UserNameTxtBox.Clear();
                    PasswordTxtBox.Clear();
                }
            }
        }
        
        //Calls a stored procedure the adds a row to the SQL database containig the user's login credentials
        static void AddCredentials(SqlConnection conn, string UserName, string Password)
        {
            SqlCommand AddCreds = new SqlCommand("AddLoginCreds", conn);
            AddCreds.CommandType = CommandType.StoredProcedure;
            AddCreds.Parameters.AddWithValue("UserName", UserName);
            AddCreds.Parameters.AddWithValue("Password", Password);
            AddCreds.ExecuteNonQuery();
            conn.Close();
        }

        //Checks to see if the login credentials entered by the user match a set of credentials that exist in the database
        private void LoginButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source = ADAM-LAPTOP,1433\\MSSQLSERVER; Initial Catalog = Passwords;User Id = PWDBUser; Password = PWDBUser;" +
                "Trusted_Connection = True; MultipleActiveResultSets = True;"))
            {
                conn.Open();

                string UserInputHash = CreateHash(UserNameTxtBox.Text);
                string PassInputHash = CreateHash(PasswordTxtBox.Text);

                List<string> LoginNames = new List<string>();
                List<string> Passwords = new List<string>();

                SqlCommand Authenticate = new SqlCommand("SELECT * FROM LoginCreds", conn);
                using (SqlDataReader Logins = Authenticate.ExecuteReader())
                {
                    while (Logins.Read())
                    {
                        string EncryptedUserName = Logins["UserName"] as string;
                        LoginNames.Add(EncryptedUserName);

                        string EncryptedPassword = Logins["Password"] as string;
                        Passwords.Add(EncryptedPassword);
                    }

                    //Checks if the input in the UserName text box matches a user name from the database and if so, runs a sql query to retrieve the 
                    //password related to that user name and check it against the input in the Password text box. If both match, the user is granted access to the next form. 
                    if (LoginNames.Contains(UserInputHash))
                    {
                        string RelatedPassword;

                        SqlCommand CheckCredRelation = new SqlCommand("SELECT Password FROM LoginCreds WHERE UserName = '" + UserInputHash + "'", conn);
                        using (SqlDataReader Password = CheckCredRelation.ExecuteReader())
                        {
                            while (Password.Read())
                            {
                                RelatedPassword = Password["Password"] as string;

                                if (PassInputHash == RelatedPassword)
                                {
                                    this.Hide();

                                    Form MainForm = new MainForm();
                                    MainForm.Show();

                                    UserNameTxtBox.Clear();
                                    PasswordTxtBox.Clear();
                                }
                                else
                                {
                                    string LoginFailed = "Login failed. Please try again.";
                                    string LoginFailedTitle = "Login Failed!";
                                    MessageBoxButtons OK = MessageBoxButtons.OK;
                                    MessageBox.Show(LoginFailed, LoginFailedTitle, OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        string LoginFailed = "Login failed. Please try again.";
                        string LoginFailedTitle = "Login Failed!";
                        MessageBoxButtons OK = MessageBoxButtons.OK;
                        MessageBox.Show(LoginFailed, LoginFailedTitle, OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        //Accepts a string as input, converts that string to a byte[], converts that byte[] to a hash using SHA256, and then converts the hash back to a string.
        public string CreateHash(string input)
        {
            HashAlgorithm sha = SHA256.Create(); 
            byte[] hashData = sha.ComputeHash(Encoding.Default.GetBytes(input));
            string hashDataString = System.Convert.ToBase64String(hashData);
            return hashDataString;
        }

        //Hides user input in the 'password' text box
        private void PasswordTxtBox_TextChanged(object sender, EventArgs e)
        {
            PasswordTxtBox.PasswordChar = '*';
        }
    }
}
