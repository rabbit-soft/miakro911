using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using rabnet.filters;
using log4net;

namespace rabnet.components
{
    public delegate IDataGetter RSBPrepareHandler();
    public delegate void RSBEventHandler();
    public delegate void RSBItemEventHandler(IData data);

    public partial class RabStatusBar : StatusStrip
    {
        const int IMG_STOP = 0;
        const int IMG_REFRESH = 1;
        const int IMG_FILTER_OFF = 2;
        const int IMG_EXCEL = 3;
        const int IMG_FILTER_ON = 4;

        const int LABELS_COUNT = 5;
        //delegate void progressCallBack2(int min,int max);

        protected static readonly ILog _logger = LogManager.GetLogger(typeof(RabStatusBar));

        private ToolStripProgressBar pb = new ToolStripProgressBar();
        private ToolStripButton btRefreshStop = new ToolStripButton();
        private ToolStripButton btFilter = new ToolStripButton();
        private ToolStripButton btExcel = new ToolStripButton();
        private List<ToolStripLabel> _labels = new List<ToolStripLabel>();
        //private int btnStatus=0;
        private FilterPanel _filterPanel;
        private DataThread _dataThread;

        //public event RSBEventHandler StopClick;
        /// <summary>
        /// Необходим.
        /// Происходит перед началом получения данных
        /// </summary>
        public event RSBPrepareHandler PrepareGet;
        public event RSBEventHandler OnFinishUpdate;
        public event RSBItemEventHandler ItemGet;
        private RSBEventHandler _excelButtonClick = null;
        //private DTProgressHandler _progressInvoker = null;

        /// <summary>
        /// Конструктор статусБара
        /// </summary>
        public RabStatusBar()
        {
            InitializeComponent();

            RenderMode = ToolStripRenderMode.Professional;
            ///создаем Лэйблы для дальнейшего использования
            for (int i = 0; i < LABELS_COUNT; i++) {
                _labels.Add(new ToolStripLabel());
            }
            ///добавляем компоненты на статус бар
            Items.Add(_labels[0]);
            Items.Add(new ToolStripSeparator());
            ///progress Bar
            Items.Add(pb);
            pb.Step = 1;

            ///кнопка Обновить\Остановить
            Items.Add(btRefreshStop);
            btRefreshStop.Image = imageList1.Images[IMG_REFRESH];
            btRefreshStop.Tag = 0;
            btRefreshStop.ToolTipText = "Обновить список";
            ///кнопка фильтров
            Items.Add(btFilter);
            btFilter.Image = imageList1.Images[IMG_FILTER_OFF];
            btFilter.Visible = false;
            btFilter.ToolTipText = "Показать фильт";
            ///кнопка Excel
            Items.Add(btExcel);
            btExcel.Image = imageList1.Images[IMG_EXCEL];
            btExcel.Visible = false;
            btExcel.ToolTipText = "Сохранить содержимое списка в Excel";
            ///надписи
            Items.Add(new ToolStripSeparator());
            Items.Add(_labels[1]);
            Items.Add(new ToolStripSeparator());
            Items.Add(_labels[2]);
            Items.Add(new ToolStripSeparator());
            Items.Add(_labels[3]);
            Items.Add(new ToolStripSeparator());
            Items.Add(_labels[4]);

            btRefreshStop.Click += new EventHandler(this.btn_Click);
            btFilter.Click += new EventHandler(this.filt_Click);
            btExcel.Click += new EventHandler(this.excel_Click);
        }

        /// <summary>
        /// Делегат нажатия на кнопку Excel, если 'null' то кнопка не видна. 
        /// Подробнее: RabNetPanel.MakeExcel
        /// </summary>
        public RSBEventHandler ExcelButtonClick
        {
            get { return _excelButtonClick; }
            set
            {
                _excelButtonClick = value;
                if (value == null) {
                    btExcel.Visible = false;
                } else {
                    btExcel.Visible = true;
                }
            }
        }

        public FilterPanel FilterPanel
        {
            get { return _filterPanel; }
            set
            {
                _filterPanel = value;
                if (_filterPanel != null) {
                    btFilter.Visible = true;
                    if (!DesignMode) {
                        _filterPanel.Visible = false;
                    }
                    _filterPanel.BorderStyle = BorderStyle.FixedSingle;
                    _filterPanel.OnHide = new RSBEventHandler(filterHide);
                } else {
                    btFilter.Visible = false;
                }
            }
        }

        public bool Working
        {
            get { return _dataThread != null; }
        }

        public bool FilterOn
        {
            get { return btFilter.Image == imageList1.Images[IMG_FILTER_OFF]; }
            set { btFilter.Image = imageList1.Images[value ? IMG_FILTER_ON : IMG_FILTER_OFF]; }
        }

