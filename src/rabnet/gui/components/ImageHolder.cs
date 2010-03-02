using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace rabnet
{
    public partial class ImageHolder : UserControl
    {
		Bitmap _ArrowDown;
		Bitmap _ArrowUp;

        public ImageHolder()
        {
            InitializeComponent();
			DrawArrows();
        }

		private void DrawArrows()
		{
			Graphics g;
			_ArrowDown = new Bitmap(11, 11);
			g = Graphics.FromImage(_ArrowDown);
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawPolygon(SystemPens.ControlDark, new Point[] { new Point(9, 3), new Point(5, 7), new Point(1, 3) });
			g.FillPolygon(SystemBrushes.ControlDark, new Point[] { new Point(9, 3), new Point(5, 7), new Point(1, 3) });
			g.Dispose();

			_ArrowUp = new Bitmap(11, 11);
			g = Graphics.FromImage(_ArrowUp);
			g.SmoothingMode = SmoothingMode.HighQuality;
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;
			g.DrawPolygon(SystemPens.ControlDark, new Point[] { new Point(5, 3), new Point(9, 7), new Point(1, 7) });
			g.FillPolygon(SystemBrushes.ControlDark, new Point[] { new Point(5, 3), new Point(9, 7), new Point(1, 7) });
			g.Dispose();
		}

        private static ImageHolder obj=null;
        public static ImageHolder get()
        {
            if (obj == null)
                obj = new ImageHolder();
            return obj;
        }

        public Image getImage(int index)
        {


			if (index == 0)
			{
				return _ArrowDown;
			}

			if (index == 1)
			{
				return _ArrowUp;
			}

            return imageList1.Images[index];
        }
    }
}
