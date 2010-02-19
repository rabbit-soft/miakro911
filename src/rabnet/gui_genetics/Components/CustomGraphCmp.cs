using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace rabnet
{
	/// <summary>
	/// Template for custom graphical control
	/// </summary>
	public partial class CustomGraphCmp : UserControl
	{
		private Graphics Graph;
		const BufferedGraphics NO_MANAGED_BACK_BUFFER = null;
		private BufferedGraphicsContext GraphicManager;
		protected BufferedGraphics ManagedBackBuffer;

		public virtual void DrawingProc()
		{
		}

		public CustomGraphCmp()
		{
			InitializeComponent();

			Graph = CreateGraphics();
			Application.ApplicationExit += new EventHandler(MemoryCleanup);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			GraphicManager = BufferedGraphicsManager.Current;
			GraphicManager.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);
			ManagedBackBuffer = GraphicManager.Allocate(this.CreateGraphics(), ClientRectangle);
			InitBuffer();
		}

		private void InitBuffer()
		{
			SolidBrush BgBrush = new SolidBrush(this.BackColor);
			//SolidBrush BgBrush = new SolidBrush(Color.Pink);
			ManagedBackBuffer.Graphics.FillRectangle(BgBrush, new Rectangle(0, 0, this.Width, this.Height));
			DrawingProc();
		}

		private void MemoryCleanup(object sender, EventArgs e)
		{
			// clean up the memory

			if (ManagedBackBuffer != NO_MANAGED_BACK_BUFFER)
				ManagedBackBuffer.Dispose();
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			// Draw On ManagedBackBuffer.Graphics;
			//Debug.WriteLine(count++);
			//base.OnPaint(e);
			ManagedBackBuffer.Render(Graph);
		}

		private void CustomGraphCmp_Resize(object sender, EventArgs e)
		{
			if (ManagedBackBuffer != NO_MANAGED_BACK_BUFFER)
				ManagedBackBuffer.Dispose();


			GraphicManager.MaximumBuffer =
				  new Size(this.Width + 1, this.Height + 1);

			ManagedBackBuffer =
				GraphicManager.Allocate(this.CreateGraphics(),
												ClientRectangle);
			InitBuffer();
			//base.OnPaint(e);
			ManagedBackBuffer.Render(Graph);
		}

		public void RedrawMe()
		{
			InitBuffer();
			ManagedBackBuffer.Render(Graph);
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
		}

	}
}