        public void Run()
        {
            if ((int)btRefreshStop.Tag == 0) {
                btRefreshStop.PerformClick();
            }
        }

        public void Stop()
        {
            if ((int)btRefreshStop.Tag == 1) {
                btRefreshStop.PerformClick();
            }
        }

        public void SetText(int item, String text)
        {
            SetText(item, text, false);
        }
        public void SetText(int item, String text, bool error)
        {
            _labels[item].Text = text;
            if (error) {
                _labels[item].ForeColor = Color.Crimson;
            }
        }

        #region progress
        private void initProgress(int max)
        {
            if (this.InvokeRequired) {
                DTProgressHandler d = new DTProgressHandler(initProgress);
                this.Invoke(d, new object[] { max });
            } else {
                while (!this.IsHandleCreated) {
                    System.Threading.Thread.Sleep(100);///todo не знаю хорошее ли это решение может быть DeadLock
                }
                pb.Minimum = 0;
                pb.Maximum = max;
                pb.Value = 0;

                btRefreshStop.Image = imageList1.Images[IMG_STOP];
                btRefreshStop.Tag = 1;
            }
        }

        private void progress()
        {
            pb.PerformStep();
            pb.Invalidate();
        }

        private void endProgress()
        {
            ///если загрузку останавливает пользователь, то прогресс бар застывает и не сбраcывается
            pb.Value = pb.Minimum;
            pb.Invalidate();
            btRefreshStop.Image = imageList1.Images[IMG_REFRESH];
            btRefreshStop.Tag = 0;
        }
        #endregion progress

        private void filterHide()
        {
            if (_filterPanel == null || !_filterPanel.Visible) {
                return;
            }

            Parent.Controls.Remove(_filterPanel);
            _filterPanel.Visible = false;
            btFilter.Checked = false;
            Run();
        }

        private void filterShow()
        {
            if (_filterPanel == null) {
                return;
            }

            Parent.Controls.Add(_filterPanel);
            if ((int)btRefreshStop.Tag != 0) {
                stopDataThread();
            }
            _filterPanel.Left = btFilter.Bounds.Left;
            _filterPanel.Top = Top - _filterPanel.Height;
            _filterPanel.Visible = true;
            btFilter.Checked = true;
            _filterPanel.BringToFront();
        }

        public void FilterSwitch()
        {
            btFilter.PerformClick();
        }

        #region clicks
        private void btn_Click(object sender, EventArgs e)
        {
            ///если кнопка имеет вид "Обновить"
            if ((int)btRefreshStop.Tag == 0) {
                if (PrepareGet != null) {
                    IDataGetter dg = PrepareGet();
                    startDataThread(dg);
                }
            } else {
                stopDataThread();
                _dataThread_OnFinish();
            }
        }

        private void filt_Click(object sender, EventArgs e)
        {
            if (_filterPanel == null || Working) {
                return;
            }

            if (_filterPanel.Visible) {
                filterHide();
            } else {
                filterShow();
            }
        }

        private void excel_Click(object sender, EventArgs e)
        {
            if (Working) {
                return;
            }

            this.Parent.Enabled = false;
            ExcelButtonClick();
            this.Parent.Enabled = true;
        }

        #endregion clicks
        private void startDataThread(IDataGetter dg)
        {
            if (_dataThread != null) {
                this.stopDataThread();
            }
            _dataThread = new DataThread();
            _dataThread.OnItems += new DTItemsHandler(_dataThread_onItems);
            _dataThread.OnFinish += new RSBEventHandler(_dataThread_OnFinish);
            _dataThread.InitMaxProgress += new DTProgressHandler(initProgress);
            //_dataThread.Progress +=new DTProgressHandler(progress);
            _dataThread.Run(dg);
        }

        protected void _dataThread_OnFinish()
        {
            if (this.InvokeRequired) {
                RSBEventHandler d = new RSBEventHandler(_dataThread_OnFinish);
                this.Invoke(d);
            } else {
                if (OnFinishUpdate != null) {
                    OnFinishUpdate();
                }
                endProgress();
                stopDataThread();
            }
        }

        protected void _dataThread_onItems(IDataGetter dataGetter)
        {
            if (this.InvokeRequired) {
                DTItemsHandler d = new DTItemsHandler(_dataThread_onItems);
                this.Invoke(d, new object[] { dataGetter });
            } else {
                IData it;
                while ((it = dataGetter.GetNextItem()) != null) {
                    if (this.ItemGet != null) {
                        this.ItemGet(it);
                    }
                    this.progress();
                }
            }
        }

        public void stopDataThread()
        {
            if (_dataThread == null) {
                return;
            }

            _dataThread.Stop();
            _dataThread = null;
        }
    }
}
