using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class FarmDrawer : UserControl
    {
        public class DrawTier
        {
            private String type;
            private String delims;
            private String nests;
            private String heaters;
            private string[] rbs;
            private Graphics g;
            private Rectangle r;
            private bool repair;
            private Font f = new Font("Arial", 8);
            public DrawTier(String type,String delims,String nests,String heaters,String[] rabbits,bool repair)
            {
                this.repair=repair;
                this.type = type;
                this.delims = delims;
                this.nests = nests;
                rbs=rabbits;
                this.heaters = heaters;
            }
            public void draw(Graphics g, Rectangle r)
            {
                this.g=g;
                this.r=r;
                switch (type)
                {
                    case "female":
                        drawPart(0,1,"",rbs[0],nests[0],heaters[0],null);
                        break;
                    case "dfemale":
                        drawPart(0,0.5,"а",rbs[0],nests[0],heaters[0],null);
                        drawPart(0.5,0.5,"б",rbs[1],nests[1],heaters[1],null);
                        break;
                    case "complex":
                        drawPart(0, 0.5, "а", rbs[0], nests[0], heaters[0], null);
                        drawPart(0.5, 0.25, "б", rbs[1], 'N', 'N', null);
                        drawPart(0.75, 0.25, "в", rbs[2], 'N', 'N', null);
                        break;
                    case "jurta":
                        bool fst = delims[0] == '1';
                        if (fst)
                        {
                            drawPart(0, 0.25, "а", rbs[0], 'N', 'N', null);
                            drawPart(0.25, 0.75, "б", rbs[1], nests[0], heaters[0], new double[]{0.62});
                        }
                        else
                        {
                            drawPart(0, 0.62, "а", rbs[0], nests[0], heaters[0], new double[] { 0.25 });
                            drawPart(0.62, 0.38, "б", rbs[1], 'N', 'N', null);
                        }
                        break;
                    case "quarta":
                        drawQuarta();
                        break;
                    case "vertep":
                        drawPart(0, 0.5, "а", rbs[0], 'N', 'N', null);
                        drawPart(0.5, 0.5, "б", rbs[1], 'N', 'N', null);
                        break;
                    case "barin":
                        if (delims[0] == '1')
                        {
                            drawPart(0, 0.5, "а", rbs[0], 'N', 'N', null);
                            drawPart(0.5, 0.5, "б", rbs[1], 'N', 'N', null);
                        }
                        else
                            drawPart(0, 1, "аб", rbs[0], 'N', 'N', new double[]{ 0.5});
                        break;
                    case "cabin":
                        drawPart(0, 0.65, "а", rbs[0], nests[0], heaters[0], null);
                        drawPart(0.65, 0.35, "б", rbs[1], 'N', 'N', null);
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
                while (i < 3)
                {
                    if (delims[i] == '1')
                    {
                        drawPart(start, x, nm, rbs[i], 'N', 'N', dl.ToArray());
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
                drawPart(start, x, nm, rbs[i], 'N', 'N', dl.ToArray());
            }
            public void drawPart(double start,double sz,String area,String rabbit,Char nest,Char heater,double[] fd)
            {
                Rectangle rct = r;
                rct.Width = (int)Math.Round(r.Width * sz);
                rct.Offset((int)Math.Round(r.Width * start), 0);
                if (repair)
                    g.FillRectangle(Brushes.Silver,rct);
                else
                    g.DrawRectangle(Pens.Black,rct);
                if (fd != null && fd.Length>0)
                    for (int i = 0; i < fd.Length; i++)
                    {
                        int xpos = (int)Math.Round(r.Width * fd[i]);
                        g.DrawLine(Pens.Lavender,new Point(xpos,rct.Top),new Point(xpos,rct.Bottom));
                    }
                g.DrawString(area, f, Brushes.Black, rct.X, rct.Y);
                FarmDrawer.drawtext(rabbit, rct, 8, g,false);
                Rectangle nrct=rct;
                nrct.Height = (int)Math.Round(rct.Height * 0.2);
                nrct.Offset(0, (int)Math.Round(rct.Height*0.8));
                nrct.Width = rct.Width / 2;
                Rectangle hrct=nrct;
                hrct.Offset(rct.Width / 2, 0);
                if (nest!='N')
                {
                    g.FillRectangle(nest=='1'?Brushes.Gold:Brushes.Silver,nrct);
                    FarmDrawer.drawtext("гнездо"+(nest=='1'?"":":нет"),nrct,10,g,false);
                }
                if (heater !='N')
                {
                    Brush b = Brushes.Silver;
                    String stat = "Нет";
                    if (heater == '1')
                    {
                        b = Brushes.Blue;
                        stat = "Выкл";
                    }
                    if (heater == '2')
                    {
                        b = Brushes.Red;
                        stat = "Вкл";
                    }
                    g.FillRectangle(b, hrct);
                    FarmDrawer.drawtext("грелка:"+stat,hrct,10,g,false);
                }

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
            this.t1=t1;
            this.t2 = t2;
            this.id = id;
            Refresh();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (t1!=null)
            {
                drawHouse(e.Graphics,e.ClipRectangle);
            }
        }

        public static void drawtext(String text,Rectangle r,int sz,Graphics g,bool bottom)
        {
            StringFormat sf=new StringFormat();
            sf.Alignment=StringAlignment.Center;
            sf.LineAlignment = bottom?StringAlignment.Far:StringAlignment.Center;
            g.DrawString(text,new Font("Arial",sz),Brushes.Black,
                new RectangleF(r.X,r.Y,r.Width,r.Height),sf);
        }

        private void drawHouse(Graphics g,Rectangle rect)
        {
            Rectangle r1=rect;
            Rectangle[] r2=new Rectangle[]{rect,rect};
            r1.Height=(int)Math.Round(rect.Height*0.15);
            r2[0].Height=(int)Math.Round(rect.Height*0.85);
            r2[0].Offset(0,r1.Height);
            r2[1]=r2[0];
            if (r2!=null)
            {
                r2[0].Height=r2[0].Height/2;
                r2[1].Height=r2[0].Height;
                r2[1].Offset(0,r2[0].Height);
            }
            g.DrawLines(new Pen(Color.Black), new Point[] {
                new Point(r1.X,r1.Bottom),
                new Point(r1.Right/2,r1.Y),
                new Point(r1.Right,r1.Bottom)
            });
            drawtext(id.ToString(), r1, 12, g,true);
            g.DrawRectangle(new Pen(Color.Black, 2), r2[0]);
            t1.draw(g, r2[0]);
            if (t2 != null)
            {
                g.DrawRectangle(new Pen(Color.Black, 2), r2[1]);
                t2.draw(g, r2[1]);
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
    }
}
