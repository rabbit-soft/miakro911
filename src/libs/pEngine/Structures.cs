/*
 * Если что-нибудь поменял здесь, то нужно поменять привязку в файле EngStructConv и
 * в nodeControl изменить DisplayProperty
 * 
 * Если есть конструктор с параметрами, то ОБЯЗАТЕЛЬНО должен быть пустой конструктор
 */

using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;

namespace pEngine
{
    public class sStation : IComparable<sStation>
    {
        [XmlRpcMissingMapping(MappingAction.Ignore)] 
        public string TermID="0";
        [XmlRpcMissingMapping(MappingAction.Ignore)] 
        public string Name;
        
        /// <summary>
        /// Если "1" , то станция удалена
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Deleted = "0";
        
        /// <summary>
        /// Nested3 путь родительской группы.
        /// Если isGroup=true, то сдеть отображается N3 путь группы
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string N3Ppath;
       
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Address;
        
        [ XmlRpcMissingMapping(MappingAction.Ignore)]
        public double Money =0;
        
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string DPrinterState;
        
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string DCasherState;
        
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int PrinterState;
        
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int CasherState;

        /// <summary>
        /// Сколько секунд назад было соединение с сервером
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string LastConnect = "";
        
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string LastConTD;

        /// <summary>
        /// Сколько секунд назад был последний платеж
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string LastPay = "";

        /// <summary>
        /// Как давно был последний платеж
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string LastPayTD = "0";

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string StationID;

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Block0 = "0";//TODO если не пригодится то удалить

        //[XmlRpcMissingMapping(MappingAction.Ignore)]
        //public string Block1 = "0";//TODO если не пригодится то удалить

        //[XmlRpcMissingMapping(MappingAction.Ignore)]
        //public string Block2 = "0";//TODO если не пригодится то удалить

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string BlockNU = "0";//TODO если не пригодится то удалить

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string TariffName;//TODO если не пригодится то удалить
        
        /// <summary>
        /// При какой сумме красить в Желтый
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string MoneyInfo = "15000";
        
        /// <summary>
        /// При какой сумме красить в Красный 
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string MoneyAlert = "30000";

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Version;

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string GroupID;

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string TariffID;

        public int CompareTo(sStation other)
        {
            int res = int.Parse(this.TermID).CompareTo(int.Parse(other.TermID));
            return res;
        }
    }

    public class sGroup:IComparable<sGroup>
    {
        [XmlRpcMember("G_ID")] 
        public string GroupID;
        [XmlRpcMember("G_NAME")] 
        public string GroupName;
        //[XmlRpcMember("Level")]
        public string Level;
        [XmlRpcMember("N3Ppath")]
        public string N3Ppath;
        [XmlRpcMember("G_TYPE")]
        public string GroupType;
        //[XmlRpcMember("ProcessGroup")]
        public string ProcessGroup;
        /// <summary>
        /// Nested3 путь данной групы
        /// </summary>
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string N3path
        {
            get
            {
                if (N3Ppath == "")
                    return GroupID;
                return N3Ppath + "." + GroupID;
            }
        }

        public int CompareTo(sGroup other)
        {
            int res = int.Parse(this.Level).CompareTo(int.Parse(other.Level));
            if(res == 0)
                res = int.Parse(this.GroupID).CompareTo(int.Parse(other.GroupID));
            return res;      
        }
    }
    
    public class sPermission:IComparable<sPermission>//,ICloneable
    {
        [XmlRpcMember("WID")] 
        public readonly string PermID;
        [XmlRpcMember("W_Nick")] 
        public readonly string Nick;
        [XmlRpcMember("W_Name")]
        public readonly string Description;
        [XmlRpcMember("W_Parent")]
        public readonly string Parent;

        public sPermission(string pid, string nick, string descr, string parent)
        {
            this.PermID = pid;
            this.Nick = nick;
            this.Description = descr;
            this.Parent = parent;
        }
        public sPermission(string pid, string descr):this( pid,"",descr,"0"){}
        public sPermission() { }

        public int CompareTo(sPermission other)
        {
            try
            {
                int res = int.Parse(this.Parent).CompareTo(int.Parse(other.Parent));
                if (res != 0)
                    return res;
                else
                    return this.Description.CompareTo(other.Description);
            }
            catch { return 0; }
        }

