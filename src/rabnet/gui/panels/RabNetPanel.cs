using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace rabnet
{
    public class RabNetPanel:UserControl
    {
        protected RabStatusBar rsb=null;
        private FilterPanel fp=null;
        protected ListViewColumnSorter cs = null;
        public RabNetPanel()
        {
            InitializeComponent();
        }
        public RabNetPanel(RabStatusBar sb,FilterPanel fp):this()
        {
            rsb=sb;
            this.fp=fp;
        }
        public virtual void close()
        {
            if (fp != null)
                fp.close();
        }
        public RabNetPanel(RabStatusBar sb):this(sb,null){}
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
            rsb.filterPanel = fp;
            Size = Parent.Size;
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            rsb.itemGet+=new RabStatusBar.RSBItemEventHandler(this.itemGet);
            rsb.prepareGet += new RabStatusBar.RSBPrepareEventHandler(this.prepareGet);
            rsb.run();
        }
        /// <summary>
        /// Отвязывает, Фильтр, prepareGet, itemGet
        /// </summary>
        public virtual void deactivate()
        {
            rsb.filterPanel = null;
            rsb.prepareGet -= this.prepareGet;
            rsb.itemGet -= this.itemGet;
        }
        /// <summary>
        /// Выполняет виртуальный метод  "onPrepare"  текущей активной панели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>Возвращает результат запроса.(Фактически представляет собой MySqlDataReader)</returns>
        private IDataGetter prepareGet(object sender, EventArgs e)
        {
            Filters f = null;
            if (fp!=null)
                f = fp.getFilters();
            return onPrepare(f);
        }
        /// <summary>
        /// Выполняет виртуальный метод  "onItem"  текущей активной панели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void itemGet(object sender, RabStatusBar.RSBItemEvent e)
        {
            onItem(e.data);
        }
        /// <summary>
        /// Тело метода содержится в наследниках класса RabNetPanel
        /// Выполняет обработку одной строчки из результата обращения к БД
        /// </summary>
        /// <param name="data"></param>
        protected virtual void onItem(IData data)
        {
        }

        /// <summary>
        /// Тело метода содержится в наследниках класса RabNetPanel.
        /// Выполняется перед началом получения данных из Базы Данных
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        protected virtual IDataGetter onPrepare(Filters f)
        {
            return null;
        }

        public virtual ContextMenuStrip getMenu()
        {
            return null;
        }

    }
}
