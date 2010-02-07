using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace mia_conv
{
    class Bon
    {
        public MFByte weight = new MFByte("weight");
        public MFByte body = new MFByte("body");
        public MFByte hair = new MFByte("hair");
        public MFByte color = new MFByte("color");
        public MFByte manual = new MFByte("manual");
        public static String[] bonNames = { "B_UNKNOWN", "B_THIRD", "B_SECOND", "B_FIRST", "B_ELITE", "MAX_BON" };
        public String bondescr(MFByte obj)
        {
            return obj.log() + "(" + bonNames[obj.value()] + ") ";
        }
        public void read(BinaryReader br, float ver)
        {
            weight.read(br, ver);
            body.read(br, ver);
            hair.read(br, ver);
            color.read(br, ver);
            manual.read(br,ver);
        }
        public String log()
        {
            return "BON= " + bondescr(weight) + bondescr(body) + bondescr(hair) + bondescr(color) + manual.log();
        }

    }

    class Fucker
    {
        public MFByte live = new MFByte("live");
        public MFUShort name_key = null;
        public MFUShort gecnt = null;
        public List<ushort> genesis = null;
        public MFString name = null;
        public MFUShort breed = new MFUShort("breed");
        public MFUShort fucks = new MFUShort("fucks");
        public MFUShort children = new MFUShort("children");
        public MFByte my_fuck_is_last = new MFByte("my_fuck_is_last");
        public Fucker(BinaryReader br, float ver)
        {
            read(br, ver);
        }
        public void read(BinaryReader br, float ver)
        {
            live.read(br, ver);
            if (live.value()!=0)
            {
                name_key = new MFUShort("name_key");
                name_key.read(br, ver);
            }
            else
            {
                gecnt = new MFUShort("genesis_count");
                genesis = new List<ushort>();
                name = new MFString("name");
                gecnt.read(br, ver);
                for (int i = 0; i < (int)gecnt.value(); i++)
                    genesis.Add(br.ReadUInt16());
                name.read(br, ver);
            }
            breed.read(br, ver);
            fucks.read(br, ver);
            children.read(br, ver);
            my_fuck_is_last.read(br, ver);
        }
        public String log()
        {
            String str="-=fucker=-\r\n"+live.log()+"\r\n";
            if (live.value() != 0)
            {
                str += name_key.log() + "\r\n";
            }
            else
            {
                str += "genesis(" + gecnt.value().ToString() + ")=";
                for (int i = 0; i < (int)gecnt.value(); i++)
                    str += String.Format("{0:d} ",genesis[i]);
                str += "\r\n"+name.log() + "\r\n";
            }
            str += breed.log() + "\r\n" + fucks.log() + "\r\n" + children.log() + "\r\n" + my_fuck_is_last.log() + "\r\n";
            return str + "-=fucker_end=-";
        }
    }

    class RabFemale
    {
        public MFByte child_count = new MFByte("child_count");
        public MFByte borns = new MFByte("borns");
        public MFDate ev_date = new MFDate("ev_date");
        public MFDate last_okrol = new MFDate("last_okrol");
        public MFByte ev_type = new MFByte("ev_type");
        public MFByte lost_babies = new MFByte("lost_babies");
        public MFUShort overall_babies= new MFUShort("overall_babies");
        public MFUShort fuckers_count = new MFUShort("fuckers_count");
        public List<Fucker> fuckers = new List<Fucker>();
        public MFRabbits suckers = null;
        public MFString worker = new MFString("worker");
        public MFByte no_kuk = new MFByte("no_kuk");
        public MFByte no_lact = new MFByte("no_lact");
        private MFRabNames nmales = null;
        private MFRabNames nfemales = null;
        public RabFemale(BinaryReader br, float ver, MFRabNames males, MFRabNames females)
        {
            nmales = males;
            nfemales = females;
            suckers=new MFRabbits("suckers",nmales,nfemales,null);
            read(br, ver);
        }
        public void read(BinaryReader br, float ver)
        {
            child_count.read(br,ver);
            DBG.dbg2(child_count.log(),br);
            borns.read(br,ver);
            DBG.dbg2(borns.log(), br);
            ev_date.read(br,ver);
            DBG.dbg2(ev_date.log(), br);
            last_okrol.read(br, ver);
            DBG.dbg2(last_okrol.log(), br);
            ev_type.read(br, ver);
            DBG.dbg2(ev_type.log(), br);
            if (ver>3.1)
                lost_babies.read(br, ver);
            DBG.dbg2(lost_babies.log(), br);
            if (ver>5.1)
                overall_babies.read(br, ver);
            DBG.dbg2(overall_babies.log(), br);
            suckers.read(br, ver);
            DBG.dbg2(suckers.log(), br);
            fuckers_count.read(br, ver);
            DBG.dbg2(fuckers_count.log(), br);
            for (int i = 0; i < (int)fuckers_count.value(); i++)
            {
                fuckers.Add(new Fucker(br, ver));
                DBG.dbg2(fuckers[i].log(), br);
            }
            if (ver > 5.1)
            {
                worker.read(br, ver);
                DBG.dbg2(worker.log(), br);
                no_kuk.read(br, ver);
                DBG.dbg2(no_kuk.log(), br);
                no_lact.read(br, ver);
                DBG.dbg2(no_lact.log(), br);
            }
        }
        public String log()
        {
            String str = child_count.log()+"\r\n"+borns.log()+"\r\n"+ev_date.log()+"\r\n"+last_okrol.log()+"\r\n";
            str += ev_type.log() + "\r\n" + lost_babies.log() + "\r\n"+overall_babies.log()+"\r\n" + fuckers_count.log() + "\r\n";
            for (int i = 0; i < (int)fuckers_count.value(); i++)
            {
                str += fuckers[i].log() + "\r\n";
            }
            str += suckers.log() + "\r\n";
            str += worker.log() + "\r\n" + no_kuk.log() + "\r\n" + no_lact.log();
            return str;
        }
    }

    class Rabbit
    {
        public MFByte sex = new MFByte("sex");
        public String[] sexName = { "VOID", "MALE", "FEMALE", "MAX_SEX" };
        public Bon bon = new Bon();
        public MFUShort number = new MFUShort("number");
        public MFUShort unique = new MFUShort("unique");
        public MFUShort namekey = new MFUShort("namekey");
        public MFUShort surkey = new MFUShort("surkey");
        public MFUShort pathkey = new MFUShort("pathkey");
        public MFString notes = new MFString("notes");
        public MFByte butcher = new MFByte("butcher");
        public MFByte risk = new MFByte("risk");
        public MFByte okrol_num = new MFByte("okrol");
        public MFUShort where = new MFUShort("where");
        public MFByte tier = new MFByte("tier");
        public MFByte tier_id = new MFByte("tier_id");
        public MFByte area = new MFByte("area");
        public MFSByte rate = new MFSByte("rate");
        public MFByte group = new MFByte("group");
        public MFByte breed = new MFByte("breed");
        public MFByte multi = new MFByte("multi");
        public MFUShort zone = new MFUShort("zone");
        public ushort weightcnt = 0;
        public List<ulong> weights = new List<ulong>();
        public MFDate borndate = new MFDate("borndate");
        public ushort gencnt = 0;
        public List<ushort> genesis = new List<ushort>();
        public List<IMFCommon> proplist = new List<IMFCommon>();
        public List<IMFCommon> feproplist = new List<IMFCommon>();
        public MFDate lastfuck = null;
        public MFByte status = null;
        public RabFemale female = null;
        private MFRabNames males = null;
        private MFRabNames females = null;
        public Rabbit(BinaryReader br, float ver, MFRabNames nmales,MFRabNames nfemales)
        {
            males = nmales;
            females = nfemales;
            if (ver>3)
                proplist.Add(number);
            proplist.Add(unique);
            proplist.Add(namekey);
            proplist.Add(surkey);
            proplist.Add(pathkey);
            proplist.Add(notes);
            proplist.Add(butcher);
            if (ver > 3)
                proplist.Add(risk);
            proplist.Add(okrol_num);
            proplist.Add(where);
            proplist.Add(tier);
            proplist.Add(tier_id);
            proplist.Add(area);
            proplist.Add(rate);
            proplist.Add(group);
            proplist.Add(breed);
            proplist.Add(multi);
            proplist.Add(zone);
            read(br, ver);
        }
        public void read(BinaryReader br,float ver)
        {
            sex.read(br,ver);
            DBG.dbg1(sex.log());
            if (ver>3)
                bon.read(br, ver);
            DBG.dbg1(bon.log());
            for (int i = 0; i < proplist.Count; i++)
            {
                proplist[i].read(br, ver);
                DBG.dbg1(proplist[i].log());
            }
            weightcnt = br.ReadUInt16();
            DBG.dbg1("weight="+weightcnt.ToString());
            for (int i = 0; i < weightcnt; i++)
            {
                weights.Add(br.ReadUInt32());
                DBG.dbg1("weight["+i.ToString()+"]=" + weights[i].ToString());
            }
            borndate.read(br, ver);
            DBG.dbg1(borndate.log());
            gencnt = br.ReadUInt16();
            for (int i = 0; i < gencnt; i++)
                genesis.Add(br.ReadUInt16());
            if (sex.value() == 1)
            {
                lastfuck = new MFDate("lastfuck");
                status = new MFByte("status");
                lastfuck.read(br,ver);
                status.read(br,ver);
            }
            if (sex.value() == 2)
                female = new RabFemale(br, ver, males, females);
        }
        public String log()
        {
            String str = "-=Rabbit=-\r\n";
            str += sex.log() +" ("+ sexName[sex.value()] + ")\r\n";
            str += bon.log() + "\r\n";
            for (int i = 0; i < proplist.Count; i++)
            {
                str += proplist[i].log();
                if (proplist[i] == namekey)
                {
                    if (sex.value()==1)
                        str+=" "+males.getname((ushort)namekey.value());
                    if (sex.value()==2)
                        str+=" "+females.getname((ushort)namekey.value());
                }
                if (proplist[i] == surkey)
                {
                        str += " " + females.getname((ushort)surkey.value());
                }
                if (proplist[i] == pathkey)
                {
                        str += " " + males.getname((ushort)pathkey.value());
                }
                str += "\r\n";
            }
            str += "weights("+weightcnt.ToString()+")=";
            for (int i = 0; i < weightcnt;i++ )
            {
                ushort weight=(ushort)(weights[i] & 0xFFFF);
                ushort dt=(ushort)((weights[i] >> 16)& 0xFFFF);
                DateTime sdt=(new DateTime(1899,12,30)).AddDays(dt);
                String hlp=String.Format("{0:X}({1:s}-{2:d})",weights[i],sdt.ToShortDateString(),weight);
                str += hlp + " ";
            }
            str += "\r\n" + borndate.log() + "\r\ngenesis(" + gencnt.ToString() + ")=";
            for (int i = 0; i < gencnt; i++)
                str += genesis[i].ToString() + " ";
            str += "\r\n";
            if (sex.value() == 1)
            {
                str += lastfuck.log() + "\r\n";
                str += status.log() + "\r\n";
            }
            if (sex.value() == 2)
                str += female.log() + "\r\n";
            str += "-=Rabbit_End=-";
            return str;
        }
    }

    class MFRabbits:MFCommon,IMFCommon
    {
        public MFUShort count=new MFUShort("count");
        public List<Rabbit> rabbits = new List<Rabbit>();
        private MFRabNames nmales = null;
        private MFRabNames nfemales = null;
        private MiaFile mf = null;
        public MFRabbits(String name, MFRabNames males, MFRabNames females,MiaFile mf) : base(name)
        { nmales = males; nfemales = females; this.mf = mf; }
        public MFRabbits(String name, int li, MFRabNames males, MFRabNames females,MiaFile mf) : base(name, li)
        { nmales = males; nfemales = females; this.mf = mf; }

        public void read(BinaryReader br, float ver)
        {
            count.read(br, ver);
            int cnt = (int)count.value();
            for (int i = 0; i < (int)count.value(); i++)
            {
                if (mf != null)
                    mf.setpb(100*i/cnt);
                rabbits.Add(new Rabbit(br, ver, nmales, nfemales));
            }
        }


        public override String strval()
        {
            String str="-=RABBITS=-\r\n"+count.log()+"\r\n";
            for (int i = 0; i < (int)count.value(); i++)
                str += rabbits[i].log() + "\r\n";
            str += "-=RABBITS_END=-";
            return str;
        }
    }
}
