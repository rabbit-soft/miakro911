using System;
using System.Collections.Generic;
using System.IO;


namespace mia_conv
{
    class DBG
    {
        public static void dbg(String s)
        {
            Console.WriteLine(s);
        }
        public static void dbg1(String s)
        {//dbg(s);
        }
        public static void dbg2(String s)
        {//dbg(s);
        }
        public static void dbg2(String s, BinaryReader br)
        {

            //dbg(s+" "+String.Format("@{0:X}",br.BaseStream.Position));
        }
        public static void dbg3(String s) { dbg(s); }


    }


    interface IMFCommon
    {
        void read(BinaryReader stream, float ver);
        String log();
        String strval();
        int logindex();
    }

    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    class MFCommon
    {
        protected String name;
        private int lind = 0;
        public int tag = 0;
        public MFCommon(String sname) : this(sname, -1) { }
        public MFCommon(String sname, int li)
        {
            name = sname;
            lind = li;
        }
        public virtual String strval() { throw new Exception("Not Implemented"); }
        public String log()
        {
            return name + "=" + strval();
        }
        public int logindex() { return lind; }
    }

    class MFString : MFCommon, IMFCommon
    {
        private String val;
        public MFString(String sname) : base(sname) { }
        public MFString(String sname, int li) : base(sname, li) { }
        public MFString(BinaryReader br, float ver)
            : this("")
        {
            read(br, ver);
        }
        public void read(BinaryReader stream, float ver)
        {
            Byte sz = stream.ReadByte();
            val = "";
            for (int i = 0; i < sz; i++) {
                val += stream.ReadChar();
            }
        }
        public String value()
        {
            return val;
        }
        public override String strval() { return value(); }
    }

    class MFUInt : MFCommon, IMFCommon
    {
        private UInt32 sz;
        protected UInt64 val;
        public MFUInt(String name) : this(4, name) { }
        public MFUInt(UInt32 size, String name) : this(4, name, -1) { }
        public MFUInt(String name, int li) : this(4, name, li) { }
        public MFUInt(UInt32 size, String name, int li) : base(name, li) { sz = size; }
        public UInt64 value()
        {
            return val;
        }
        public override String strval()
        {
            return String.Format("{0:d} (0x{0:X})", val);
        }
        public virtual void read(BinaryReader br, float ver)
        {
            val = br.ReadUInt32();
        }
    }

    class MFInt : MFCommon, IMFCommon
    {
        private UInt32 sz;
        protected Int64 val;
        public MFInt(String name) : this(4, name) { }
        public MFInt(UInt32 size, String name) : this(4, name, -1) { }
        public MFInt(String name, int li) : this(4, name, li) { }
        public MFInt(UInt32 size, String name, int li) : base(name, li) { sz = size; }
        public Int64 value()
        {
            return val;
        }
        public override String strval()
        {
            return String.Format("{0:d} (0x{0:X})", val);
        }
        public virtual void read(BinaryReader br, float ver)
        {
            val = br.ReadInt32();
        }
    }

    class MFChar : MFInt, IMFCommon
    {
        public MFChar(String name) : base(name) { }
        public MFChar(String name, int li) : base(2, name, li) { }
        public override void read(BinaryReader br, float ver) { val = br.ReadInt16(); }
    }
    class MFSByte : MFInt, IMFCommon
    {
        public MFSByte(String name) : base(name) { }
        public MFSByte(String name, int li) : base(1, name, li) { }
        public override void read(BinaryReader br, float ver) { val = br.ReadSByte(); }
    }

    class MFByte : MFUInt, IMFCommon
    {
        public MFByte(String name) : base(name) { }
        public MFByte(String name, int li) : base(1, name, li) { }
        public override void read(BinaryReader br, float ver) { val = br.ReadByte(); }
    }

    class MFUShort : MFUInt, IMFCommon
    {
        public MFUShort(String name) : base(name) { }
        public MFUShort(String name, int li) : base(2, name, li) { }
        public override void read(BinaryReader br, float ver) { val = br.ReadUInt16(); }
    }

    class MFULong : MFUInt, IMFCommon
    {
        public MFULong(String name) : base(name) { }
        public MFULong(String name, int li) : base(4, name, li) { }
        public override void read(BinaryReader br, float ver) { val = br.ReadUInt32(); }
    }

    class MFDate : MFCommon, IMFCommon
    {
        private DateTime val;
        private ushort usval;
        public MFDate(String name) : base(name) { }
        public MFDate(String name, int li) : base(name, li) { }
        public DateTime value() { return val; }
        public ushort binary_value() { return usval; }
        public void read(BinaryReader br, float ver)
        {
            usval = br.ReadUInt16();
            val = new DateTime(1899, 12, 30).AddDays(usval);
        }
        public override String strval()
        {
            return val.ToShortDateString() + " (" + String.Format("{0:d}=0x{0:X}", usval) + ")";
        }

    }

    class MFStringList : MFCommon, IMFCommon
    {
        public MFUShort count = new MFUShort("count");
        public List<MFString> strings = new List<MFString>();
        public MFStringList(String name) : base(name) { }
        public MFStringList(String name, int li) : base(name, li) { }
        public MFStringList(BinaryReader br, float ver) : base("") { read(br, ver); }
        public void read(BinaryReader br, float ver)
        {
            count.read(br, ver);
            for (int i = 0; i < (int)count.value(); i++) {
                strings.Add(new MFString(br, ver));
            }
        }

        public override string strval()
        {
            String str = "-=String_list=-\r\n";
            str += count.log() + "\r\n";
            for (int i = 0; i < (int)count.value(); i++) {
                str += String.Format("s{0:d}{1:s}\r\n", i, strings[i].log());
            }
            str += "-=string_list_end=-";
            return str;
        }
    }

    class MFListItem : MFCommon, IMFCommon
    {
        public MFString caption = new MFString("caption");
        public MFByte subitems = new MFByte("subitems");
        public List<MFString> subs = new List<MFString>();
        public MFListItem() : base("li") { }
        public MFListItem(BinaryReader br, float ver)
            : base("li")
        {
            read(br, ver);
        }
        public void read(BinaryReader br, float ver)
        {
            caption.read(br, ver);
            subitems.read(br, ver);
            for (int i = 0; i < (int)subitems.value(); i++)
                subs.Add(new MFString(br, ver));
        }
        public override string strval()
        {
            String str = caption.log() + " " + subitems.log() + " ";
            for (int i = 0; i < (int)subitems.value(); i++) {
                str += "s" + i.ToString() + "=" + subs[i].value() + " ";
            }
            return str;
        }

    }

    class MFListView : MFCommon, IMFCommon
    {
        public MFInt size = new MFInt("size");
        public List<MFListItem> items = new List<MFListItem>();
        public MFListView(String name, int li) : base(name, li) { }
        public MFListView(String name) : base(name) { }
        public void read(BinaryReader br, float ver, MiaFile mf)
        {
            size.read(br, ver);
            int cnt = (int)size.value();
            for (int i = 0; i < size.value(); i++) {
                if (mf != null) {
                    mf.Setpb(i, cnt);
                }
                items.Add(new MFListItem(br, ver));
            }

        }
        public void read(BinaryReader br, float ver)
        {
            read(br, ver, null);
        }
        public override String strval()
        {
            String str = "-=ListView=-\r\n";
            str += size.log() + Environment.NewLine;
            for (int i = 0; i < size.value(); i++) {
                str += "item" + i.ToString() + "=" + items[i].log() + Environment.NewLine;
            }
            return str + "-=ListView_End=-";
        }
    }


}
