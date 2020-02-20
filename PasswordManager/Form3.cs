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
using System.IO;
using System.Configuration;

namespace PasswordManager
{
    public partial class RetCredForm : Form
        //Allows the user to view any saved credentials as well as make any desired updates
    {
        public RetCredForm()
        {
            InitializeComponent();
            FillCombo();
        }

        private void RetCredForm_Load(object sender, EventArgs e)
        {

        }

        //Runs SQL queries to retrieve data from the database and populates the text boxes with that data
        //Also decrypts the received username and password data
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedAccount = CredentialDDL.Text;

            using (SqlConnection conn = new SqlConnection("Data Source = ADAM-LAPTOP,1433\\MSSQLSERVER; Initial Catalog = Passwords;User Id = PWDBUser; Password = PWDBUser;" +
                "Trusted_Connection = True; MultipleActiveResultSets = True;"))
            {
                conn.Open();

                SqlCommand ShowUserName = new SqlCommand("SELECT ID from Passwords WHERE Account =  '" + SelectedAccount + "'", conn);
                using (SqlDataReader UserName = ShowUserName.ExecuteReader())
                {
                    while (UserName.Read())
                    {
                        byte[] SelectedUserID = UserName.GetValue(0) as byte[];
                        byte[] UserKeyDecryption = Convert.FromBase64String(GetDecryptionData(SelectedAccount + "UserKey"));
                        byte[] UserIVDecryption = Convert.FromBase64String(GetDecryptionData(SelectedAccount + "UserIV"));
                        UserNameTxtBox.Text = DecryptInputFromBytes(SelectedUserID, UserKeyDecryption, UserIVDecryption);
                    }
                }

                SqlCommand ShowPassword = new SqlCommand("SELECT Password from Passwords WHERE Account =  '" + SelectedAccount + "'", conn);
                using (SqlDataReader Password = ShowPassword.ExecuteReader())
                {
                    while (Password.Read())
                    {
                        byte[] SelectedPassword = Password.GetValue(0) as byte[];
                        byte[] PassKeyDecryption = Convert.FromBase64String(GetDecryptionData(SelectedAccount + "PassKey"));
                        byte[] PassIVDecryption = Convert.FromBase64String(GetDecryptionData(SelectedAccount + "PassIV"));
                        PasswordTxtBox.Text = DecryptInputFromBytes(SelectedPassword, PassKeyDecryption, PassIVDecryption);
                    }
                }

                SqlCommand ShowWebsite = new SqlCommand("SELECT Website from Passwords WHERE Account =  '" + SelectedAccount + "'", conn);
                using (SqlDataReader WebSite = ShowWebsite.ExecuteReader())
                {
                    while (WebSite.Read())
                    {
                        string SelectedWebsite = WebSite["Website"] as string;
                        WebAddressTxtBox.Text = SelectedWebsite;
                    }
                }

                SqlCommand ShowNotes = new SqlCommand("SELECT Notes from Passwords WHERE Account =  '" + SelectedAccount + "'", conn);
                using (SqlDataReader Notes = ShowNotes.ExecuteReader())
                {
                    while (Notes.Read())
                    {
                        string SelectedNotes = Notes["Notes"] as string;
                        NotesTxtBox.Text = SelectedNotes;
                    }
                }
            }    
        }

        //Closes the form
        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Decrypts the input from a byte array to a string
        public string DecryptInputFromBytes(byte[] CipherText, byte[] Key, byte[] IV)
        {
            string PlainText = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform Decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(CipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, Decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            PlainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return PlainText;
        }

        //Retrieves the encryption key and IV from app.config
        private static string GetDecryptionData(string Key)
        {
            return ConfigurationManager.AppSettings[Key];
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

        //Runs a SQL query to populate the dropdown list with existing accounts from the database
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

        //Checks if the user has made a selection and shows a message box if not
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (CredentialDDL.Text == "Select an account. . .")
            {
                string message = "Please select an account to update";
                string title = "No Account Selected";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                MessageBox.Show(message, title, buttons, MessageBoxIcon.Error);

            }
            
            //Defines the arguments for the UpdateCredentials() method, calling the encryption method to do so
            else
            {
                using (SqlConnection conn = new SqlConnection("Data Source = ADAM-LAPTOP,1433\\MSSQLSERVER; Initial Catalog = Passwords;User Id = PWDBUser; Password = PWDBUser;" +
                "Trusted_Connection = True; MultipleActiveResultSets = True;"))
                {
                    conn.Open();

                    string Account = CredentialDDL.Text;
                    string WebAddress = WebAddressTxtBox.Text;
                    string Notes = NotesTxtBox.Text;

                    byte[] UserKeyDecryption = System.Convert.FromBase64String(GetDecryptionData(Account + "UserKey"));
                    byte[] UserIVDecryption = System.Convert.FromBase64String(GetDecryptionData(Account + "UserIV"));
                    byte[] UserName = EncryptInputToBytes(UserNameTxtBox.Text, UserKeyDecryption, UserIVDecryption);

                    byte[] PassKeyDecryption = System.Convert.FromBase64String(GetDecryptionData(Account + "PassKey"));
                    byte[] PassIVDecryption = System.Convert.FromBase64String(GetDecryptionData(Account + "PassIV"));
                    byte[] Password = EncryptInputToBytes(PasswordTxtBox.Text, PassKeyDecryption, PassIVDecryption);


                    UserNameTxtBox.Clear();
                    PasswordTxtBox.Clear();
                    WebAddressTxtBox.Clear();
                    NotesTxtBox.Clear();

                    UpdateCredentials(conn, Account, UserName, Password, WebAddress, Notes);

                    string Message = "Credentials Updated!";
                    string Title = "Done";
                    MessageBoxButtons Buttons = MessageBoxButtons.OK;
                    MessageBox.Show(Message, Title, Buttons, MessageBoxIcon.Information);
                }
            }
        }

        //Calls a stored procedure to update the database with any changes the user made to their credentials
        static void UpdateCredentials(SqlConnection conn, string Account, byte[] UserName, byte[] Password, string WebAddress, string Notes)
        {
            SqlCommand UpdateCreds = new SqlCommand("UpdateCredentials", conn);
            UpdateCreds.CommandType = CommandType.StoredProcedure;
            UpdateCreds.Parameters.AddWithValue("Account", Account);
            UpdateCreds.Parameters.AddWithValue("UserName", UserName);
            UpdateCreds.Parameters.AddWithValue("Password", Password);
            UpdateCreds.Parameters.AddWithValue("WebAddress", WebAddress);
            UpdateCreds.Parameters.AddWithValue("Notes", Notes);

            UpdateCreds.ExecuteNonQuery();
        }
    }
}
