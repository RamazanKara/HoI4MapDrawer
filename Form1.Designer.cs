﻿namespace ProvinceMapper
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.VerticalSplit = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.pbSource = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.VerticalSplit)).BeginInit();
            this.VerticalSplit.Panel1.SuspendLayout();
            this.VerticalSplit.Panel2.SuspendLayout();
            this.VerticalSplit.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSource)).BeginInit();
            this.SuspendLayout();
            // 
            // VerticalSplit
            // 
            this.VerticalSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VerticalSplit.Location = new System.Drawing.Point(0, 0);
            this.VerticalSplit.Name = "VerticalSplit";
            // 
            // VerticalSplit.Panel1
            // 
            this.VerticalSplit.Panel1.Controls.Add(this.statusStrip1);
            // 
            // VerticalSplit.Panel2
            // 
            this.VerticalSplit.Panel2.AutoScroll = true;
            this.VerticalSplit.Panel2.Controls.Add(this.pbSource);
            this.VerticalSplit.Size = new System.Drawing.Size(705, 467);
            this.VerticalSplit.SplitterDistance = 235;
            this.VerticalSplit.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 445);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(235, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(118, 17);
            this.StatusLabel.Text = "toolStripStatusLabel1";
            // 
            // pbSource
            // 
            this.pbSource.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbSource.Location = new System.Drawing.Point(0, 0);
            this.pbSource.Name = "pbSource";
            this.pbSource.Size = new System.Drawing.Size(100, 50);
            this.pbSource.TabIndex = 0;
            this.pbSource.TabStop = false;
            this.pbSource.MouseLeave += new System.EventHandler(this.pbSource_MouseLeave);
            this.pbSource.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbSource_MouseMove);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 30000;
            this.toolTip1.InitialDelay = 1;
            this.toolTip1.ReshowDelay = 1;
            this.toolTip1.ShowAlways = true;
            this.toolTip1.UseAnimation = false;
            this.toolTip1.UseFading = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 467);
            this.Controls.Add(this.VerticalSplit);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Province Mapper";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.VerticalSplit.Panel1.ResumeLayout(false);
            this.VerticalSplit.Panel1.PerformLayout();
            this.VerticalSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VerticalSplit)).EndInit();
            this.VerticalSplit.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer VerticalSplit;
        private System.Windows.Forms.PictureBox pbSource;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
    }
}

