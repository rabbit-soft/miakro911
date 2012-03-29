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
}
