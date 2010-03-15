using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using log4net;

namespace rabnet
{
	public delegate Boolean EvSearchGoingOn(RabbitCommandMessage cmd);

	public partial class RabbitPair : UserControl
	{

		protected static readonly ILog log = LogManager.GetLogger(typeof(RabbitPair));

		public event EvSearchGoingOn SearchGoingOn;


		private RabbitGen _mom;
		private RabbitGen _dad;
		private Rectangle _MyRect;
		private int _MyCenter;

		public int _id=-1;

		private int _WindowRabbitID;
		public int WindowRabbitID
		{
			get { return _WindowRabbitID; }
			set
			{
				_WindowRabbitID = value;
				MaleRabbit.WindowRabbitID = value;
				FemaleRabbit.WindowRabbitID = value;
			}
		}

		public RabbitPair()
		{
			InitializeComponent();
			_MyCenter = (int)(this.Width / 2);
			_MyRect = new Rectangle(this.Location, this.Size);
			MaleRabbit.SetParentPair(this);
			FemaleRabbit.SetParentPair(this);

		}

		#region Genome Data Part

		public void SetMom(RabbitGen r)
		{
			if (r != null)
			{
				_mom = r;
				FemaleRabbit.Genom = r.breed.ToString();
				FemaleRabbit.SetRabbit(r);
				if (_TreeParentPair != null)
				{
					_TreeParentPair.RefreshGenom();
				}
			}
		}

		public void SetDad(RabbitGen r)
		{
			if (r != null)
			{
				_dad = r;
				MaleRabbit.Genom = r.breed.ToString();
				MaleRabbit.SetRabbit(r);
				if (_TreeParentPair != null)
				{
					_TreeParentPair.RefreshGenom();
				}
			}
		}

		public RabbitGen GetMom()
		{
			return _mom;
		}

		public RabbitGen GetDad()
		{
			return _dad;
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
			get { return (MaleRabbit.GetRabbit() != null) ? MaleRabbit.Genom : ""; }
			set { MaleRabbit.Genom = value; }
		}
		public string FemaleGenom
		{
			get { return (FemaleRabbit.GetRabbit() != null) ? FemaleRabbit.Genom : ""; }
			set { FemaleRabbit.Genom = value; }
		}

		public string Genom
		{
			get { return ConcatGenoms(FemaleGenom, MaleGenom); }
		}

