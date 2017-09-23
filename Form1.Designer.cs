namespace ProvinceMapper
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
            this.pbSource = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbSource)).BeginInit();
            this.SuspendLayout();
            // 
            // pbSource
            // 
            this.pbSource.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbSource.Location = new System.Drawing.Point(0, 0);
            this.pbSource.Name = "pbSource";
            this.pbSource.Size = new System.Drawing.Size(100, 50);
            this.pbSource.TabIndex = 0;
            this.pbSource.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 467);
            this.AutoScroll = true;
            this.Controls.Add(this.pbSource);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Province Mapper";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbSource;
    }
}

