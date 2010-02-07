using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace mia_conv
{
    class MFBuildPlan : MFCommon, IMFCommon
    {
        TreeNode tr = new TreeNode("Build Plan");
        public MFBuildPlan(int li):base("buildPlan",li){}
        public void read(BinaryReader br,float ver)
        {
            TreeNode curnode = tr;
            Byte depth = br.ReadByte();
            Byte predepth = 0;
            while (depth != 0)
            {
                MFString nm = new MFString("");
                nm.read(br,ver);
                TreeNode nd = new TreeNode(nm.value());
                if (depth == predepth)
                {
                    curnode.Parent.Nodes.Add(nd);
                }
                else if (depth > predepth)
                {
                    curnode.Nodes.Add(nd);
                }
                else
                {
                    int dd = predepth - depth;
                    TreeNode snd = curnode.Parent;
                    while (dd > 0)
                    {
                        snd = snd.Parent;
                        dd--;
                    }
                    snd.Nodes.Add(nd);
                }
                curnode = nd;
                predepth = depth;
                depth = br.ReadByte();
            }
        }
        public TreeNode value() { return tr; }
        public String getstrnode(TreeNode nd, int dph)
        {
            String str = "";
            for (int i = 0; i < dph; i++) str += " ";
            str += nd.Text + "\r\n";
            for (int i = 0; i < nd.Nodes.Count; i++)
                str+=getstrnode(nd.Nodes[i], dph + 1);
            return str;
        }
        public override String strval()
        {
            return "-=BUILD_PLAN_START=-"+getstrnode(tr, 0)+"-=BUILD_PLAN_END=-";
        }
    }


    class Tier
    {
//       enum TierType { NO_TIER,T_FEMALE,T_DFEMALE,T_COMPLEX,T_JURTA,T_QUARTA,T_VERTEP,T_BARIN,T_CABIN,MAX_TIER_TYPE };
        private String[] tierNames={"NO_TIER","FEMALE","DFEMALE","COMPLEX","JURTA","QUARTA","VERTEP","BARIN","CABIN","MAX_TIER_TYPE"};
        private Byte[] tierBusyCount={0,1,2,3,2,4,2,2,2,0};
        private Byte[] tierHeaterCount={0,1,2,1,1,0,0,0,1,0};
        private Byte[] tierNestCount={0,1,2,1,1,0,0,0,1,0};
        private Byte[] tierDelimsCount={0,0,0,0,0,3,0,1,0,0};
        private Byte[] tierNWBCount={0,0,0,0,1,0,0,0,0,0};
        public Byte type=0;
        public Byte repair;
        public MFString notes = new MFString("notes");
        public Byte[] busies=new Byte[4];
        public Byte[] heaters = new Byte[2];
        public Byte[] nests=new Byte[2];
        public Byte[] delims=new Byte[3];
        public Byte nest_wbig=new Byte();
        public Tier(BinaryReader br, float ver)
        {
            Read(br, ver);
        }
        public void Read(BinaryReader br,float ver)
        {
            type=br.ReadByte();
            repair=br.ReadByte();
            notes.read(br,ver);
            for (int i=0;i<tierBusyCount[type];i++)
                busies[i]=br.ReadByte();
            for (int i=0;i<tierHeaterCount[type];i++)
                heaters[i]=br.ReadByte();
            for (int i=0;i<tierNestCount[type];i++)
                nests[i]=br.ReadByte();
            for (int i=0;i<tierDelimsCount[type];i++)
                delims[i]=br.ReadByte();
            if (tierNWBCount[type]==1)
                nest_wbig=br.ReadByte();
        }

        public String log()
        {
            String str="  -=Tier=-\r\n";
            str+=String.Format("   type:{0:d} - {1:s}\r\n",type,tierNames[type]);
            str+=String.Format("   repair:{0:d}\r\n",repair);
            str+="   "+notes.log()+"\r\n   ";
            for (int i=0;i<tierBusyCount[type];i++)
                str+=String.Format("busy[{0:d}]={1:d} ",i,busies[i]);
            for (int i=0;i<tierHeaterCount[type];i++)
                str+=String.Format("heater[{0:d}]={1:d} ",i,heaters[i]);
            for (int i=0;i<tierNestCount[type];i++)
                str+=String.Format("nest[{0:d}]={1:d} ",i,nests[i]);
            for (int i=0;i<tierDelimsCount[type];i++)
                str+=String.Format("delim[{0:d}]={1:d} ",i,delims[i]);
            if (tierNWBCount[type]==1)
                str+=String.Format("net_wbig={0:d}",nest_wbig);

            str+="\r\n  -=Tier_end=-";
            return str;
        }

    }
    class MiniFarm
    {
        public short id=0;
        public Tier upper=null;
        public Byte haslower=0;
        public Tier lower=null;
        public MiniFarm(BinaryReader br, float ver)
        {
            read(br, ver);
        }
        public void read(BinaryReader br,float ver)
        {
            id=br.ReadInt16();
            upper=new Tier(br,ver);
            haslower=br.ReadByte();
            if (haslower>0)
                lower=new Tier(br,ver);
        }
        public String log()
        {
            String str=" -=MiniFarm_"+id.ToString()+"=-\r\n";
            str+=" upper="+upper.log()+"\r\n";
            str+=String.Format(" haslower={0:d}\r\n",haslower);
            if (haslower>0)
                   str+=" lower="+lower.log()+"\r\n";
            str+=" -=MiniFarm_"+id.ToString()+"_End=-";
            return str;
        }
    }


    class MFBuilds : MFCommon, IMFCommon
    {
        public MFBuilds(int li) : base("Builds",li) { }
        public MFUShort count=new MFUShort("count");
        public List<MiniFarm> minifarms=new List<MiniFarm>();
        public void read(BinaryReader br, float ver)
        {
            count.read(br,ver);
            for (int i=0;i<(int)count.value();i++)
                minifarms.Add(new MiniFarm(br,ver));
        }
        public List<MiniFarm> value() { return minifarms; }
        public override String strval()
        {
            String str="-=BUILDINGS=-\r\n";
            str+=count.log()+"\r\n";
            for (int i=0;i<(int)count.value();i++)
                str+=minifarms[i].log()+"\r\n";
            str+="-=BUILDINGS_END=-";
            return str;
        }
    }

    class RabName
    {
        public MFUShort key=new MFUShort("key");
        public MFUShort surkey=new MFUShort("surkey");
        public MFString name=new MFString("name");
        public MFString surname=new MFString("surname");
        public RabName(BinaryReader br,float ver)
        {
            read(br,ver);
        }
        public void read(BinaryReader br,float ver)
        {
            key.read(br,ver);
            surkey.read(br,ver);
            name.read(br,ver);
            surname.read(br,ver);
        }
        public String log()
        {
            return " "+key.log()+" "+surkey.log()+" "+name.log()+" "+surname.log();
        }
    }
    class MFRabNames : MFCommon, IMFCommon
    {
        private MFUShort count=new MFUShort("count");
        public List<RabName> rabnames=new List<RabName>();
        public MFRabNames(String name,int li) : base(name,li) { }
        public void read(BinaryReader br,float ver)
        {
            count.read(br,ver);
            for (int i=0;i<(int)count.value();i++)
                rabnames.Add(new RabName(br, ver));
        }
        public String getname(ushort key)
        {
            if (key == 0) return "";
            for (int i = 0; i < (int)count.value(); i++)
            {
                if (key == rabnames[i].key.value())
                    return rabnames[i].name.value();
                if (key == rabnames[i].surkey.value())
                    return rabnames[i].surname.value();
            }
            return null;
        }
        public override String strval()
        {
            String str="-=RABNAMELIST=-\r\n"+count.log()+"\r\n";
            for (int i=0;i<(int)count.value();i++)
                str+=rabnames[i].log()+"\r\n";
            str+="-=RABNAMELIST_END=-";
            return str;
        }
    }

    class Trans
    {
        public Byte transferType = 0;
        public static String[] trNames = {"MEAT_SOLD","SKIN_SOLD","RABBITS","FEED","OTHER","MEAT","SKIN","USED_FEED","OTSEV","MAX_TRANS_TYPE"};
        public MFString notes=new MFString("notes");
        public MFDate when=new MFDate("when");
        public MFULong units=new MFULong("units");
        public static String[] masks = { "awpP", "aspP", "SanbwpP", "anWkpP", "SanWkpP", "amBNnA", "amXbsnA", "anWk", "SaWpkP", "" };
        //awpPsSnbWkmBNAX
        public MFUShort age = null; //a
        public MFUShort sweight = null; //w
        public MFULong lweight = null; //W
        public MFString price=null; //P
        public MFString partner=null; //p
        public MFByte issold = null; //S
        public MFByte skintype=null; //s
        public static String[] skinTypeNames={"SK_UNKNOWN","LUXURY","I","II","III","IV","MAX_SKIN"};
        public MFString name = null; //n
        public MFUShort breed=null; //b
        public MFString kind = null; //k
        public MFDate murder = null; //m
        public MFUShort brutto = null; //B
        public MFUShort netto = null; //N
        public MFString address = null; //A
        public MFByte sex = null; //X
        public Trans(BinaryReader br, float ver)
        {
            read(br, ver);
        }

        public IMFCommon getobj(char code,bool create)
        {
            IMFCommon obj = null;
            switch (code)
            {
                case 'a': if (create) age = new MFUShort("age"); obj = age; break;
                case 'w': if (create) sweight = new MFUShort("weight"); obj = sweight; break;
                case 'W': if (create) lweight = new MFULong("weight"); obj = lweight; break;
                case 'P': if (create) price = new MFString("price"); obj = price; break;
                case 'p': if (create) partner = new MFString("partner"); obj = partner; break;
                case 'S': if (create) issold = new MFByte("is_sold"); obj = issold; break;
                case 's': if (create) skintype = new MFByte("skintype"); obj = skintype; break;
                case 'n': if (create) name = new MFString("name"); obj = name; break;
                case 'b': if (create) breed = new MFUShort("breed"); obj = breed; break;
                case 'k': if (create) kind = new MFString("kind"); obj = kind; break;
                case 'm': if (create) murder = new MFDate("murder"); obj = murder; break;
                case 'B': if (create) brutto = new MFUShort("brutto"); obj = brutto; break;
                case 'N': if (create) netto = new MFUShort("netto"); obj = netto; break;
                case 'A': if (create) address = new MFString("address"); obj = address; break;
                case 'X': if (create) sex = new MFByte("sex"); obj = sex; break;
            }
            return obj;
        }

        public void read(BinaryReader br, float ver)
        {
            transferType = br.ReadByte();
            notes.read(br, ver);
            when.read(br, ver);
            units.read(br, ver);
            String msk=masks[transferType];
            for (int i = 0; i < msk.Length; i++)
            {
                IMFCommon obj = getobj(msk[i], true);
                obj.read(br, ver);
            }
        }

        public String log()
        {
            String str = " -=Transfer=-\r\n ";
            str += String.Format("type={0:d} {1:s}\r\n ", transferType, trNames[transferType]);
            str += notes.log() + "\r\n ";
            str += when.log() + "\r\n ";
            str += units.log() + "\r\n ";
            String msk = masks[transferType];
            for (int i = 0; i < msk.Length; i++)
            {
                IMFCommon obj = getobj(msk[i], false);
                str += obj.log();
                if (msk[i] == 's')
                    str += " " + skinTypeNames[skintype.value()];
                str += "\r\n ";
            }
            str += "-=Transfer_end=-";
            return str;
        }
    }

    class MFTransTable : MFCommon, IMFCommon
    {
        public long count = 0;
        public List<Trans> transes = new List<Trans>();
        public MFTransTable(int li) : base("trans_table", li) { }
        public void read(BinaryReader br,float ver)
        {
            count = br.ReadUInt32();
            for (long i = 0; i < count; i++)
                transes.Add(new Trans(br, ver));
        }
        public override String strval()
        {
            String str = "-=translist=-\r\ncount="+count.ToString()+"\r\n";
            for (int i = 0; i < count; i++)
                str += transes[i].log()+"\r\n";
            str += "-=translist_end=-";
            return str;
        }
    }

    class Filter
    {
        public int count = 34;
        public List<Int32> values = new List<Int32>();
        public String[] names={ "ALL_ENABLED","MALES_ENABLED","FEMALES_ENABLED","NOSEX_ENABLED","BOYS_ENABLED","CANDIDATES_ENABLED","FATHERS_ENABLED",
	"GIRLS_ENABLED","BRIDES_ENABLED","PERVO_ENABLED","MOTHERS_ENABLED","FEMALES_BAD","MALES_BAD","SUKROL","KUKU","FAMILY","WORKS_ENABLED",
	"FROM_AGE","TILL_AGE","USE_FROM_WEIGHT","FROM_WEIGHT","USE_TILL_WEIGHT","TILL_WEIGHT","FROM_SUKROL","TILL_SUKROL",
	"B_ALL_ENABLED","B_FREE","B_BUSY","B_SELRABBITS","B_MALE","B_FEMALE","B_OTHER","B_NESTS","B_HEATERS",
	"MAX_FCONTROLS" };
        public Filter(BinaryReader br, float ver)
        {
            read(br, ver);
        }
        public void read(BinaryReader br, float ver)
        {
            if (ver < 4.3)
                count = 32;
            for (int i = 0; i < count; i++)
                values.Add(br.ReadInt32());
        }
        public String log()
        {
            String str = "";
            for (int i = 0; i < count; i++)
                str += names[i] + "=" + values[i].ToString() + " ";
            return str;
        }
    }

    class MFFilterForm : MFCommon, IMFCommon
    {
        public MFStringList combo = new MFStringList("combo");
        public MFInt lookat = new MFInt("lookat");
        public MFUShort max = new MFUShort("max");
        public List<Filter> filters = new List<Filter>();
        public MFFilterForm(String name,int li):base(name,li){}
        public MFFilterForm(String name) : base(name) { }
        public void read(BinaryReader br,float ver)
        {
            lookat.read(br,ver);
            max.read(br, ver);
            for (int i = 0; i < (int)max.value(); i++)
                filters.Add(new Filter(br, ver));
            combo.read(br, ver);
        }
        public override String strval()
        {
            String str = "-=FilterForm=-\r\n" + lookat.log() + "\r\n" + max.log() + "\r\n";
            for (int i = 0; i < (int)max.value(); i++)
                str += "f"+i.ToString()+"="+filters[i].log() + "\r\n";
            str += combo.log() + "\r\n";
            return str + "-=FilterForm_End=-";
        } 
    }

    class MFTransForm : MFCommon, IMFCommon
    {
        public int valcount = 364;
        public int skincount = 5;
        public List<ulong> values = new List<ulong>();
        public List<MFString> skinnames = new List<MFString>();
        public MFString pricePerKilo = new MFString("PricePerKilo");
        public MFString feedPrice = new MFString("FeedPrice");
        public MFStringList skinBuyers=new MFStringList("SkinBuyers");
        public MFStringList bodyBuyers = new MFStringList("BodyBuyers");
        public MFStringList rabbitPartner = new MFStringList("RabbitPartner");
        public MFStringList feedPartner = new MFStringList("FeedPartner");
        public MFStringList kind = new MFStringList("Kind");
        public MFStringList otherPartner = new MFStringList("OtherPartner");
        public MFStringList feedType = new MFStringList("FeedType");
        public MFStringList otherKind = new MFStringList("OtherKind");
        public MFStringList otherProduct = new MFStringList("OtherProduct");
        public MFStringList usedFeedType = new MFStringList("UsedFeedType");
        public MFStringList usedFeedSpec = new MFStringList("UsedFeedSpec");
        public MFStringList otsevBuyer = new MFStringList("OtsevBuyer");
        public MFTransForm(String name, int li) : base(name, li) { }
        public MFTransForm(String name) : base(name) { }
        public void read(BinaryReader br, float ver)
        {
            for (int i = 0; i < valcount; i++)
                values.Add(br.ReadUInt32());
            for (int i = 0; i < skincount; i++)
                skinnames.Add(new MFString(br, ver));
            pricePerKilo.read(br, ver);
            feedPrice.read(br, ver);
            skinBuyers.read(br, ver);
            bodyBuyers.read(br, ver);
            rabbitPartner.read(br, ver);
            feedPartner.read(br, ver);
            kind.read(br, ver);
            otherPartner.read(br, ver);
            feedType.read(br, ver);
            otherKind.read(br, ver);
            otherProduct.read(br, ver);
            if (ver > 3.9)
            {
                usedFeedType.read(br, ver);
                usedFeedSpec.read(br, ver);
                otsevBuyer.read(br, ver);
            }
        }
        public override String strval()
        {
            String str = "-=TransFrom=-\r\nvalues=";
            for (int i=0;i<valcount;i++)
                str+=values[i].ToString()+" ";
            str+="\r\nskinnames=";
            for (int i=0;i<skincount;i++)
                str+=skinnames[i].log();
            str+="\r\n"+pricePerKilo.log()+"\r\n"+feedPrice.log()+"\r\n";
            str+=skinBuyers.log()+"\r\n"+bodyBuyers.log()+"\r\n"+rabbitPartner.log()+"\r\n"+feedPartner.log()+"\r\n";
            str+=kind.log()+"\r\n"+otherPartner.log()+"\r\n"+feedType.log()+"\r\n"+otherKind.log()+"\r\n";
            str+=otherProduct.log()+"\r\n"+usedFeedType.log()+"\r\n"+usedFeedSpec.log()+"\r\n"+otsevBuyer.log()+"\r\n";
            return str + "-=TransForm_End=-";
        }
    }

    class SubscruberOrJob
    {
        public MFByte on=new MFByte("on");
        public bool issub=true;
        public MFString job=new MFString("job");
        public MFString name=new MFString("name");
        public SubscruberOrJob(BinaryReader br,float ver,bool isjob)
        {
            issub=!isjob;
            if (issub)
                on.read(br,ver);
            job.read(br,ver);
            name.read(br,ver);
        }
        public String log()
        {
            return (issub?on.log():"")+" "+job.log()+" "+name.log();
        }
    }


    class MFParamForm : MFCommon, IMFCommon
    {
       public MFByte pervonest=new MFByte("pervonest");public MFByte kukunest=new MFByte("kukunest");    public MFByte mothernest=new MFByte("mothernest"); public MFByte heater=new MFByte("heater");
       public MFByte okrol=new MFByte("okrol");       public MFByte kuk=new MFByte("kuk");       public MFByte pravka_1=new MFByte("pravka_1"); public MFByte count_2=new MFByte("count_2");
       public MFByte count_3=new MFByte("count_3");   public MFByte endkuku=new MFByte("endkuku");       public MFByte vacc=new MFByte("vacc");    public MFByte vudvorenie=new MFByte("vudvorenie");
       public MFByte countsuckers=new MFByte("countsuckers");    public MFByte vyazkamother=new MFByte("vyazkamother");       public MFByte vyazkapervo=new MFByte("vyazkapervo"); public MFByte rasselboys=new MFByte("rasselboys");
       public MFByte killfemales=new MFByte("killfemales");    public MFByte killbrides=new MFByte("killbrides");       public MFByte killboys=new MFByte("killboys");    public MFByte max_age_diff=new MFByte("max_age_diff");
       public MFByte automode=new MFByte("automode");       public MFByte rescopies=new MFByte("rescopies");     public MFByte tab_abbr=new MFByte("tab_abbr");       public MFByte double_sur=new MFByte("double_sur");
       public MFByte heterosis=new MFByte("heterosis");       public MFByte inbreeding=new MFByte("inbreeding");       public MFByte report_full_addr=new MFByte("report_full_addr");
       public MFByte SOME_OPTION = new MFByte("SOME_OPTION");
        public MFByte use_from=new MFByte("use_from");      public MFByte use_till=new MFByte("use_till");       public MFDate from=new MFDate("from");       public MFDate till=new MFDate("till");
       public MFDate from_heater=new MFDate("from_heater");       public MFDate till_heater=new MFDate("till_heater");       public MFByte h_from_checked=new MFByte("h_from_checked");       public MFByte h_till_checked=new MFByte("h_till_checked");
       public MFByte show_tier_types=new MFByte("show_tier_types");       public MFByte show_area_types=new MFByte("show_area_types");       public MFChar sluchka_filter=new MFChar("sluchka_filter");       public MFDate today=new MFDate("today");
       public MFByte otsad_boys_mother=new MFByte("otsad_boys_mother");       public MFByte otsad_boys_pervo=new MFByte("otsad_boys_pervo");       public MFULong zoo_bits=new MFULong("zoo_bits");       public MFByte job_grouping=new MFByte("job_grouping");
       public MFByte name_show=new MFByte("name_show");       public MFByte ignore_last_fuck=new MFByte("ignore_last_fuck");       public MFByte partners_limit=new MFByte("partners_limit");       public MFByte limit_value=new MFByte("limit_value");
       public MFByte sec_ignore=new MFByte("sec_ignore");       public MFByte auto_kuk=new MFByte("auto_kuk");        public MFByte jurta_sync=new MFByte("jurta_sync");       public MFByte make_brides=new MFByte("make_brides");
       public MFByte sell_mothers_with_babies=new MFByte("sell_mothers_with_babies");       public MFByte imm_age_diff=new MFByte("imm_age_diff");       public MFUShort arctime=new MFUShort("arctime");      public MFUShort lost_days=new MFUShort("lost_days");
       public MFByte use_feed_spec=new MFByte("use_feed_spec");       public MFByte auto_arc=new MFByte("auto_arc");       public MFByte no_kuk=new MFByte("no_kuk");     public MFByte no_gen_mix=new MFByte("no_gen_mix");
       public MFChar holost_punish=new MFChar("holost_punish");       public MFChar imm_heater=new MFChar("imm_heater");       public MFByte rotation=new MFByte("rotation");       public MFByte rot_speed=new MFByte("rot_speed");
       public MFByte no_jurta_kuk=new MFByte("no_jurta_kuk");       public MFChar shed_scale=new MFChar("shed_scale");       public MFByte show_gen_tree=new MFByte("show_gen_tree");       public MFByte show_young_gen_tree=new MFByte("show_young_gen_tree");
       public MFUShort gen_tree_width=new MFUShort("gen_tree_width");       public MFUShort young_gen_tree_width=new MFUShort("young_gen_tree_width");
       public MFByte show_numbers=new MFByte("show_numbers");       public MFByte averfeed=new MFByte("averfeed");

       public MFByte number_before_name=new MFByte("number_before_name"); // Номера перед именами  #v>6.1
       public byte[] reserved=null;	//99

       public MFInt next_svid=new MFInt("next_svid");       //#v>5.1
       public MFInt svid_remark_cnt=new MFInt("svid_remark_cnt");
       public List<MFString> svid_remarks=new List<MFString>();
       public MFString svid_head=new MFString("svid_head");
       public MFString svid_farm = new MFString("svid_farm");
       public byte[] reserved2=null;  //100			
       public List<SubscruberOrJob> subscriber=new List<SubscruberOrJob>();

       public MFByte jobcnt=new MFByte("jobcnt");
       public List<SubscruberOrJob> jobs=new List<SubscruberOrJob>();

        public List<IMFCommon> allobj=new List<IMFCommon>();

        public MFParamForm(String name, int li) : base(name, li) { }
        public MFParamForm(String name) : base(name) { }
        public void read(BinaryReader br, float ver)
        {
            allobj.Add(pervonest);allobj.Add(kukunest);allobj.Add(mothernest);allobj.Add(heater);allobj.Add(okrol);allobj.Add(kuk);
            allobj.Add(pravka_1);allobj.Add(count_2);allobj.Add(count_3);allobj.Add(endkuku);allobj.Add(vacc);allobj.Add(vudvorenie);
            allobj.Add(countsuckers);allobj.Add(vyazkamother);allobj.Add(vyazkapervo);allobj.Add(rasselboys);allobj.Add(killfemales);
            allobj.Add(killbrides);allobj.Add(killboys);allobj.Add(max_age_diff);allobj.Add(automode);allobj.Add(rescopies);allobj.Add(tab_abbr);
            allobj.Add(double_sur);allobj.Add(heterosis);allobj.Add(inbreeding);allobj.Add(report_full_addr);
            allobj.Add(SOME_OPTION);
            allobj.Add(use_from);allobj.Add(use_till);
            allobj.Add(from);allobj.Add(till);allobj.Add(from_heater);allobj.Add(till_heater);allobj.Add(h_from_checked);allobj.Add(h_till_checked);
            allobj.Add(show_tier_types);allobj.Add(show_area_types);allobj.Add(sluchka_filter);allobj.Add(today);allobj.Add(otsad_boys_mother);
            allobj.Add(otsad_boys_pervo);allobj.Add(zoo_bits);allobj.Add(job_grouping);allobj.Add(name_show);allobj.Add(ignore_last_fuck);allobj.Add(partners_limit);
            allobj.Add(limit_value);allobj.Add(sec_ignore);allobj.Add(auto_kuk);allobj.Add(jurta_sync);allobj.Add(make_brides);allobj.Add(sell_mothers_with_babies);
            allobj.Add(imm_age_diff);allobj.Add(arctime);allobj.Add(lost_days);allobj.Add(use_feed_spec);allobj.Add(auto_arc);allobj.Add(no_kuk);allobj.Add(no_gen_mix);
            allobj.Add(holost_punish);allobj.Add(imm_heater);allobj.Add(rotation);allobj.Add(rot_speed);allobj.Add(no_jurta_kuk);allobj.Add(shed_scale);
            allobj.Add(show_gen_tree);allobj.Add(show_young_gen_tree);allobj.Add(gen_tree_width);allobj.Add(young_gen_tree_width);allobj.Add(show_numbers);allobj.Add(averfeed);
            for (int i = 0; i < allobj.Count; i++)
            {
                allobj[i].read(br, ver);
                DBG.dbg3(allobj[i].log());
            }
            if (ver > 6.1)
            {  number_before_name.read(br, ver);
            DBG.dbg3(number_before_name.log());
                reserved = br.ReadBytes(99);
                DBG.dbg3(reserved.ToString());
            }
            if (ver > 5.1)
            {
                next_svid.read(br, ver); DBG.dbg3(next_svid.log());
                svid_remark_cnt.read(br, ver); DBG.dbg3(svid_remark_cnt.log());
                for (int i = 0; i < svid_remark_cnt.value(); i++)
                {
                    svid_remarks.Add(new MFString(br, ver));
                    DBG.dbg3("rem"+i.ToString()+"="+svid_remarks[i].value());
                }
                svid_head.read(br, ver); DBG.dbg3(svid_head.log());
                svid_farm.read(br, ver); DBG.dbg3(svid_farm.log());
                reserved2 = br.ReadBytes(100);
                for (int i=0;i<5;i++)
                    subscriber.Add(new SubscruberOrJob(br,ver,false));
            }
            jobcnt.read(br, ver); DBG.dbg3(jobcnt.log());
            for (int i=0;i<(int)jobcnt.value();i++)
                jobs.Add(new SubscruberOrJob(br,ver,true));
        }

        public override string strval()
        {
            String str = "-=ParamForm=-\r\n";
            for (int i = 0; i < allobj.Count; i++)
                str += allobj[i].log() + "\r\n";
            str += number_before_name.log() + "\r\nreserved=";
           // if (reserved!=null)
           // for (int i=0;i<99;i++) str+=String.Format("{0:X} ",reserved[i]);
            str += "\r\n" + next_svid.log() + "\r\n" + svid_remark_cnt.log() + "\r\nremarks=-=start=-";
            for (int i = 0; i < svid_remark_cnt.value(); i++)
                str += svid_remarks[i].value() + "\r\n";
            str += "-=end=-\r\n" + svid_head.log() + "\r\n" + svid_farm.log() + "\r\nreserved2=";
            //if (reserved2!=null)
            //for (int i = 0; i < 100; i++) str += String.Format("{0:X} ", reserved2[i]);
            str += "\r\n";
            if (subscriber.Count > 0)
                for (int i = 0; i < 5; i++)
                    str += "subscriber" + i.ToString() + "=" + subscriber[i].log()+"\r\n";
            str += jobcnt.log() + "\r\n";
            for (int i = 0; i <(int) jobcnt.value(); i++)
                str += "job" + i.ToString() + "=" + jobs[i].log()+"\r\n";
            return str + "-=ParamForm_End=-";
        } 
    }

    class Acceptor
    {
        public MFUShort unique = new MFUShort("unique");
        public MFByte lack = new MFByte("lack");
        public MFByte hybrid = new MFByte("hybrid");
        public MFByte newgroup = new MFByte("newgroup");
        public MFInt gendiff = new MFInt("gendiff");
        public MFInt distance = new MFInt("distance");
        public MFUShort donor_best = new MFUShort("donor_best");
        public MFUShort acceptor_best = new MFUShort("acceptor_best");
        public Acceptor(BinaryReader br, float ver)
        {
            unique.read(br, ver); lack.read(br, ver);
            hybrid.read(br, ver); newgroup.read(br, ver);
            gendiff.read(br, ver); distance.read(br, ver);
            donor_best.read(br, ver); acceptor_best.read(br,ver);
        }
        public String log()
        {
            return unique.log() + " " + lack.log() + " " + hybrid.log() + " " + newgroup.log() + " " +
                gendiff.log() + " " + distance.log() + " " + donor_best.log() + " " + acceptor_best.log(); 
        }
    }

    class Donor
    {
        public MFUShort unique = new MFUShort("unique");
        public MFUShort acccnt = new MFUShort("acccount");
        public List<Acceptor> acc = new List<Acceptor>();
        public MFByte surplus = new MFByte("surplus");
        public MFByte immediate = new MFByte("immediate");
        public Donor(BinaryReader br, float ver)
        {
            unique.read(br, ver);
            acccnt.read(br,ver);
            for (int i=0;i<(int)acccnt.value();i++)
                acc.Add(new Acceptor(br,ver));
            surplus.read(br, ver);
            immediate.read(br, ver);
        }
        public String log()
        {
            String str = "  -=donor=-\r\n  " + unique.log() + "\r\n  " + acccnt.log() + "\r\n  ";
            for (int i = 0; i < (int)acccnt.value(); i++)
                str += "acc" + i.ToString() + "=" + acc[i].log() + "\r\n  ";
            str += surplus.log() + "\r\n  " + immediate.log() + "\r\n  ";
            return str + "-=donor_end=-";
        }
    }

    class ZooJob
    {
        public MFString caption = new MFString("caption");
        public MFByte subcount = new MFByte("subcount");
        public List<MFString> subs = new List<MFString>();
        public MFByte type = new MFByte("type");
        public MFByte uniquer = new MFByte("uniquer");
        public MFByte uniquescnt = new MFByte("uniquescnt");
        public List<ushort> uniques = new List<ushort>();
        public ZooJob(BinaryReader br, float ver)
        {
            caption.read(br, ver);
            subcount.read(br, ver);
            for (int i = 0; i < (int)subcount.value(); i++)
                subs.Add(new MFString(br, ver));
            type.read(br, ver);
            uniquer.read(br, ver);
            uniquescnt.read(br, ver);
            for (int i = 0; i < (int)uniquescnt.value(); i++)
                uniques.Add(br.ReadUInt16());
        }
        public String log()
        {
            String str = " -=ZooJob=-\r\n ";
            str += caption.log() + "\r\n " + subcount.log() + "\r\n ";
            for (int i = 0; i < (int)subcount.value(); i++)
                str += "sub" + i.ToString() + "=" + subs[i].value() + "\r\n ";
            str += type.log() + "\r\n " + uniquer.log() + "\r\n " + uniquescnt.log() + "\r\n uniques=";
            for (int i = 0; i < (int)uniquescnt.value(); i++)
                str += String.Format("{0:d}({0:X}) ",uniques[i]);
            return str + "\r\n -=ZooJob_end=-";
        }
    }

    class MFZooForm : MFCommon, IMFCommon
    {
        public MFInt items = new MFInt("items");
        public MFDate zoodate = new MFDate("zoodate");
        public MFUShort donorcnt = new MFUShort("donotcnt");
        public List<Donor> donors = new List<Donor>();
        public MFUShort zoocnt = new MFUShort("zoocnt");
        public List<ZooJob> zoojobs = new List<ZooJob>();
        public MFInt strcount = new MFInt("strcount");
        public List<MFString> strings = new List<MFString>();
        public MFZooForm(String name, int li) : base(name, li) { }
        public void read(BinaryReader br, float ver)
        {
            items.read(br,ver);
            if (items.value()>0)
            {
                zoodate.read(br,ver);
                donorcnt.read(br, ver);
                for (int i = 0; i < (int)donorcnt.value(); i++)
                    donors.Add(new Donor(br, ver));
                zoocnt.read(br, ver);
                for (int i = 0; i < (int)zoocnt.value(); i++)
                    zoojobs.Add(new ZooJob(br, ver));
                if (ver > 5.1)
                {
                    strcount.read(br, ver);
                    for (int i = 0; i < strcount.value();i++)
                        strings.Add(new MFString(br, ver));
                }
            }
        }
        public override string  strval()
        {
            String str = "-=ZooForm=-\r\n";
            str += items.log() + "\r\n";
            if (items.value() > 0)
            {
                str += zoodate.log() + "\r\n" + donorcnt.log() + "\r\n";
                for (int i = 0; i < (int)donorcnt.value(); i++)
                    str += "don" + i.ToString() + "=" + donors[i].log() + "\r\n";
                str += zoocnt.log() + "\r\n";
                for (int i = 0; i < (int)zoocnt.value(); i++)
                    str += "zoo" + i.ToString() + "=" + zoojobs[i].log() + "\r\n";
                str += strcount.log() + "\r\n";
                for (int i = 0; i < strcount.value(); i++)
                    str += "str" + i.ToString() + "=" + strings[i].value() + "\r\n";
            }
            return str + "-=ZooForm_End=-";
        }
    }

    class MFGraphForm : MFCommon, IMFCommon
    {
        public MFListView reasons = new MFListView("reasons");
        public MFListView workers = new MFListView("workers");
        public MFListView lost = new MFListView("lost");
        private MiaFile mf;
        public MFGraphForm(String name, int li,MiaFile mf) : base(name, li) {
            this.mf = mf;
        }
        public void read(BinaryReader br, float ver)
        {
            reasons.read(br, ver);
            workers.read(br, ver);
            lost.read(br, ver,mf);
        }
        public override string strval()
        {
            return "-=GraphForm=-\r\n"+reasons.log() + "\r\n" + workers.log() + "\r\n" + lost.log()+"\r\n-=GraphForm_End=-";
        }
    }

    class ArcPlan
    {
        public MFDate date = new MFDate("date");
        public MFInt count = new MFInt("count");
        public List<MFStringList> works = new List<MFStringList>();
        public ArcPlan(BinaryReader br,float ver)
        {
            date.read(br, ver);
            count.read(br, ver);
            for (int i = 0; i < count.value(); i++)
                works.Add(new MFStringList(br, ver));
        }
        public String log()
        {
            String str=" -=ArcWork=-\r\n "+date.log()+"\r\n "+count.log()+"\r\n ";
            for (int i = 0; i < count.value(); i++)
                str += "work" + i.ToString() + "=" + works[i].log() + "\r\n ";
            return str+"-=ArcWork end=-";
        }
    }

    class MFArchiveForm : MFCommon, IMFCommon
    {
        public MFInt size = new MFInt("size");
        public List<ArcPlan> plans = new List<ArcPlan>();
        public MFRabbits dead=null;
        public MFListView dead2 = new MFListView("dead");
        public float dver = 0;
        public MFArchiveForm(String name, int li, MFRabNames males, MFRabNames females) : base(name, li)
        {
            dead = new MFRabbits("dead", males, females,null);
        }
        public void read(BinaryReader br, float ver)
        {
            dver = ver;
            size.read(br, ver);
            for (int i = 0; i < size.value(); i++)
                plans.Add(new ArcPlan(br, ver));
            if (ver > 5.1)
                dead.read(br, ver);
            else
                dead2.read(br, ver);
        }
        public override string strval()
        {
            String str="-=ArchiveForm=-\r\n"+size.log()+"\r\n";
            for (int i = 0; i < size.value(); i++)
                str += "plan" + i.ToString() + "=" + plans[i].log() + "\r\n";
            if (dver > 5.1)
                str += dead.log() + "\r\n";
            else
                str += dead2.log() + "\r\n";
            return str+"-=ArchiveForm_End=-";
        }
    }

    class Weighter
    {
        public MFByte on = new MFByte("on");
        public MFUShort start = new MFUShort("start");
        public MFUShort interval = new MFUShort("interval");
        public Weighter(BinaryReader br, float ver)
        {
            on.read(br, ver);
            start.read(br, ver);
            interval.read(br, ver);
        }
        public String log()
        {
            return on.log() + " " + start.log() + " " + interval.log();
        }
    }

    class MFWeightList : MFCommon, IMFCommon
    {
        public MFByte count = new MFByte("count");
        public List<Weighter> weighters = new List<Weighter>();
        public MFByte laston = new MFByte("laston");
        public MFUShort lastpos = new MFUShort("lastvalue");
        public MFWeightList(String name, int li) : base(name, li) { }
        public void read(BinaryReader br, float ver)
        {
            count.read(br, ver);
            for (int i = 0; i < (int)count.value(); i++)
                weighters.Add(new Weighter(br, ver));
            laston.read(br, ver);
            lastpos.read(br, ver);
        }

        public override String strval()
        {
            String str = "-=WeightList=-\r\n"+count.log()+"\r\n";
            for (int i = 0; i < (int)count.value(); i++)
                str += "wgt" + i.ToString() + "=" + weighters[i].log()+"\r\n";
            str += laston.log() + "\r\n" + lastpos.log() + "\r\n";
            return str += "-=WeightList_End=-";
        }
    }

}
