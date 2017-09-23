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
        private Bitmap bmpDest;

        private SortedList<int, Province> srcChroma;
        private SortedList<int, Province> destChroma;

        private float scaleFactor = 1.0f;

        private void Form1_Load(object sender, EventArgs e)
        {
            srcChroma = new SortedList<int, Province>();
            foreach (Province p in Program.sourceDef.provinces)
            {
                srcChroma.Add(p.rgb.ToArgb(), p);
            }

            destChroma = new SortedList<int, Province>();
            foreach (Province p in Program.targetDef.provinces)
            {
                destChroma.Add(p.rgb.ToArgb(), p);
            }

            // force rescale/redraw
            cbZoom.SelectedIndex = 0;
        }

        private Point srcPt;
        private void pbSource_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X != srcPt.X || e.Y != srcPt.Y)
            {
                srcPt.X = e.X;
                srcPt.Y = e.Y;
                Color c = bmpSrc.GetPixel(srcPt.X, srcPt.Y);
                Province p = null;
                if (srcChroma.TryGetValue(c.ToArgb(), out p))
                {
                    toolTip1.Show(p.ToString(), pbSource, new Point(srcPt.X, srcPt.Y - 20));
                    StatusLabel.Text = p.ToString();
                }
            }
        }

        private Point destPt;
        private void pbTarget_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X != destPt.X || e.Y != destPt.Y)
            {
                destPt.X = e.X;
                destPt.Y = e.Y;
                Color c = bmpDest.GetPixel(destPt.X, destPt.Y);
                Province p = null;
                if (destChroma.TryGetValue(c.ToArgb(), out p))
                {
                    toolTip1.Show(p.ToString(), pbTarget, new Point(destPt.X, destPt.Y - 20));
                    StatusLabel.Text = p.ToString();
                }
            }
        }

        private List<Province> oldSrcSelection = new List<Province>();
        private List<Province> srcSelection = new List<Province>();

        private List<Province> oldDestSelection = new List<Province>();
        private List<Province> destSelection = new List<Province>();

        private void pbSource_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.RemoveAll();
            StatusLabel.Text = String.Empty;
        }

        private void pbTarget_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.RemoveAll();
            StatusLabel.Text = String.Empty;
        }

        private void createSelPBs(bool force)
        {
            createSelPBs(force, srcSelection, oldSrcSelection, srcChroma.Values, pbSource);
            createSelPBs(force, destSelection, oldDestSelection, destChroma.Values, pbTarget);
        }

        private void createSelPBs(bool force, List<Province> newSelection, List<Province> oldSelection, IList<Province> provinces, PictureBox pb)
        {
            if (force || !newSelection.SequenceEqual(oldSelection))
            {
                Rectangle invalidRect = Rectangle.Empty;
                if (force)
                {
                    invalidRect = new Rectangle(0, 0, pb.Image.Width, pb.Image.Height);
                }
                else
                {
                    if (newSelection.Count > 0)
                        invalidRect = Program.ScaleRect(newSelection[0].Rect, scaleFactor);
                    else if (oldSelection.Count > 0)
                        invalidRect = Program.ScaleRect(oldSelection[0].Rect, scaleFactor);
                    foreach (Province p in newSelection)
                    {
                        invalidRect = Rectangle.Union(invalidRect, Program.ScaleRect(p.Rect, scaleFactor));
                    }
                    foreach (Province p in oldSelection)
                    {
                        invalidRect = Rectangle.Union(invalidRect, Program.ScaleRect(p.Rect, scaleFactor));
                    }
                }

                Graphics g = Graphics.FromImage(pb.Image);
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.FillRectangle(Brushes.Transparent, invalidRect);
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;

                // disable interpolation and smoothing to preserve chroma
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                foreach (Province p in newSelection)
                {
                    Rectangle scaledRect = Program.ScaleRect(p.Rect, scaleFactor);
                    if (Rectangle.Intersect(scaledRect, invalidRect) != Rectangle.Empty)
                        g.DrawImage(p.SelectionMask, scaledRect);
                }
                pb.Invalidate(invalidRect);

                oldSelection.Clear();
                oldSelection.AddRange(newSelection);
            }
        }

        private bool skipSelPBRedraw = false;
        private void lbMappings_SelectedIndexChanged(object sender, EventArgs e)
        {
            srcSelection.Clear();
            destSelection.Clear();

            if (!skipSelPBRedraw)
                createSelPBs(false);
        }

        private void tbFitSelection_Click(object sender, EventArgs e)
        {
            if (srcSelection.Count > 0)
            {
                Rectangle srcFit = Program.ScaleRect(srcSelection[0].Rect, scaleFactor);
                foreach (Province p in srcSelection)
                {
                    srcFit = Rectangle.Union(srcFit, Program.ScaleRect(p.Rect, scaleFactor));
                }
                Point fitCenter = new Point(srcFit.X + srcFit.Width / 2, srcFit.Y + srcFit.Height / 2);
                Point panelCenter = new Point(HorizontalSplit.Panel1.Width / 2, HorizontalSplit.Panel1.Height / 2);
                Point offset = new Point(fitCenter.X - panelCenter.X, fitCenter.Y - panelCenter.Y);
                HorizontalSplit.Panel1.AutoScrollPosition = offset;
            }

            if (destSelection.Count > 0)
            {
                Rectangle destFit = Program.ScaleRect(destSelection[0].Rect, scaleFactor);
                foreach (Province p in destSelection)
                {
                    destFit = Rectangle.Union(destFit, Program.ScaleRect(p.Rect, scaleFactor));
                }
                Point fitCenter = new Point(destFit.X + destFit.Width / 2, destFit.Y + destFit.Height / 2);
                Point panelCenter = new Point(HorizontalSplit.Panel1.Width / 2, HorizontalSplit.Panel1.Height / 2);
                Point offset = new Point(fitCenter.X - panelCenter.X, fitCenter.Y - panelCenter.Y);
                HorizontalSplit.Panel2.AutoScrollPosition = offset;
            }
        }

        private void tbUnmapped_Click(object sender, EventArgs e)
        {
            tbUnmapped.Checked = true;
            tbSelection.Checked = false;
            createSelPBs(true);
        }

        private void tbSelection_Click(object sender, EventArgs e)
        {
            tbSelection.Checked = true;
            tbUnmapped.Checked = false;
            createSelPBs(true);
        }

        private void cbZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbZoom.SelectedItem != null)
                scaleFactor = float.Parse(cbZoom.SelectedItem.ToString().TrimEnd('x'));

            if (pbSource.BackgroundImage != null)
                pbSource.BackgroundImage.Dispose();
            if (pbSource.Image != null)
                pbSource.Image.Dispose();

            pbSource.BackgroundImage = bmpSrc = Program.CleanResizeBitmap(Program.sourceMap.map,
                (int)(Program.sourceMap.map.Width * scaleFactor), (int)(Program.sourceMap.map.Height * scaleFactor));
            pbSource.Size = bmpSrc.Size;
            pbSource.Image = new Bitmap(bmpSrc.Width, bmpSrc.Height);
            Graphics g = Graphics.FromImage(pbSource.Image);
            g.FillRectangle(Brushes.Transparent, new Rectangle(new Point(0, 0), bmpSrc.Size));
            g.Flush();

            if (pbTarget.BackgroundImage != null)
                pbTarget.BackgroundImage.Dispose();
            if (pbTarget.Image != null)
                pbTarget.Image.Dispose();

            pbTarget.BackgroundImage = bmpDest = Program.CleanResizeBitmap(Program.targetMap.map,
                (int)(Program.targetMap.map.Width * scaleFactor), (int)(Program.targetMap.map.Height * scaleFactor));
            pbTarget.Size = bmpDest.Size;
            pbTarget.Image = new Bitmap(bmpDest.Width, bmpDest.Height);
            g = Graphics.FromImage(pbTarget.Image);
            g.FillRectangle(Brushes.Transparent, new Rectangle(new Point(0, 0), bmpDest.Size));
            g.Flush();

            createSelPBs(true);
        }
    }
}
