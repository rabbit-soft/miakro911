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

        public class ExBadBuildingType : RabNetException
        {
            public ExBadBuildingType() : base("Неверный тип минифермы.") { }
        }
        public class ExFarmNotEmpty : RabNetException
        {
            public ExFarmNotEmpty() : base("Ферма не пуста") { }
        }
        public class ExDelimSetDenied : RabNetException
        {
            public ExDelimSetDenied() : base("Необходимо сначала расселить смежные клетки.") { }
        }

        private int id = 0;

        private Building b;

        private RabNetEngine _eng;

        public RabNetEngBuilding(int tierId, RabNetEngine eng)
        {
            this.id = tierId;
            this._eng = eng;
            b = eng.db().getBuilding(id);
        }

        public static RabNetEngBuilding FromPlace(string place, RabNetEngine eng)
        {
            RabPlace rp = RabPlace.Parse(place);            
            int[] tiers = eng.db().getTiers(rp.Farm);
            return new RabNetEngBuilding(tiers[rp.Floor == 2 ? 1 : 0], eng);
        }

        public int tid 
        { 
            get { return id; }
        }

        public void commit()
        {
            _eng.db().updateBuilding(b);
        }

        public void setRepair(bool value)
        {
            if (b.Repair == value) {
                return;
            }
            if (value) {
                for (int i = 0; i < b.Sections; i++) {
                    if (b.Busy[i].ID != 0) {
                        throw new ExFarmNotEmpty();
                    }
                }
            }
            _eng.logs().log(value ? LogType.REPAIR_ON : LogType.REPAIR_OFF, 0, b.Farm.ToString());
            b.Repair = value;
            this.commit();
        }
        /// <summary>
        /// Установка гнездовья в клетку А
        /// </summary>
        /// <param name="value">установить или убрать</param>
        public void setNest(bool value, int sec = 0)
        {
            char newValue = value ? '1' : '0';
            if (b.Nests[sec] == newValue) {
                return;
            }
            _eng.logs().log(value ? LogType.NEST_ON : LogType.NEST_OFF, b.Busy[sec].ID, b.SmallName(sec));
            
            char [] cNests = b.Nests.ToCharArray();
            cNests[sec] = newValue;
            b.Nests = new String(cNests);
            this.commit();

            if (!value && _eng.options().getBoolOption(Options.OPT_ID.NEST_OUT_WITH_HEATER)) {
                this.setHeater(HEATER_UNSET, sec);
            }
        }

        public void setHeater(int value, int sec = 0)
        {
            if (value == 2 || value > 3) { value = 3; }
            if (b.Heaters[sec] == value.ToString()[0]) {
                return;
            }
            LogType tp = LogType.HEATER_OUT;
            if (value == 1) { tp = LogType.HEATER_OFF; }
            if (value == 3) { tp = LogType.HEATER_ON; }
            _eng.logs().log(tp, b.Busy[sec].ID, b.SmallName(sec));

            b.Heaters = String.Format("{0:D1}", value) + b.Heaters.Substring(1);
            char[] cHeaters = b.Heaters.ToCharArray();
            cHeaters[sec] = (char)value;
            b.Heaters = new String(cHeaters);

            this.commit();
        }

        public void SetOneDelim(bool value)
        {
            if (b.Busy[0].ID != 0 || b.Busy[1].ID != 0) {
                throw new ExDelimSetDenied();
            }
            b.Delims = (value ? "1" : "0") + b.Delims.Substring(1);
            this.commit();
        }

        public void setVigul(int value)
        {
            SetOneDelim(value == 1);
        }

        public void SetDelim1(bool value)
        {
            SetOneDelim(value);
        }

        public void SetDelim2(bool value)
        {
            if (b.Busy[1].ID != 0 || b.Busy[2].ID != 0) {
                throw new ExDelimSetDenied();
            }
            b.Delims = b.Delims.Substring(0, 1) + (value ? "1" : "0") + b.Delims.Substring(2);
            this.commit();
        }

        public void SetDelim3(bool value)
        {
            if (b.Busy[2].ID != 0 || b.Busy[3].ID != 0) {
                throw new ExDelimSetDenied();
            }
            b.Delims = b.Delims.Substring(0, 2) + (value ? "1" : "0");
            this.commit();
        }

        public BuildingType Type
        {
            get { return b.Type; }
        }

        /// <summary>
        /// Удаляет гнездовье там где сидит кролик
        /// </summary>
        /// <param name="rId"></param>
        public void RabbitNestOut(int rId)
        {
            int ind = -1;
            for (int i = 0; i < b.Busy.Length; i++) {
                if (b.Busy[i].ID == rId) {
                    ind = i;
                    break;
                }
            }

            if (ind == -1 || ind >= b.Nests.Length) {
                return;
            }

            if (b.Nests[ind] == '1') {                
                setNest(false, ind);                
            }
        }
    }
}
