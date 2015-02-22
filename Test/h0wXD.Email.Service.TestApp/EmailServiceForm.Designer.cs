namespace h0wXD.Email.Service.TestApp
{
    partial class EmailServiceForm
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
            this.ControlButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ControlButton
            // 
            this.ControlButton.Location = new System.Drawing.Point(12, 12);
            this.ControlButton.Name = "ControlButton";
            this.ControlButton.Size = new System.Drawing.Size(230, 46);
            this.ControlButton.TabIndex = 0;
            this.ControlButton.Text = "Pause";
            this.ControlButton.UseVisualStyleBackColor = true;
            this.ControlButton.Click += new System.EventHandler(this.ControlButton_Click);
            // 
            // EmailServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 70);
            this.Controls.Add(this.ControlButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EmailServiceForm";
            this.Text = "Email Service Test App";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ControlButton;
    }
}

