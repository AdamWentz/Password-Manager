namespace PasswordManager
{
    partial class DeleteCredForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.CredentialDDL = new System.Windows.Forms.ComboBox();
            this.DoneButton = new System.Windows.Forms.Button();
            this.DeleteAcctButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(49, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(501, 37);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please select an account to delete";
            // 
            // CredentialDDL
            // 
            this.CredentialDDL.FormattingEnabled = true;
            this.CredentialDDL.Location = new System.Drawing.Point(146, 116);
            this.CredentialDDL.Name = "CredentialDDL";
            this.CredentialDDL.Size = new System.Drawing.Size(284, 21);
            this.CredentialDDL.TabIndex = 0;
            this.CredentialDDL.Text = "Select an account. . .";
            this.CredentialDDL.SelectedIndexChanged += new System.EventHandler(this.CredentialDDL_SelectedIndexChanged);
            // 
            // DoneButton
            // 
            this.DoneButton.Location = new System.Drawing.Point(311, 187);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(119, 35);
            this.DoneButton.TabIndex = 2;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // DeleteAcctButton
            // 
            this.DeleteAcctButton.Location = new System.Drawing.Point(146, 187);
            this.DeleteAcctButton.Name = "DeleteAcctButton";
            this.DeleteAcctButton.Size = new System.Drawing.Size(119, 35);
            this.DeleteAcctButton.TabIndex = 1;
            this.DeleteAcctButton.Text = "Delete Account";
            this.DeleteAcctButton.UseVisualStyleBackColor = true;
            this.DeleteAcctButton.Click += new System.EventHandler(this.DeleteAcctButton_Click);
            // 
            // DeleteCredForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 269);
            this.Controls.Add(this.DeleteAcctButton);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.CredentialDDL);
            this.Controls.Add(this.label1);
            this.Name = "DeleteCredForm";
            this.Text = "Delete Account";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CredentialDDL;
        private System.Windows.Forms.Button DoneButton;
        private System.Windows.Forms.Button DeleteAcctButton;
    }
}