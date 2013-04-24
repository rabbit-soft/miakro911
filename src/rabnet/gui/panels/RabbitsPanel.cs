using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Runtime.InteropServices;
using rabnet.forms;
using rabnet.filters;
using rabnet.components;

namespace rabnet.panels
{
    public partial class RabbitsPanel : RabNetPanel
    {
        private bool manual = true;
        private int gentree=10;
        const int NFIELD = 7;
        const int STATUSFIELD = 5;
        const int SEXFIELD = 1;
        const int NAMEFIELD = 0;

        public RabbitsPanel(): base() { }
        public RabbitsPanel(RabStatusBar rsb):base(rsb,new RabbitsFilter())
        {
            _colSort = new ListViewColumnSorter(listView1, new int[] {2,8,9 },Options.OPT_ID.RAB_LIST);
            listView1.ListViewItemSorter = null;
			miGenetic.Enabled = GeneticsManagerSafe.GeneticsModuleTest();
            MakeExcel = new RSBEventHandler(this.makeExcel);
        }

        protected override IDa{FUCK}etter onPrepare(Filters f)
        {
            base.onPrepare(f);           
            gentree = Engine.opt().getIntOption(Options.OPT_ID.GEN_TREE)-1;
            Options op = Engine.opt();
            f[Filters.SHORT] = op.getOption(Options.OPT_ID.SHORT_NAMES);
            f[Filters.SHOW_BLD_TIERS] = op.getOption(Options.OPT_ID.SHOW_TIER_TYPE);
            f[Filters.SHOW_BLD_DESCR] = op.getOption(Options.OPT_ID.SHOW_TIER_SEC);
            f[Filters.DBL_SURNAME] = op.getOption(Options.OPT_ID.DBL_SURNAME);
            f[Filters.SHOW_OKROL_NUM] = op.getOption(Options.OPT_ID.SHOW_NUMBERS);
            f[Filters.MAKE_BRIDE] = op.getOption(Options.OPT_ID.MAKE_BRIDE);
            //flt["suc"] = op.getOption(Options.OPT_ID.COUNT_SUCKERS);
            f[Filters.MAKE_CANDIDATE] = op.getOption(Options.OPT_ID.MAKE_CANDIDATE);
            _runF = f;           
            IDa{FUCK}etter dg = Engine.db2().getRabbits(f);
            _rsb.SetText(1, dg.getCount().ToString() + " записей");
            _rsb.SetText(2, dg.getCount2().ToString() + " кроликов");
            return dg;
        }

        protected override void onItem(IData data)
        {
            AdultRabbit rab = (data as AdultRabbit);
            ListViewItem li = listView1.Items.Add(rab.NameFull);
            li.Tag = rab;
            li.SubItems.Add(rab.FSex());
            li.SubItems.Add(rab.Age.ToString());
            li.SubItems.Add(rab.BreedName);
            li.SubItems.Add(rab.FWeight());
            li.SubItems.Add(rab.FStatus(_runF.safeBool(Filters.SHORT), _runF.safeInt(Filters.MAKE_CANDIDATE), _runF.safeInt(Filters.MAKE_BRIDE)));
            li.SubItems.Add(rab.FFlags());
            li.SubItems.Add(rab.FGroup());
            li.SubItems.Add(rab.KidsAge == -1 ? "" : rab.KidsAge.ToString());
            li.SubItems.Add(rab.Rate.ToString());
            li.SubItems.Add(rab.FBon(_runF.safeBool(Filters.SHORT)));
            li.SubItems.Add(rab.FAddress(_runF.safeBool(Filters.SHOW_BLD_TIERS), _runF.safeBool(Filters.SHOW_BLD_DESCR)));
            li.SubItems.Add(rab.Notes);
        }

        private bool isGirl()
        {
            string sd = listView1.SelectedItems[0].SubItems[STATUSFIELD].Text;
            return (sd=="Девочка" || sd=="Дев");
        }

        private bool isBride()
        {
            string sd = listView1.SelectedItems[0].SubItems[STATUSFIELD].Text;
            return (sd == "Невеста" || sd == "Нвс");
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!manual || MainForm.MustClose) return;

            MainForm.StillWorking();
            makeSelectedCount();
            if (listView1.SelectedItems.Count != 1) return;
            