        public override bool Equals(object other)
        {
            return this.PermID == (other as sPermission).PermID;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /*public object Clone()
        {
            return new sPermission(this.PermID,this.Nick,this.Description,this.Parent);
        }*/
    }

    public class sProcessGroup
    {
        [XmlRpcMember("G_ID")]
        public string Id;
        [XmlRpcMember("G_NAME")]
        public string Name;
    }

    public interface IOpOrSvc
    {
        string Id { get; }
        string Name { get; }
        string NameRus { get; }
    }

    public class sOperator:IOpOrSvc
    {
        [XmlRpcMember("OP_ID")]
        public string Id {get;set;}
        [XmlRpcMember("OP_NAME")]
        public string Name { get; set; }
        [XmlRpcMember("OP_FULL_NAME")]
        public string NameRus { get; set; }
    }

    public class sServiceType : IOpOrSvc
    {
        [XmlRpcMember("st_id")]
        public string Id { get; set; }
        [XmlRpcMember("st_name")]
        public string Name { get; set; }
        [XmlRpcMember("st_name_rus")]
        public string NameRus { get; set; }
    }

    public class sTariff
    {
        [XmlRpcMember("TAR_ID")]
        public string Id;
        [XmlRpcMember("NAME")]
        public string Name;
    }

    public class sStationInfo
    {
        [XmlRpcMember("S_NAME")]
        public string Name;
        [XmlRpcMember("S_ADDRESS")]
        public string Address;
        [XmlRpcMember("S_ID")]
        public string StationID;
        [XmlRpcMember("T_ID")]
        public string TermID;
        [XmlRpcMember("T_VERSIONS")]
        public string Version;
        [XmlRpcMember("msgs")]
        public string Messages;
        [XmlRpcMember("T_B_NUMBER")]
        public string Number;
        [XmlRpcMember("T_B_ACCOUNT")]
        public string Account;
    }

    public class sPay: IComparable<sPay>
    {
        [XmlRpcMember("id")]
        public string ID="";
        [XmlRpcMember("Terminal")]
        public string TermID = "";
        [XmlRpcMember("CheckNumber")]
        public int Check = 0;
        [XmlRpcMember("ClientDate")]
        public string AcceptDate = "";
        [XmlRpcMember("PROC_DATE")]
        public string ProcessDate = "";
        [XmlRpcMember("Summ")]
        public double AcceptSumm = 0;
        [XmlRpcMember("PaySumm")]
        public double ProcessSumm = 0;
        [XmlRpcMember("Account")]
        public string Account = "";
        [XmlRpcMember("PROC_RESULT")]
        public string Result = "";
        [XmlRpcMember("gw")]
        public string GateWay = "";
        [XmlRpcMember("Cmd")]
        public string AccountType = "";
        [XmlRpcMember("stat")]
        public string Station = "";


        public int CompareTo(sPay other)
        {
            return 0;
        }
    }

    public class sAccountType
    {
        [XmlRpcMember("CMD_ID")]
        public string ID;
        [XmlRpcMember("COMMENT")]
        public string Comment;
        [XmlRpcMember("NAME")]
        public string Name;
    }

    public class sResendComment
    {
        [XmlRpcMember("cmt_text")] 
        public string Text;
        [XmlRpcMember("cmt_priority")]
        public string Priority;
    }

    public class sIncase : IComparable<sIncase>
    {
        [XmlRpcMember("T_ID")]
        public string TermID;
        [XmlRpcMember("stat")]
        public string Station;
        [XmlRpcMember("dt")]
        public string IncaseDate;
        [XmlRpcMember("AcceptDate")]
        public string AcceptDate;
        [XmlRpcMember("SvrPaysSumm")]
        public int ServerSumm;
        [XmlRpcMember("CliCashSumm")]
        public int ClientSumm;
        [XmlRpcMember("m1")]
        public int m1;
        [XmlRpcMember("m2")]
        public int m2;
        [XmlRpcMember("m5")]
        public int m5;
        [XmlRpcMember("m10m")]
        public int m10c;
        [XmlRpcMember("m10")]
        public int m10b;
        [XmlRpcMember("m50")]
        public int m50;
        [XmlRpcMember("m100")]
        public int m100;
        [XmlRpcMember("m500")]
        public int m500;
        [XmlRpcMember("m1000")]
        public int m1000;
        [XmlRpcMember("m5000")]
        public int m5000;
        [XmlRpcMember("rmoney")]
        public string TotalSumm;
        [XmlRpcMember("Document_ID")]
        public string DocumentID;
        [XmlRpcMember("path")]
        public string N3Ppath;
        [XmlRpcMember("acc")]
        public string TermAccount;
        [XmlRpcMember("canEdit")]
        public string CanEdit;
        [XmlRpcMember("StartCheckNumber")]
        public string StartCheck;
        [XmlRpcMember("EndCheckNumber")]
        public string EndCheck;

