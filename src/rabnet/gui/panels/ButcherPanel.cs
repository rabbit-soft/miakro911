using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using rabnet.components;

namespace rabnet
{
    public partial class ButcherPanel : RabNetPanel
    {
        const int AGE_FIELD = 1;
        const int NFIELD = 3;

        public ButcherPanel() : base() { }
        public ButcherPanel(RabStatusBar sb): base(sb)
        {
            _colSort = new ListViewColumnSorter(lvButcherDates, new int[] {  }, Options.OPT_ID.BUTCHER_DATE_LIST);
            _colSort2 = new ListViewColumnSorter(lvVictims, new int[] { AGE_FIELD, NFIELD }, Options.OPT_ID.VICTIMS_LIST);
            lvButcherDates.ListViewItemSorter = null;
            MakeExcel = new RabStatusBar.ExcelButtonClickDelegate(this.makeExcel);
        }

        private void drawMealList()
        {
            lvMeat.Clear();
            switch(Engine.opt().getIntOption(Options.OPT_ID.BUCHER_TYPE))
            {
                case 0:
                    lvMeat.Columns.Add("Продукция");
                    lvMeat.Columns.Add("Количество");
                    lvMeat.Columns.Add("Единицы изм.");
                    lvMeat.Columns.Add("Пользователь");
                    break;
                case 1:
                    lvMeat.Columns.Add("Продукция");
                    lvMeat.Columns.Add("Продано (шт)");
                    lvMeat.Columns.Add("На сумму");
                    lvMeat.Columns.Add("Вес");
                    break;
            }
            foreach(ColumnHeader ch in lvMeat.Columns )
                ch.Width =120;          
        }

        protected override IDataGetter onPrepare(Filters f)
        {
            _colSort.Prepare();
            if (f == null) f = new Filters();
            f.Add("type", Engine.opt().getOption(Options.OPT_ID.BUCHER_TYPE));
            IDataGetter dg = DataThread.Db().getButcherDates(f);
            _rsb.SetText(1, dg.getCount().ToString() + " дат забоя");
            _rsb.SetText(2, dg.getCount2().ToString() + " забито");
            return dg;
        }

        protected override void onItem(IData data)
        {
            if (data == null)
            {
                _colSort.Restore();
                return;
            }
            if (data == null) return;
            ButcherDate bd = (data as ButcherDate);
            ListViewItem lvi = lvButcherDates.Items.Add(bd.Date.ToShortDateString());
            lvi.SubItems.Add(bd.Victims.ToString());
            lvi.SubItems.Add(bd.Products.ToString());
            //colSort.SemiReady();
        }

        public override ContextMenuStrip getMenu()
        {
            return miMeal;
        }

        private void lvButcherDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvButcherDates.SelectedItems.Count == 0) return;
            DateTime date = DateTime.Parse(lvButcherDates.SelectedItems[0].SubItems[0].Text);
            AdultRabbit[] rabbits = Engine.get().db().GetVictims(date);
            lvVictims.Items.Clear();
            foreach (AdultRabbit rab in rabbits)
            {
                ListViewItem lvi = lvVictims.Items.Add(rab.NameFull);
                lvi.SubItems.Add(rab.Age.ToString());
                lvi.SubItems.Add(rab.BreedName);
                lvi.SubItems.Add(rab.FGroup());
                lvi.SubItems.Add(rab.AddressSmall);
            }

            lvMeat.Items.Clear();
            /*List<sMeat> meats = Engine.get().db().getMeats(date);
            foreach (sMeat m in meats)
            {
                ListViewItem lvi = lvMeat.Items.Add(m.ProductType);
                lvi.SubItems.Add(m.Amount.ToString()+" "+m.Units);
                lvi.SubItems.Add(m.User);
            }*/
            if (Engine.opt().getIntOption(Options.OPT_ID.BUCHER_TYPE) == 0)//1- scale
            {
                List<sMeat> mts = Engine.db().getMeats(date);
                foreach (sMeat mt in mts)
                {
                    ListViewItem lvi = lvMeat.Items.Add(mt.ProductType);
                    lvi.SubItems.Add(mt.Amount.ToString());
                    lvi.SubItems.Add(mt.Units);
                    lvi.SubItems.Add(mt.User);
                }
            }
            else
            {
                List<ScalePLUSummary> summarys = Engine.get().db().getPluSummarys(date);
                foreach (ScalePLUSummary sm in summarys)
                {
                    ListViewItem lvi = lvMeat.Items.Add(String.Format("[{0:d}] {1:s}", sm.ProdId, sm.ProdName));
                    lvi.SubItems.Add(sm.TotalSell.ToString());
                    lvi.SubItems.Add(sm.TotalSumm.ToString());
                    lvi.SubItems.Add(sm.TotalWeight.ToString());
                }
            }
        }

        private void makeExcel()
        {
#if !DEMO
            ExcelMaker.MakeExcelFromLV(lvMeat, "Продукция");
#endif
        }

        private void ButcherPanel_Load(object sender, EventArgs e)
        {
            drawMealList();
        }

        private void miDelete_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (lvMeat.SelectedItems.Count == 0) return;
            if (!CAS.CasLP16.Instance.Connected)
            {
                MessageBox.Show("Соединение с весами не установлено");
                return;
            }
            CAS.ScaleForm.StopMonitoring(false);
            int pid = (lvMeat.SelectedItems[0].Tag as ScalePLUSummary).ProdId;
            int sid = (lvMeat.SelectedItems[0].Tag as ScalePLUSummary).Id;
            CAS.CasLP16.Instance.CleadPLUSummary(pid);
            CAS.CasLP16.Instance.LoadPLUs();
            DateTime lc = CAS.CasLP16.Instance.GetPLUbyID(pid).LastClear;
            Engine.db().deletePLUsummary(sid,lc);
#endif
        }

        private void lvVictims_SelectedIndexChanged(object sender, EventArgs e)
        {
            makeSelectedCount();
        }

        private void makeSelectedCount()
        {
            int rows = lvVictims.SelectedItems.Count;
            int cnt = 0;
            foreach (ListViewItem li in lvVictims.SelectedItems)
                cnt += selCount(li.Index);
            _rsb.SetText(3, String.Format("Выбрано {0:d} строк - {1:d} кроликов", rows, cnt));
        }

        private int selCount(int index)
        {
            if (index < 0) return 0;
            String s = lvVictims.Items[index].SubItems[NFIELD].Text;
            int c = 1;
            if (s[0] == '+') c += int.Parse(s.Substring(1));
            if (s[0] == '[') c = int.Parse(s.Substring(1, s.Length - 2));
            return c;
        }
    }
}
