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
        delegate void progressCallBack2(int min,int max);
        public delegate IDataGetter RSBPrepareEventHandler(object sender, EventArgs e);
        public delegate void RSBEventHandler(object sender, RSBClickEvent e);
        public delegate void ExcelButtonClickDelegate();

        private ToolStripProgressBar pb = new ToolStripProgressBar();
        private ToolStripButton btn = new ToolStripButton();
        private ToolStripButton filt = new ToolStripButton();
        private ToolStripButton btExcel = new ToolStripButton();
        private List<ToolStripLabel> labels = new List<ToolStripLabel>();
        private int btnStatus=0;

        public event EventHandler StopClick;
        public event EventHandler RefreshClick;

        public class RSBClickEvent : EventArgs
        {
            public int type;
            public RSBClickEvent(int type):base()
            {
                this.type = type;
            }
        }
        
        public event RSBEventHandler StopRefreshClick;       
        public event RSBPrepareEventHandler PrepareGet;       
        private ExcelButtonClickDelegate excelButtonClick = null;

        /// <summary>
        /// Делегат нажатия на кнопку Excel, если 'null' то кнопка не видна. 
        /// Подробнее: RabNetPanel.MakeExcel
        /// </summary>
        public ExcelButtonClickDelegate ExcelButtonClick
        {
            get {return excelButtonClick; }
            set 
            {
                excelButtonClick = value;
                if (value == null)
                    btExcel.Visible = false;
                else btExcel.Visible = true;
            }
        }

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
        private UserControl fpan;
        public UserControl filterPanel
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
                    filt.Visible = false;              
            }
        }

        /// <summary>
        /// Конструктор статусБара
        /// </summary>
        public RabStatusBar()
        {
            InitializeComponent();
            
            RenderMode = ToolStripRenderMode.Professional;
            for (int i = 0; i < 5; i++)
                labels.Add(new ToolStripLabel());
            Items.Add(labels[0]);
            Items.Add(new ToolStripSeparator());
            Items.Add(pb);
            Items.Add(btn); btn.Image = imageList1.Images[1];
            Items.Add(filt); filt.Image = imageList1.Images[2]; filt.Visible = false;
            Items.Add(btExcel); btExcel.Image = imageList1.Images[3]; btExcel.Visible = false;
            Items.Add(new ToolStripSeparator());
            Items.Add(labels[1]);
            Items.Add(new ToolStripSeparator());
            Items.Add(labels[2]);
            Items.Add(new ToolStripSeparator());
            Items.Add(labels[3]);
            Items.Add(new ToolStripSeparator());
            Items.Add(labels[4]);
            btnStatus=0;
            btn.Click += new EventHandler(this.onBtnClick);
            filt.Click += new EventHandler(this.onFiltClick);
            btExcel.Click += new EventHandler(this.onExcelClick);
        }

        private void initialHints()
        {

            btn.ToolTipText = "Обновить список";
            filt.ToolTipText = "Показать фильт";
            btExcel.ToolTipText = "Сохранить содержимое списка в Excel";
        }

        public void SetText(int item,String text)
        {
            SetText(item, text, false);
        }
        public void SetText(int item, String text,bool error)
        {
            labels[item].Text = text;
            if(error)
                labels[item].ForeColor = Color.Crimson;
        }

        public void Run()
        {
            if (btnStatus==0)
                btn.PerformClick();
        }
        public void initProgress(int min,int max)
        {
#if DEBUG
            try
            {
#endif
                if (this.InvokeRequired)
                {
                    progressCallBack2 d = new progressCallBack2(initProgress);
                    this.Invoke(d, new object[] { min, max });
                }
                else
                {
                    pb.Minimum = min;
                    pb.Maximum = max;
                    pb.Value = min;

                    btn.Image = imageList1.Images[0];
                    btnStatus = 1;
                }
#if DEBUG
            }
            catch (Exception) { }
#endif
        }
        public void InitProgress(int max)
        {
            initProgress(0, max);
        }
        public void InitProgress()
        {
            initProgress(0, 100);
        }
        public void Progress(int progress)
        {
            pb.Value = progress;
            pb.Invalidate();
        }
        public void EndProgress()
        {
            pb.Value = pb.Minimum;
            pb.Invalidate();
            btn.Image = imageList1.Images[1];
            btnStatus=0;
        }
        public void EmergencyStop()
        {
            btn.Image = imageList1.Images[1];
            btnStatus=0;
        }

        private void onItem(object sender, EventArgs e)
        {
            if (itemGet != null)
                itemGet(this, new RSBItemEvent(sender as IData));
        }

        public void FilterHide()
        {
            if (fpan != null)
            {
                Parent.Controls.Remove(fpan);
                fpan.Visible = false;
                filt.Checked = false;
                Run();
            }
        }

        public void FilterShow()
        {
            if (fpan != null)
            {
                Parent.Controls.Add(fpan);
                if (btnStatus != 0)
                    DataThread.Get().Stop();
                fpan.Left = filt.Bounds.Left;
                fpan.Top = Top - fpan.Height;
                fpan.Visible = true;
                filt.Checked = true;
                fpan.BringToFront();
            }
        }

        public void FilterSwitch()
        {
            filt.PerformClick();
        }

        #region clicks

        private void onBtnClick(object sender, EventArgs e)
        {
            RSBClickEvent ev = new RSBClickEvent(btnStatus);
            if (StopRefreshClick != null)
                StopRefreshClick(this, ev);
            if (btnStatus == 0)
            {
                if (RefreshClick != null)
                    RefreshClick(this, null);
                if (PrepareGet != null)
                {
                    DataThread.Get4run().Run(PrepareGet(this, null), this, this.onItem);
                }
            }
            else
            {
                if (StopClick != null)
                    StopClick(this, null);
            }
        }

        private void onFiltClick(object sender, EventArgs e)
        {
            if (fpan!=null)
            {
                if (fpan.Visible)
                    FilterHide();
                else
                    FilterShow();
            }
        }

        private void onExcelClick(object sender, EventArgs e)
        {
            this.Parent.Enabled = false;
            ExcelButtonClick();
            this.Parent.Enabled = true;
        }

        #endregion clicks

    }
}
