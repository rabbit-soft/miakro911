using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetEngBuilding
    {
        public class ExBadBuildingType : ApplicationException
        {
            public ExBadBuildingType() : base("Неверный тип минифермы.") { }
        }
        public class ExFarmNotEmpty : ApplicationException
        {
            public ExFarmNotEmpty() : base("Ферма не пуста") { }
        }

        private int id = 0;
        private Building b;
        private RabNetEngine eng;
        public RabNetEngBuilding(int tid,RabNetEngine eng)
        {
            this.id=tid;
            this.eng = eng;
            b = eng.db().getBuilding(id);
        }
        public static RabNetEngBuilding fromPlace(string place,RabNetEngine eng)
        {
            String[] p = place.Split(',');
            int[] tiers = eng.db().getTiers(int.Parse(p[0]));
            return new RabNetEngBuilding(tiers[(p[1] == "2") ? 1 : 0], eng);
        }
        public int tid{get{return id;}}

        public void commit()
        {
            eng.db().updateBuilding(b);
        }

        public void setRepair(bool value)
        {
            if (b.Repair == value)
                return;
            if (value)
            {
                for (int i = 0; i < b.Sections; i++)
                    if (b.Busy[i] != 0)
                        throw new ExFarmNotEmpty();
            }
            eng.logs().log(value ? RabNetLogs.LogType.REPAIR_ON : RabNetLogs.LogType.REPAIR_OFF, 0, b.Farm.ToString());
            b.Repair = value;
            commit();
        }
        /// <summary>
        /// Установка гнездовья в клетку А
        /// </summary>
        /// <param name="value">установить или убрать</param>
        public void setNest(bool value)
        {
            if (b.Nests[0] == (value ? '1' : '0'))
                return;
            eng.logs().log(value ? RabNetLogs.LogType.NEST_ON : RabNetLogs.LogType.NEST_OFF, b.Busy[0], b.smallname[0]);
            b.Nests = (value ? "1" : "0")+b.Nests.Substring(1);
            commit();
        }
        /// <summary>
        /// Установка гнездовья в клетку Б
        /// </summary>
        /// <param name="value">установить или убрать</param>
        public void setNest2(bool value)
        {
            if (b.Nests[1] == (value ? '1' : '0'))
                return;
            eng.logs().log(value ? RabNetLogs.LogType.NEST_ON : RabNetLogs.LogType.NEST_OFF, b.Busy[1],b.smallname[1]);
            b.Nests = b.Nests.Substring(0, 1) + (value ? '1' : '0');
            commit();
        }

        public void setHeater(int value)
        {
            if (value == 2) value = 3;
            if (b.Heaters[0] == value.ToString()[0])
                return;
            RabNetLogs.LogType tp = RabNetLogs.LogType.HEATER_OUT;
            if (value == 1) tp = RabNetLogs.LogType.HEATER_OFF;
            if (value == 3) tp = RabNetLogs.LogType.HEATER_ON;
            eng.logs().log(tp, b.Busy[0], b.smallname[0]);
            b.Heaters = String.Format("{0:D1}",value) + b.Heaters.Substring(1);
            commit();
        }

        public void setHeater2(int value)
        {
            if (value == 2) value = 3;
            if (b.Heaters[1] == value.ToString()[0])
                return;
            RabNetLogs.LogType tp = RabNetLogs.LogType.HEATER_OUT;
            if (value == 1) tp = RabNetLogs.LogType.HEATER_OFF;
            if (value == 3) tp = RabNetLogs.LogType.HEATER_ON;
            eng.logs().log(tp, b.Busy[1], b.smallname[1]);
            b.Heaters = b.Heaters.Substring(0, 1) + String.Format("{0:D1}", value);
            commit();
        }

        public void setDelim(bool value)
        {
            b.Delims = (value ? "1" : "0") + b.Delims.Substring(1);
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
            b.Delims = b.Delims.Substring(0, 1) + (value ? "1" : "0")+b.Delims.Substring(2);
            commit();
        }

        public void setDelim3(bool value)
        {
            b.Delims = b.Delims.Substring(0,2)+(value ? "1" : "0");
            commit();
        }

        public string type { get { return b.TypeName; } }

    }
}
