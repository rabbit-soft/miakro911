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
        public RabNetPanel(RabStatusBar sb):this(sb,null){}
        protected virtual void InitializeComponent() { }
        public virtual void activate()
        {
            rsb.filterPanel = fp;
            Size = Parent.Size;
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            rsb.itemGet+=new RabStatusBar.RSBItemEventHandler(this.itemGet);
            rsb.prepareGet += new RabStatusBar.RSBPrepareEventHandler(this.prepareGet);
            rsb.run();
        }
        public virtual void deactivate()
        {
            rsb.filterPanel = null;
            rsb.prepareGet -= this.prepareGet;
            rsb.itemGet -= this.itemGet;
        }
        private IDataGetter prepareGet(object sender, EventArgs e)
        {
            Filters f = null;
            if (fp!=null)
                f = fp.getFilters();
            return onPrepare(f);
        }
        private void itemGet(object sender, RabStatusBar.RSBItemEvent e)
        {
            onItem(e.data);
        }

        protected virtual void onItem(IData data)
        {
        }

        protected virtual IDataGetter onPrepare(Filters f)
        {
            return null;
        }

    }
}
