/*
 * Если что-нибудь поменял здесь, то нужно поменять привязку в файле EngStructConv и
 * в nodeControl изменить DisplayProperty
 */

using System;
using System.Collections.Generic;
using System.Text;
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
}
