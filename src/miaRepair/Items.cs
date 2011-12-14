using System;
using System.Collections.Generic;
using System.Text;

namespace miaRepair
{
    class repRabbit
    {
        internal readonly int rID;
        /// <summary>
        /// ID матери
        /// </summary>
        internal int Mother;
        /// <summary>
        /// ID отца
        /// </summary>
        internal int Father;
        internal Sex Sex;
        internal int NameId;
        /// <summary>
        /// Фамилия по матери
        /// </summary>
        internal int SurnameID;
        /// <summary>
        /// Фамилия по отцу
        /// </summary>
        internal int SecnameID;
        internal DateTime Born = DateTime.MinValue;
        internal int ParentID = 0;
        internal DateTime EventDate;
        internal string Name;

        internal repRabbit(int rid, int mother, int father, string sex, int name, int surname, int secname, DateTime born, int parent, DateTime ev_date, string namestr)
        {
            this.rID = rid;
            this.Mother = mother;
            this.Father = father;
            switch (sex)
            {
                case "male": this.Sex = Sex.Male; break;
                case "female": this.Sex = Sex.Female; break;
                case "void": this.Sex = Sex.Void; break;
            }
            this.NameId = name;
            this.SurnameID = surname;
            this.SecnameID = secname;
            this.Born = born;
            this.ParentID = parent;
            this.EventDate = ev_date;
            this.Name = namestr;
        }
    }

    class repName
    {
        internal readonly int nID;
        internal readonly Sex nameSex;
        internal readonly string NameStr;
        internal int useRabbit;
        internal readonly bool Blocked = false;
        internal readonly DateTime BlockDate;

        internal repName(int id, string sex, string name, int use, DateTime block)
        {
            this.nID = id;
            switch (sex)
            {
                case "male": this.nameSex = Sex.Male; break;
                case "female": this.nameSex = Sex.Female; break;
                case "void": this.nameSex = Sex.Void; break;
            }
            this.NameStr = name;
            this.useRabbit = use;
            if (block != DateTime.MinValue)
                this.Blocked = true;
            this.BlockDate = block;
        }
    }

    class repFuck
    {
        internal enum State { Sukrol, Okrol, Proholost };

        internal readonly int fID;
        private int _sheID;
        private int _heID;
        private DateTime _startDate;
        private DateTime _endDate;
        private State _fState;
        private int _children;

        private bool _modified = false;

        internal repFuck(int id, int rabid, int partner, DateTime date, DateTime end_date, string state, int children)
        {
            this.fID = id;
            _sheID = rabid;
            _heID = partner;
            this.StartDate = date;
            this.EndDate = end_date;
            switch (state)
            {
                case "okrol": _fState = State.Okrol; break;
                case "sukrol": _fState = State.Sukrol; break;
                case "proholost": _fState = State.Proholost; break;
            }
            _children = children;
        }
        /// <summary>
        /// Были ли внесены изменения
        /// </summary>
        internal bool Modified { get { return _modified; } }
        /// <summary>
        /// ID самца
        /// </summary>
        internal int HeID { get { return _heID; } set { _heID = value; _modified = true; } }
        /// <summary>
        /// ID крольчихи
        /// </summary>
        internal int SheID { get { return _sheID; } set { _sheID = value; _modified = true; } }
        internal State FuckState { get { return _fState; } set { _fState = value; _modified = true; } }
        internal int Children { get { return _children; } set { _children = value; _modified = true; } }
        internal DateTime StartDate { get { return _startDate; } set { _startDate = value; _modified = true; } }
        internal DateTime EndDate { get { return _endDate; } set { _endDate = value; _modified = true; } }
    }

    class repTier
    {
        internal readonly int tID;
        internal String Type;
        private int _busy1;
        private int _busy2;
        private int _busy3;
        private int _busy4;
        private bool _modivied = false;

        internal repTier(int id, string type, int busy1, int busy2, int busy3, int busy4)
        {
            tID = id;
            Type = type;
            _busy1 = busy1;
            _busy2 = busy2;
            _busy3 = busy3;
            _busy4 = busy4;
        }
        internal bool Modified {get{return _modivied;}}

        internal int Busy1 { get { return _busy1; } set { _busy1 = value; _modivied = true; } }
        internal int Busy2 { get { return _busy2; } set { _busy2 = value; _modivied = true; } }
        internal int Busy3 { get { return _busy3; } set { _busy3 = value; _modivied = true; } }
        internal int Busy4 { get { return _busy4; } set { _busy4 = value; _modivied = true; } }

    }
}
