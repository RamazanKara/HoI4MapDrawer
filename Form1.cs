using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ProvinceMapper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Bitmap bmpSrc;

        private void Form1_Load(object sender, EventArgs e)
        {
            if (pbSource.BackgroundImage != null)
                pbSource.BackgroundImage.Dispose();
            if (pbSource.Image != null)
                pbSource.Image.Dispose();

            pbSource.BackgroundImage = bmpSrc = Program.CleanResizeBitmap(Program.sourceMap.map,
                (int)(Program.sourceMap.map.Width), (int)(Program.sourceMap.map.Height));
            pbSource.Size = bmpSrc.Size;
            pbSource.Image = new Bitmap(bmpSrc.Width, bmpSrc.Height);
            Graphics g = Graphics.FromImage(pbSource.Image);
            g.FillRectangle(Brushes.Transparent, new Rectangle(new Point(0, 0), bmpSrc.Size));
            g.Flush();
        }
    }
}
