using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace rabnet.components
{
    public partial class FarmDrawer : UserControl
    {
        public delegate void FDEventHandler(object sender, BuildingControl.BCEvent e);

        public event FDEventHandler ValueChanged = null;

        const char N = 'N';

        private bool _manual = false;

        public class DrawTier
        {
            private Building _build;
            //private BuildingType _type;
            //private String _delims;
            //private String _nests;
            //private String _heaters;
            private string[] _rbs;
            private Graphics _graf;
            private Rectangle _rect;
            //private bool _repair;
            //public int _id = 0;
            //            private Font f = new Font("Arial", 8);
            private Font f = SystemFonts.DefaultFont;

            public DrawTier(Building b, List<string> rabs)
            {
                _build = b;
                _rbs = rabs.ToArray();
                //_id = id;
                //_repair=repair;
                //_type = type;
                //_delims = delims;
                //_nests = nests;
                //_rbs=residents;
                //_heaters = heaters;
            }

            public Building Building { get { return _build; } }

            public void Draw(Graphics g, Rectangle r, BuildingControl bControl)
            {
                _graf = g;
                _rect = r;
                bControl.SetType(_build.Type);
                bControl.repair = _build.Repair;
                switch (_build.Type) {
                    case BuildingType.Female:
                        DrawPart(0, 1, "", _rbs[0], _build.Nests[0], _build.Heaters[0], null);
                        setNest(bControl, _build.Nests[0], _build.Heaters[0], false);
                        break;

                    case BuildingType.DualFemale:
                        DrawPart(0, 0.5, "а", _rbs[0], _build.Nests[0], _build.Heaters[0], null);
                        DrawPart(0.5, 0.5, "б", _rbs[1], _build.Nests[1], _build.Heaters[1], null);
                        DrawWall(0, 0.5);
                        DrawWall(0.5, 0.5);
                        setNest(bControl, _build.Nests[0], _build.Heaters[0], false);
                        setNest(bControl, _build.Nests[1], _build.Heaters[1], true);
                        break;

                    case BuildingType.Complex:
                        DrawPart(0, 0.5, "а", _rbs[0], _build.Nests[0], _build.Heaters[0], null);
                        DrawPart(0.5, 0.25, "б", _rbs[1], N, N, null);
                        DrawPart(0.75, 0.25, "в", _rbs[2], N, N, null);
                        DrawWall(0, 0.5);
                        DrawWall(0.5, 0.25);
                        //drawWall(0.75, 0.25);
                        setNest(bControl, _build.Nests[0], _build.Heaters[0], false);
                        break;

                    case BuildingType.Jurta:
                        bool fst = _build.Delims[0] == '1';
                        if (fst) {
                            DrawPart(0, 0.25, "а", _rbs[0], N, N, null);
                            DrawPart(0.25, 0.75, "б", _rbs[1], _build.Nests[0], _build.Heaters[0], new double[] { 0.62 });
                            DrawWall(0, 0.25);
                            //drawWall(0.25, 0.75);
                        } else {
                            DrawPart(0, 0.62, "а", _rbs[0], _build.Nests[0], _build.Heaters[0], new double[] { 0.25 });
                            DrawPart(0.62, 0.38, "б", _rbs[1], N, N, null);
                            DrawWall(0, 0.62);
                            //drawWall(0.62, 0.38);
                        }
                        setNest(bControl, _build.Nests[0], _build.Heaters[0], false);
                        setVigul(bControl, _build.Delims[0]);
                        break;

                    case BuildingType.Quarta:
                        DrawQuarta();
                        setDelims(bControl, _build.Delims);
                        break;

                    case BuildingType.Vertep:
                        DrawPart(0, 0.5, "а", _rbs[0], N, N, null);
                        DrawPart(0.5, 0.5, "б", _rbs[1], N, N, null);
                        DrawWall(0, 0.5);
                        //drawWall(0.5, 0.5);
                        break;

                    case BuildingType.Barin:
                        if (_build.Delims[0] == '1') {
                            DrawPart(0, 0.5, "а", _rbs[0], N, N, null);
                            DrawPart(0.5, 0.5, "б", _rbs[1], N, N, null);
                            DrawWall(0, 0.5);
                            //drawWall(0.5, 0.5);
                        } else {
                            DrawPart(0, 1, "аб", _rbs[0], N, N, new double[] { 0.5 });
                            DrawWall(0, 1);
                        }
                        setDelim(bControl, _build.Delims[0]);
                        break;

                    case BuildingType.Cabin:
                        DrawPart(0, 1, "а", _rbs[0], N, N, null);
                        //drawPart(0.65, 0.35, "б", rbs[1], N, N, null);
                        //drawWall(0, 1);
                        //drawWall(0.65, 0.35);
                        //setNest(p, nests[0], heaters[0], false);
                        break;
                }
            }

            public void DrawQuarta()
            {
                const double UNIT = 0.25;
                double size = UNIT;
                double start = 0.0;
                string names = "абвг";
                string nm = "";
                List<double> deleted = new List<double>();
                Dictionary<double, double> walls = new Dictionary<double, double>();
                bool preDel = false;

                for (int i = 0; i < _build.Sections; i++) {
                    nm += names[i];
                    if (i + 1 <= _build.Sections - 1 && _build.Delims[i] == '0') {
                        deleted.Add(size);
                        preDel = true;
                        size += UNIT;
                    } else {
                        DrawPart(start, size, nm, _rbs[i + (preDel ? -1 : 0)], N, N, deleted.ToArray());
                        walls.Add(start, size);
                        start += size;
                        size = UNIT;
                        deleted.Clear();
                        preDel = false;
                        nm = "";
                    }

                }
                ///рисуем реальные перегородки
                foreach (KeyValuePair<double, double> wl in walls) {
                    DrawWall(wl.Key, wl.Value);
                }
            }

            /// <summary>
            /// Нарисовать стенку строения
            /// </summary>
            /// <param name="start">От какой нозиции начать отчет</param>
            /// <param name="sz">На каком растоянии от старта нарисовать стенку</param>
            public void DrawWall(double start, double sz)
            {
                Point p1 = new Point();
                Point p2 = new Point();
                if ((start + sz <= 1) && (start + sz > 0)) {
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
            /// <param name="rabName">Имя кролика</param>
            /// <param name="nest">Гнездовье?</param>
            /// <param name="heater">Грелка?</param>
            /// <param name="deletedWalls">Удаленные перегородки</param>
            public void DrawPart(double start, double sz, String area, String rabName, Char nest, Char heater, double[] deletedWalls)
            {
                Rectangle rct = _rect;
                RectangleF rctF = _rect;
                int wdth = 0; //Лекарство от захождения Секции за правую стенку
                if ((Math.Round(_rect.Width * start) + Math.Round(_rect.Width * sz)) > _rect.Width) {
                    wdth = (int)(_rect.Width - Math.Round(_rect.Width * start));
                } else {
                    wdth = (int)Math.Round(_rect.Width * sz);
                }
                rct.Offset((int)Math.Round(_rect.Width * start), 0);//Установка начала области
                rct.Width = wdth;
                if (_build.Repair) {
                    _graf.FillRectangle(Brushes.Silver, rct);
                } else {
                    _graf.FillRectangle(Brushes.White, rct);
                }
                ///рисуем убраные стенки
                if (deletedWalls != null && deletedWalls.Length > 0) {
                    for (int i = 0; i < deletedWalls.Length; i++) {
                        int xpos = (int)Math.Round(rct.X + _rect.Width * deletedWalls[i]);
                        _graf.DrawLine(Pens.Lavender, new Point(xpos, rct.Top + 1), new Point(xpos, rct.Bottom - 1));
                    }
                }
                _graf.DrawString(area, f, Brushes.Black, rct.X, rct.Y);
                //пишем имена кроликов сидящих в клетке
                FarmDrawer.DrawText(rabName, rct, 8, _graf, false);
                Rectangle nrct = rct;
                nrct.Height = (int)Math.Round(rct.Height * 0.2);
                nrct.Offset(0, (int)Math.Round(rct.Height * 0.8));
                nrct.Width = rct.Width / 2 + 1;
                Rectangle hrct = nrct;
                hrct.Offset(rct.Width / 2, 0);
                if (nest != N) {
                    _graf.FillRectangle(nest == '1' ? Brushes.Gold : Brushes.Silver, nrct);
                    FarmDrawer.DrawText("гнездо" + (nest == '1' ? "" : ":нет"), nrct, 10, _graf, false);
                }
                if (heater != N) {
                    Brush b = Brushes.Silver;
                    String stat = "Нет";
                    if (heater == '1') {
                        b = Brushes.Blue;
                        stat = "Выкл";
                    }
                    if (heater == '3') {
                        b = Brushes.Red;
                        stat = "Вкл";
                    }
                    _graf.FillRectangle(b, hrct);
                    FarmDrawer.DrawText("грелка:" + stat, hrct, 10, _graf, false);
                }
                _graf.DrawLine(Pens.Black, new Point(rct.Left, rct.Top), new Point(rct.Right, rct.Top));//Основание крыши
                _graf.DrawLine(Pens.Black, new Point(rct.Right, rct.Bottom), new Point(rct.Left, rct.Bottom));//Основание домика
            }

            private void setNest(BuildingControl p, char nest, char heater, bool second)
            {
                if (second) {
                    p.nest2 = (nest == '1');
                } else {
                    p.nest = (nest == '1');
                }

                int htr = 2;
                if (heater == '0') {
                    htr = 0;
                }
                if (heater == '1') {
                    htr = 1;
                }

                if (second) {
                    p.heater2 = htr;
                } else {
                    p.heater = htr;
                }
            }

            private void setVigul(BuildingControl p, char delim)
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
        }

        private DrawTier _t1 = null;
        private DrawTier _t2 = null;
        private int id = 0;

        public FarmDrawer()
        {
            InitializeComponent();
        }

        public void SetFarm(int id, DrawTier t1, DrawTier t2)
        {
            this._t1 = t1;
            this._t2 = t2;
            this.id = id;
            bc1.Visible = bc2.Visible = false;
            Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //			e.Graphics.Clear(Color.White);
            e.Graphics.Clear(SystemColors.Control);
            if (_t1 != null) {
                _manual = true;
                drawHouse(e.Graphics, e.ClipRectangle);
                _manual = false;
            }
        }

        /// <summary>
        /// Пишет текст в указанной области
        /// </summary>
        /// <param name="text">Какой текст писать</param>
        /// <param name="r">В какой прямоугольник вписывать</param>
        /// <param name="sz">Размер шрифта</param>
        /// <param name="g">На какой графике рисовать</param>
        public static void DrawText(String text, Rectangle r, int sz, Graphics g, bool bottom)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = bottom ? StringAlignment.Far : StringAlignment.Center;
            //			g.DrawString(text, new Font("Arial", sz), Brushes.Black,
            //				new RectangleF(r.X, r.Y, r.Width, r.Height), sf);
            g.DrawString(text, new Font(SystemFonts.DefaultFont.Name, sz), Brushes.Black, new RectangleF(r.X, r.Y, r.Width, r.Height), sf);
        }

        private void drawHouse(Graphics g, Rectangle rect)
        {
            Rectangle r1 = rect;
            Rectangle[] r2 = new Rectangle[] { rect, rect };
            r1.Height = (int)Math.Round(rect.Height * 0.15);
            r2[0].Height = (int)Math.Round(rect.Height * 0.85);
            r2[0].Width -= 1;//чтобы видно было правую стенку
            r2[0].Offset(0, r1.Height);
            r2[1] = r2[0];
            if (r2 != null) {
                r2[0].Height = r2[0].Height / 2;
                r2[1].Height = r2[0].Height;
                r2[1].Offset(0, r2[0].Height - 1);
            }

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.FillPolygon(Brushes.White, new Point[] {
                new Point(r1.X, r1.Bottom),
                new Point(r1.Right/2, r1.Y),
                new Point(r1.Right, r1.Bottom)
            });
            g.DrawLines(Pens.Black, new Point[] {
                new Point(r1.X, r1.Bottom),
                new Point(r1.Right/2, r1.Y),
                new Point(r1.Right, r1.Bottom)
            });
            g.SmoothingMode = SmoothingMode.None;

            DrawText(id.ToString(), r1, 12, g, true);
            g.DrawRectangle(new Pen(Color.Black, 1), r2[0]);

            bc1.Top = r2[0].Top + pictureBox1.Top;// +1;
            bc1.Height = r2[0].Height + 1;
            _t1.Draw(g, r2[0], bc1);
            bc1.Visible = true;
            if (_t2 != null) {
                bc2.Top = r2[1].Top + pictureBox1.Top;// +2;
                bc2.Height = r2[1].Height + 1;
                r2[1].Offset(0, 1);
                r2[1].Height -= 1;
                g.DrawRectangle(new Pen(Color.Black, 1), r2[1]);
                _t2.Draw(g, r2[1], bc2);
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

        private void bc_ValueChanged(object sender, BuildingControl.BCEvent e)
        {
            if (_manual) {
                return;
            }

            e.farm = id;
            e.tier = (sender == bc1 ? _t1.Building.ID : _t2.Building.ID);
            if (ValueChanged != null) {
                ValueChanged(this, e);
            }
        }

    }
}
