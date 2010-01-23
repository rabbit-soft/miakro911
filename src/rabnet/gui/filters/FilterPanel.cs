using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;

namespace rabnet
{
    public class FilterPanel:UserControl
    {
        private RabStatusBar rsb = null;
        private String fname = "";
        private Options.OPT_ID opid=Options.OPT_ID.NONE;
        public FilterPanel(RabStatusBar rsb,String name,Options.OPT_ID opid):this()
        {
            initAgain();
            fname = name;
            this.opid=opid;
            this.rsb = rsb;
            clearFilters();
            loadFilters();
            String s = Engine.opt().getOption(opid);
            if (s!="")
                setFilters(Filters.makeFromString(s));
        }
        public FilterPanel()
        {
            InitializeComponent();
        }

        protected virtual void InitializeComponent(){}
        protected virtual void initAgain(){}

        public virtual void close()
        {
            Engine.opt().setOption(opid, getFilters().toString());
        }

        public void hide()
        {
            rsb.filterHide();
        }

        public virtual Filters getFilters() { return null; }
        public virtual void setFilters(Filters f){}
        public virtual void loadFilters() 
        {
            if (fs!=null)
                fs.Items.Clear();
            fs.Items.Add("Очистить");
            foreach (String s in Engine.db().getFilterNames(fname))
                fs.Items.Add(s);
            fs.SelectedIndex = -1;
            fs.Text = "";
        }
        public virtual void clearFilters(){
        }

        private ComboBox fs;
        public ComboBox FilterCombo
        {
            get{return fs;}
            set { fs = value; fs.SelectedIndexChanged += new EventHandler(this.filterSelect); }
        }
        private Button sBtn;
        public Button SaveButton
        {
            get { return sBtn; }
            set { sBtn = value; sBtn.Click += new EventHandler(this.saveClick); }
        }
        private Button hBtn;
        public Button HideBtn
        {
            get { return hBtn; }
            set { hBtn = value; hBtn.Click += new EventHandler(this.hideClick); }
        }

        private void hideClick(Object sender,EventArgs e)
        {
            hide();
        }
        private void saveClick(Object sender, EventArgs e)
        {
            if (fs.SelectedIndex==0 || fs.Text=="")
                return;
            Engine.db().setFilter(fname, fs.Text, getFilters());
            loadFilters();
        }
        private void filterSelect(Object sender, EventArgs e)
        {
            if (fs.Text == "")
                return;
            if (fs.SelectedIndex == 0)
            {
                fs.SelectedIndex = -1;
                fs.Text = "";
                clearFilters();
            }
            else
                setFilters(Engine.db().getFilter(fname,fs.Text));
        }

    }
}
