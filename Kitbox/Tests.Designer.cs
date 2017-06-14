namespace Kitbox
{
    partial class Tests
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
            this.screen = new System.Windows.Forms.Panel();
            this.test_input = new System.Windows.Forms.TextBox();
            this.compute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // screen
            // 
            this.screen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.screen.Location = new System.Drawing.Point(12, 36);
            this.screen.Name = "screen";
            this.screen.Size = new System.Drawing.Size(250, 250);
            this.screen.TabIndex = 0;
            this.screen.Visible = false;
            // 
            // test_input
            // 
            this.test_input.Location = new System.Drawing.Point(12, 10);
            this.test_input.Name = "test_input";
            this.test_input.Size = new System.Drawing.Size(196, 20);
            this.test_input.TabIndex = 1;
            // 
            // compute
            // 
            this.compute.Location = new System.Drawing.Point(214, 10);
            this.compute.Name = "compute";
            this.compute.Size = new System.Drawing.Size(48, 21);
            this.compute.TabIndex = 2;
            this.compute.Text = "test";
            this.compute.UseVisualStyleBackColor = true;
            this.compute.Click += new System.EventHandler(this.compute_Click);
            // 
            // Tests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 358);
            this.Controls.Add(this.compute);
            this.Controls.Add(this.test_input);
            this.Controls.Add(this.screen);
            this.Name = "Tests";
            this.Text = "Tests";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel screen;
        private System.Windows.Forms.TextBox test_input;
        private System.Windows.Forms.Button compute;
    }
}