namespace PasswordManager
{
    partial class MainForm
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
            this.AddCredButton = new System.Windows.Forms.Button();
            this.ViewCredButton = new System.Windows.Forms.Button();
            this.DeleteCredButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // AddCredButton
            // 
            this.AddCredButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddCredButton.Location = new System.Drawing.Point(220, 170);
            this.AddCredButton.Name = "AddCredButton";
            this.AddCredButton.Size = new System.Drawing.Size(282, 51);
            this.AddCredButton.TabIndex = 0;
            this.AddCredButton.Text = "Add Account";
            this.AddCredButton.UseVisualStyleBackColor = true;
            this.AddCredButton.Click += new System.EventHandler(this.Button1_Click);
            // 
            // ViewCredButton
            // 
            this.ViewCredButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewCredButton.Location = new System.Drawing.Point(220, 245);
            this.ViewCredButton.Name = "ViewCredButton";
            this.ViewCredButton.Size = new System.Drawing.Size(282, 51);
            this.ViewCredButton.TabIndex = 1;
            this.ViewCredButton.Text = "View/Update Account";
            this.ViewCredButton.UseVisualStyleBackColor = true;
            this.ViewCredButton.Click += new System.EventHandler(this.Button2_Click);
            // 
            // DeleteCredButton
            // 
            this.DeleteCredButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteCredButton.Location = new System.Drawing.Point(220, 323);
            this.DeleteCredButton.Name = "DeleteCredButton";
            this.DeleteCredButton.Size = new System.Drawing.Size(282, 51);
            this.DeleteCredButton.TabIndex = 2;
            this.DeleteCredButton.Text = "Delete Account";
            this.DeleteCredButton.UseVisualStyleBackColor = true;
            this.DeleteCredButton.Click += new System.EventHandler(this.ChangeCredButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(166, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 37);
            this.label1.TabIndex = 5;
            this.label1.Text = "What would you like to do?";
            // 
            // MainForm
            // 
            this.AccessibleName = "MainForm";
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(712, 441);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DeleteCredButton);
            this.Controls.Add(this.ViewCredButton);
            this.Controls.Add(this.AddCredButton);
            this.Name = "MainForm";
            this.Text = "Password Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddCredButton;
        private System.Windows.Forms.Button ViewCredButton;
        private System.Windows.Forms.Button DeleteCredButton;
        private System.Windows.Forms.Label label1;
    }
}