		private string ConcatGenoms(string gfemale, string gmale)
		{
			if (gmale == "")
			{
				return gfemale;
			}
			if (gfemale == "")
			{
				return gmale;
			}
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

		public void SetRabbitsGenoms(string fg, string mg)
		{
			MaleRabbit.Genom = mg;
			FemaleRabbit.Genom = fg;
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
		}

		#endregion

		#region Positioning part

		private Control _ParentControl;

		public void SetParentControl(Control c)
		{
			if (_ParentControl == null)
			{
				_ParentControl = c;
				this.BackColor = c.BackColor;
				MaleRabbit.BackColor = this.BackColor;
				FemaleRabbit.BackColor = this.BackColor;
				c.Controls.Add(this);

			}
		}

		private RabbitPair _TreeParentPair;
		
		public void SetTreeParentPair(RabbitPair p)
		{
			_TreeParentPair = p;
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

		public void OrganizePos(ref Rectangle rect, ref int center)
		{
			if ((_TreeChildFPair == null) && (_TreeChildMPair == null))
			{
				rect = new Rectangle(this.Location, this.Size);
				center = (int)(this.Width / 2.0);
				return;
			}

			Rectangle crect = new Rectangle();
			int ccent = 0;

			int t = this.Top;
			int b = this.Bottom;
			int l = this.Left;
			int r = this.Right;

			if (_TreeChildFPair != null)
			{
				_TreeChildFPair.OrganizePos(ref crect, ref ccent);
				PlaceFemale(ref crect, ccent);
				b = Math.Max(b, crect.Bottom);
				l = Math.Min(l, crect.Left);
			}

			if (_TreeChildMPair != null)
			{
				_TreeChildMPair.OrganizePos(ref crect, ref ccent);
				PlaceMale(ref crect, ccent);
				b = Math.Max(b, crect.Bottom);
				r = Math.Max(r, crect.Right);
			}

			rect.X = l;
			rect.Y = t;
			rect.Height = b - t;
			rect.Width = r - l;

			center = this.Left - l + (int)(this.Width / 2);

			_MyRect = rect;
			_MyCenter = center;

			Graphics g = _ParentControl.CreateGraphics();
			g.DrawRectangle(Pens.Purple, _MyRect);
			g.DrawLine(Pens.Green, _MyRect.Left + _MyCenter, this.Top, _MyRect.Left + _MyCenter, this.Top - 20);

		}

		public void PlaceFemale(ref Rectangle rect, int center)
		{
			int t = this.Top + this.Height + 10;
			int l = this.Left + (int)(this.Width / 2) - rect.Width + center - (int)(_TreeChildFPair.Width / 2) - 20;
			int dx = l - _TreeChildFPair.Left;
			int dy = t - _TreeChildFPair.Top;
			_TreeChildFPair.MoveBy(dx, dy);
			_TreeChildFPair.MeetNeighbors();
			UpdateFArrowPos();
			rect.Offset(dx, dy);
		}

		public void PlaceMale(ref Rectangle rect, int center)
		{
			int t = this.Top + this.Height + 10;
			int l = this.Left + (int)(this.Width / 2) + center - (int)(_TreeChildMPair.Width / 2) + 20;
			int dx = l - _TreeChildMPair.Left;
			int dy = t - _TreeChildMPair.Top;
			_TreeChildMPair.MoveBy(dx, dy);
			_TreeChildMPair.MeetNeighbors();
			UpdateMArrowPos();
			rect.Offset(dx, dy);
		}

		public void MeetNeighbors()
		{
			FemaleRabbit.MeetNeighbors();
			MaleRabbit.MeetNeighbors();
		}

		public void SetNewLocation(int left, int top)
		{
			int dx = left - this.Left;
			int dy = top - this.Top;

			MoveBy(dx, dy);
		}

		public void MoveBy(int dx, int dy)
		{
			this.Left = this.Left + dx;
			this.Top = this.Top + dy;
			_MyRect.Offset(dx, dy);
			if (_TreeChildFPair != null)
			{
				_TreeChildFPair.MoveBy(dx, dy);
				UpdateFArrowPos();
			}
			if (_TreeChildMPair != null)
			{
				_TreeChildMPair.MoveBy(dx, dy);
				UpdateMArrowPos();
			}
		}
		
		
		private RabbitPair _TreeChildFPair;

		public void SetTreeChildFPair(RabbitPair p)
		{
			_TreeChildFPair = p;

			_TreeFArrow = new ArrowImg();
			_TreeFArrow.BackColor = this.BackColor;
			_TreeFArrow.Location = new Point(0, 0);
			_TreeFArrow.SetLeft();
			_TreeFArrow.SetStart();
//			_TreeFArrow.SetBothEnds();
			_ParentControl.Controls.Add(_TreeFArrow);

			p.SetTreeGenderSide(2);
			p.SetTreeParentPair(this);
			p.ReplaceGenomeColors(FemaleRabbit.GetGenomColors());
			p.WindowRabbitID = _WindowRabbitID;
		}

		private RabbitPair _TreeChildMPair;

		public void SetTreeChildMPair(RabbitPair p)
		{
			_TreeChildMPair = p;

			_TreeMArrow = new ArrowImg();
			_TreeMArrow.BackColor = this.BackColor;
			_TreeMArrow.Location = new Point(0, 0);
			_TreeMArrow.SetRight();
			_TreeMArrow.SetStart();
//			_TreeMArrow.SetBothEnds();
			_ParentControl.Controls.Add(_TreeMArrow);

			p.SetTreeGenderSide(1);
			p.SetTreeParentPair(this);
			p.ReplaceGenomeColors(MaleRabbit.GetGenomColors());
			p.WindowRabbitID = _WindowRabbitID;
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


		public void RedrawMe()
		{
			FemaleRabbit.RedrawMe();
			MaleRabbit.RedrawMe();
		}

		public Boolean SearchFromChild(RabbitCommandMessage cmd)
		{
			return SearchFromChild(cmd, true);
		}

		public Boolean SearchFromChild(RabbitCommandMessage cmd, bool EvProc)
		{
			Boolean res = false;
			if (_TreeParentPair != null)
			{
				res = res || this._TreeParentPair.SearchUp(_TreeGenderSide, cmd);
			}
			if ((EvProc) && (SearchGoingOn != null))
			{
				res = res || SearchGoingOn(cmd);
			}
			if (_TreeChildFPair != null)
			{
				res = res || this._TreeChildFPair.SearchDown(cmd);
			}
			if (_TreeChildMPair != null)
			{
				res = res || this._TreeChildMPair.SearchDown(cmd);
			}
			return res;
		}

		public Boolean SearchDown(RabbitCommandMessage cmd)
		{
			Boolean res = false;
			if (_TreeChildFPair != null)
			{
				res = res || this._TreeChildFPair.SearchDown(cmd);
			}
			if (_TreeChildMPair != null)
			{
				res = res || this._TreeChildMPair.SearchDown(cmd);
			}

			res = res || this.FemaleRabbit.SearchFromParent(cmd);
			res = res || this.MaleRabbit.SearchFromParent(cmd);
			if (SearchGoingOn != null)
			{
				res = res || SearchGoingOn(cmd);
			}
		
			return res;
		}

		public Boolean SearchUp(int g, RabbitCommandMessage cmd)
		{
			Boolean res = false;
			if (_TreeParentPair != null)
			{
				res = res || _TreeParentPair.SearchUp(_TreeGenderSide, cmd);
			}
			if (g == 1)
			{
				if (_TreeChildFPair != null)
				{
					res = res || _TreeChildFPair.SearchDown(cmd);
				}
			}
			if (g == 2)
			{
				if (_TreeChildMPair != null)
				{
					res = res || _TreeChildMPair.SearchDown(cmd);
				}
			}
			res = res || this.FemaleRabbit.SearchFromParent(cmd);
			res = res || this.MaleRabbit.SearchFromParent(cmd);
			if (SearchGoingOn != null)
			{
				res = res || SearchGoingOn(cmd);
			}
			return res;
		}
		#endregion
	}
}
