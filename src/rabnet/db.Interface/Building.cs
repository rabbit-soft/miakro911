using System;

namespace rabnet
{
    public static class BuildingType
    {
        public const string Female = "female";
        public const string Female_Rus = "Крольчихин";
        public const string Female_Short = "крлч";

        public const string DualFemale = "dfemale";
        public const string DualFemale_Rus = "Двукрольчихин";
        public const string DualFemale_Short = "2крл";

        public const string Jurta = "jurta";
        public const string Jurta_Rus = "Юрта";
        public const string Jurta_Short = "юрта";

        public const string Quarta = "quarta";
        public const string Quarta_Rus = "Кварта";
        public const string Quarta_Short = "кврт";

        public const string Vertep = "vertep";
        public const string Vertep_Rus = "Вертеп";
        public const string Vertep_Short = "вртп";

        public const string Barin = "barin";
        public const string Barin_Rus = "Барин";
        public const string Barin_Short = "барн";

        public const string Cabin = "cabin";
        public const string Cabin_Rus = "Хижина";
        public const string Cabin_Short = "хижн";

        public const string Complex = "complex";
        public const string Complex_Rus = "Комплексный";
        public const string Complex_Short = "кмпл";
    }

    public class Building : IData
    {
        public readonly int ID;
        public readonly int Farm;
        public readonly int TierID;
        public readonly int Sections;
        public String[] Areas;
        public String[] fdeps;
        public readonly string TypeName;
        public readonly string TypeName_Rus;
        public string Delims;
        public string[] Notes;
        public bool Repair;
        public string Nests;
        public string Heaters;
        public string Address;
        public string[] fuses;
        public int[] Busy;
        public int NestHeaterCount;
        public string[] fullname;
        public string[] smallname;
        public string[] medname;

        public Building(int id, int farm, int tier_id, string type, string typeLoc, string delims, string notes, bool repair, int seccnt)
        {
            ID = id;
            this.Farm = farm;
            TierID = tier_id;
            TypeName = type;
            TypeName_Rus = typeLoc;
            Delims = delims;            
            Repair = repair;
            Sections = seccnt;
            fullname = new string[Sections];
            smallname = new string[Sections];
            medname = new string[Sections];
            Notes = new string[Sections];

            string[] ntsTmp = notes.Split(new char[]{'|'});
            for (int i = 0; i < Sections; i++)
            {
                fullname[i] = Building.FullRName(Farm, TierID, i, TypeName, Delims, false, true, true);
                smallname[i] = Building.FullRName(Farm, TierID, i, TypeName, Delims, true, false, false);
                medname[i] = Building.FullRName(Farm, TierID, i, TypeName, Delims, false, true, false);
                Notes[i] = i<ntsTmp.Length?ntsTmp[i]:"";
            }
            
        }
        #region IBuilding Members
        public string dep(int id) { return fdeps[id]; }
        public string use(int id) { return fuses[id]; }
        #endregion

        /**
         * Гамбит не знал как лучше сделать строку, которую можно установить в начале программы,
         * ее мог использовать класс Building и BuildingPanel    
         */
        #region format
        private static int _smbls = 6;
        private static char _dsym = ' ';

        /// <summary>
        /// Установить формат адреса МИНИфермы (По просьбе Татищево)
        /// </summary>
        /// <param name="symbols">Сколько цифр в строке адреса</param>
        /// <param name="defchar">Символ заполнитель</param>
        public static void SetDefFmt(int symbols, char defchar)
        {
            if (symbols < 4) _smbls = 4;
            else if (symbols > 10) _smbls = 10;
            else _smbls = symbols;
            if (defchar != '/' && defchar != '\\') _dsym = defchar;
        }
        public static void SetDefFmt(char defchar)
        {
            SetDefFmt(_smbls, defchar);
        }
        public static void SetDefFmt(int symbols)
        {
            SetDefFmt(symbols, _dsym);
        }

        /// <summary>
        /// Правило форматирования Номера клетки
        /// </summary>
        public static string Format(int farmN, int symbols, char defchar)
        {
            if (symbols < 4) symbols = 4;
            if (symbols > 10) symbols = 10;
            if (defchar == '/' || defchar == '\\') defchar = _dsym;

            string res = farmN.ToString();
            if (res.Length < symbols)
            {
                while (symbols != res.Length)
                    res = defchar + res;
            }
            return res;
        }
        /// <summary>
        /// Приводит номер фермы к нужному виду 
        /// (начало заполнено нулями или пробелами)
        /// </summary>
        /// <param name="farmN">Номер фермы</param>
        /// <returns></returns>
        public static string Format(int farmN)
        {
            return Format(farmN, _smbls, _dsym);
        }
        public static string Format(string farmN)
        {
            int fn = 0;
            int.TryParse(farmN, out fn);
            return fn == 0 ? farmN : Format(fn, _smbls, _dsym);
        }

        #endregion

#region static

        public static bool HasNest(String type, int sec, String nests)
        {
            int c = GetRNHCount(type);
            if (c == 0) return false;
            if (type == BuildingType.DualFemale)
                return (nests[sec] == '1');
            return (nests[0] == '1');
        }

