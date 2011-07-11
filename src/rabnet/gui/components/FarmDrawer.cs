using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace rabnet
{
    public partial class FarmDrawer : UserControl
    {
        const char N = 'N';

        private bool manual=false;

        public class DrawTier
        {
            private String type;
            private String delims;
            private String nests;
            private String heaters;
            private string[] rbs;
            private Graphics _graf;
            private Rectangle _rect;
            private bool repair;
            public int id = 0;
//            private Font f = new Font("Arial", 8);
			private Font f = SystemFonts.DefaultFont;
			public DrawTier(int id, String type, String delims, String nests, String heaters, String[] rabbits, bool repair)
            {
                this.id = id;
                this.repair=repair;
                this.type = type;
                this.delims = delims;
                this.nests = nests;
                rbs=rabbits;
                this.heaters = heaters;
            }

            private void setNest(BuildingControl p, char nest, char heater, bool second)
            {
                if (second)
                    p.nest2 = (nest == '1');
                else
                    p.nest = (nest == '1');
                int htr = 2;
                if (heater == '0') htr = 0;
                if (heater == '1') htr = 1;
                if (second)
                    p.heater2 = htr;
                else
                    p.heater = htr;
            }
            private void setVigul(BuildingControl p,char delim)
            {
                p.vigul = (delim == '1');
            }
            private void setDelim(BuildingControl p, char delim)
            {
                p.delim = (delim == '1');
            }
            private void setDelims(BuildingControl p, string delims)
            {
                p.delim1 = (delims[0] == '1');
                p.delim2 = (delims[1] == '1');
                p.delim3 = (delims[2] == '1');
            }

            public void draw(Graphics g, Rectangle r,BuildingControl p)
            {              

                this._graf=g;
                this._rect=r;
                p.setType(type);
                p.repair = repair;
                switch (type)
                {
                    case myBuildingType.Female:
                        drawPart(0,1,"",rbs[0],nests[0],heaters[0],null);
                        setNest(p, nests[0], heaters[0], false);
                        break;
                    case myBuildingType.DualFemale:
                        drawPart(0, 0.5,"а",rbs[0],nests[0],heaters[0],null);
                        drawPart(0.5, 0.5,"б",rbs[1],nests[1],heaters[1],null);
						drawWall(0, 0.5);
						drawWall(0.5, 0.5);
						setNest(p, nests[0], heaters[0], false);
                        setNest(p, nests[1], heaters[1], true);
                        break;
                    case myBuildingType.Complex:
                        drawPart(0, 0.5, "а", rbs[0], nests[0], heaters[0], null);
                        drawPart(0.5, 0.25, "б", rbs[1], N, N, null);
                        drawPart(0.75, 0.25, "в", rbs[2], N, N, null);
						drawWall(0, 0.5);
						drawWall(0.5, 0.25);
						//drawWall(0.75, 0.25);
						setNest(p, nests[0], heaters[0], false);
                        break;
                    case myBuildingType.Jurta:
                        bool fst = delims[0] == '1';
                        if (fst)
                        {
                            drawPart(0, 0.25, "а", rbs[0], N, N, null);
                            drawPart(0.25, 0.75, "б", rbs[1], nests[0], heaters[0], new double[]{0.62});
							drawWall(0, 0.25);
							//drawWall(0.25, 0.75);
						}
                        else
                        {
                            drawPart(0, 0.62, "а", rbs[0], nests[0], heaters[0], new double[] { 0.25 });
                            drawPart(0.62, 0.38, "б", rbs[1], N, N, null);
							drawWall(0, 0.62);
							//drawWall(0.62, 0.38);
						}
                        setNest(p, nests[0], heaters[0], false);
                        setVigul(p, delims[0]);
                        break;
                    case myBuildingType.Quarta:
                        drawQuarta();
                        setDelims(p, delims);
                        break;
                    case myBuildingType.Vertep:
                        drawPart(0, 0.5, "а", rbs[0], N, N, null);
                        drawPart(0.5, 0.5, "б", rbs[1], N, N, null);
						drawWall(0, 0.5);
						//drawWall(0.5, 0.5);
						break;
                    case myBuildingType.Barin:
						if (delims[0] == '1')
						{
							drawPart(0, 0.5, "а", rbs[0], N, N, null);
							drawPart(0.5, 0.5, "б", rbs[1], N, N, null);
							drawWall(0, 0.5);
							//drawWall(0.5, 0.5);
						}
						else
						{
							drawPart(0, 1, "аб", rbs[0], N, N, new double[] { 0.5 });
							drawWall(0, 1);
						}
						setDelim(p, delims[0]);
                        break;
                    case myBuildingType.Cabin:
                        drawPart(0, 1, "а", rbs[0], N, N, null);
                        //drawPart(0.65, 0.35, "б", rbs[1], N, N, null);
						//drawWall(0, 1);
						//drawWall(0.65, 0.35);
						//setNest(p, nests[0], heaters[0], false);
                        break;
                }
            }
            public void drawQuarta()
            {
                int i = 0;
                double x = 0.25;
                double start = 0.0;
                string names="абвг";
                string nm="а";
                List<double> dl=new List<double>();
				Dictionary<double,double> wls=new Dictionary<double,double>();

                while (i < 3)
                {
                    if (delims[i] == '1')
                    {
                        drawPart(start, x, nm, rbs[i], N, N, dl.ToArray());
						wls.Add(start,x);
						i++;
                        start += x;
                        nm = ""+names[i];
                        dl.Clear();
					}
                    else
                    {
                        i++;
                        dl.Add(x);
                        x += 0.25;
                        nm += names[i];
                    }
                }
                drawPart(start, x, nm, rbs[i], N, N, dl.ToArray());
				wls.Add(start, x);
				foreach (KeyValuePair<double, double> wl in wls)
				{
					drawWall(wl.Key, wl.Value);
				}
			}

            /// <summary>
            /// Нарисовать стенку строения
            /// </summary>
            /// <param name="start">От какой нозиции начать отчет</param>
            /// <param name="sz">На каком растоянии от старта нарисовать стенку</param>
			public void drawWall(double start, double sz)
			{
				Point p1 = new Point();
				Point p2 = new Point();
				if ((start + sz <= 1) && (start + sz > 0))
				{
					p1.X = (int)Math.Round(_rect.Width * sz) + (int)Math.Round(_rect.Width * start);
					p1.Y = _rect.Top;
                    p2.X = p1.X;
					p2.Y = _rect.Bottom;
					_graf.DrawLine(Pens.Black, p1, p2);
				}

			}

            /// <summary>
            /// Ресуем Клетку (секцию)
            /// </summary>
            /// <param name="start">Нало рисования (%)</param>
            /// <param name="sz">Ширина клетки (%)</param>
            /// <param name="area">А Б В Г</param>
            /// <param name="rabbit">Имя кролика</param>
            /// <param name="nest">Гнездовье?</param>
            /// <param name="heater">Грелка?</param>
            public void drawPart(double start,double sz,String area,String rabbit,Char nest,Char heater,double[] fd)
            {
                Rectangle rct = _rect;
				RectangleF rctF = _rect;
                int wdth = 0;//Лекарство от захождения Секции за правую стенку
                if ((Math.Round(_rect.Width * start) + Math.Round(_rect.Width * sz)) > _rect.Width)
                    wdth = (int)(_rect.Width - Math.Round(_rect.Width * start));
                else wdth = (int)Math.Round(_rect.Width * sz);
                rct.Offset((int)Math.Round(_rect.Width * start), 0);//Установка начала области
                rct.Width = wdth;
				if (repair)
				{
					_graf.FillRectangle(Brushes.Silver, rct);
				}
				else
				{
					_graf.FillRectangle(Brushes.White, rct);
				}
				if (fd != null && fd.Length > 0)
				{
					for (int i = 0; i < fd.Length; i++)
					{
						int xpos = (int)Math.Round(_rect.Width * fd[i]);
						_graf.DrawLine(Pens.Lavender, new Point(xpos, rct.Top + 1), new Point(xpos, rct.Bottom - 1));
					}
				}
                _graf.DrawString(area, f, Brushes.Black, rct.X, rct.Y);
                FarmDrawer.drawtext(rabbit, rct, 8, _graf,false);
                Rectangle nrct = rct;
                nrct.Height = (int)Math.Round(rct.Height * 0.2);
                nrct.Offset(0, (int)Math.Round(rct.Height * 0.8));
                nrct.Width = rct.Width / 2+1;
                Rectangle hrct=nrct;
                hrct.Offset(rct.Width / 2, 0);
                if (nest!=N)
                {
                    _graf.FillRectangle(nest == '1' ? Brushes.Gold : Brushes.Silver, nrct);
                    FarmDrawer.drawtext("гнездо" + (nest == '1' ? "" : ":нет"), nrct, 10, _graf, false);
                }
                if (heater !=N)
                {
                    Brush b = Brushes.Silver;
                    String stat = "Нет";
                    if (heater == '1')
                    {
                        b = Brushes.Blue;
                        stat = "Выкл";
                    }
                    if (heater == '3')
                    {
                        b = Brushes.Red;
                        stat = "Вкл";
                    }
                    _graf.FillRectangle(b, hrct);
                    FarmDrawer.drawtext("грелка:" + stat, hrct, 10, _graf, false);
                }
                _graf.DrawLine(Pens.Black, new Point(rct.Left, rct.Top), new Point(rct.Right, rct.Top));//Основание крыши
				_graf.DrawLine(Pens.Black, new Point(rct.Right, rct.Bottom), new Point(rct.Left, rct.Bottom));//Основание домика
            }
        }

        private DrawTier t1 = null;
        private DrawTier t2 = null;
        private int id = 0;

        public FarmDrawer()
        {
            InitializeComponent();
        }

        public void setFarm(int id,DrawTier t1,DrawTier t2)
        {
            this.t1 = t1;
            this.t2 = t2;
            this.id = id;
            bc1.Visible = bc2.Visible = false;
            Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
//			e.Graphics.Clear(Color.White);
			e.Graphics.Clear(SystemColors.Control);
            if (t1!=null)
            {
                manual = true;
                drawHouse(e.Graphics,e.ClipRectangle);
                manual = false;
            }
        }

        public static void drawtext(String text,Rectangle r,int sz,Graphics g,bool bottom)
        {
            StringFormat sf=new StringFormat();
            sf.Alignment=StringAlignment.Center;
            sf.LineAlignment = bottom?StringAlignment.Far:StringAlignment.Center;
//			g.DrawString(text, new Font("Arial", sz), Brushes.Black,
//				new RectangleF(r.X, r.Y, r.Width, r.Height), sf);
			g.DrawString(text, new Font(SystemFonts.DefaultFont.Name, sz), Brushes.Black,
				new RectangleF(r.X, r.Y, r.Width, r.Height), sf);
		}


        private void drawHouse(Graphics g,Rectangle rect)
        {
            Rectangle r1 = rect;
            Rectangle[] r2 = new Rectangle[] { rect, rect };
            r1.Height=(int)Math.Round(rect.Height*0.15);
            r2[0].Height=(int)Math.Round(rect.Height*0.85);
            r2[0].Width -= 1;//чтобы видно было правую стенку
            r2[0].Offset(0,r1.Height);
            r2[1]=r2[0];
            if (r2!=null)
            {
                r2[0].Height=r2[0].Height/2;
                r2[1].Height=r2[0].Height;
                r2[1].Offset(0,r2[0].Height-1);
            }

			g.SmoothingMode = SmoothingMode.HighQuality;
			g.FillPolygon(Brushes.White, new Point[] {
                new Point(r1.X,r1.Bottom),
                new Point(r1.Right/2,r1.Y),
                new Point(r1.Right,r1.Bottom)
            });
			g.DrawLines(Pens.Black, new Point[] {
                new Point(r1.X,r1.Bottom),
                new Point(r1.Right/2,r1.Y),
                new Point(r1.Right,r1.Bottom)
            });
			g.SmoothingMode = SmoothingMode.None;

            drawtext(id.ToString(), r1, 12, g,true);
            g.DrawRectangle(new Pen(Color.Black, 1), r2[0]);

			bc1.Top = r2[0].Top + pictureBox1.Top;// +1;
            bc1.Height = r2[0].Height+1;
            t1.draw(g, r2[0], bc1);
			bc1.Visible = true;
            if (t2 != null)
            {
				bc2.Top = r2[1].Top + pictureBox1.Top;// +2;
				bc2.Height = r2[1].Height+1;
				r2[1].Offset(0,1);
				r2[1].Height -= 1;
				g.DrawRectangle(new Pen(Color.Black, 1), r2[1]);
                t2.draw(g, r2[1],bc2);
                bc2.Visible = true;
            }
        }

        private void FarmDrawer_Move(object sender, EventArgs e)
        {
            Refresh();
        }

        private void FarmDrawer_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        private void FarmDrawer_Paint(object sender, PaintEventArgs e)
        {
            pictureBox1.Refresh();
        }

        public delegate void FDEventHandler(object sender, BuildingControl.BCEvent e);
        public event FDEventHandler ValueChanged = null;

        private void bc_ValueChanged(object sender, BuildingControl.BCEvent e)
        {
            if (manual)
                return;
            e.farm = id;
            e.tier = (sender == bc1 ? t1.id : t2.id);
            if (ValueChanged != null)
                ValueChanged(this, e);
		}

    }
}
