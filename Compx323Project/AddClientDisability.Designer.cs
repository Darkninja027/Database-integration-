namespace Compx323Project
{
    partial class AddClientDisability
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
            this.clientDropDown = new System.Windows.Forms.ComboBox();
            this.disabilityDropDown = new System.Windows.Forms.ComboBox();
            this.labelNewClientTitle = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // clientDropDown
            // 
            this.clientDropDown.FormattingEnabled = true;
            this.clientDropDown.Location = new System.Drawing.Point(21, 70);
            this.clientDropDown.Name = "clientDropDown";
            this.clientDropDown.Size = new System.Drawing.Size(121, 21);
            this.clientDropDown.TabIndex = 0;
            // 
            // disabilityDropDown
            // 
            this.disabilityDropDown.FormattingEnabled = true;
            this.disabilityDropDown.Location = new System.Drawing.Point(21, 124);
            this.disabilityDropDown.Name = "disabilityDropDown";
            this.disabilityDropDown.Size = new System.Drawing.Size(121, 21);
            this.disabilityDropDown.TabIndex = 1;
            // 
            // labelNewClientTitle
            // 
            this.labelNewClientTitle.AutoSize = true;
            this.labelNewClientTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNewClientTitle.Location = new System.Drawing.Point(18, 18);
            this.labelNewClientTitle.Name = "labelNewClientTitle";
            this.labelNewClientTitle.Size = new System.Drawing.Size(254, 16);
            this.labelNewClientTitle.TabIndex = 2;
            this.labelNewClientTitle.Text = "Please fill out the following sections";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(169, 226);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(94, 23);
            this.btnSubmit.TabIndex = 4;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(21, 226);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(97, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "SELECT CLIENT";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "SELECT DISABILITY";
            // 
            // AddClientDisability
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labelNewClientTitle);
            this.Controls.Add(this.disabilityDropDown);
            this.Controls.Add(this.clientDropDown);
            this.Name = "AddClientDisability";
            this.Text = "New Client Disability";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox clientDropDown;
        private System.Windows.Forms.ComboBox disabilityDropDown;
        private System.Windows.Forms.Label labelNewClientTitle;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}