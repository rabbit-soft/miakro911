using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace rabnet
{
    public partial class RabStatusBar : StatusStrip
    {
        private ToolStripProgressBar pb = new ToolStripProgressBar();
        private ToolStripButton btn = new ToolStripButton();
        private ToolStripButton filt = new ToolStripButton();
        private List<ToolStripLabel> labels = new List<ToolStripLabel>();
        private int btnStatus=0;
        public event EventHandler stopClick;
        public event EventHandler refreshClick;
        public class RSBClickEvent : EventArgs
        {
            public int type;
            public RSBClickEvent(int type):base()
            {
                this.type = type;
            }
        }
        public delegate void RSBEventHandler(object sender,RSBClickEvent e);
        public event RSBEventHandler stopRefreshClick;
        public delegate IDataGetter RSBPrepareEventHandler(object sender, EventArgs e);
        public event RSBPrepareEventHandler prepareGet;
        public class RSBItemEvent:EventArgs
        {
            public IData data;
            public RSBItemEvent(IData data)
            {
                this.data = data;
            }
        }
        public delegate void RSBItemEventHandler(object sender, RSBItemEvent e);
        public event RSBItemEventHandler itemGet;
        private Panel fpan;
        public Panel filterPanel
        {
            get
            {
                return fpan;
            }
            set
            {
                fpan = value;
                if (fpan != null)
                {
                    filt.Visible = true;
                    if (!DesignMode)
                        fpan.Visible = false;
                    fpan.BorderStyle = BorderStyle.FixedSingle;
                }
                else
                {
                    filt.Visible = false;
                }
            }
        }
        public RabStatusBar()
        {
            InitializeComponent();
            RenderMode = ToolStripRenderMode.Professional;
            for (int i=0;i<5;i++)
                labels.Add(new ToolStripLabel());
            Items.Add(labels[0]);
            Items.Add(new ToolStripSeparator());
            Items.Add(pb);
            Items.Add(btn);
            btn.Image = imageList1.Images[1];
            filt.Image = imageList1.Images[2];
            filt.Visible = false;
            Items.Add(filt);
            Items.Add(new ToolStripSeparator());
            Items.Add(labels[1]);
            Items.Add(new ToolStripSeparator());
            Items.Add(labels[2]);
            Items.Add(new ToolStripSeparator());
            Items.Add(labels[3]);
            Items.Add(new ToolStripSeparator());
            Items.Add(labels[4]);
            btnStatus=0;
            btn.Click += new EventHandler(this.OnBtnClick);
            filt.Click += new EventHandler(this.OnFiltClick);
        }

        public void setText(int item,String text)
        {
            labels[item].Text=text;
        }
        public void run()
        {
            if (btnStatus==0)
                btn.PerformClick();
        }
        public void initProgress(int min,int max)
        {
            pb.Minimum=min;
            pb.Maximum=max;
            pb.Value=min;
            btn.Image=imageList1.Images[0];
            btnStatus=1;
        }
        public void initProgress(int max)
        {
            initProgress(0, max);
        }
        public void initProgress()
        {
            initProgress(0, 100);
        }
        public void progress(int progress)
        {
            pb.Value = progress;
            pb.Invalidate();
        }
        public void endProgress()
        {
            pb.Value = pb.Minimum;
            pb.Invalidate();
            btn.Image=imageList1.Images[1];
            btnStatus=0;
        }
        public void emergencyStop()
        {
            btn.Image=imageList1.Images[1];
            btnStatus=0;
        }
        private void OnBtnClick(object sender,EventArgs e)
        {
            RSBClickEvent ev = new RSBClickEvent(btnStatus);
            if (stopRefreshClick != null)
                stopRefreshClick(this, ev);
            if (btnStatus == 0)
            {
                if (refreshClick != null)
                    refreshClick(this, null);
                if (prepareGet!=null)
                {
                    DataThread.get4run().Run(prepareGet(this, null), this, this.OnItem);
                }
            }
            else
            {
                if (stopClick != null)
                    stopClick(this, null);
            }
        }
        private void OnItem(object sender, EventArgs e)
        {
            if (itemGet != null)
                itemGet(this, new RSBItemEvent(sender as IData));
        }
        public void filterHide()
        {
            if (fpan != null)
            {
                fpan.Visible = false;
                filt.Checked = false;
                run();
            }
        }
        public void filterShow()
        {
            if (fpan != null)
            {
                if (btnStatus != 0)
                    DataThread.get().stop();
                fpan.Left = filt.Bounds.Left;
                fpan.Top = Top - fpan.Height;
                fpan.Visible = true;
                filt.Checked = true;
            }
        }
        public void filterSwitch()
        {
            filt.PerformClick();
        }
        private void OnFiltClick(object sender, EventArgs e)
        {
            if (fpan!=null)
            {
                if (fpan.Visible)
                    filterHide();
                else
                    filterShow();
            }
        }

    }
}
