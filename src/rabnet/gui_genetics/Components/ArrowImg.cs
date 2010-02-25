using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace rabnet
{
	public partial class ArrowImg : CustomGraphCmp
	{

		private Boolean _left = false;
		private Boolean _right = false;
		private int _top = 0;
		private float _arrowh = 10;
		private float _arroww = 8;
		private Boolean _start = false;
		private Boolean _end = true;
		private Graphics gr;

		public ArrowImg()
		{
			InitializeComponent();
			//_right = true;
		}

		public void SetBothEnds()
		{
			_start = true;
			_end = true;
			RedrawMe();
		}

		public void SetStart()
		{
			_start = true;
			_end = false;
			RedrawMe();
		}

		public void SetEnd()
		{
			_start = false;
			_end = true;
			RedrawMe();
		}

		public void SetLeft()
		{
			_left = true;
			_right = false;
			RedrawMe();
		}

		public void SetRight()
		{
			_left = false;
			_right = true;
			RedrawMe();
		}

		public int WidthA
		{
			get { return (int)(this.Width - _arroww); }
			set { this.Width = (int)(value + _arroww); }
		}

		public int LeftA
		{
			get { return (int)(this.Left + _arroww/2); }
			set { this.Left = (int)(value - _arroww/2); }
		}

		public int TopA
		{
			get { return (int)(this.Top + _arroww/2); }
			set { this.Top = (int)(value - _arroww/2); }
		}

		public int HeightA
		{
			get { return (int)(this.Height - _arroww); }
			set { this.Height = (int)(value + _arroww); }
		}

		public override void DrawingProc(Graphics g)
		{
			Pen pen = new Pen(Color.Black);
			Pen penGr = new Pen(Color.DarkGray);

			SolidBrush brush = new SolidBrush(Color.Black);



			if (_left)
			{
				g.DrawLine(pen, this.Width - 1, _top + _arroww / 2, _arroww / 2, _top + _arroww / 2);

				g.DrawLine(pen, _arroww / 2, _top + _arroww / 2, _arroww / 2, this.Height - 1);

				if (_end)
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.FillPolygon(brush, new PointF[] {	new PointF(_arroww / 2, this.Height - 1), 
																					new PointF(0, this.Height - 1 - _arrowh), 
																					new PointF(_arroww, this.Height - 1 - _arrowh) });
					g.SmoothingMode = SmoothingMode.None;
				}

				if (_start)
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.FillPolygon(brush, new PointF[] {	new PointF(this.Width, _top + _arroww / 2),
																					new PointF(this.Width - _arrowh, _top), 
																					new PointF(this.Width - _arrowh, _top + _arroww)});
					g.SmoothingMode = SmoothingMode.None;
				}

			}
			if (_right)
			{
				g.DrawLine(pen, this.Width - 1 - _arroww / 2, _top + _arroww / 2, 0, _top + _arroww / 2);

				g.DrawLine(pen, this.Width - 1 - _arroww / 2, _top + _arroww / 2, this.Width - 1 - _arroww / 2, this.Height - 1);

				if (_end)
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.FillPolygon(brush, new PointF[] {	new PointF(this.Width - 1 - _arroww / 2, this.Height - 1), 
																					new PointF(this.Width - 1 - _arroww, this.Height - 1 - _arrowh), 
																					new PointF(this.Width - 1, this.Height - 1 - _arrowh) });
					g.SmoothingMode = SmoothingMode.None;
				}

				if (_start)
				{
					g.SmoothingMode = SmoothingMode.AntiAlias;
					g.FillPolygon(brush, new PointF[] {	new PointF(0, _top + _arroww / 2),
																					new PointF(_arrowh, _top), 
																					new PointF(_arrowh, _top + _arroww)});
					g.SmoothingMode = SmoothingMode.None;
				}

			}


			//			g.SmoothingMode = SmoothingMode.AntiAlias;


			//			g.SmoothingMode = SmoothingMode.None;
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			int w = this.Width;
			int h = this.Height;
			if (w < _arroww)
			{
				w = (int)_arroww;
			}
			if (h < _arrowh)
			{
				h = (int)_arrowh;
			}
			this.Size = new Size(w, h);
			base.OnSizeChanged(e);
		}

	}
}