        public static String GetRDescr(String type, bool shr, int sec, String delims)
        {
            String res = "";
            switch (type)
            {
                case BuildingType.Female:
                case BuildingType.DualFemale: res = shr ? "гн+выг" : "гнездовое+выгул"; break;
                case BuildingType.Complex:
                    if (sec == 0)
                        res = shr ? "гн+выг" : "гнездовое+выгул";
                    else
                        res = shr ? "отк" : "откормочное";
                    break;
                case BuildingType.Jurta:
                    if (sec == 0)
                    {
                        if (delims[0] == '0')
                            res = (shr ? "гн" : "гнездовое") + "+";
                        res += shr ? "мвг" : "м.выгул";
                    }
                    else
                    {
                        if (delims[0] == '1')
                            res = (shr ? "гн" : "гнездовое") + "+";
                        res += shr ? "бвг" : "б.выгул";
                    }
                    break;
                case BuildingType.Cabin:
                case BuildingType.Quarta: res = shr ? "отк" : "откормочное"; break;
                case BuildingType.Vertep:
                case BuildingType.Barin: res = shr ? "врт" : "Вертеп"; break;
            }
            return res;
        }
        public static String GetRSec(String type, int sec, String delims)
        {
            if (type == BuildingType.Female)
                return "";
            String secnames = "абвг";
            String res = "" + secnames[sec];
            if (type == BuildingType.Quarta && delims != "111")
            {
                for (int i = sec - 1; i >= 0 && (delims[i] == '1'); i--)
                    if (delims[i] == '0') res = secnames[i] + res;
                for (int i = sec; i < 3 && delims[i] == '1'; i++)
                    if (delims[i] == '0') res = res + secnames[i + 1];
            }
            else if (type == BuildingType.Barin && delims[0] == '0')
                res = "аб";
            return res;
        }
        public static String GetRName(String type, bool shr)
        {
            String res = "Нет";
            switch (type)
            {
                case BuildingType.Female: res = shr ? BuildingType.Female_Short : BuildingType.Female_Rus; break;
                case BuildingType.DualFemale: res = shr ? BuildingType.DualFemale_Short : BuildingType.DualFemale_Rus; break;
                case BuildingType.Complex: res = shr ? BuildingType.Complex_Short : BuildingType.Complex_Rus; break;
                case BuildingType.Jurta: res = shr ? BuildingType.Jurta_Short : BuildingType.Jurta_Rus; break;
                case BuildingType.Quarta: res = shr ? BuildingType.Quarta_Short : BuildingType.Quarta_Rus; break;
                case BuildingType.Vertep: res = shr ? BuildingType.Vertep_Short : BuildingType.Vertep_Rus; break;
                case BuildingType.Barin: res = shr ? BuildingType.Barin_Short : BuildingType.Barin_Rus; break;
                case BuildingType.Cabin: res = shr ? BuildingType.Cabin_Short : BuildingType.Cabin_Rus; break;
            }
            return res;
        }

        /// <summary>
        /// Возвращает количество секций у данного типа МИНИфермы
        /// </summary>
        public static int GetRSecCount(String type)
        {
            int res = 2;
            switch (type)
            {
                case BuildingType.Cabin:
                case BuildingType.Female: res = 1; break;
                case BuildingType.Complex: res = 3; break;
                case BuildingType.Quarta: res = 4; break;
            }
            return res;
        }

        public static int GetRNHCount(String type)
        {
            int res = 1;
            switch (type)
            {
                case BuildingType.DualFemale: res = 2; break;
                case BuildingType.Quarta:
                case BuildingType.Vertep:
                case BuildingType.Barin: res = 0; break;
            }
            return res;
        }

        public static String FullRName(int farm, int tierid, int sec, String type, String delims, bool shrt, bool showTier, bool ShowDescr)
        {
            String res = Building.Format(farm);
            if (tierid == 1) res += "^";
            if (tierid == 2) res += "-";
            res += GetRSec(type, sec, delims);
            if (showTier)
                res += " [" + GetRName(type, shrt) + "]";
            if (ShowDescr)
                res += " (" + GetRDescr(type, shrt, sec, delims) + ")";
            return res;
        }

        public static String FullPlaceName(String rabplace, bool shrt, bool showTier, bool showDescr)
        {
            if (rabplace == "")
                return Rabbit.NULL_ADDRESS;
            String[] dts = rabplace.Split(',');
            return FullRName(int.Parse(dts[0]), int.Parse(dts[1]), int.Parse(dts[2]), dts[3], dts[4], shrt, showTier, showDescr);
        }

        public static String FullPlaceName(String rabplace)
        {
            return FullPlaceName(rabplace, false, false, false);
        }

        public static bool HasNest(String rabplace)
        {
            if (rabplace == "")
                return false;
            String[] dts = rabplace.Split(',');
            return HasNest(dts[3], int.Parse(dts[2]), dts[5]);
        }

#endregion static
    }
}
