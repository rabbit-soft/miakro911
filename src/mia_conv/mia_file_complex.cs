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
                    curnode.Parent.Parent.Nodes.Add(nd);
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
        public static String[] masks = { "awpP", "aspP", "SanbwpP", "anWkpP", "S", "amBNnA", "amXbsnA", "anWk", "SaWpkP","" };
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
        public static int count = 34;
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
}
