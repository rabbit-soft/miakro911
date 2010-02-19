using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
	public partial class RabbitPair : CustomGraphCmp
	{
		
		public double FemalePlodK
		{
			get { return FemaleRabbit.PlodK; }
			set { FemaleRabbit.PlodK = value; }
		}

		public double MalePlodK
		{
			get { return MaleRabbit.PlodK; }
			set { MaleRabbit.PlodK = value; }
		}

		public double FemaleRodK
		{
			get { return FemaleRabbit.RodK; }
			set { FemaleRabbit.RodK = value; }
		}

		public double MaleRodK
		{
			get { return MaleRabbit.RodK; }
			set { MaleRabbit.RodK = value; }
		}

		private Boolean _OrderedGenom = false;
		public Boolean OrderedGenom
		{
			get { return _OrderedGenom; }
			set
			{
				_OrderedGenom = value;
				FemaleRabbit.OrderedGenom = value;
				MaleRabbit.OrderedGenom = value;
				if (_TreeChildFPair != null)
				{
					_TreeChildFPair.OrderedGenom = value;
				}

				if (_TreeChildMPair != null)
				{
					_TreeChildMPair.OrderedGenom = value;
				}
			}
		}

		public string MaleGenom
		{
			get { return MaleRabbit.Genom; }
			set { MaleRabbit.Genom = value; }
		}
		public string FemaleGenom
		{
			get { return FemaleRabbit.Genom; }
			set { FemaleRabbit.Genom = value; }
		}

		public string Genom
		{
			get { return ConcatGenoms(FemaleGenom, MaleGenom); }
		}

		private string ConcatGenoms(string gfemale, string gmale)
		{
			string[] gsfemale = gfemale.Split(new Char[] { ';' });
			string[] gsmale = gmale.Split(new Char[] { ';' });

			if (gsmale.Length > gsfemale.Length)
			{
				string[] g = { };
				Array.Resize<string>(ref g, gsmale.Length);

				int k = gsmale.Length / gsfemale.Length;

				for (int i = 0; i < gsfemale.Length; i++)
				{
					for (int j = 0; j < k; j++)
					{
						g[i * k + j] = gsfemale[i];
					}
				}
				gsfemale = g;
			}

			if (gsfemale.Length > gsmale.Length)
			{
				string[] g = { };
				Array.Resize<string>(ref g, gsfemale.Length);

				int k = gsfemale.Length / gsmale.Length;

				for (int i = 0; i < gsmale.Length; i++)
				{
					for (int j = 0; j < k; j++)
					{
						g[i * k + j] = gsmale[i];
					}
				}
				gsmale = g;
			}

			string res = "";

			for (int i = 0; i < gsfemale.Length; i++)
			{
				res += gsfemale[i];
				res += ";";
			}

			for (int i = 0; i < gsmale.Length; i++)
			{
				res += gsmale[i];
				if (i < gsmale.Length - 1)
				{
					res += ";";
				}
			}

			return res;
		}

		public void SetRabbitsIDs(int fid, int mid)
		{
			MaleRabbit.RabbitID = mid;
			FemaleRabbit.RabbitID = fid;
		}

		public void SetRabbitsGenoms(string fg, string mg)
		{
			MaleRabbit.Genom = mg;
			FemaleRabbit.Genom = fg;
//			RedrawMe();
			if (_TreeParentPair != null)
			{
				_TreeParentPair.RefreshGenom();
			}
		}

		public void RefreshGenom()
		{
			if (_TreeChildFPair != null)
			{
				FemaleRabbit.Genom = _TreeChildFPair.Genom;
			}
			if (_TreeChildMPair != null)
			{
				MaleRabbit.Genom = _TreeChildMPair.Genom;
			}
			if (_TreeParentPair != null)
			{
				_TreeParentPair.RefreshGenom();
			}
		}

		private Control _ParentControl;

		public void SetParentControl(Control c)
		{
			if (_ParentControl == null)
			{
				_ParentControl = c;
				c.Controls.Add(this);
			}
		}

		private RabbitPair _TreeParentPair;
		
		public void SetTreeParentPair(RabbitPair p)
		{
			_TreeParentPair = p;
			ReportMySize();
		}

		/// <summary>
		/// Gender:	0 - Undefined
		///			1 - Male
		///			2 - Female
		/// </summary>
		private int _TreeGenderSide = 0;

		public void SetTreeGenderSide(int g)
		{
			if ((g == 1) || (g == 2))
			{
				_TreeGenderSide = g;
			}
		}

		private void UpdateChildFPos()
		{
			if (_TreeChildFPair != null)
			{
				_TreeChildFPair.Top = this.Top + this.Height + 10;
				_TreeChildFPair.Left = this.Left + (int)(this.Width / 2) - _TreeChildFSize.Width + _TreeChildFCenter - (int)(_TreeChildFPair.Width / 2) - 20;
				UpdateFArrowPos();
			}
		}

		private void UpdateChildMPos()
		{
			if (_TreeChildMPair != null)
			{
				_TreeChildMPair.Top = this.Top + this.Height + 10;
				_TreeChildMPair.Left = this.Left + (int)(this.Width / 2) + _TreeChildMCenter - (int)(_TreeChildMPair.Width / 2) + 20;
				UpdateMArrowPos();
			}
		}

		private Size _TreeChildFSize;
		private Size _TreeChildMSize;
		private int _TreeChildFCenter;
		private int _TreeChildMCenter;

		public void TreeChildSizeUpdate(int g, Size s,int c)
		{
			if (g==1)
			{
				_TreeChildMSize = s;
				_TreeChildMCenter = c;
				UpdateChildMPos();
			}
			if (g==2)
			{
				_TreeChildFSize = s;
				_TreeChildFCenter = c;
				UpdateChildFPos();
			}
			ReportMySize();
		}

		private void ReportMySize()
		{
			int l = this.Left;
			int r = this.Right;
			int t = this.Top;
			int b = this.Bottom;
			int c = (int)(this.Width / 2);

			if (_TreeChildFPair != null)
			{
				l = (int)(_TreeChildFPair.Left + (_TreeChildFPair.Width / 2) - (_TreeChildFSize.Width / 2));
				b = (int)(_TreeChildFPair.Bottom);
				c = (int)(this.Left - l + this.Width / 2);
			}
			if (_TreeChildMPair != null)
			{
				r = (int)(_TreeChildMPair.Left + (_TreeChildMPair.Width / 2) + (_TreeChildMSize.Width / 2));
				b = (int)(_TreeChildMPair.Bottom);
				c = (int)(this.Width / 2);
			}
			if ((_TreeChildFPair != null) && (_TreeChildMPair != null))
			{
				b = Math.Max((int)(_TreeChildFPair.Bottom), (int)(_TreeChildMPair.Bottom));
				c = (int)((r - l) / 2);
			}
			Size s = new Size(r - l, b - t);
			if (_TreeParentPair != null)
			{
				_TreeParentPair.TreeChildSizeUpdate(this._TreeGenderSide, s, c);
			}
		}

		public void ReplaceGenomeColors(Dictionary<int, Color> gcs)
		{
			FemaleRabbit.ReplaceGenomeColors(gcs);
			MaleRabbit.ReplaceGenomeColors(gcs);
			if (_TreeChildFPair != null)
			{
				_TreeChildFPair.ReplaceGenomeColors(gcs);
			}
			if (_TreeChildMPair != null)
			{
				_TreeChildMPair.ReplaceGenomeColors(gcs);
			}
			if (_TreeParentPair != null)
			{
				_TreeParentPair.RefreshGenom();
			}
		}

		private RabbitPair _TreeChildFPair;

		public void SetTreeChildFPair(RabbitPair p)
		{
			_TreeChildFPair = p;
			_TreeFArrow = new ArrowImg();
			_TreeFArrow.Location = new Point(0, 0);
			_TreeFArrow.SetLeft();
			_TreeFArrow.SetBothEnds();
			_ParentControl.Controls.Add(_TreeFArrow);
			p.SetTreeGenderSide(2);
			p.SetTreeParentPair(this);
			p.ReplaceGenomeColors(FemaleRabbit.GetGenomColors());
		}

		private RabbitPair _TreeChildMPair;

		public void SetTreeChildMPair(RabbitPair p)
		{
			_TreeChildMPair = p;
			_TreeMArrow = new ArrowImg();
			_TreeMArrow.Location = new Point(0, 0);
			_TreeMArrow.SetRight();
			_TreeMArrow.SetBothEnds();
			_ParentControl.Controls.Add(_TreeMArrow);
			p.SetTreeGenderSide(1);
			p.SetTreeParentPair(this);
			p.ReplaceGenomeColors(MaleRabbit.GetGenomColors());
		}

		private ArrowImg _TreeMArrow;
		private ArrowImg _TreeFArrow;

		private void UpdateMArrowPos()
		{
			if (_TreeMArrow != null)
			{
				if (_TreeChildMPair != null)
				{
					_TreeMArrow.Left = this.Right;
					_TreeMArrow.TopA = this.Top + (int)(this.MaleRabbit.Height / 2) + this.MaleRabbit.Top;
					_TreeMArrow.WidthA = (int)(_TreeChildMPair.Left - this.Right + _TreeChildMPair.Width / 2);
					_TreeMArrow.Height = _TreeChildMPair.Top - _TreeMArrow.Top;// this.Height + 10 - ((int)(this.MaleRabbit.Height / 2) + this.MaleRabbit.Top);
				}
			}
		}
		private void UpdateFArrowPos()
		{
			if (_TreeFArrow != null)
			{
				if (_TreeChildFPair != null)
				{
					_TreeFArrow.LeftA = _TreeChildFPair.Left+ _TreeChildFPair.Width / 2;
					_TreeFArrow.TopA = this.Top + (int)(this.FemaleRabbit.Height / 2) + this.FemaleRabbit.Top;
					_TreeFArrow.Width = this.Left - _TreeFArrow.Left;
					_TreeFArrow.Height = _TreeChildFPair.Top - _TreeFArrow.Top;// this.Height + 10 - ((int)(this.FemaleRabbit.Height / 2) + this.FemaleRabbit.Top);
				}
			}
		}

		public Boolean AddGenomeColor(int id, Color color)
		{
			Boolean fres = FemaleRabbit.AddGenomeColor(id, color);
			Boolean mres = MaleRabbit.AddGenomeColor(id, color);

			if (_TreeChildFPair != null)
			{
				_TreeChildFPair.AddGenomeColor(id, color);
			}

			if (_TreeChildMPair != null)
			{
				_TreeChildMPair.AddGenomeColor(id, color);
			}

			return fres && mres;
		}

		public override void RedrawMe()
		{
			FemaleRabbit.RedrawMe();
			MaleRabbit.RedrawMe();
		}


		public RabbitPair()
		{
			InitializeComponent();
			MaleRabbit.SetParentPair(this);
			FemaleRabbit.SetParentPair(this);

		}

		private void RabbitPair_LocationChanged(object sender, EventArgs e)
		{
			UpdateChildMPos();
			UpdateChildFPos();
		}

		public void SearchFromChild(int rid, int cmd)
		{
			if (_TreeParentPair != null)
			{
				this._TreeParentPair.SearchUp(_TreeGenderSide, rid, cmd);
			}
			if (_TreeChildFPair != null)
			{
				this._TreeChildFPair.SearchDown(rid, cmd);
			}
			if (_TreeChildMPair != null)
			{
				this._TreeChildMPair.SearchDown(rid, cmd);
			}
		}

		public void SearchDown(int rid, int cmd)
		{
			if (_TreeChildFPair != null)
			{
				this._TreeChildFPair.SearchDown(rid, cmd);
			}
			if (_TreeChildMPair != null)
			{
				this._TreeChildMPair.SearchDown(rid, cmd);
			}
			this.FemaleRabbit.SearchFromParent(rid, cmd);
			this.MaleRabbit.SearchFromParent(rid, cmd);
		}

		public void SearchUp(int g,int rid, int cmd)
		{
			if (_TreeParentPair != null)
			{
				_TreeParentPair.SearchUp(_TreeGenderSide, rid, cmd);
			}
			if (g == 1)
			{
				if (_TreeChildFPair != null)
				{
					_TreeChildFPair.SearchDown(rid, cmd);
				}
			}
			if (g == 2)
			{
				if (_TreeChildMPair != null)
				{
					_TreeChildMPair.SearchDown(rid, cmd);
				}
			}
			this.FemaleRabbit.SearchFromParent(rid, cmd);
			this.MaleRabbit.SearchFromParent(rid, cmd);
		}
	}
}
