using System;
using System.IO;
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
using System.Configuration;

namespace PasswordManager
{
    public partial class AddCredForm : Form

        //This form allows the user to input credentials which are then stored in the SQL database
    {
        public AddCredForm()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }


        //Creates AES objects which are used to encrypt the text in the username and password text boxes
        private Aes UserAes = Aes.Create();
        private Aes PassAes = Aes.Create();

        List<string> Accounts = new List<string>();
        
        public void Button1_Click(object sender, EventArgs e)
        {

            using (SqlConnection conn = new SqlConnection("Data Source = ADAM-LAPTOP,1433\\MSSQLSERVER; Initial Catalog = Passwords;User Id = PWDBUser; Password = PWDBUser;" +
                "Trusted_Connection = True; MultipleActiveResultSets = True;"))
            {
                conn.Open();

                //Runs a SQL query to retrieve a list of credentials that already exists in the database based on the account name
                void CheckAccountList()
                {

                    SqlCommand RetrieveAcct = new SqlCommand("SELECT * FROM Passwords", conn);
                    using (SqlDataReader Accts = RetrieveAcct.ExecuteReader())
                    {
                        while (Accts.Read())
                        {
                            string ColumnItem = Accts["Account"] as string;
                            Accounts.Add(ColumnItem);
                        }
                    }
                }

                CheckAccountList();

                //Checks if the account entered into the account text box already exists in the databse and displays a message box if so.
                if(Accounts.Contains(AccountTxtBox.Text))
                {
                    string message = "Account Already exists. Please use a different account name or update the existing account.";
                    string title = "Duplicate account";
                    MessageBoxButtons ok = MessageBoxButtons.OK;
                    MessageBox.Show(message, title, ok, MessageBoxIcon.Error);
                }
                
                //Defines the arguments for the AddCredentials() method, calling the encryption method to do so
                else
                {
                    string Account = AccountTxtBox.Text;
                    string WebAddress = WebAddressTxtBox.Text;
                    string Notes = NotesTxtBox.Text;

                    byte[] UserName = EncryptInputToBytes(UserNameTxtBox.Text, UserAes.Key, UserAes.IV);
                    SetDecryptionData(AccountTxtBox.Text + "UserKey", UserAes.Key);
                    SetDecryptionData(AccountTxtBox.Text + "UserIV", UserAes.IV);
                    UserAes.Dispose();
                    byte[] Password = EncryptInputToBytes(PasswordTxtBox.Text, PassAes.Key, PassAes.IV);
                    SetDecryptionData(AccountTxtBox.Text + "PassKey", PassAes.Key);
                    SetDecryptionData(AccountTxtBox.Text + "PassIV", PassAes.IV);
                    PassAes.Dispose();

                    UserNameTxtBox.Clear();
                    PasswordTxtBox.Clear();
                    WebAddressTxtBox.Clear();
                    AccountTxtBox.Clear();
                    NotesTxtBox.Clear();

                    AddCredentials(conn, Account, UserName, Password, WebAddress, Notes);

                    string Message = "Credentials Saved!";
                    string Title = "Done";
                    MessageBoxButtons Buttons = MessageBoxButtons.OK;
                    MessageBox.Show(Message, Title, Buttons, MessageBoxIcon.Information);


                }
            }
        }
        
        //Calls on a stored procedure to send the text entered into the text boxes to the SQL database as strings.
        //Info is setnt as byte arrays in the case of the username and password text boxes
        static void AddCredentials(SqlConnection conn, string Account, byte[] UserName, byte[] Password, string WebAddress, string Notes)
        {
            SqlCommand AddCreds = new SqlCommand("AddCredentials", conn);
            AddCreds.CommandType = CommandType.StoredProcedure;
            AddCreds.Parameters.AddWithValue("Account", Account);
            AddCreds.Parameters.AddWithValue("UserName", UserName);
            AddCreds.Parameters.AddWithValue("Password", Password);
            AddCreds.Parameters.AddWithValue("WebAddress", WebAddress);
            AddCreds.Parameters.AddWithValue("Notes", Notes);
            AddCreds.ExecuteNonQuery();
        }

        //Converts the string input to an encrypted byte array
        public byte[] EncryptInputToBytes(string plainText, byte[] Key, byte[] IV)
        {

            byte[] encrypted;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return encrypted;
        }

        //Stores the input in app.config for later use
        private static void SetDecryptionData(string Key, byte[] Value)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string ValueString = System.Convert.ToBase64String(Value);
            configuration.AppSettings.Settings.Add(Key, ValueString);
            configuration.AppSettings.Settings[Key].Value = ValueString;
            configuration.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
        }

        //Closes the form
        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region "Unused event Handlers"
        private void UserNameTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void AccountTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label6_Click(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void PasswordTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void NotesTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void WebAddressTxtBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}
