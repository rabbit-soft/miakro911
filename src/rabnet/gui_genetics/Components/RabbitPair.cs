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
		private Rectangle _myRect;
		private int _myCenter;

		public int _id=-1;

		private int _windowRabbitID;
		public int WindowRabbitID
		{
			get { return _windowRabbitID; }
			set
			{
				_windowRabbitID = value;
				MaleRabbit.WindowRabbitID = value;
				FemaleRabbit.WindowRabbitID = value;
			}
		}

		public RabbitPair()
		{
			InitializeComponent();
			_myCenter = (int)(this.Width / 2);
			_myRect = new Rectangle(this.Location, this.Size);
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
				if (_treeParentPair != null)
				{
					_treeParentPair.RefreshGenom();
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
				if (_treeParentPair != null)
				{
					_treeParentPair.RefreshGenom();
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


		private Boolean _orderedGenom = false;
		public Boolean OrderedGenom
		{
			get { return _orderedGenom; }
			set
			{
				_orderedGenom = value;
				FemaleRabbit.OrderedGenom = value;
				MaleRabbit.OrderedGenom = value;
				if (_treeChildFPair != null)
				{
					_treeChildFPair.OrderedGenom = value;
				}

				if (_treeChildMPair != null)
				{
					_treeChildMPair.OrderedGenom = value;
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
			if (_treeParentPair != null)
			{
				_treeParentPair.RefreshGenom();
			}
		}

		public void RefreshGenom()
		{
			if (_treeChildFPair != null)
			{
				FemaleRabbit.Genom = _treeChildFPair.Genom;
			}
			if (_treeChildMPair != null)
			{
				MaleRabbit.Genom = _treeChildMPair.Genom;
			}
			if (_treeParentPair != null)
			{
				_treeParentPair.RefreshGenom();
			}
		}

		public void ReplaceGenomeColors(Dictionary<int, Color> gcs)
		{
			FemaleRabbit.ReplaceGenomeColors(gcs);
			MaleRabbit.ReplaceGenomeColors(gcs);
			if (_treeChildFPair != null)
			{
				_treeChildFPair.ReplaceGenomeColors(gcs);
			}
			if (_treeChildMPair != null)
			{
				_treeChildMPair.ReplaceGenomeColors(gcs);
			}
		}

		#endregion

		#region Positioning part

		private Control _parentControl;

		public void SetParentControl(Control c)
		{
			if (_parentControl == null)
			{
				_parentControl = c;
				this.BackColor = c.BackColor;
				MaleRabbit.BackColor = this.BackColor;
				FemaleRabbit.BackColor = this.BackColor;
				c.Controls.Add(this);

			}
		}

		private RabbitPair _treeParentPair;
		
		public void SetTreeParentPair(RabbitPair p)
		{
			_treeParentPair = p;
		}

		/// <summary>
		/// Gender:	0 - Undefined
		///			1 - Male
		///			2 - Female
		/// </summary>
		private int _treeGenderSide = 0;

		public void SetTreeGenderSide(int g)
		{
			if ((g == 1) || (g == 2))
			{
				_treeGenderSide = g;
			}
		}

		public void OrganizePos(ref Rectangle rect, ref int center)
		{
			if ((_treeChildFPair == null) && (_treeChildMPair == null))
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

			if (_treeChildFPair != null)
			{
				_treeChildFPair.OrganizePos(ref crect, ref ccent);
				PlaceFemale(ref crect, ccent);
				b = Math.Max(b, crect.Bottom);
				l = Math.Min(l, crect.Left);
			}

			if (_treeChildMPair != null)
			{
				_treeChildMPair.OrganizePos(ref crect, ref ccent);
				PlaceMale(ref crect, ccent);
				b = Math.Max(b, crect.Bottom);
				r = Math.Max(r, crect.Right);
			}

			rect.X = l;
			rect.Y = t;
			rect.Height = b - t;
			rect.Width = r - l;

			center = this.Left - l + (int)(this.Width / 2);

			_myRect = rect;
			_myCenter = center;

			Graphics g = _parentControl.CreateGraphics();
			g.DrawRectangle(Pens.Purple, _myRect);
			g.DrawLine(Pens.Green, _myRect.Left + _myCenter, this.Top, _myRect.Left + _myCenter, this.Top - 20);

		}

		public void PlaceFemale(ref Rectangle rect, int center)
		{
			int t = this.Top + this.Height + 10;
			int l = this.Left + (int)(this.Width / 2) - rect.Width + center - (int)(_treeChildFPair.Width / 2) - 20;
			int dx = l - _treeChildFPair.Left;
			int dy = t - _treeChildFPair.Top;
			_treeChildFPair.MoveBy(dx, dy);
			_treeChildFPair.MeetNeighbors();
			UpdateFArrowPos();
			rect.Offset(dx, dy);
		}

		public void PlaceMale(ref Rectangle rect, int center)
		{
			int t = this.Top + this.Height + 10;
			int l = this.Left + (int)(this.Width / 2) + center - (int)(_treeChildMPair.Width / 2) + 20;
			int dx = l - _treeChildMPair.Left;
			int dy = t - _treeChildMPair.Top;
			_treeChildMPair.MoveBy(dx, dy);
			_treeChildMPair.MeetNeighbors();
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
			_myRect.Offset(dx, dy);
			if (_treeChildFPair != null)
			{
				_treeChildFPair.MoveBy(dx, dy);
				UpdateFArrowPos();
			}
			if (_treeChildMPair != null)
			{
				_treeChildMPair.MoveBy(dx, dy);
				UpdateMArrowPos();
			}
		}
		
		
		private RabbitPair _treeChildFPair;

		public void SetTreeChildFPair(RabbitPair p)
		{
			_treeChildFPair = p;

			_treeFArrow = new ArrowImg();
			_treeFArrow.BackColor = this.BackColor;
			_treeFArrow.Location = new Point(0, 0);
			_treeFArrow.SetLeft();
			_treeFArrow.SetStart();
//			_TreeFArrow.SetBothEnds();
			_parentControl.Controls.Add(_treeFArrow);

			p.SetTreeGenderSide(2);
			p.SetTreeParentPair(this);
			p.ReplaceGenomeColors(FemaleRabbit.GetGenomColors());
			p.WindowRabbitID = _windowRabbitID;
		}

		private RabbitPair _treeChildMPair;

		public void SetTreeChildMPair(RabbitPair p)
		{
			_treeChildMPair = p;

			_treeMArrow = new ArrowImg();
			_treeMArrow.BackColor = this.BackColor;
			_treeMArrow.Location = new Point(0, 0);
			_treeMArrow.SetRight();
			_treeMArrow.SetStart();
//			_TreeMArrow.SetBothEnds();
			_parentControl.Controls.Add(_treeMArrow);

			p.SetTreeGenderSide(1);
			p.SetTreeParentPair(this);
			p.ReplaceGenomeColors(MaleRabbit.GetGenomColors());
			p.WindowRabbitID = _windowRabbitID;
		}

		private ArrowImg _treeMArrow;
		private ArrowImg _treeFArrow;

		private void UpdateMArrowPos()
		{
			if (_treeMArrow != null)
			{
				if (_treeChildMPair != null)
				{
					_treeMArrow.Left = this.Right;
					_treeMArrow.TopA = this.Top + (int)(this.MaleRabbit.Height / 2) + this.MaleRabbit.Top;
					_treeMArrow.WidthA = (int)(_treeChildMPair.Left - this.Right + _treeChildMPair.Width / 2);
					_treeMArrow.Height = _treeChildMPair.Top - _treeMArrow.Top;// this.Height + 10 - ((int)(this.MaleRabbit.Height / 2) + this.MaleRabbit.Top);
				}
			}
		}
		private void UpdateFArrowPos()
		{
			if (_treeFArrow != null)
			{
				if (_treeChildFPair != null)
				{
					_treeFArrow.LeftA = _treeChildFPair.Left+ _treeChildFPair.Width / 2;
					_treeFArrow.TopA = this.Top + (int)(this.FemaleRabbit.Height / 2) + this.FemaleRabbit.Top;
					_treeFArrow.Width = this.Left - _treeFArrow.Left;
					_treeFArrow.Height = _treeChildFPair.Top - _treeFArrow.Top;// this.Height + 10 - ((int)(this.FemaleRabbit.Height / 2) + this.FemaleRabbit.Top);
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

		public Boolean SearchFromChild(RabbitCommandMessage cmd, bool evProc)
		{
			Boolean res = false;
			if (_treeParentPair != null)
			{
				res = res || this._treeParentPair.SearchUp(_treeGenderSide, cmd);
			}
			if ((evProc) && (SearchGoingOn != null))
			{
				res = res || SearchGoingOn(cmd);
			}
			if (_treeChildFPair != null)
			{
				res = res || this._treeChildFPair.SearchDown(cmd);
			}
			if (_treeChildMPair != null)
			{
				res = res || this._treeChildMPair.SearchDown(cmd);
			}
			return res;
		}

		public Boolean SearchDown(RabbitCommandMessage cmd)
		{
			Boolean res = false;
			if (_treeChildFPair != null)
			{
				res = res || this._treeChildFPair.SearchDown(cmd);
			}
			if (_treeChildMPair != null)
			{
				res = res || this._treeChildMPair.SearchDown(cmd);
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
			if (_treeParentPair != null)
			{
				res = res || _treeParentPair.SearchUp(_treeGenderSide, cmd);
			}
			if (g == 1)
			{
				if (_treeChildFPair != null)
				{
					res = res || _treeChildFPair.SearchDown(cmd);
				}
			}
			if (g == 2)
			{
				if (_treeChildMPair != null)
				{
					res = res || _treeChildMPair.SearchDown(cmd);
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
