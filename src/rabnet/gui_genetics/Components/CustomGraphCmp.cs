using System;
using System.Drawing;
using System.Windows.Forms;

namespace rabnet
{
	/// <summary>
	/// Template for custom graphical control
	/// </summary>
	public partial class CustomGraphCmp : UserControl
	{
		private Graphics _graph;

		public virtual void DrawingProc(Graphics g)
		{
		}

		public CustomGraphCmp()
		{
			InitializeComponent();

			this.SetStyle(
			 ControlStyles.UserPaint |
			 ControlStyles.AllPaintingInWmPaint |
			 ControlStyles.OptimizedDoubleBuffer, true);

			_graph = CreateGraphics();
			InitBuffer();
		}

		private void InitBuffer()
		{
			SolidBrush bgBrush = new SolidBrush(this.BackColor);
			//SolidBrush BgBrush = new SolidBrush(Color.Pink);
			_graph.FillRectangle(bgBrush, new Rectangle(0, 0, this.Width, this.Height));
			DrawingProc(_graph);
		}


		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			// Draw On ManagedBackBuffer.Graphics;
			//Debug.WriteLine(count++);
			//base.OnPaint(e);
//			InitBuffer();
			SolidBrush bgBrush = new SolidBrush(this.BackColor);
			e.Graphics.FillRectangle(bgBrush, new Rectangle(0, 0, this.Width, this.Height));
			DrawingProc(e.Graphics);
		}

		public virtual void RedrawMe()
		{
			InitBuffer();
		}

		protected override void OnSizeChanged(EventArgs e)
		{
			int w = this.Width;
			int h = this.Height;
			if (w < 0)
			{
				w = 0;
			}
			if (h < 0)
			{
				h = 0;
			}
			this.Size = new Size(w, h);
			base.OnSizeChanged(e);
			if (_graph != null)
			{
				_graph.Dispose();
				_graph = CreateGraphics();
			}
		}

	}
}
