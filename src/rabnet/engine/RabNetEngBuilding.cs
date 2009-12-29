using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetEngBuilding
    {
        class ExBadBuildingType : ApplicationException
        {
            public ExBadBuildingType() : base("Неверный тип минифермы.") { }
        }

        private int id=0;
        private Building b;
        private RabNetEngine eng;
        public RabNetEngBuilding(int tid,RabNetEngine eng)
        {
            this.id=tid;
            this.eng = eng;
            b = eng.db().getBuilding(id);
        }
        public int tid{get{return id;}}

        public void commit()
        {
            eng.db().updateBuilding(b);
        }

        public void setRepair(bool value)
        {
            b.frepair = value;
            commit();
        }
        public void setNest(bool value)
        {
            b.fnests = (value ? "1" : "0")+b.fnests.Substring(1);
            commit();
        }
        public void setNest2(bool value)
        {
            b.fnests=b.fnests.Substring(0,1)+(value ? '1' : '0');
            commit();
        }
        public void setHeater(int value)
        {
            if (value == 2) value = 3;
            b.fheaters = String.Format("{0:D1}",value) + b.fheaters.Substring(1);
            commit();
        }
        public void setHeater2(int value)
        {
            if (value == 2) value = 3;
            b.fheaters = b.fheaters.Substring(0,1)+String.Format("{0:D1}", value);
            commit();
        }
        public void setDelim(bool value)
        {
            b.fdelims = (value ? "1" : "0") + b.fdelims.Substring(1);
            commit();
        }
        public void setDelim1(bool value)
        {
            setDelim(value);
        }
        public void setVigul(int value)
        {
            setDelim(value == 1);
        }
        public void setDelim2(bool value)
        {
            b.fdelims = b.fdelims.Substring(0, 1) + (value ? "1" : "0")+b.fdelims.Substring(2);
            commit();
        }
        public void setDelim3(bool value)
        {
            b.fdelims = b.fdelims.Substring(0,2)+(value ? "1" : "0");
            commit();
        }

    }
}
