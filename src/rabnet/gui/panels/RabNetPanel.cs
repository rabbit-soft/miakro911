using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using log4net;
using rabnet.filters;
using rabnet.components;

namespace rabnet.panels
{
    public class RabNetPanel : UserControl
    {
        protected Filters _runF;
        protected RabStatusBar _rsb = null;
        protected FilterPanel filterPanel = null;
        protected ListViewColumnSorter _colSort = null;
        protected ListViewColumnSorter _colSort2 = null;

        protected ILog _logger;

        /// <summary>
        /// Делегат определяющий обработчик, когда жмут на кнопку Excel.
        /// Если наследники присвоят обработчик, то кнопка Excel покажется.
        /// </summary>
        public RSBEventHandler MakeExcel = null;

        public RabNetPanel()
        {
            InitializeComponent();
            _logger = LogManager.GetLogger(this.GetType());
        }

        public RabNetPanel(RabStatusBar sb, FilterPanel fp)
            : this()
        {
            _rsb = sb;
            this.filterPanel = fp;
        }

        public virtual void close()
        {
            if (filterPanel != null) {
                filterPanel.close();
            }
        }

        public RabNetPanel(RabStatusBar sb) : this(sb, null) { }

        protected virtual void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RabNetPanel
            // 
            this.DoubleBuffered = true;
            this.Name = "RabNetPanel";
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Назначает событиям (itemGet, prepareGet) которые пренадлежат компоненту rabStatusBar, обработчики из активной, в данный момент, панели.
        /// </summary>
        public virtual void activate()
        {
            _rsb.FilterPanel = filterPanel;
            Size = Parent.Size;
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            _rsb.ItemGet += new RSBItemEventHandler(onItem);
            _rsb.PrepareGet += new RSBPrepareHandler(prepareGet);
            _rsb.OnFinishUpdate += new RSBEventHandler(onFinishUpdate);
            _rsb.ExcelButtonClick = MakeExcel;
            _rsb.Run();
        }

        /// <summary>
        /// Отвязывает, Фильтр, prepareGet, itemGet
        /// </summary>
        public virtual void deactivate()
        {
            _rsb.Stop();
            if (_rsb.FilterPanel != null) {
                _rsb.FilterPanel.Visible = false;
                _rsb.FilterPanel = null;
            }
            _rsb.PrepareGet -= prepareGet;
            _rsb.ItemGet -= onItem;
            _rsb.OnFinishUpdate -= onFinishUpdate;
            _rsb.ExcelButtonClick = null;
        }

        /// <summary>
        /// Выполняет виртуальный метод  "onPrepare"  текущей активной панели
        /// </summary>
        /// <returns>Возвращает результат запроса.(Фактически представляет собой MySqlDataReader)</returns>
        private IDataGetter prepareGet()
        {
            Filters f = null;
            if (filterPanel != null) {
                f = filterPanel.getFilters();
            }
            return onPrepare(f);
        }

        //private void onItem_Invoker(IData data)
        //{
        //    if (this.InvokeRequired) {
        //        RSBItemEventHandler d = new RSBItemEventHandler(onItem_Invoker);
        //        this.Invoke(d, new object[] { data });
        //    } else {
        //        onItem(data);
        //    }
        //}

        //private void onFinishUpdate_Invoker()
        //{
        //    if (this.InvokeRequired) {
        //        RSBEventHandler d = new RSBEventHandler(onFinishUpdate_Invoker);
        //        this.Invoke(d);
        //    } else {
        //        onFinishUpdate();
        //    }
        //}

        /// <summary>
        /// Тело метода содержится в наследниках класса RabNetPanel
        /// Выполняет обработку одной строчки из результата обращения к БД
        /// </summary>
        /// <param name="data"></param>
        protected virtual void onItem(IData data) { }

        /// <summary>
        /// Тело метода содержится в наследниках класса RabNetPanel.
        /// Выполняется перед началом получения данных из Базы Данных
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        protected virtual IDataGetter onPrepare(Filters f)
        {
            _rsb.FilterOn = f.Count != 0;            
            _colSort.PrepareForUpdate();
            this.Enabled = false;
            return null;
        }

        protected virtual void onFinishUpdate()
        {
            _colSort.RestoreAfterUpdate();
            this.Enabled = true;
        }

        public virtual ContextMenuStrip getMenu()
        {
            return null;
        }

    }
}
