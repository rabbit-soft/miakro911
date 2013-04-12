using System;
using System.Collections.Generic;
using System.Text;

namespace rabnet
{
    public class RabNetEngBuilding 
    {
        private const int HEATER_UNSET = 0;
        private const int HEATER_OFF = 1;
        private const int HEATER_ON = 3;

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
        private RabNetEngine _eng;

        public RabNetEngBuilding(int tid,RabNetEngine eng)
        {
            this.id=tid;
            this._eng = eng;
            b = eng.db().getBuilding(id);
        }
        public static RabNetEngBuilding FromPlace(string place,RabNetEngine eng)
        {
            String[] p = place.Split(',');
            int[] tiers = eng.db().getTiers(int.Parse(p[0]));
            return new RabNetEngBuilding(tiers[(p[1] == "2") ? 1 : 0], eng);
        }
        public int tid{get{return id;}}

        public void commit()
        {
            _eng.db().updateBuilding(b);
        }

        public void setRepair(bool value)
        {
            if (b.Repair == value)
                return;
            if (value)
            {
                for (int i = 0; i < b.Sections; i++)
                    if (b.Busy[i].ID != 0)
                        throw new ExFarmNotEmpty();
            }
            _eng.logs().log(value ? LogType.REPAIR_ON : LogType.REPAIR_OFF, 0, b.Farm.ToString());
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
            _eng.logs().log(value ? LogType.NEST_ON : LogType.NEST_OFF, b.Busy[0].ID, b.SmallName(0));
            b.Nests = (value ? "1" : "0")+b.Nests.Substring(1);
            commit();

            if (!value && _eng.options().getBoolOption(Options.OPT_ID.NEST_OUT_WITH_HEATER))
                setHeater(HEATER_UNSET);
        }
        /// <summary>
        /// Установка гнездовья в клетку Б
        /// </summary>
        /// <param name="value">установить или убрать</param>
        public void setNest2(bool value)
        {
            if (b.Nests[1] == (value ? '1' : '0'))
                return;
            _eng.logs().log(value ? LogType.NEST_ON : LogType.NEST_OFF, b.Busy[1].ID,b.SmallName(1));
            b.Nests = b.Nests.Substring(0, 1) + (value ? '1' : '0');
            commit();

            if(!value && _eng.options().getBoolOption(Options.OPT_ID.NEST_OUT_WITH_HEATER))
                setHeater2(HEATER_UNSET);
        }

        public void setHeater(int value)
        {
            if (value == 2 || value>3) value = 3;
            if (b.Heaters[0] == value.ToString()[0])
                return;
            LogType tp = LogType.HEATER_OUT;
            if (value == 1) tp = LogType.HEATER_OFF;
            if (value == 3) tp = LogType.HEATER_ON;
            _eng.logs().log(tp, b.Busy[0].ID, b.SmallName(0));
            b.Heaters = String.Format("{0:D1}",value) + b.Heaters.Substring(1);
            commit();
        }

        public void setHeater2(int value)
        {
            if (value == 2 || value > 3) value = 3;
            if (b.Heaters[1] == value.ToString()[0])
                return;
            LogType tp = LogType.HEATER_OUT;
            if (value == 1) tp = LogType.HEATER_OFF;
            if (value == 3) tp = LogType.HEATER_ON;
            _eng.logs().log(tp, b.Busy[1].ID, b.SmallName(1));
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

        public BuildingType Type { get { return b.Type; } }

        /// <summary>
        /// Удаляет гнездовье там где сидит кролик
        /// </summary>
        /// <param name="rId"></param>
        public void RabbitNestOut(int rId)
        {
            int ind = -1;
            for (int i = 0; i < b.Busy.Length;i++ )          
                if (b.Busy[i].ID == rId)
                {
                    ind = i;
                    break;
                }
            if (ind == -1 || ind>=b.Nests.Length) return;

            if (b.Nests[ind] == '1')
            {
                if (ind == 0)
                    setNest(false);
                else if (ind == 1)
                    setNest2(false);
            }           
        }
    }
}
