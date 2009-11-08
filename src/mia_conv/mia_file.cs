using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace mia_conv
{


    class MiaFile
    {
        public List<IMFCommon> l1=new List<IMFCommon>();
        public MFString ver=new MFString("version",-2);
        public float dver = 0;
        public MFDate date = new MFDate("date",0);
        public MFBuildPlan buildPlan = new MFBuildPlan(1);
        public MFBuilds builds = new MFBuilds(2);
        public MFRabNames male_names = new MFRabNames("male_names",3);
        public MFRabNames female_names = new MFRabNames("female_names",4);
        public MFTransTable trans_table = new MFTransTable(5);
        public MFStringList zone_list = new MFStringList("zone_list", 6);
        public MFStringList breed_list = new MFStringList("breed_list", 7);
        public MFRabbits rabbits = null;
        public MFFilterForm filterform = new MFFilterForm("FilterForm", 9);
        public MFTransForm transform = new MFTransForm("TransForm", 10);
        public MFParamForm paramform = new MFParamForm("ParamForm", 11);
        public MFZooForm zooform = new MFZooForm("ZooForm", 12);
        public MFGraphForm graphform = new MFGraphForm("GraphForm", 13);
        public MFArchiveForm arcform =null;
        public MFString thisfarm = new MFString("thisfarm", 15);
        public MFString farmid = new MFString("farmid", 15);
        public MFWeightList wlist = new MFWeightList("WeightList", 16);
        CheckedListBox clb1 = null;
        

        public MiaFile(CheckedListBox lb)
        {
            rabbits = new MFRabbits("AllRabbits", 8, male_names, female_names);
            arcform= new MFArchiveForm("ArchiveForm", 14,male_names,female_names);
            clb1 = lb;
            l1.Add(date);
            l1.Add(buildPlan);
            l1.Add(builds);
            l1.Add(male_names);
            l1.Add(female_names);
            l1.Add(trans_table);
            l1.Add(zone_list);
            l1.Add(breed_list);
            l1.Add(rabbits);
            l1.Add(filterform);
            l1.Add(transform);
            l1.Add(paramform);
            l1.Add(zooform);
        }

        public void objread(IMFCommon obj, BinaryReader br,TextBox log)
        {
            obj.read(br,dver);
            if (obj.logindex()>-1 && obj.logindex()<clb1.Items.Count)
            if (clb1.GetItemChecked(obj.logindex()))
                log.Text += obj.log() + "\r\n";
            if (obj.logindex()==-2)
                log.Text += obj.log() + "\r\n";
        }

        public void readobjs(List<IMFCommon> objs, BinaryReader br, TextBox log)
        {
            for (int i = 0; i < objs.Count; i++)
                objread(objs[i],br,log);
        }

        public void LoadFromFile(String filename, TextBox log)
        {
            log.Clear();
            FileStream sfs = new FileStream(filename, FileMode.Open);
            BinaryReader fs=new BinaryReader(sfs,Encoding.GetEncoding("Windows-1251"));
            objread(ver, fs, log);
            String sv = ver.value();
            dver = float.Parse(sv.Substring(sv.IndexOf(" V")+2).Replace('.',','));
            if (dver < 4)
            {
                objread(date, fs, log);
                objread(date, fs, log);
            }
            if (dver > 3.0) l1.Add(graphform);
            if (dver > 3.9) l1.Add(arcform);
            if (dver > 4.3) { l1.Add(thisfarm); l1.Add(farmid); }
            if (dver > 5.1) l1.Add(wlist);
            readobjs(l1, fs, log);
            log.Text += String.Format("\r\nREAD ENDS AT FILEPOS {0:d} ({0:X}) OF {1:d} ({1:X})",sfs.Position,sfs.Length);
            fs.Close();
            sfs.Close();
        }
    }
}
