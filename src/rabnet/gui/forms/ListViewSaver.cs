using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace rabnet
{
    class ListViewSaver
    {

        public static void save(Options.OPT_ID op,ListView lv)
        {
            if (op == Options.OPT_ID.NONE) return;
            String res="";
            for (int i=0;i<lv.Columns.Count;i++)
                res+=lv.Columns[i].Width.ToString()+",";
            if (lv.ListViewItemSorter!=null)
            {
                ListViewColumnSorter cs=lv.ListViewItemSorter as ListViewColumnSorter;
                int so=0;
                if (cs.Order==SortOrder.Ascending) so=1;
                if (cs.Order==SortOrder.Descending) so=2;
                res+=cs.SortColumn.ToString()+","+so.ToString();
            }
            res=res.Trim(',');
            Engine.opt().setOption(op,res);
        }
        /// <summary>
        /// Устанавливает ширину колонок компонента ListView
        /// </summary>
        /// <param name="op">enum opt_id</param>
        /// <param name="lv">ListViev</param>
        public static void load(Options.OPT_ID op,ListView lv)
        {
            if (op == Options.OPT_ID.NONE) return;
            String val = Engine.opt().getOption(op);
            if (val == "" || val =="0") return;
            String[] cls=val.Split(',');
            int i;
            for (i=0;i<lv.Columns.Count;i++)
                lv.Columns[i].Width = int.Parse(cls[i]); //не безопасно
            if (lv.ListViewItemSorter!=null && cls.Length>i)
            {
                ListViewColumnSorter cs=lv.ListViewItemSorter as ListViewColumnSorter;
                int sc = int.Parse(cls[i]);
                if (sc >= lv.Columns.Count)
                {
                    cs.Order = SortOrder.None;
                    cs.SortColumn = 0;
                    return;
                }
                cs.SortColumn = sc;
                int so=int.Parse(cls[i+1]);
                cs.Order=SortOrder.None;
                if (so==1) cs.Order=SortOrder.Ascending;
                if (so==2) cs.Order=SortOrder.Descending;
                lv.Sort();
            }
        }

        public static int saveItem(ListView lv)
        {
            if (lv.SelectedItems.Count!=1) return -1;
            return lv.SelectedItems[0].Index;
        }

        public static void loadItem(ListView lv,int item)
        {
            if (item==-1) return;
            if (lv.Items.Count <= item) return;
            lv.Items[item].Selected=true;
            lv.Items[item].EnsureVisible();
        }
    }
}