            RabTreeData dt = Engine.db().rabbitGenTree((int)listView1.SelectedItems[0].{FUCK});
            if (dt != null && dt.Name != null)
            {
                TreeNode tn = tvGens.InsertNode(dt, true);
                tn.ForeColor = Color.Blue;
            }
            else
            {
                MessageBox.Show(@"Не возможно найти информацию по данной записи.
Возможно данного кролика списал другой сетевой пользователь программы.
Во избежании проблем, придется обновить Лист поголовья", "Не могу найти запись");
                _rsb.Run();
            }
        }

        /// <summary>
        /// Устанавливает набор возможных действий
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="multi"></param>
        /// <param name="kids"></param>
        private void setMenu(int sex,int multi,bool kids)
        {
            ///miIncome.Visible alvays true
            miPassport.Visible = miBon.Visible =  
                proholostMenuItem.Visible = miCountKids.Visible = okrolMenuItem.Visible = miFucks.Visible = miLust.Visible = 
                miBoysOut.Visible = miYoungersOut.Visible = misFemale.Visible=
                miKill.Visible =
                svidMenuItem.Visible = miRealize.Visible = plemMenuItem.Visible = miPlanReplace.Visible = misPrint.Visible =
                miPlaceExchange.Visible =miReplace.Visible =  misExtra.Visible = miExport.Visible=
                miGenetic.Visible = false;

            if (sex < 0) return;
            misPrint.Visible =             
                plemMenuItem.Visible = 
                miKill.Visible = 
                miRealize.Visible =
                miReplace.Visible = 
                misExtra.Visible =
                miExport.Visible =
                miGenetic.Visible = true;

            miCountKids.Visible = miYoungersOut.Visible = misFemale.Visible = kids;

            if (multi == 1)
            {
                miBon.Visible = true;
                if (sex != 3)
                {
                    miPassport.Visible = true;
                }
                if (sex == 2 )
                {
                    misFemale.Visible = true;
                    miFucks.Text = isBride() ? "Случка" : "Вязка";
                    miLust.Visible = !isGirl();
                    miFucks.Visible = !isGirl() && GroupCount() == 1;
                }
                if (sex == 1 || sex == 2 || sex == 4)
                {
                    svidMenuItem.Visible = true;
                }
            }
            else if (multi == 2)
                miPlaceExchange.Visible = true;
                                  
            if (sex == 4)
            {
                misFemale.Visible = proholostMenuItem.Visible = okrolMenuItem.Visible = true;
            }
            if (multi > 1 || (multi==1 && listView1.SelectedItems[0].SubItems[NFIELD].Text[0]=='['))
                miPlanReplace.Visible=true;
        }

        public override ContextMenuStrip getMenu()
        {
            setMenu(-1,0,false);
            return actMenu;
        }

