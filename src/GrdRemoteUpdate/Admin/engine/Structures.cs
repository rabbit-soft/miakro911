/*
 * Если что-нибудь поменял здесь, то нужно поменять привязку в файле EngStructConv и
 * в nodeControl изменить DisplayProperty
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using CookComputing.XmlRpc;

namespace pEngine
{

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

    public class sClient
    {
        [XmlRpcMember("c_id")]
        public readonly string Id;
        [XmlRpcMember("c_org")]
        public readonly string Organization;
        [XmlRpcMember("c_contact")]
        public readonly string ContactMan;
        [XmlRpcMember("c_address")]
        public readonly string Address;
        [XmlRpcMember("c_money")]
        public readonly string Money;
        [XmlRpcMember("c_saas")]
        public readonly bool SAAS;
        [XmlRpcMember("dongles"),
        XmlRpcMissingMapping(MappingAction.Ignore)]
        public sDongle[] Dongles;

    }

    public class sDongle
    {
        [XmlRpcMember("d_id")]
        public string Id;
        [XmlRpcMember("u_flags")]
        public string Flags;
        [XmlRpcMember("u_farms")]
        public string Farms;
        [XmlRpcMember("sd")]
        public string StartDate;
        [XmlRpcMember("ed")]
        public string EndDate;
        [XmlRpcMember("u_time_flags")]
        public string TimeFlags;
        [XmlRpcMember("tfe")]
        public string TimeFlagsEnd;
    }

    public class sPayment
    {
        [XmlRpcMember("m_date")]
        public string Date;
        [XmlRpcMember("m_debet")]
        public string Debet;
        [XmlRpcMember("m_credit")]
        public string Credit;
        [XmlRpcMember("m_comment")]
        public string Comment;
    }

    public class sWebRepOneDay
    {
        public string Date;
        public string Fucks;
        public string Okrols;
        public string Proholosts;
        public string Born;
        public string Killed;
        public string Deads;
        public string Rabbits;

        public sWebRepOneDay(string vals)
        {
            FieldInfo fi;
            Type t = this.GetType();           
            string[] cols = vals.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string pair in cols)
            {
                string[] dict = pair.Split(new char[] { '=' });
                fi = t.GetField(dict[0]);
                if (fi != null)
                    fi.SetValue(this, dict[1]);
            }            
        }
        
    }

    public class sDump
    {
        [XmlRpcMember("farm")]
        public string Farm;
        [XmlRpcMember("datetime")]
        public string Datetime;
        [XmlRpcMember("filename")]
        public string FileName;
        [XmlRpcMember("md5dump")]
        public string MD5;
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

        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string LocalFilePath;
        [XmlRpcMissingMapping(MappingAction.Ignore)]
        public string LocalFileMD5;
    }
}
