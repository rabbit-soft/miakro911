using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace mia_conv
{

    class MiaFile
    {
        public List<IMFCommon> L1=new List<IMFCommon>();
        public MFString Ver=new MFString("version",-2);
        public float Dver = 0;
        public MFDate Date = new MFDate("date",0);
        public MFBuildPlan BuildPlan = new MFBuildPlan(1);
        public MFBuilds Builds = new MFBuilds(2);
        public MFRabNames MaleNames = new MFRabNames("male_names",3);
        public MFRabNames FemaleNames = new MFRabNames("female_names",4);
        public MFTransTable TransTable = new MFTransTable(5);
        public MFStringList ZoneList = new MFStringList("zone_list", 6);
        public MFStringList BreedList = new MFStringList("breed_list", 7);
        public MFRabbits Rabbits = null;
        public MFFilterForm Filterform = new MFFilterForm("FilterForm", 9);
        public MFTransForm Transform = new MFTransForm("TransForm", 10);
        public MFParamForm Paramform = new MFParamForm("ParamForm", 11);
        public MFZooForm Zooform = new MFZooForm("ZooForm", 12);
        public MFGraphForm Graphform = null;
        public MFArchiveForm Arcform =null;
        public MFString Thisfarm = new MFString("thisfarm", 15);
        public MFString Farmid = new MFString("farmid", 15);
        public MFWeightList Wlist = new MFWeightList("WeightList", 16);
        CheckedListBox clb1 = null;
        ProgressBar pb = null;
        Label lbl = null;
        private int _pval;

        private long _label_ticks = 0;
        private long _pb1_ticks = 0;
        private string _label_nm = "";

        public MiaFile(CheckedListBox lb,ProgressBar pb,Label lbl)
        {
            this.pb = pb;
            this.lbl = lbl;
            pb.Value = 0;
            Graphform = new MFGraphForm("GraphForm", 13,this);
            Rabbits = new MFRabbits("AllRabbits", 8, MaleNames, FemaleNames,this);
            Arcform= new MFArchiveForm("ArchiveForm", 14,MaleNames,FemaleNames);
            clb1 = lb;
            L1.Add(Date);
            L1.Add(BuildPlan);
            L1.Add(Builds);
            L1.Add(MaleNames);
            L1.Add(FemaleNames);
            L1.Add(TransTable);
            L1.Add(ZoneList);
            L1.Add(BreedList);
            L1.Add(Rabbits);
            L1.Add(Filterform);
            L1.Add(Transform);
            L1.Add(Paramform);
            L1.Add(Zooform);
        }

        public void Objread(IMFCommon obj, BinaryReader br,TextBox log)
        {
            obj.read(br,Dver);
            if (obj.logindex()>-1 && obj.logindex()<clb1.Items.Count)
            if (clb1.GetItemChecked(obj.logindex()))
                log.Text += obj.log() + "\r\n";
            if (obj.logindex()==-2)
                log.Text += obj.log() + "\r\n";
            log.Select(log.Text.Length, 0);
            log.ScrollToCaret();
        }

        public void Readobjs(List<IMFCommon> objs, BinaryReader br, TextBox log)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                SetLabelName("Object "+i.ToString());
                Setpbpart(i, objs.Count);
                Objread(objs[i], br, log);
            }
            pb.Value = 0;
        }

        public void LoadFromFile(String filename, TextBox log)
        {
            log.Clear();
            FileStream sfs = new FileStream(filename, FileMode.Open);
            BinaryReader fs=new BinaryReader(sfs,Encoding.GetEncoding("Windows-1251"));
            Objread(Ver, fs, log);
            String sv = Ver.value();
            Dver = float.Parse(sv.Substring(sv.IndexOf(" V")+2).Replace('.',','));
            if (Dver < 4)
            {
                Objread(Date, fs, log);
                Objread(Date, fs, log);
            }
            if (Dver > 3.0) L1.Add(Graphform);
            if (Dver > 3.9) L1.Add(Arcform);
            if (Dver > 4.3) { L1.Add(Thisfarm); L1.Add(Farmid); }
            if (Dver > 5.1) L1.Add(Wlist);
            Readobjs(L1, fs, log);
            log.Text += String.Format("\r\nREAD ENDS AT FILEPOS {0:d} ({0:X}) OF {1:d} ({1:X})",sfs.Position,sfs.Length);
            fs.Close();
            sfs.Close();
        }

        public void Setpbpart(int part,int of)
        {
            pb.Value = 100 / of * part;
            clb1.Tag = pb.Value;
            pb.Tag = 100 / of;
            pb.Refresh();
            _pval = pb.Value;
            Application.DoEvents();
        }

        public void SetLabel(int part,int of)
        {
            lbl.Text = _label_nm + " -> " + part.ToString() + "/" + of.ToString();
        }

        public void SetLabelName(string nm)
        {
            lbl.Text = nm;
            _label_nm = nm;
            Application.DoEvents();
        }

        public void Setpb(int i, int cnt)
        {
            if (_pb1_ticks + 500 < Environment.TickCount)
            {
                SetLabel(i, cnt);

                int p = 100 * i / cnt;
                int val = (int)clb1.Tag + ((int)pb.Tag * p / 100);
                if (val > 100)
                    val = 100;
                if (_pval == val)
                    return;
                pb.Value = val;
                pb.Refresh();
                _pval = pb.Value;
                _pb1_ticks = Environment.TickCount;

                Application.DoEvents();
            }
        }
    }
}