        public int CompareTo(sIncase other)
        {
            int res = int.Parse(this.TermID).CompareTo(int.Parse(other.TermID));
            return res;
        }
    }

    public class sServer
    {
        [XmlRpcMember("Srv_Name")]
        public string Name;
        [XmlRpcMember("Srv_IP")]
        public string Address;

        public sServer(string name, string address)
        {
            this.Name = name;
            this.Address = address;
        }
        public sServer() { }
    }

    public class sReport:IComparable<sReport>
    {
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string N3Ppath = "";
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string TermID;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Name;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Operator;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Date;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string PaySystem;
        //[XmlRpcMissingMapping(MappingAction.Ignore)]
        //public string GroupID;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int ReceivedPays=0;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double ReceivedMoney=0;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double ProcessedMoney=0;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double Fee=0;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double Profit=0;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public int ProcessedPays=0;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public double Comission = 0;

        public int CompareTo(sReport other)
        {
            return 0;
        }

    }

    public class sUserPermission:IComparable<sUserPermission>
    {
        public string Id;
        public string Name;
        /// <summary>
        /// Какую группу может просматривать пользователь.
        /// </summary>
        public string GroupID;
        /// <summary>
        /// N3 путь Пользователя-родителя
        /// </summary>
        public string Parent;
        public bool Block;
        public sPermission[] Permiss;

        public int CompareTo(sUserPermission other)
        {
            return Helper.N3Ppath_Compare(ref this.Parent,ref other.Parent);
        }
    }

    public class sUpdateFile
    {
        public string Name;
        /// <summary>
        /// Каталог в папке обновления, в котором находится файл.
        /// <remarks>
        /// Не должен начинаться прямой косой черты(/).
        /// Должен заканчиваться на прямую косую черту(/).</remarks>
        /// </summary>
        public string Path;
        public string md5;
        public string Version;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string PathName
        {
            get { return Path + Name; }
        }
    }

    /// <summary>
    /// Flash Interface Button
    /// </summary>
    public class sFIButton
    {
        [XmlRpcMember("fb_id")]
        public int ID=0;
        [XmlRpcMember("fb_name"), XmlRpcMissingMapping(MappingAction.Ignore)]
        public string Name="";
        [XmlRpcMember("call_type"), XmlRpcMissingMapping(MappingAction.Ignore)]
        public int CallType=0;
        [XmlRpcMember("call_id"), XmlRpcMissingMapping(MappingAction.Ignore)]
        public int CallId=0;
        [XmlRpcMember("image_name"), XmlRpcMissingMapping(MappingAction.Ignore)]
        public string ImageName;
        [XmlRpcMember("order"), XmlRpcMissingMapping(MappingAction.Ignore)]
        public int Order=0;
        [XmlRpcMember("suborder"), XmlRpcMissingMapping(MappingAction.Ignore)]
        public int SubOrder=0;
        [XmlRpcMember("properties"), XmlRpcMissingMapping(MappingAction.Ignore)]
        public KVPair[] Properties;
    }

    /// <summary>
    /// Key-Value Pair
    /// </summary>
    public class KVPair
    {
        [XmlRpcMember("fp_property")]
        public string Name;
        [XmlRpcMember("fp_value")]
        public string Value;

        public KVPair(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
        public KVPair() { }
    }

    public class sFIMenu
    {
        [XmlRpcMember("fm_id")]
        public int Id;
        [XmlRpcMember("fm_name")]
        public string Name;
        [XmlRpcMember("fm_type")]
        public int Type;
        [XmlRpcMember("buttons"),XmlRpcMissingMapping(MappingAction.Ignore)]
        public sFIButton[] Buttons;
    }
    
}
