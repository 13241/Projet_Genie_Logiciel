namespace Kitbox
{
    partial class Viewer2
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
            this.Y = new System.Windows.Forms.Label();
            this.X = new System.Windows.Forms.Label();
            this.DimensionTester = new System.Windows.Forms.SplitContainer();
            this.bottom = new System.Windows.Forms.Panel();
            this.rear = new System.Windows.Forms.Panel();
            this.right = new System.Windows.Forms.Panel();
            this.top = new System.Windows.Forms.Panel();
            this.left = new System.Windows.Forms.Panel();
            this.front = new System.Windows.Forms.Panel();
            this.selected = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.DimensionTester)).BeginInit();
            this.DimensionTester.SuspendLayout();
            this.SuspendLayout();
            // 
            // Y
            // 
            this.Y.AutoSize = true;
            this.Y.Location = new System.Drawing.Point(63, 59);
            this.Y.Name = "Y";
            this.Y.Size = new System.Drawing.Size(35, 13);
            this.Y.TabIndex = 16;
            this.Y.Text = "label2";
            // 
            // X
            // 
            this.X.AutoSize = true;
            this.X.Location = new System.Drawing.Point(22, 59);
            this.X.Name = "X";
            this.X.Size = new System.Drawing.Size(35, 13);
            this.X.TabIndex = 15;
            this.X.Text = "label1";
            // 
            // DimensionTester
            // 
            this.DimensionTester.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DimensionTester.Location = new System.Drawing.Point(424, 12);
            this.DimensionTester.Name = "DimensionTester";
            this.DimensionTester.Size = new System.Drawing.Size(370, 200);
            this.DimensionTester.SplitterDistance = 198;
            this.DimensionTester.TabIndex = 14;
            this.DimensionTester.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.ChangeScaling);
            // 
            // bottom
            // 
            this.bottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bottom.Location = new System.Drawing.Point(218, 424);
            this.bottom.Name = "bottom";
            this.bottom.Size = new System.Drawing.Size(200, 200);
            this.bottom.TabIndex = 12;
            // 
            // rear
            // 
            this.rear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rear.Location = new System.Drawing.Point(630, 218);
            this.rear.Name = "rear";
            this.rear.Size = new System.Drawing.Size(200, 200);
            this.rear.TabIndex = 8;
            // 
            // right
            // 
            this.right.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.right.Location = new System.Drawing.Point(424, 218);
            this.right.Name = "right";
            this.right.Size = new System.Drawing.Size(200, 200);
            this.right.TabIndex = 9;
            // 
            // top
            // 
            this.top.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.top.Location = new System.Drawing.Point(218, 12);
            this.top.Name = "top";
            this.top.Size = new System.Drawing.Size(200, 200);
            this.top.TabIndex = 10;
            // 
            // left
            // 
            this.left.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.left.Location = new System.Drawing.Point(12, 218);
            this.left.Name = "left";
            this.left.Size = new System.Drawing.Size(200, 200);
            this.left.TabIndex = 11;
            // 
            // front
            // 
            this.front.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.front.Location = new System.Drawing.Point(218, 218);
            this.front.Name = "front";
            this.front.Size = new System.Drawing.Size(200, 200);
            this.front.TabIndex = 7;
            // 
            // selected
            // 
            this.selected.AutoSize = true;
            this.selected.Location = new System.Drawing.Point(22, 89);
            this.selected.Name = "selected";
            this.selected.Size = new System.Drawing.Size(35, 13);
            this.selected.TabIndex = 17;
            this.selected.Text = "label1";
            // 
            // Viewer2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 636);
            this.Controls.Add(this.selected);
            this.Controls.Add(this.Y);
            this.Controls.Add(this.X);
            this.Controls.Add(this.DimensionTester);
            this.Controls.Add(this.bottom);
            this.Controls.Add(this.rear);
            this.Controls.Add(this.right);
            this.Controls.Add(this.top);
            this.Controls.Add(this.left);
            this.Controls.Add(this.front);
            this.Name = "Viewer2";
            this.Text = "Viewer2";
            this.Click += new System.EventHandler(this.Viewer2_Click);
            ((System.ComponentModel.ISupportInitialize)(this.DimensionTester)).EndInit();
            this.DimensionTester.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Y;
        private System.Windows.Forms.Label X;
        private System.Windows.Forms.SplitContainer DimensionTester;
        private System.Windows.Forms.Panel bottom;
        private System.Windows.Forms.Panel rear;
        private System.Windows.Forms.Panel right;
        private System.Windows.Forms.Panel top;
        private System.Windows.Forms.Panel left;
        private System.Windows.Forms.Panel front;
        private System.Windows.Forms.Label selected;
    }
}