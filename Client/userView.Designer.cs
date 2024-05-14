using System.ComponentModel;

namespace Client
{
    partial class userView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.label2 = new System.Windows.Forms.Label();
            this.listCompetitions = new System.Windows.Forms.ListBox();
            this.listParticipants = new System.Windows.Forms.ListBox();
            this.logoutButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nameField = new System.Windows.Forms.TextBox();
            this.birthDateField = new System.Windows.Forms.DateTimePicker();
            this.registerButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.probaField = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(154, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Competitions";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(507, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Participants";
            // 
            // listCompetitions
            // 
            this.listCompetitions.FormattingEnabled = true;
            this.listCompetitions.ItemHeight = 16;
            this.listCompetitions.Location = new System.Drawing.Point(44, 106);
            this.listCompetitions.Name = "listCompetitions";
            this.listCompetitions.Size = new System.Drawing.Size(324, 196);
            this.listCompetitions.TabIndex = 2;
            // 
            // listParticipants
            // 
            this.listParticipants.FormattingEnabled = true;
            this.listParticipants.ItemHeight = 16;
            this.listParticipants.Location = new System.Drawing.Point(408, 106);
            this.listParticipants.Name = "listParticipants";
            this.listParticipants.Size = new System.Drawing.Size(327, 196);
            this.listParticipants.TabIndex = 3;
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(717, 498);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(104, 29);
            this.logoutButton.TabIndex = 4;
            this.logoutButton.Text = "Log out";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click_1);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(309, 332);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(183, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Registration Form";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(118, 386);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 39);
            this.label4.TabIndex = 6;
            this.label4.Text = "Name: ";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(108, 459);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(115, 34);
            this.label5.TabIndex = 7;
            this.label5.Text = "BirthDate:";
            // 
            // nameField
            // 
            this.nameField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameField.Location = new System.Drawing.Point(262, 390);
            this.nameField.Name = "nameField";
            this.nameField.Size = new System.Drawing.Size(196, 30);
            this.nameField.TabIndex = 8;
            // 
            // birthDateField
            // 
            this.birthDateField.Location = new System.Drawing.Point(262, 461);
            this.birthDateField.Name = "birthDateField";
            this.birthDateField.Size = new System.Drawing.Size(196, 22);
            this.birthDateField.TabIndex = 9;
            // 
            // registerButton
            // 
            this.registerButton.Location = new System.Drawing.Point(550, 420);
            this.registerButton.Name = "registerButton";
            this.registerButton.Size = new System.Drawing.Size(93, 39);
            this.registerButton.TabIndex = 10;
            this.registerButton.Text = "Register";
            this.registerButton.UseVisualStyleBackColor = true;
            this.registerButton.Click += new System.EventHandler(this.registerButton_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(118, 515);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 34);
            this.label6.TabIndex = 11;
            this.label6.Text = "proba\r\n:";
            // 
            // probaField
            // 
            this.probaField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.probaField.Location = new System.Drawing.Point(262, 519);
            this.probaField.Name = "probaField";
            this.probaField.Size = new System.Drawing.Size(196, 30);
            this.probaField.TabIndex = 12;
            // 
            // userView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 574);
            this.Controls.Add(this.probaField);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.registerButton);
            this.Controls.Add(this.birthDateField);
            this.Controls.Add(this.nameField);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.listParticipants);
            this.Controls.Add(this.listCompetitions);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "userView";
            this.Text = "userView";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox probaField;

        private System.Windows.Forms.Label label6;

        private System.Windows.Forms.Button registerButton;

        private System.Windows.Forms.TextBox nameField;
        private System.Windows.Forms.DateTimePicker birthDateField;

        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Label label4;

        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.Button logoutButton;

        private System.Windows.Forms.ListBox listParticipants;

        private System.Windows.Forms.ListBox listCompetitions;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Label label1;

        #endregion
    }
}