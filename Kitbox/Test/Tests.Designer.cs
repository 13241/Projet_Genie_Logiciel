﻿namespace Kitbox
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
            this.LEFT = new System.Windows.Forms.Button();
            this.UP = new System.Windows.Forms.Button();
            this.DOWN = new System.Windows.Forms.Button();
            this.RIGHT = new System.Windows.Forms.Button();
            this.ZOOM = new System.Windows.Forms.Button();
            this.UNZOOM = new System.Windows.Forms.Button();
            this.AddBox = new System.Windows.Forms.Button();
            this.ChangeSurface = new System.Windows.Forms.Button();
            this.RemoveBox = new System.Windows.Forms.Button();
            this.ResizeBox = new System.Windows.Forms.Button();
            this.ChangeColor = new System.Windows.Forms.Button();
            this.Color_input = new System.Windows.Forms.TextBox();
            this.Pointer = new System.Windows.Forms.Label();
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
            this.screen.GotFocus += new System.EventHandler(this.OnFocus);
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
            // LEFT
            // 
            this.LEFT.Location = new System.Drawing.Point(347, 135);
            this.LEFT.Name = "LEFT";
            this.LEFT.Size = new System.Drawing.Size(27, 23);
            this.LEFT.TabIndex = 3;
            this.LEFT.Text = "←";
            this.LEFT.UseVisualStyleBackColor = true;
            this.LEFT.Click += new System.EventHandler(this.LEFT_Click);
            // 
            // UP
            // 
            this.UP.Location = new System.Drawing.Point(380, 121);
            this.UP.Name = "UP";
            this.UP.Size = new System.Drawing.Size(27, 23);
            this.UP.TabIndex = 4;
            this.UP.Text = "↑";
            this.UP.UseVisualStyleBackColor = true;
            this.UP.Click += new System.EventHandler(this.UP_Click);
            // 
            // DOWN
            // 
            this.DOWN.Location = new System.Drawing.Point(380, 150);
            this.DOWN.Name = "DOWN";
            this.DOWN.Size = new System.Drawing.Size(27, 23);
            this.DOWN.TabIndex = 5;
            this.DOWN.Text = "↓";
            this.DOWN.UseVisualStyleBackColor = true;
            this.DOWN.Click += new System.EventHandler(this.DOWN_Click);
            // 
            // RIGHT
            // 
            this.RIGHT.Location = new System.Drawing.Point(413, 135);
            this.RIGHT.Name = "RIGHT";
            this.RIGHT.Size = new System.Drawing.Size(27, 23);
            this.RIGHT.TabIndex = 6;
            this.RIGHT.Text = "→";
            this.RIGHT.UseVisualStyleBackColor = true;
            this.RIGHT.Click += new System.EventHandler(this.RIGHT_Click);
            // 
            // ZOOM
            // 
            this.ZOOM.Location = new System.Drawing.Point(380, 179);
            this.ZOOM.Name = "ZOOM";
            this.ZOOM.Size = new System.Drawing.Size(27, 23);
            this.ZOOM.TabIndex = 7;
            this.ZOOM.Text = "+";
            this.ZOOM.UseVisualStyleBackColor = true;
            this.ZOOM.Click += new System.EventHandler(this.ZOOM_Click);
            // 
            // UNZOOM
            // 
            this.UNZOOM.Location = new System.Drawing.Point(380, 208);
            this.UNZOOM.Name = "UNZOOM";
            this.UNZOOM.Size = new System.Drawing.Size(27, 23);
            this.UNZOOM.TabIndex = 8;
            this.UNZOOM.Text = "-";
            this.UNZOOM.UseVisualStyleBackColor = true;
            this.UNZOOM.Click += new System.EventHandler(this.UNZOOM_Click);
            // 
            // AddBox
            // 
            this.AddBox.Location = new System.Drawing.Point(12, 292);
            this.AddBox.Name = "AddBox";
            this.AddBox.Size = new System.Drawing.Size(75, 23);
            this.AddBox.TabIndex = 9;
            this.AddBox.Text = "AddBox";
            this.AddBox.UseVisualStyleBackColor = true;
            this.AddBox.Visible = false;
            this.AddBox.Click += new System.EventHandler(this.AddBox_Click);
            // 
            // ChangeSurface
            // 
            this.ChangeSurface.Location = new System.Drawing.Point(93, 292);
            this.ChangeSurface.Name = "ChangeSurface";
            this.ChangeSurface.Size = new System.Drawing.Size(75, 23);
            this.ChangeSurface.TabIndex = 10;
            this.ChangeSurface.Text = "ChangeSurface";
            this.ChangeSurface.UseVisualStyleBackColor = true;
            this.ChangeSurface.Visible = false;
            this.ChangeSurface.Click += new System.EventHandler(this.ChangeSurface_Click);
            // 
            // RemoveBox
            // 
            this.RemoveBox.Location = new System.Drawing.Point(12, 321);
            this.RemoveBox.Name = "RemoveBox";
            this.RemoveBox.Size = new System.Drawing.Size(75, 23);
            this.RemoveBox.TabIndex = 11;
            this.RemoveBox.Text = "RemoveBox";
            this.RemoveBox.UseVisualStyleBackColor = true;
            this.RemoveBox.Visible = false;
            this.RemoveBox.Click += new System.EventHandler(this.RemoveBox_Click);
            // 
            // ResizeBox
            // 
            this.ResizeBox.Location = new System.Drawing.Point(93, 321);
            this.ResizeBox.Name = "ResizeBox";
            this.ResizeBox.Size = new System.Drawing.Size(75, 23);
            this.ResizeBox.TabIndex = 12;
            this.ResizeBox.Text = "ResizeBox";
            this.ResizeBox.UseVisualStyleBackColor = true;
            this.ResizeBox.Visible = false;
            this.ResizeBox.Click += new System.EventHandler(this.ResizeBox_Click);
            // 
            // ChangeColor
            // 
            this.ChangeColor.Location = new System.Drawing.Point(268, 36);
            this.ChangeColor.Name = "ChangeColor";
            this.ChangeColor.Size = new System.Drawing.Size(75, 23);
            this.ChangeColor.TabIndex = 13;
            this.ChangeColor.Text = "ChangeColor";
            this.ChangeColor.UseVisualStyleBackColor = true;
            this.ChangeColor.Visible = false;
            this.ChangeColor.Click += new System.EventHandler(this.ChangeColor_Click);
            // 
            // Color_input
            // 
            this.Color_input.Location = new System.Drawing.Point(268, 10);
            this.Color_input.Name = "Color_input";
            this.Color_input.Size = new System.Drawing.Size(196, 20);
            this.Color_input.TabIndex = 14;
            this.Color_input.Visible = false;
            // 
            // Pointer
            // 
            this.Pointer.AutoSize = true;
            this.Pointer.Location = new System.Drawing.Point(174, 292);
            this.Pointer.Name = "Pointer";
            this.Pointer.Size = new System.Drawing.Size(40, 13);
            this.Pointer.TabIndex = 15;
            this.Pointer.Text = "Pointer";
            // 
            // Tests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 358);
            this.Controls.Add(this.Pointer);
            this.Controls.Add(this.Color_input);
            this.Controls.Add(this.ChangeColor);
            this.Controls.Add(this.ResizeBox);
            this.Controls.Add(this.RemoveBox);
            this.Controls.Add(this.ChangeSurface);
            this.Controls.Add(this.AddBox);
            this.Controls.Add(this.UNZOOM);
            this.Controls.Add(this.ZOOM);
            this.Controls.Add(this.RIGHT);
            this.Controls.Add(this.DOWN);
            this.Controls.Add(this.UP);
            this.Controls.Add(this.LEFT);
            this.Controls.Add(this.compute);
            this.Controls.Add(this.test_input);
            this.Controls.Add(this.screen);
            this.Name = "Tests";
            this.Text = "Tests";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UnBook);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel screen;
        private System.Windows.Forms.TextBox test_input;
        private System.Windows.Forms.Button compute;
        private System.Windows.Forms.Button LEFT;
        private System.Windows.Forms.Button UP;
        private System.Windows.Forms.Button DOWN;
        private System.Windows.Forms.Button RIGHT;
        private System.Windows.Forms.Button ZOOM;
        private System.Windows.Forms.Button UNZOOM;
        private System.Windows.Forms.Button AddBox;
        private System.Windows.Forms.Button ChangeSurface;
        private System.Windows.Forms.Button RemoveBox;
        private System.Windows.Forms.Button ResizeBox;
        private System.Windows.Forms.Button ChangeColor;
        private System.Windows.Forms.TextBox Color_input;
        private System.Windows.Forms.Label Pointer;
    }
}