        private void miPassport_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            try
            {
                RabbitInfo ri = new RabbitInfo((int)listView1.SelectedItems[0].{FUCK});
                ri.Working += new WorkingHandler(MainForm.StillWorking);
                if (ri.ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch(Exception exc)
            {
                MessageBox.Show(exc.Message);
                _rsb.Run();
            }
        }

        void ri_Working()
        {
            throw new NotImplementedException();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            listView1_SelectedIndexChanged(null, null);
            actMenu_Opening(null, null);
            miPassport.PerformClick();
        }  

        private int GroupCount()
        {
            String s = listView1.SelectedItems[0].SubItems[NFIELD].Text;
            if (s[0] == '[') return int.Parse(s.Substring(1, s.Length - 2));
            return 1;
        }

        private int selCount(int index)
        {
            if (index < 0) return 0;
            String s = listView1.Items[index].SubItems[NFIELD].Text;
            int c = 1;
            if (s[0] == '+')
            {
                int t=0;
                if (s.Contains("("))
                {
                    int.TryParse(s.Split(' ')[1], out t);                    
                }
                else                
                    int.TryParse(s.TrimStart('+'),out t);                
                c += t;
            }
            if (s[0] == '[') 
                int.TryParse(s.Substring(1, s.Length - 2), out c); //c = int.Parse(s.Substring(1, s.Length - 2));
            return c;
        }

        private void makeSelectedCount()
        {
            int rows = listView1.SelectedItems.Count;
            int cnt = 0;
            foreach (ListViewItem li in listView1.SelectedItems)
                cnt += selCount(li.Index);
            _rsb.SetText(3, String.Format("Выбрано {0:d} строк - {1:d} кроликов",rows,cnt));
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            manual = false;
        }

        private void listView1_MouseUp(object sender, MouseEventArgs e)
        {
            manual = true;
            listView1_SelectedIndexChanged(null, null);
        }

        //private String getBon(String b) //todo проверить на схожесть с RabbitBon
        //{
        //    if (b.Length == 1)
        //    {
        //        switch (b)
        //        {
        //            case "1": return "III";
        //            case "2": return "II";
        //            case "3": return "I";
        //            case "4": return "Элита";
        //        }
        //        return "Нет";
        //    }
        //    string minbon="5";
        //    for (int i = 1; i<b.Length; i++)
        //        if (b[i] < minbon[0])
        //            minbon = "" + b[i];
        //    return getBon(minbon);
        //}

        private void actMenu_Opening(object sender, CancelEventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                setMenu(-1, 0, false);
                return;
            }
            string sx = "";
            for (int i = 0; i < listView1.SelectedItems.Count && sx.Length < 2; i++)
            {
                String s = listView1.SelectedItems[i].SubItems[SEXFIELD].Text;
                if (s[0] == 'C'/*rus*/)
                    s = "S";
                if (!sx.Contains(s))
                    sx += s;
            }
            int isx = 3;
            if (sx == "?") isx = 0;
            if (sx == "м") isx = 1;
            if (sx == "ж") isx = 2;
            if (sx == "S") isx = 4;
            bool kids = false;
            if (listView1.SelectedItems.Count == 1 && listView1.SelectedItems[0].SubItems[NFIELD].Text[0] == '+')
                kids = true;
            setMenu(isx, listView1.SelectedItems.Count, kids);
        }

        private void tsmiIDshow_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;

            MessageBox.Show("r_id = "+listView1.SelectedItems[0].{FUCK}.ToString());
        }

        private void makeExcel()
        {         
#if !DEMO
            ExcelMaker.MakeExcelFromLV(listView1, "Поголовье");
#endif
        }

        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt || e.Shift || e.Control|| !char.IsLetter((char)e.KeyValue)) return;

            //KeysConverter ks = new KeysConverter();
            //CultureInfo ci = CultureInfo.CurrentUICulture;
            //string letter = cic.ConvertToString(e.KeyData);//todo при нажатии русской буквы переходить к строке
            //listView1.FindItemWithText("п").EnsureVisible(); 
        }

        #region menuItems

        private void miIncome_Click(object sender, EventArgs e)
        {

#if !DEBUG
            try
            {
#endif
            DialogResult res = (new IncomeForm()).ShowDialog();

            if (res == DialogResult.Ignore)
            {
#if !DEMO
                if (new EPasportForm(true).ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
#else
                DemoErr.DemoNoModuleMsg();
#endif
            }
            else if (res == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
#if !DEBUG
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
#endif

        }

        private void miReplace_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) return;

            try
            {
                ReplaceForm rpf = new ReplaceForm();
                foreach (ListViewItem li in listView1.SelectedItems)
                    rpf.AddRabbit((int)li.{FUCK});
                if (rpf.ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
        }

        private void miPlaceGhange_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 2) return;

            try
            {
                ReplaceForm rpf = new ReplaceForm();
                rpf.AddRabbit((int)listView1.SelectedItems[0].{FUCK});
                rpf.AddRabbit((int)listView1.SelectedItems[1].{FUCK});
                rpf.SetAction(ReplaceForm.Action.CHANGE);
                if (rpf.ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
        }

        private void miKill_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) return;

            try
            {
                if (Engine.db().getDeadReasons().Get().ColNames.Length == 0)
                    throw new Exception("Нет ни одной причины списания. Вы можете добавить их в  меню Вид->Причины списания");

                KillForm f = new KillForm();
                foreach (ListViewItem li in listView1.SelectedItems)
                    f.addRabbit((int)li.{FUCK});
                if (f.ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
        }

        private void miYoungersOut_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;

            try
            {
                if (PreReplaceYoungersForm.MakeChoice((int)listView1.SelectedItems[0].{FUCK}) == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _rsb.Run();
            }
        }

        private void svidMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (listView1.SelectedItems.Count != 1) return;

            XmlDocument[] docs = ReportHelperExt.GetRabbitPlem((int)listView1.SelectedItems[0].{FUCK});
            ReportViewForm rf = new ReportViewForm(myReportType.RABBIT, docs);
            rf.ShowDialog();
            if (rf.IsPrinted)
            {
                int num = Engine.opt().getIntOption(Options.OPT_ID.NEXT_SVID);
                Engine.opt().setOption(Options.OPT_ID.NEXT_SVID, num + 1);
            }
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void plemMenuItem_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (listView1.SelectedItems.Count < 1) return;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.AppendChild(doc.CreateElement("Rows"));
                int cnt = 0;
                string brd = "";
                foreach (ListViewItem li in listView1.SelectedItems)
                {
                    RabNetEngRabbit r = Engine.get().getRabbit((int)li.{FUCK});
                    cnt += r.Group;
                    if (brd == "")
                        brd = r.BreedName;
                    if (r.BreedName != brd)
                        brd = "none";
                    ReportHelperExt.rabToXml(r, null, doc);
                }
                XmlDocument doc2 = new XmlDocument();
                XmlElement rw = (XmlElement)doc2.AppendChild(doc2.CreateElement("Rows")).AppendChild(doc2.CreateElement("Row"));
                ReportHelper.Append(rw, doc2, "date", DateTime.Now.Date.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                ReportHelper.Append(rw, doc2, "breed", brd);
                ReportHelper.Append(rw, doc2, "count", cnt.ToString());
                //rw.AppendChild(doc2.CreateElement("date")).AppendChild(doc2.CreateTextNode(DateTime.Now.Date.ToShortDateString()+" "+DateTime.Now.ToLongTimeString()));
                //rw.AppendChild(doc2.CreateElement("breed")).AppendChild(doc2.CreateTextNode(brd));
                //rw.AppendChild(doc2.CreateElement("count")).AppendChild(doc2.CreateTextNode(cnt.ToString()));
                new ReportViewForm(myReportType.PRIDE, new XmlDocument[] { doc, doc2 }).ShowDialog();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void miRealize_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (listView1.SelectedItems.Count < 1) return;
            Filters f = new Filters();
            f["cnt"] = listView1.SelectedItems.Count.ToString();
            for (int i = 0; i < listView1.SelectedItems.Count; i++)
                f["r" + i.ToString()] = ((int)listView1.SelectedItems[i].{FUCK}).ToString();
            new ReportViewForm(myReportType.REALIZE, Engine.db().makeReport(myReportType.REALIZE, f)).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void miGenetic_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1) return;

            try
            {
#if PROTECTED
                if (!RabGRD.GRD.Instance.GetFlag(RabGRD.GRD_Base.FlagType.Genetics))                
                    throw new Exception("Текущая лицензия не распространяется на данный модуль");

#endif
                if (GeneticsManagerSafe.GeneticsModuleTest())
                {
                    for (int i = 0; i < listView1.SelectedItems.Count; i++)                    
                        GeneticsManagerSafe.AddNewGenetics(((int)listView1.SelectedItems[i].{FUCK}));                   
                }
                else                
                    throw new Exception("Не найден модуль 'gui_genetics.dll'!\nПроверьте правильность установки программы.");               
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
        }

        private void miPlanReplace_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (listView1.SelectedItems.Count < 1) return;

            XmlDocument doc = new XmlDocument();
            doc.AppendChild(doc.CreateElement("Rows"));
            foreach (ListViewItem li in listView1.SelectedItems)
            {
                XmlElement rw = (XmlElement)doc.DocumentElement.AppendChild(doc.CreateElement("Row"));
                ReportHelper.Append(rw, doc, "age", li.SubItems[2].Text); //rw.AppendChild(doc.CreateElement("age")).AppendChild(doc.CreateTextNode(li.SubItems[2].Text));
                String cn = li.SubItems[11].Text;
                if (cn.IndexOf('[') > -1)
                    cn = cn.Remove(cn.IndexOf('['));
                ReportHelper.Append(rw, doc, "address", cn); //rw.AppendChild(doc.CreateElement("address")).AppendChild(doc.CreateTextNode(cn));
                cn = li.SubItems[NFIELD].Text;
                if (cn[0] == '[')
                    cn = int.Parse(cn.Substring(1, cn.Length - 2)).ToString();
                if (cn == "-") cn = "1";
                ReportHelper.Append(rw, doc, "count", cn);//rw.AppendChild(doc.CreateElement("count")).AppendChild(doc.CreateTextNode(cn));
            }
            new ReportViewForm(myReportType.REPLACE, doc).ShowDialog();
#else
            DemoErr.DemoNoReportMsg();
#endif
        }

        private void miBon_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            try
            {
                int rid = (int)listView1.SelectedItems[0].{FUCK};
                if ((new BonForm(rid)).ShowDialog() == DialogResult.OK)
                    _rsb.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
        }

        private void proholostMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            try
            {
                int rid = (int)listView1.SelectedItems[0].{FUCK};
                if ((new Proholost(rid)).ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
        }

        private void countKidsMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;
            try
            {
                CountKids f = new CountKids((int)listView1.SelectedItems[0].{FUCK});
                if (f.ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
        }

        private void okrolMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;

            try
            {
                OkrolForm dlg = new OkrolForm((int)listView1.SelectedItems[0].{FUCK});
                if (dlg.ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
        }

        private void miFucks_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;

            try
            {
                MakeFuckForm dlg = new MakeFuckForm((int)listView1.SelectedItems[0].{FUCK});
                if (dlg.ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
        }

        private void miLust_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;

            int rid = (int)listView1.SelectedItems[0].{FUCK};
            Engine.db().SetRabbitVaccine(rid, Vaccine.V_ID_LUST);
            if(!MainForm.MustClose)
                _rsb.Run();
        }

        private void miBoysOut_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count != 1) return;

            try
            {
                ReplaceForm rpf = new ReplaceForm();
                rpf.AddRabbit((int)listView1.SelectedItems[0].{FUCK});
                rpf.SetAction(ReplaceForm.Action.BOYSOUT);
                if (rpf.ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                _logger.Warn(exc);
                _rsb.Run();
            }
        }  

        private void miEPasportMake_Click(object sender, EventArgs e)
        {
#if !DEMO
            if (listView1.SelectedItems.Count == 0) return;
            
            try
            {
                List<int> rIds = new List<int>();
                foreach (ListViewItem it in listView1.SelectedItems)
                    rIds.Add((int)it.{FUCK});
                if ((new EPasportForm(rIds)).ShowDialog() == DialogResult.OK && !MainForm.MustClose)
                    _rsb.Run();
            }
            catch(RabNetException exc)
            {
                MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK,MessageBoxIcon.Stop);
            }
#else
            DemoErr.DemoNoModuleMsg();
#endif
        }

        #endregion menuItems

        


        //private void insertNode(TreeNode nd,TreeData data)
        //{
        //    if (data.Childrens!=null)
        //    for (int i = 0; i < data.Childrens.Count; i++)
        //        if (data.Childrens[i] != null)
        //        {
        //            TreeNode n = nd.Nodes.Add(data.Childrens[i].Name);
        //            insertNode(n, data.Childrens[i]);
        //        }
        //}

        //private void логЗаписиToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!Directory.Exists("zapis")) Directory.CreateDirectory("zapis");
        //        string path = "zapis/log_" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".txt";
        //        StreamWriter rw = new StreamWriter(path);

        //        foreach (ListViewItem x in listView1.Items)
        //        {
        //            string s = x.SubItems[1].Text;
        //            for (int i = 1; i < x.SubItems.Count - 1; i++) 
        //            {
        //                if (i==2 || i==8) continue;
        //                if (i == 1 || x.SubItems[i].Text.StartsWith("С"))
        //                {
        //                    s += "|С";
        //                    continue;
        //                }
        //                s += "|" + x.SubItems[i].Text;
        //            }

        //            rw.WriteLine(s);
        //        }
        //        rw.Close();
        //        MessageBox.Show("Запись прошла успешно\n\rИмя файла: " + path, "Сохранение в файл");

        //    }
        //    catch (Exception exep)
        //    {
        //        MessageBox.Show(exep.Message);
        //    }

        //}
    }
}
